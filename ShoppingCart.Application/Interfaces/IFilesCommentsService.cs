using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface IFilesCommentsService
    {
        IQueryable<FileCommentViewModel> GetComments();

        FileCommentViewModel GetComment(Guid id);
        void AddComment(FileCommentViewModel f);
    }
}
