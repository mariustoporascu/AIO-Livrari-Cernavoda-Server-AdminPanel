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
        public async Task Do(CompanieVMUI vm)
        {
            var restaurant = new Companie
            {
                CompanieId = vm.CompanieId,
                Name = vm.Name,
                Photo = vm.Photo,
                TelefonNo = vm.TelefonNo,
                Opening = vm.Opening,
                TipCompanieRefId = vm.TipCompanieRefId,
                IsActive = vm.IsActive,
            };
            _context.Companies.Add(restaurant);
            await _context.SaveChangesAsync();
        }
    }
}
