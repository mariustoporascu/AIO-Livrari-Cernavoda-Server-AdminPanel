using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Companii
{
    public class UpdateCompanie
    {
        private readonly OnlineShopDbContext _context;

        public UpdateCompanie(OnlineShopDbContext context)
        {
            _context = context;
        }
        public async Task Do(Companie vm)
        {
            _context.Companies.Update(vm);
            await _context.SaveChangesAsync();
        }
    }
}
