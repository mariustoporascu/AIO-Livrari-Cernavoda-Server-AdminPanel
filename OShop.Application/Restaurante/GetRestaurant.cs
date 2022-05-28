using Microsoft.EntityFrameworkCore;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Restaurante
{
    public class GetRestaurant
    {
        private readonly OnlineShopDbContext _context;

        public GetRestaurant(OnlineShopDbContext context)
        {
            _context = context;
        }
        public RestaurantVMUI Do(int? restaurantId)
        {
            var vm = _context.Restaurante.AsNoTracking().FirstOrDefault(prod => prod.RestaurantId == restaurantId);
            return new RestaurantVMUI
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
        }
    }
}
