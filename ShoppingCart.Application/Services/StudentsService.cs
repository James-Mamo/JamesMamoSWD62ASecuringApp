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
            //ProductViewModel >>>>>> Product

            /* Product p = new Product()
             {
                 Name = model.Name,
                 Description = model.Description,
                 ImageUrl = model.ImageUrl,
                 Price = model.Price,
                 Stock = model.Stock,
                 CategoryId = model.Category.Id
             };

             _productsRepo.AddProduct(p);
            */

 

            _studentsRepo.AddStudent(_autoMapper.Map<Student>(model));
        }
    }
}
