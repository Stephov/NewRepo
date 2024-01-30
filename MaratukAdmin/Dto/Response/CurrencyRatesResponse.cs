using MaratukAdmin.Entities;

namespace MaratukAdmin.Dto.Response
{
    public class CurrencyRatesResponse 
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CodeIso { get; set; }
        public double OfficialRate { get; set; }
        public double InternaRate { get; set; }
    }
}
