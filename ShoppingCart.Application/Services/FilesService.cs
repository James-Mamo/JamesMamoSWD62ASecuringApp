using AutoMapper;
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
    public class FilesService : IFilesService
    {
        private IFilesRepository _filesRepo;
        private IMapper _autoMapper;
        public FilesService(IFilesRepository filesRepo, IMapper autoMapper)
        {
            _filesRepo = filesRepo;
            _autoMapper = autoMapper;
        }


        public void AddFile(FileViewModel f)
        {
            _filesRepo.AddFile(_autoMapper.Map<File>(f));
        }

        public FileViewModel GetFile(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FileViewModel> GetFiles()
        {
            throw new NotImplementedException();
        }
    }
}
