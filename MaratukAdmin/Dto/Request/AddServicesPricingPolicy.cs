using MaratukAdmin.Entities;

namespace MaratukAdmin.Dto.Request
{
    public class AddServicesPricingPolicy
    {
        public int PriceBlockServicesId { get; set; }
        public int CurrencyId { get; set; }
        public double Netto { get; set; }
        public double Parcent { get; set; }
        public double Bruto { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime SaleDate { get; set; }
        public int AgeFrom { get; set; }
        public int AgeUpTo { get; set; }
        public int CountFrom { get; set; }
        public int CountUpTo { get; set; }
        public bool StopSale { get; set; }

    }
}
