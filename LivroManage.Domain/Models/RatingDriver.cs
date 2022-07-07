namespace LivroManage.Domain.Models
{
    public class RatingDriver
    {

        public string DriverRefId { get; set; }
        public ApplicationUser Driver { get; set; }
        public int OrderRefId { get; set; }
        public Order Orderz { get; set; }
        public int Rating { get; set; }
    }
}
