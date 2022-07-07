using LivroManage.Domain.Models;
using System.Collections.Generic;

namespace LivroManage.Application.ViewModels
{
    public class OrderVM : Order
    {
        public OrderVM(Order order)
        {
            OrderId = order.OrderId;
            Status = order.Status;
            CustomerId = order.CustomerId;
            TotalOrdered = order.TotalOrdered;
            TransportFee = order.TransportFee;
            Created = order.Created;
            IsOrderPayed = order.IsOrderPayed;
            TelephoneOrdered = order.TelephoneOrdered;
            PaymentMethod = order.PaymentMethod;
            Comments = order.Comments;
            UserLocationId = order.UserLocationId;
            EstimatedTime = order.EstimatedTime;
            HasUserConfirmedET = order.HasUserConfirmedET;
            CompanieGaveRating = order.CompanieGaveRating;
            ClientGaveRatingDriver = order.ClientGaveRatingDriver;
            ClientGaveRatingCompanie = order.ClientGaveRatingCompanie;
            DriverGaveRating = order.DriverGaveRating;
            CompanieRefId = order.CompanieRefId;
            DriverRefId = order.DriverRefId;

        }
        public UserLocations Location { get; set; }
        public IEnumerable<ProductInOrder> ProductsInOrder { get; set; }
        public OrderInfo OrderInfo { get; set; }
        public Companie Companie { get; set; }
        public int? RatingClientDeLaSofer { get; set; }
        public int? RatingClientDeLaCompanie { get; set; }
        public int? RatingDriver { get; set; }
        public int? RatingCompanie { get; set; }
    }
}
