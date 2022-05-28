using OShop.Application.FileManager;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Restaurante
{
    public class DeleteRestaurant
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public DeleteRestaurant(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task Do(int restaurantId)
        {
            var restaurant = _context.Restaurante
                .FirstOrDefault(product => product.RestaurantId == restaurantId);
            _context.Restaurante.Remove(restaurant);
            if (!string.IsNullOrEmpty(restaurant.Photo))
                _fileManager.RemoveImage(restaurant.Photo, "RestaurantPhoto");
            await _context.SaveChangesAsync();
        }
    }
}
