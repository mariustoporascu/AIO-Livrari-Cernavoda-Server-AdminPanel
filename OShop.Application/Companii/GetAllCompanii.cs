using Microsoft.EntityFrameworkCore;
using OShop.Application.FileManager;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace OShop.Application.Companii
{
    public class GetAllCompanii
    {
        private readonly OnlineShopDbContext _context;

        public GetAllCompanii(OnlineShopDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CompanieVMUI> Do() =>
            _context.Companies.AsNoTracking().AsEnumerable().Select(categ => new CompanieVMUI
            {
                CompanieId = categ.CompanieId,
                Name = categ.Name,
                Photo = categ.Photo,
                TelefonNo = categ.TelefonNo,
                Opening = categ.Opening,
                TransportFees = _context.TransportFees.AsNoTracking().AsEnumerable().Where(fee => fee.CompanieRefId == 1 && fee.TipCompanieRefId == categ.TipCompanieRefId),
                IsActive = categ.IsActive,
                VisibleInApp = categ.VisibleInApp,
                TemporaryClosed = categ.TemporaryClosed,
                TipCompanieRefId = categ.TipCompanieRefId,
                //Image = (categ.Photo == null || categ.Photo == "") ? null : Convert.ToBase64String(getBytes(_fileManager.ImageStream(categ.Photo))),
            });
        public IEnumerable<CompanieVMUI> Do(int tipCompanieId) =>
            _context.Companies.AsNoTracking().AsEnumerable().Where(comp => comp.TipCompanieRefId == tipCompanieId).Select(categ => new CompanieVMUI
            {
                CompanieId = categ.CompanieId,
                Name = categ.Name,
                Photo = categ.Photo,
                TelefonNo = categ.TelefonNo,
                Opening = categ.Opening,
                TemporaryClosed = categ.TemporaryClosed,
                TransportFees = _context.TransportFees.AsNoTracking().AsEnumerable().Where(fee => fee.CompanieRefId == 1 && fee.TipCompanieRefId == categ.TipCompanieRefId),
                IsActive = categ.IsActive,
                VisibleInApp = categ.VisibleInApp,
                TipCompanieRefId = categ.TipCompanieRefId,
                //Image = (categ.Photo == null || categ.Photo == "") ? null : Convert.ToBase64String(getBytes(_fileManager.ImageStream(categ.Photo))),
            });
        private byte[] getBytes(FileStream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return buffer;
        }


    }
    public class CompanieVMUI
    {
        public int CompanieId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Photo { get; set; }
        public string TelefonNo { get; set; }
        public string Image { get; set; }
        public DateTime Opening { get; set; }
        public bool TemporaryClosed { get; set; }
        public bool VisibleInApp { get; set; }
        public bool IsActive { get; set; }
        public int TipCompanieRefId { get; set; }
        public IEnumerable<TransportFee> TransportFees { get; set; }
    }
}
