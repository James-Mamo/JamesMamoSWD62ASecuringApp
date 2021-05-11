using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class FileComment
    {
        [Key]
        public Guid Id { get; set; }
        public string Comment { get; set; }

        [ForeignKey("File")]
        public Guid FileFk { get; set; }


        public string Owner { get; set; }
    }
}
