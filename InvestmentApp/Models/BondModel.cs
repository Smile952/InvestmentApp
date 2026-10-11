namespace InvestmentApp.Models
{
    public class BondModel
    {
        public string Uid { get; set; }
        public string Name { get; set; }
        public Decimal Nominal { get; set; }
        public Decimal Price { get; set; }
        public Decimal CouponSum { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
