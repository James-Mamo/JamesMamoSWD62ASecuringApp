using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class TaskFileRepository : ITaskFileRepository
    {
        public ShoppingCartDbContext _context;

        public TaskFileRepository(ShoppingCartDbContext context)
        {
            _context = context;
        }

        public void AddFileToTask(TaskFile f)
        {
               
                _context.TasksFiles.Add(f);
                
                _context.SaveChanges();
        }
    }
}
