using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ITasksRepository
    {
        IQueryable<Task> GetTasks();
        Task GetTask(Guid id);
        Guid AddTask(Task t);
    }
}
