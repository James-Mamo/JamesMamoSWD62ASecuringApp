using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class File
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }

        [Required]
        public string Path { get; set; }
        [Required]
        public string Signature { get; set; }
        [Required]
        public string Owner { get; set; }
    }
}
