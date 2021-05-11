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
    public class FilesService : IFilesService
    {
        private IFilesRepository _filesRepo;
        private IMapper _autoMapper;
        public FilesService(IFilesRepository filesRepo, IMapper autoMapper)
        {
            _autoMapper = autoMapper;
            _filesRepo = filesRepo;
        }


        public void AddFile(FileViewModel f)
        {
            _filesRepo.AddFile(_autoMapper.Map<File>(f));
        }

        public FileViewModel GetFile(Guid id)
        {
            var f = _filesRepo.GetFile(id);
            if (f == null) return null;
            else
            {
                

                var result = _autoMapper.Map<FileViewModel>(f);
                return result;
            }
        }

       

        public IQueryable<FileViewModel> GetFiles()
        {
            return _filesRepo.GetFiles().ProjectTo<FileViewModel>(_autoMapper.ConfigurationProvider);
        }
    }
}
