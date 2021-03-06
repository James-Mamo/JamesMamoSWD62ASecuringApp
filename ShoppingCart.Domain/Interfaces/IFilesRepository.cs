using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface IFilesRepository
    {
        IQueryable<File> GetFiles();
        File GetFile(Guid id);
        Guid AddFile(File f);
    }
}
