using AutoMapper;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class StudentsService : IStudentsService
    {
        private IStudentsRepository _studentsRepo;
        private IMapper _autoMapper;
        public StudentsService(IStudentsRepository studentsRepo, IMapper autoMapper)
        {
            _studentsRepo = studentsRepo;
            _autoMapper = autoMapper;
        }
        public void AddStudent(StudentViewModel model)
        {

            _studentsRepo.AddStudent(_autoMapper.Map<Student>(model));
        }
        public StudentViewModel GetStudent(string email)
        {
            var t = _studentsRepo.GetStudent(email);
            if (t == null) return null;
            else
            {

                var result = _autoMapper.Map<StudentViewModel>(t);
                return result;
            }

        }
    }
}
