﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OShop.Domain.Models
{
    public class SubCategory
    {
        [Key]
        [Column(Order = 1)]
        public int SubCategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Photo { get; set; } = "";
        public int CategoryRefId { get; set; }
        public Category Categories { get; set; }
    }
}