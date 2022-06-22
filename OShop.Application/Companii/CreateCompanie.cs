using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Companii
{
    public class CreateCompanie
    {
        private readonly OnlineShopDbContext _context;

        public CreateCompanie(OnlineShopDbContext context)
        {
            _context = context;
        }
        public async Task Do(Companie vm)
        {
            _context.Companies.Add(vm);
            await _context.SaveChangesAsync();
        }
    }
}
