using Microsoft.EntityFrameworkCore;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivroManage.Application
{
    public class OrderInfoOperations : EntityOperation<OrderInfo>
    {
        private readonly OnlineShopDbContext _context;
        public OrderInfoOperations(OnlineShopDbContext context)
        {
            _context = context;
        }

        public override async Task Create(OrderInfo model)
        {
            _context.OrdersInfos.Add(model);
            await _context.SaveChangesAsync();
        }

        public override Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override OrderInfo Get(int? id)
        {
            return _context.OrdersInfos.AsNoTracking().FirstOrDefault(orderinfo => orderinfo.OrderRefId == id);
        }

        public override IEnumerable<OrderInfo> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<OrderInfo> GetAll(int canal)
        {
            throw new NotImplementedException();
        }

        public override async Task Update(OrderInfo model)
        {
            _context.OrdersInfos.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
