﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TechBooks.Models
{
    public class Category
    {
        [Display(Name = "Category Id")]
        public int CategoryId { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
