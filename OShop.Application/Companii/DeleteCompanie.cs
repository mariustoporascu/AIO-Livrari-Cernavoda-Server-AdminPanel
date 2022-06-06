using OShop.Application.FileManager;
using OShop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Companii
{
    public class DeleteCompanie
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;

        public DeleteCompanie(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task Do(int restaurantId)
        {
            var restaurant = _context.Companies
                .FirstOrDefault(product => product.CompanieId == restaurantId);
            _context.Companies.Remove(restaurant);
            if (!string.IsNullOrEmpty(restaurant.Photo))
                _fileManager.RemoveImage(restaurant.Photo, "CompaniePhoto");
            await _context.SaveChangesAsync();
        }
    }
}
