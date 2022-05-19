using Microsoft.EntityFrameworkCore;
using OShop.Database;
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

        public IEnumerable<UnitateMasuraVMUI> Do() =>
            _context.MeasuringUnits.AsNoTracking().ToList().Select(unit => new UnitateMasuraVMUI
            {
                UnitId = unit.UnitId,
                Name = unit.Name,
            });
    }
    public class UnitateMasuraVMUI
    {
        public int UnitId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
