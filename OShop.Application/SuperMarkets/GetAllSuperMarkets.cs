using Microsoft.EntityFrameworkCore;
using OShop.Application.FileManager;
using OShop.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace OShop.Application.SuperMarkets
{
    public class GetAllSuperMarkets
    {
        private readonly OnlineShopDbContext _context;

        public GetAllSuperMarkets(OnlineShopDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SuperMarketVMUI> Do() =>
            _context.SuperMarkets.AsNoTracking().ToList().Select(categ => new SuperMarketVMUI
            {
                SuperMarketId = categ.SuperMarketId,
                Name = categ.Name,
                Photo = categ.Photo,
            });
        private byte[] getBytes(FileStream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return buffer;
        }


    }
    public class SuperMarketVMUI
    {
        public int SuperMarketId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Image { get; set; }
    }
}
