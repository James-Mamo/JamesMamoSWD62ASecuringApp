using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IQueryable<TaskFile> GetTaskFiles()
        {
            return _context.TasksFiles;
        }

        public void AddFileToTask(TaskFile f)
        {
               
                _context.TasksFiles.Add(f);
                
                _context.SaveChanges();
        }
    }
}
