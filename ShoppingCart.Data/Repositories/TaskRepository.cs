using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class TasksRepository : ITasksRepository
    {
        ShoppingCartDbContext _context;

        public TasksRepository(ShoppingCartDbContext context)
        {

            _context = context;
        }

        public Task GetTask(Guid id)
        {
            return _context.Tasks.SingleOrDefault(x => x.Id == id);
        
        }

        public Guid AddTask(Task t)
        {
            // p.Category = null; //because the runtime thinks that it needs to add a new category

            _context.Tasks.Add(t);
            _context.SaveChanges();

            return t.Id;
        }

    
        public IQueryable<Task> GetTasks()
        {
            return _context.Tasks;
        }
    }
}
