using OShop.Application.OrderInfos;
using OShop.Application.ProductInOrders;
using OShop.Database;
using OShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace OShop.Application.Orders
{
    public class CreateOrder
    {
        private readonly OnlineShopDbContext _context;

        public CreateOrder(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Do(Order vm)
        {

            _context.Orders.Add(vm);
            await _context.SaveChangesAsync();
            return vm.OrderId;
        }
    }
    public class OrderViewModel
    {

        public int OrderId { get; set; }

        [Required]
        public string Status { get; set; }

        [Range(0.01, 1000000.0)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalOrdered { get; set; }
        public decimal TransportFee { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsOrderPayed { get; set; }
        public string CustomerId { get; set; }
        public int CompanieRefId { get; set; }
        public string Comments { get; set; }
        public string EstimatedTime { get; set; }
        public bool? HasUserConfirmedET { get; set; }
        public bool TelephoneOrdered { get; set; }
        public UserLocation Location { get; set; }
        public int UserLocationId { get; set; }

        public bool CompanieGaveRating { get; set; } = false;
        public bool ClientGaveRatingDriver { get; set; } = false;
        public bool ClientGaveRatingCompanie { get; set; } = false;
        public bool DriverGaveRating { get; set; } = false;
        [DataType(DataType.DateTime)]
        public DateTime StartDelivery { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime FinishDelivery { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }
        public IEnumerable<ProductInOrder> ProductsInOrder { get; set; }
        public OrderInfo OrderInfo { get; set; }
        public Companie Companie { get; set; }
        public string DriverRefId { get; set; }
        public Driver Driver { get; set; }
        public int RatingClientDeLaSofer { get; set; }
        public int RatingClientDeLaCompanie { get; set; }
        public int RatingDriver { get; set; }
        public int RatingCompanie { get; set; }
    }
    public class Driver
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelefonNo { get; set; }

    }
    public class UserLocation
    {
        public double CoordX { get; set; }
        public double CoordY { get; set; }
    }
}
