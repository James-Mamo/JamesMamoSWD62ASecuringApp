using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class FilesRepository : IFilesRepository
    {
        ShoppingCartDbContext _context;

        public FilesRepository(ShoppingCartDbContext context)
        {

            _context = context;
        }
        public Guid AddFile(File f)
        {
            _context.Files.Add(f);
            _context.SaveChanges();

            return f.Id;
        }
    }
}
