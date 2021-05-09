using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecuringApplication.Utility;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SecuringApplication.Controllers
{
    public class FilesController : Controller
    {
        private readonly IWebHostEnvironment _host;
        private IFilesService _filesService;
        private ITasksFilesService _taskFilesService;
        public FilesController(IWebHostEnvironment host, IFilesService filesService, ITasksFilesService taskService )
        {

            _host = host;
            _filesService = filesService;
            _taskFilesService = taskService;
        }

  
        [HttpGet]
        public IActionResult Create()
        {
            

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                        //uploading the file
                        //correctness

                        Encryption enc = new Encryption();

                        MemoryStream ms = new MemoryStream();

                        file.CopyTo(ms);
                        //var fileEncrypted = enc.SymmetricEncrypt(ms.ToArray());

                        //System.IO.File.WriteAllBytes("test.pdf", fileEncrypted);

                        ////string uniqueFilename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        ////data.Path = uniqueFilename;
                        ////string absolutePath = _host.ContentRootPath + "\\ValuableFiles\\"+uniqueFilename;

                        //var bytes = System.IO.File.ReadAllBytes("test.pdf");

                        //var fileDecrypted = enc.SymmetricDecrypt(bytes);

                       

                        var keys = Encryption.GenerateAsymmetricKeys();
                        var sign = Encryption.SignData(ms, keys.PrivateKey);
                        var fileEnc = enc.HybridEncrypt(ms, keys.PublicKey);
                        data.Id = Guid.NewGuid();

                        data.Owner = User.Identity.Name;
                        data.Signature = sign;
                        data.Path = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        _filesService.AddFile(data);

                        System.IO.File.WriteAllBytes(_host.ContentRootPath + "\\ValuableFiles\\" + data.Path, fileEnc.ToArray());

                        TasksFilesViewModel taskFiles = new TasksFilesViewModel();

                        var decryptedid = Encryption.SymmetricDecrypt(id);
                        var taskId = Guid.Parse(decryptedid);

                        taskFiles.TaskFk = taskId;
                        taskFiles.FileFk = data.Id;

                        _taskFilesService.AddTaskFiles(taskFiles);
                        
                        

                        //var fileDec = Encryption.HybridDecrypt(fileEnc, keys.PrivateKey);

                        //System.IO.File.WriteAllBytes("test.pdf", fileDec.ToArray());

                        //using (FileStream fsOut = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write))
                        //{
                        //    f.CopyTo(fsOut);
                        //}
                        //f.Close();

                    }
                }
            }
            else
            {
                ModelState.AddModelError("file", "File is not valid and acceptable or size is greater than 10MB");
                return View();
            }

            data.Owner = HttpContext.User.Identity.Name;
            return View();
        }
    }
}

