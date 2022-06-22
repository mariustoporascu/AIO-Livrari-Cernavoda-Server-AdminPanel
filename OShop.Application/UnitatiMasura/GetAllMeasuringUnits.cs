using Microsoft.EntityFrameworkCore;
using OShop.Database;
using OShop.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OShop.Application.UnitatiMasura
{
    public class GetAllMeasuringUnits
    {
        private readonly OnlineShopDbContext _context;

        public GetAllMeasuringUnits(OnlineShopDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MeasuringUnit> Do() =>
            _context.MeasuringUnits.AsNoTracking().ToList();
    }

}
