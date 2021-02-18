namespace OShop.Domain.Models
{
    public class CartItems
    {
        public int CartRefId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

        public int ProductRefId { get; set; }
        public Product Products { get; set; }

        public int Quantity { get; set; }
    }
}
