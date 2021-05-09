using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class StudentViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
        public string Email { get; set; }
        public string TeacherEmail { get; set; }
        public string privateKey { get; set; }

        public string publicKey { get; set; }


    }
}
