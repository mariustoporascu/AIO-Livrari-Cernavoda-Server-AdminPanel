using Microsoft.EntityFrameworkCore;
using OShop.Application.FileManager;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace OShop.Application.Restaurante
{
    public class GetAllRestaurante
    {
        private readonly OnlineShopDbContext _context;

        public GetAllRestaurante(OnlineShopDbContext context)
        {
            _context = context;
        }

        public IEnumerable<RestaurantVMUI> Do() =>
            _context.Restaurante.AsNoTracking().ToList().Select(categ => new RestaurantVMUI
            {
                RestaurantId = categ.RestaurantId,
                Name = categ.Name,
                Photo = categ.Photo,
                TelefonNo = categ.TelefonNo,
                Opening = categ.Opening,
                MinimumOrderValue = categ.MinimumOrderValue,
                TransporFee = categ.TransporFee,
                IsActive = categ.IsActive,
                //Image = (categ.Photo == null || categ.Photo == "") ? null : Convert.ToBase64String(getBytes(_fileManager.ImageStream(categ.Photo))),
            });
        private byte[] getBytes(FileStream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return buffer;
        }


    }
    public class RestaurantVMUI
    {
        public int RestaurantId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Photo { get; set; }
        public string TelefonNo { get; set; }
        public string Image { get; set; }
        public DateTime Opening { get; set; }
        public decimal TransporFee { get; set; }
        public decimal MinimumOrderValue { get; set; }
        public bool IsActive { get; set; }

    }
}
