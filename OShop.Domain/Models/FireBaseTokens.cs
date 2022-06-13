using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Domain.Models
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
