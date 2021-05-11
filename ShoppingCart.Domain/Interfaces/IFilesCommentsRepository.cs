using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface IFilesCommentsRepository
    {
        Guid AddFileComment(FileComment s);
        FileComment GetComment(Guid id);
        IQueryable<FileComment> GetAllComments();
    }
}
