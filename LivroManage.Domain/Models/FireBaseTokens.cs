using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivroManage.Domain.Models
{
    public class FireBaseTokens
    {
        [Key]
        [Column(Order = 1)]
        public int FBId { get; set; }
        public string FBToken { get; set; }
        public string UserId { get; set; }
        public ApplicationUser AppUser { get; set; }
        public DateTime Created { get; set; }
    }
}
