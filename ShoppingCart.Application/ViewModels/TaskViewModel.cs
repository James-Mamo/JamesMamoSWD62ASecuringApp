using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class TaskViewModel
    {
   
        public Guid Id { get; set; }


        public string Title { get; set; }


        public string Description { get; set; }


        public DateTime DeadLine { get; set; }

        public string TeacherEmail { get; set; }
    }
}

