using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface ITasksService
    {
        void AddTask(TaskViewModel model);
        TaskViewModel GetTask(Guid id);
        IQueryable<TaskViewModel> GetTasks();
        IQueryable<TaskViewModel> GetTasks(string keyword);
    }
}
