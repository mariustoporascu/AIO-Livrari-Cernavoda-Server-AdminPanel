using Microsoft.EntityFrameworkCore;
using LivroManage.Database;
using LivroManage.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivroManage.Application
{
    public class ProductInOrderOperations : EntityOperation<ProductInOrder>
    {
        private readonly OnlineShopDbContext _context;
        public ProductInOrderOperations(OnlineShopDbContext context)
        {
            _context = context;
        }

        public override Task Create(ProductInOrder model)
        {
            throw new NotImplementedException();
        }
        public async Task CreateList(List<ProductInOrder> model)
        {
            foreach (var item in model)
            {
                _context.ProductInOrders.Add(item);
            }

            await _context.SaveChangesAsync();
        }

        public override Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int orderId, int productId)
        {
            var productInOrder = new ProductInOrder
            {
                OrderRefId = orderId,
                ProductRefId = productId,
            };
            _context.ProductInOrders.Remove(productInOrder);
            await _context.SaveChangesAsync();
        }

        public override ProductInOrder Get(int? id)
        {
            throw new NotImplementedException();
        }
        public ProductInOrder Get(int orderId, int productId)
        {
            return _context.ProductInOrders.AsNoTracking().FirstOrDefault(productInOrder => productInOrder.OrderRefId == orderId && productInOrder.ProductRefId == productId);
        }
        public override IEnumerable<ProductInOrder> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ProductInOrder> GetAll(int canal)
        {
            return _context.ProductInOrders.AsNoTracking().AsEnumerable().Where(productInOrder => productInOrder.OrderRefId == canal);
        }

        public override async Task Update(ProductInOrder model)
        {
            _context.ProductInOrders.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
