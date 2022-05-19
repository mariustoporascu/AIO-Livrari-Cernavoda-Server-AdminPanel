namespace OShop.Domain.Models
{
    public class RatingRestaurant
    {

        public int RestaurantRefId { get; set; }
        public Restaurant Restaurantz { get; set; }
        public int OrderRefId { get; set; }
        public Order Orderz { get; set; }
        public int Rating { get; set; }
    }
}
