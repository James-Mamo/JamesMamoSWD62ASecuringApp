using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class FileCommentsService : IFilesCommentsService
    {
        private IFilesCommentsRepository _filesCommentsRepo;
        private IMapper _autoMapper;
        public FileCommentsService(IFilesCommentsRepository filesCommentsRepo, IMapper autoMapper)
        {
            _autoMapper = autoMapper;
            _filesCommentsRepo = filesCommentsRepo;
        }

        public void AddComment(FileCommentViewModel f)
        {
            _filesCommentsRepo.AddFileComment(_autoMapper.Map<FileComment>(f));
        }

        public FileCommentViewModel GetComment(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FileCommentViewModel> GetComments()
        {
            return _filesCommentsRepo.GetAllComments().ProjectTo<FileCommentViewModel>(_autoMapper.ConfigurationProvider);
        }
    }
}
