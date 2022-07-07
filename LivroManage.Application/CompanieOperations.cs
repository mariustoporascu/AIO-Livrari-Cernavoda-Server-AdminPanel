using Microsoft.EntityFrameworkCore;
using LivroManage.Application.FileManager;
using LivroManage.Application.ViewModels;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivroManage.Application
{
    public class CompanieOperations : EntityOperation<Companie>
    {
        private readonly OnlineShopDbContext _context;
        private readonly IFileManager _fileManager;
        public CompanieOperations(OnlineShopDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public override async Task Create(Companie model)
        {
            _context.Companies.Add(model);
            await _context.SaveChangesAsync();
        }

        public override async Task Delete(int id)
        {
            var restaurant = _context.Companies.AsNoTracking().FirstOrDefault(comp => comp.CompanieId == id);
            _context.Companies.Remove(restaurant);
            if (!string.IsNullOrEmpty(restaurant.Photo))
                _fileManager.RemoveImage(restaurant.Photo, "CompaniePhoto");
            await _context.SaveChangesAsync();
        }

        public override Companie Get(int? id)
        {
            return _context.Companies.AsNoTracking().FirstOrDefault(comp => comp.CompanieId == id);
        }

        public IEnumerable<CompanieVM> GetAllVM()
        {
            return _context.Companies.AsNoTracking().AsEnumerable().Select(companie => new CompanieVM(companie)
            {
                TransportFees = _context.TransportFees.AsNoTracking().AsEnumerable().Where(fee => fee.MainCityRefId == 1 && fee.TipCompanieRefId == companie.TipCompanieRefId),
            });
        }
        public IEnumerable<CompanieVM> GetAllVM(int tipId) =>
            _context.Companies.AsNoTracking().AsEnumerable().Where(comp => comp.TipCompanieRefId == tipId).Select(companie => new CompanieVM(companie)
            {
                TransportFees = _context.TransportFees.AsNoTracking().AsEnumerable().Where(fee => fee.MainCityRefId == 1 && fee.TipCompanieRefId == companie.TipCompanieRefId),
            });

        public override IEnumerable<Companie> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Companie> GetAll(int canal)
        {
            throw new NotImplementedException();
        }

        public override async Task Update(Companie model)
        {
            _context.Companies.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
