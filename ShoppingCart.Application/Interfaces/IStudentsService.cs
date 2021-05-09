using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface IStudentsService
    {
        void AddStudent(StudentViewModel model);
    }
}
