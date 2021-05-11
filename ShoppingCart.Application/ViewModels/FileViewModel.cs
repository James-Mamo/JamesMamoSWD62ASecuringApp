using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class FileViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Please input name of File")]
        public string FileName { get; set; }
        public string Description { get; set; }

        public string Path { get; set; }
        public string Signature { get; set; }
        public string Owner { get; set; }
        
        public string Digest { get; set; }
    }
}
