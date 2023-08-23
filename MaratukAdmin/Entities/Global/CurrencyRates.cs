namespace MaratukAdmin.Entities.Global
{
    public class CurrencyRates:BaseDbEntity
    {
        public int CurrencyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double OfficialRate { get; set; }
        public double InternaRate { get; set; }
    }
}
