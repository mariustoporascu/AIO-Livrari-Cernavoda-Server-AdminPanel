using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Restaurante
{
    public class UpdateRestaurant
    {
        private readonly OnlineShopDbContext _context;

        public UpdateRestaurant(OnlineShopDbContext context)
        {
            _context = context;
        }
        public async Task Do(RestaurantVMUI vm)
        {
            var restaurant = new Restaurant
            {
                RestaurantId = vm.RestaurantId,
                Name = vm.Name,
                Photo = vm.Photo,
                TelefonNo = vm.TelefonNo,
                Opening = vm.Opening,
                MinimumOrderValue = vm.MinimumOrderValue,
                TransporFee = vm.TransporFee,
                IsActive = vm.IsActive,
            };
            _context.Restaurante.Update(restaurant);
            await _context.SaveChangesAsync();
        }
    }
}
