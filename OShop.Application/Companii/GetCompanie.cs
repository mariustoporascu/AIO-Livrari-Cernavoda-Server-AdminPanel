using Microsoft.EntityFrameworkCore;
using OShop.Application.Companii;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OShop.Application.Companii
{
    public class GetCompanie
    {
        private readonly OnlineShopDbContext _context;

        public GetCompanie(OnlineShopDbContext context)
        {
            _context = context;
        }
        public Companie Do(int? restaurantId) => _context.Companies.AsNoTracking().FirstOrDefault(prod => prod.CompanieId == restaurantId);

    }
}
