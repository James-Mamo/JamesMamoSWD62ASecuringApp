﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecuringApplication.Utility;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;

namespace SecuringApplication.Controllers
{
    public class StudentsController : Controller
    {
        private IStudentsService _studentsService;
        private readonly IWebHostEnvironment _host;

        UserManager<IdentityUser> _userManager;

        public StudentsController(IStudentsService studentsService,UserManager<IdentityUser> userManager, IWebHostEnvironment host)
        {
            _studentsService = studentsService;
            _userManager = userManager;
            _host = host;
        }
        public IActionResult Index()
        {
            return View();
        }

        void email(string from,string to,string password)
        {
            var fromAddress = new MailAddress(from, "Teacher");
            var toAddress = new MailAddress(to, "Student");
         
            const string subject = "Subject";
            const string body = "Body";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, password)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

 
        public async Task<IActionResult> CreateStudentAsync(StudentViewModel data,string passwordEntered)
        {
            try
            {
                Encryption enc = new Encryption();
                var keys = Encryption.GenerateAsymmetricKeys();

                data.Id = new Guid();
                data.TeacherEmail = User.Identity.Name;
                data.privateKey = keys.PrivateKey;
                data.publicKey = keys.PublicKey;

                _studentsService.AddStudent(data);
                TempData["feedback"] = "Student was added successfully";
                IdentityUser newUser = new IdentityUser();

                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[8];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var password = new String(stringChars);


                newUser.Email = data.Email;
                newUser.EmailConfirmed = true;
                newUser.PasswordHash = password;
                newUser.UserName = data.Email;
                

                await _userManager.CreateAsync(newUser);
                await _userManager.AddToRoleAsync(newUser,"Student");


                email(User.Identity.Name, newUser.Email, passwordEntered);

                ModelState.Clear();
            }catch(Exception e)
            {
                TempData["warning"] = "Student was not added. Check your details";
            }

            return View();
            
        }
    }
}
