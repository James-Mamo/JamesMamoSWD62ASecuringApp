using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class StudentsRepository : IStudentsRepository
    {
        ShoppingCartDbContext _context;
        public StudentsRepository(ShoppingCartDbContext context)
        {

            _context = context;
        }
        public Student GetStudent(string email)
        {
            return _context.Students.SingleOrDefault(x => x.Email == email);

        }

        public Guid AddStudent(Student s)
        {
            // p.Category = null; //because the runtime thinks that it needs to add a new category

            _context.Students.Add(s);
            _context.SaveChanges();

            return s.Id;
        }

    }
}
