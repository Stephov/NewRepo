namespace MaratukAdmin.Entities.Global
{
    public class CurrencyRates:BaseDbEntity
    {
        public int CurrencyId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CodeIso { get; set; }
        public double OfficialRate { get; set; }
        public double InternaRate { get; set; }
    }
}
