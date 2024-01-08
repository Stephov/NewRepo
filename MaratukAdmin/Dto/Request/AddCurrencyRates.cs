using MaratukAdmin.Entities;

namespace MaratukAdmin.Dto.Request
{
    public class AddCurrencyRates
    {
        public int CurrencyId { get; set; }
        public double OfficialRate { get; set; }
        public double InternaRate { get; set; }
    }
}
