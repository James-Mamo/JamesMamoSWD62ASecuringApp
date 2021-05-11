using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IQueryable<File> GetFiles()
        {
            return _context.Files;
        }

        public File GetFile(Guid id)
        {
            return _context.Files.SingleOrDefault(x => x.Id == id);
        }


        public Guid AddFile(File f)
        {
            _context.Files.Add(f);
            _context.SaveChanges();

            return f.Id;
        }
    }
}
