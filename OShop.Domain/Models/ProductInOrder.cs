namespace OShop.Domain.Models
{
    public class ProductInOrder
    {
        public int OrderRefId { get; set; }
        public Order Orders { get; set; }


        public int ProductRefId { get; set; }
        public Product Products { get; set; }

        public int UsedQuantity { get; set; }
        public string ClientComments { get; set; }
    }
}
