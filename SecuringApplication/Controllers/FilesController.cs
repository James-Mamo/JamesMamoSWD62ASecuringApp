using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecuringApplication.ActionFilters;
using SecuringApplication.Utility;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SecuringApplication.Controllers
{
    public class FilesController : Controller
    {
        private readonly IWebHostEnvironment _host;
        private IFilesService _filesService;
        private IFilesCommentsService _filesCommentsService;
        private ITasksFilesService _taskFilesService;
        private IStudentsService _studentsService;
        private ILogger<FilesController> _logger;
        public FilesController(ILogger<FilesController> logger,IWebHostEnvironment host, IFilesService filesService, ITasksFilesService taskService, IStudentsService studentsService, IFilesCommentsService filesCommentsService )
        {

            _host = host;
            _filesService = filesService;
            _taskFilesService = taskService;
            _studentsService = studentsService;
            _filesCommentsService = filesCommentsService;
            _logger = logger;
        }

        [HttpGet]
        
        [Authorize]
        public IActionResult ViewFiles(string id)
        {
            var decryptedId = Encryption.SymmetricDecrypt(id);
            var taskId = Guid.Parse(decryptedId);

            var list = _taskFilesService.GetTaskFiles().Where(x => x.TaskFk == taskId);
            var fList = _filesService.GetFiles();
            var newList = new List<TasksFilesViewModel>();
            var files = new List<FileViewModel>();
            int counter = 0;

            if (list.Count() != 0)
            {
                foreach (TasksFilesViewModel c in list)
                {
                    if (c.TaskFk == taskId)
                    {
                        newList.Add(c);
                    }
                    foreach (FileViewModel p in fList)
                    {

                        if (Guid.Equals(c.FileFk,p.Id))
                        {
                           
                            files.Add(p);
                        }
                     }
                    counter++;
                }
                ViewBag.ControllerVariable = files;
                return View(files);
            }
            else
            {
                return View();
            }

           
        }
        [Authorize]

        [HttpGet]
        public IActionResult CommentFiles()
        {
            
            return View();
        }
        public IActionResult CommentFiles(FileCommentViewModel comments,string id)
        {
            var decryptedId = Encryption.SymmetricDecrypt(id);
            var fileId = Guid.Parse(decryptedId);

            comments.FileFk = fileId;
            comments.Owner = User.Identity.Name;
            _filesCommentsService.AddComment(comments);
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }

        [Authorize]
        public void  Download(string id)
        {
            try
            {
                var decryptedid = Encryption.SymmetricDecrypt(id);
                var fileId = Guid.Parse(decryptedid);
                var retrievedFile = _filesService.GetFile(fileId);
                var retrievedStudent = _studentsService.GetStudent(retrievedFile.Owner);
                MemoryStream initialFile = new MemoryStream();
                MemoryStream encryptedFile;
                var fileRetrieved = System.IO.File.OpenRead(_host.ContentRootPath + "\\ValuableFiles\\" + retrievedFile.Path);
                fileRetrieved.CopyTo(initialFile);

                var verifyData = Encryption.VerifyData(initialFile, retrievedStudent.publicKey, retrievedFile.Signature);
                if (verifyData == true)
                {
                    var data = Encryption.HybridDecrypt(initialFile, retrievedStudent.privateKey);
                    System.IO.File.WriteAllBytes(_host.ContentRootPath + "\\ValuableFiles\\UploadedFile.pdf", data.ToArray());


                    WebClient webClient = new WebClient();

                    webClient.DownloadFile(_host.ContentRootPath + "\\ValuableFiles\\UploadedFile.pdf", @"d:\UploadedFile.pdf");
                    var allFiles = _filesService.GetFiles();

                    if(User.IsInRole("Teacher"))
                    {
                        foreach(FileViewModel f in allFiles)
                        {
                            if(f.Digest == retrievedFile.Digest)
                            {
                                TempData["feedback"] = "This file has been copied!";
                            }
                        }
                    }
                    using (MemoryStream ms = new MemoryStream())
                    using (FileStream file = new FileStream(_host.ContentRootPath + "\\ValuableFiles\\" + retrievedFile.Path, FileMode.Open, FileAccess.Read))

                    {
                        byte[] bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                        ms.Write(bytes, 0, (int)file.Length);

                        Encryption enc = new Encryption();
                        encryptedFile = enc.HybridEncrypt(ms, retrievedStudent.publicKey);
                    }

                    
                    System.IO.File.WriteAllBytes(_host.ContentRootPath + "\\ValuableFiles\\" + retrievedFile, encryptedFile.ToArray());
                    System.IO.File.Delete(_host.ContentRootPath + "\\ValuableFiles\\UploadedFile.pdf");

                    _logger.LogInformation("Files Downloaded" + DateTime.Now + " by " + User.Identity.Name + " with an IP Address of " + GetIPAddress());

                   

                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error Occured at" + DateTime.Now + " by " + User.Identity.Name + " with an IP Address of " + GetIPAddress());

            }

           

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public IActionResult Create(IFormFile file, FileViewModel data,string id)
        {
            
            byte[] whitelist = new byte[] { 37,80,68,70 };
            if (System.IO.Path.GetExtension(file.FileName) == ".pdf")
            {
                if (file != null)
                {
                    using (var f = file.OpenReadStream())
                    {
                        byte[] buffer = new byte[4];
                        f.Read(buffer, 0, 4);

                        for (int i = 0; i < whitelist.Length; i++)
                        {
                            if (whitelist[i] == buffer[i])
                            {

                            }
                            else
                            {
                                ModelState.AddModelError("file", "File is not valid and acceptable");
                                return View();
                            }
                        }
                        f.Position = 0;


                        Encryption enc = new Encryption();

                        MemoryStream ms = new MemoryStream();

                        file.CopyTo(ms);



                        var retrievedStudent = _studentsService.GetStudent(User.Identity.Name);

                        var fileEnc = enc.HybridEncrypt(ms, retrievedStudent.publicKey);
                        var sign = Encryption.SignData(fileEnc, retrievedStudent.privateKey);
                        data.Id = Guid.NewGuid();
                      
                        var tre = Encryption.VerifyData(fileEnc, retrievedStudent.publicKey, sign);
                        data.Owner = User.Identity.Name;
                        data.Signature = sign;
                    
                        data.Description = HtmlEncoder.Default.Encode(data.Description);
                        data.Path = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        data.Digest = enc.Hash(ms.ToArray().ToString());
                        _filesService.AddFile(data);

                        System.IO.File.WriteAllBytes(_host.ContentRootPath + "\\ValuableFiles\\" + data.Path, fileEnc.ToArray());

                        TasksFilesViewModel taskFiles = new TasksFilesViewModel();

                        var decryptedid = Encryption.SymmetricDecrypt(id);
                        var taskId = Guid.Parse(decryptedid);

                        taskFiles.TaskFk = taskId;
                        taskFiles.FileFk = data.Id;
                       

                        _taskFilesService.AddTaskFiles(taskFiles);
                        _logger.LogInformation("Files added at " + DateTime.Now + " by " + User.Identity.Name + " with an IP Address of " + GetIPAddress());

                    }
                }
            }
            else
            {
                ModelState.AddModelError("file", "File is not valid and acceptable or size is greater than 10MB");

                var host = Dns.GetHostEntry(Dns.GetHostName());
                
                _logger.LogError("File is not Valid at " + DateTime.Now + " by " + User.Identity.Name + " with an IP Address of " + GetIPAddress());
                return View();
                
            }

            data.Owner = HttpContext.User.Identity.Name;
            return View();
        }

        public IActionResult DisplayComments(string id)
        {
            var decryptedid = Encryption.SymmetricDecrypt(id);
            var taskId = Guid.Parse(decryptedid);
            var retrievedcomments = _filesCommentsService.GetComments().Where(x => x.FileFk == taskId);

            return View(retrievedcomments);
        }

        private string GetIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
   

}

