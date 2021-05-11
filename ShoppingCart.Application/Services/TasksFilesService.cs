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
    public class TasksFilesService : ITasksFilesService
    {
        public ITaskFileRepository _taskFilesRepo;
        private IMapper _mapper;
        public TasksFilesService(ITaskFileRepository ordersRepo, IMapper mapper)
        {
            _taskFilesRepo = ordersRepo;
            _mapper = mapper;
        }

        public void AddTaskFiles(TasksFilesViewModel data)
        {
            TaskFile tempDetails = new TaskFile();

            _taskFilesRepo.AddFileToTask(_mapper.Map<TaskFile>(data));
        }


        public IQueryable<TasksFilesViewModel> GetTaskFiles()
        {
            return _taskFilesRepo.GetTaskFiles().ProjectTo<TasksFilesViewModel>(_mapper.ConfigurationProvider);
        }
    }
}
