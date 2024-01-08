namespace MaratukAdmin.Dto.Request
{
    public class UpdateCurrencyRates
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public double OfficialRate { get; set; }
        public double InternaRate { get; set; }
    }
}
