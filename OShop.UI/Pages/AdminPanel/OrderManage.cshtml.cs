using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OShop.Application.Companii;
using OShop.Application.Orders;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OShop.UI.Pages.AdminPanel
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class OrderManageModel : PageModel
    {
        private readonly OnlineShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderManageModel(OnlineShopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public IEnumerable<OrderViewModel> Orders { get; set; }
        [BindProperty]
        public List<CompanieVMUI> Companii { get; set; }
        [BindProperty]
        public DateTime FiltruData { get; set; }
        [BindProperty]
        public int FiltruCompanie { get; set; }
        partial class Export
        {
            public int NrComanda { get; set; }
            public string NumeCompanie { get; set; }
            public string Status { get; set; }
            public decimal TotalComanda { get; set; }
            public decimal CostTransport { get; set; }
            public DateTime Data { get; set; }
        }
        partial class ExportOthers
        {
            public int NrComanda { get; set; }
            public string Status { get; set; }
            public decimal TotalComanda { get; set; }
            public DateTime Data { get; set; }
        }
        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            FiltruData = DateTime.Today;
            Companii = new GetAllCompanii(_context).Do().ToList();
            if (user.CompanieRefId > 0)
                Orders = await new GetAllOrders(_context, _userManager).Do(user.CompanieRefId);
            else
                Orders = await new GetAllOrders(_context, _userManager).Do(null, true);
            return Page();
        }
        public async Task<IActionResult> OnPostFilter()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                Companii = new GetAllCompanii(_context).Do().ToList();
                if (user.CompanieRefId > 0)
                    Orders = (await new GetAllOrders(_context, _userManager).Do(user.CompanieRefId)).Where(ord => ord.Created.Month == FiltruData.Month && ord.Created.Day == FiltruData.Day);
                else
                    Orders = (await new GetAllOrders(_context, _userManager).Do(null, true))
                        .Where(ord => ord.Created.Month == FiltruData.Month && ord.Created.Day == FiltruData.Day && (FiltruCompanie > 0 ? ord.CompanieRefId == FiltruCompanie : ord.CompanieRefId > 0));
                return Page();
            }
            return BadRequest();
        }
        public async Task<IActionResult> OnPostExport()
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.GetUserAsync(User);
                Companii = new GetAllCompanii(_context).Do().ToList();
                if (user.CompanieRefId > 0)
                    Orders = (await new GetAllOrders(_context, _userManager).Do(user.CompanieRefId)).Where(ord => ord.Created.Month == FiltruData.Month && ord.Created.Day == FiltruData.Day);
                else
                    Orders = (await new GetAllOrders(_context, _userManager).Do(null, true))
                        .Where(ord => ord.Created.Month == FiltruData.Month && ord.Created.Day == FiltruData.Day && (FiltruCompanie > 0 ? ord.CompanieRefId == FiltruCompanie : ord.CompanieRefId > 0));
                if (user.CompanieRefId > 0)
                {
                    var exportObject = Orders.Select(ord => new ExportOthers
                    {
                        NrComanda = ord.OrderId,
                        Status = ord.Status,
                        TotalComanda = ord.TotalOrdered,
                        Data = ord.Created,
                    }).ToList();
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(ExportOthers));
                    DataTable table = new DataTable();
                    foreach (PropertyDescriptor p in props)
                        table.Columns.Add(p.Name, p.PropertyType);
                    for (int i = 0; i < exportObject.Count(); i++)
                    {
                        DataRow row = table.NewRow();
                        row["NrComanda"] = exportObject[i].NrComanda;
                        row["Status"] = exportObject[i].Status;
                        row["TotalComanda"] = exportObject[i].TotalComanda;
                        row["Data"] = exportObject[i].Data;
                        table.Rows.Add(row);
                    }
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var exportDate = DateTime.UtcNow.AddHours(3);
                        wb.Worksheets.Add(table, $"Export {exportDate.Day}.{exportDate.Month}.{exportDate.Year}");
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Export {exportDate.Day}.{exportDate.Month}.{exportDate.Year}.xlsx");
                        }
                    }
                }
                else
                {
                    var exportObject = Orders.Select(ord => new Export
                    {
                        NrComanda = ord.OrderId,
                        NumeCompanie = Companii.FirstOrDefault(comp => comp.CompanieId == ord.CompanieRefId).Name,
                        Status = ord.Status,
                        TotalComanda = ord.TotalOrdered,
                        CostTransport = ord.TransportFee,
                        Data = ord.Created,
                    }).ToList();
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(Export));
                    DataTable table = new DataTable();
                    foreach (PropertyDescriptor p in props)
                        table.Columns.Add(p.Name, p.PropertyType);
                    for (int i = 0; i < exportObject.Count(); i++)
                    {
                        DataRow row = table.NewRow();
                        row["NrComanda"] = exportObject[i].NrComanda;
                        row["NumeCompanie"] = exportObject[i].NumeCompanie;
                        row["Status"] = exportObject[i].Status;
                        row["TotalComanda"] = exportObject[i].TotalComanda;
                        row["CostTransport"] = exportObject[i].CostTransport;
                        row["Data"] = exportObject[i].Data;
                        table.Rows.Add(row);
                    }
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var exportDate = DateTime.UtcNow.AddHours(3);

                        wb.Worksheets.Add(table, $"Export {exportObject[0].NumeCompanie} - {exportDate.Day}.{exportDate.Month}.{exportDate.Year}");
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Export {exportObject[0].NumeCompanie} - {exportDate.Day}.{exportDate.Month}.{exportDate.Year}.xlsx");
                        }
                    }
                }

            }
            return BadRequest();
        }
    }
}