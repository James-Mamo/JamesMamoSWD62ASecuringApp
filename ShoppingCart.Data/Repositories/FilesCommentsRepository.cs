using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class FilesCommentsRepository : IFilesCommentsRepository
    {

        ShoppingCartDbContext _context;

        public FilesCommentsRepository(ShoppingCartDbContext context)
        {

            _context = context;
        }

        public Guid AddFileComment(FileComment s)
        {
            _context.FilesComments.Add(s);
            _context.SaveChanges();

            return s.Id;
        }

        public IQueryable<FileComment> GetAllComments()
        {
            return _context.FilesComments;
        }


        public FileComment GetComment(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
