using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface IFilesService
    {
        IQueryable<FileViewModel> GetFiles();

        FileViewModel GetFile(Guid id);
        void AddFile(FileViewModel f);
    }
}
