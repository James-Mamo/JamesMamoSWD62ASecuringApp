using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;

namespace SecuringApplication.Controllers
{
    public class TasksController : Controller
    {
        private ITasksService _tasksService;
        public TasksController(ITasksService tasksService)
        {

            _tasksService = tasksService;
        }

        public IActionResult Index()
        {
            return View();
        }


        
        public IActionResult CreateTask()
        {
            var uri = Request.QueryString;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTask(TaskViewModel data)
        {

            data.TeacherEmail = User.Identity.Name;
            _tasksService.AddTask(data);


            return View();
        }

        public IActionResult TaskView()
        {
          
            var list = _tasksService.GetTasks();

            return View(list);
        }


    }
}
