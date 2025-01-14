﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivroManage.Domain.Models
{
    public class Category
    {
        [Key]
        [Column(Order = 1)]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Campul este obligatoriu")]
        public string Name { get; set; }

        public string Photo { get; set; } = "";
        public int CompanieRefId { get; set; }
        public Companie Companies { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
