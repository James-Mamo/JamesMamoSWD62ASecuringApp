using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class FileCommentViewModel
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public Guid FileFk { get; set; }

        public string Owner { get; set; }
    }
}
