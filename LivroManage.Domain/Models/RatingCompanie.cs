namespace LivroManage.Domain.Models
{
    public class RatingCompanie
    {

        public int CompanieRefId { get; set; }
        public Companie Companies { get; set; }
        public int OrderRefId { get; set; }
        public Order Orders { get; set; }
        public int Rating { get; set; }
    }
}
