using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class TasksFilesViewModel
    {
        public int Id { get; set; }


        public Guid TaskFk { get; set; }


        public Guid FileFk { get; set; }
    }
}
