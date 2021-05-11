using AutoMapper;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.AutoMapper
{
    public class DomainToViewModelProfile: Profile
    {
        public DomainToViewModelProfile()
        {
          
            CreateMap<Student, StudentViewModel>();
            CreateMap<File, FileViewModel>();
            CreateMap<Task, TaskViewModel>();
            CreateMap<TaskFile, TasksFilesViewModel>();
            CreateMap<FileComment, FileCommentViewModel>();

        }

    }
}
