namespace MaratukAdmin.Dto.Response
{
    public class PriceBlockServiceEditResponse
    {
        public int Id { get; set; }
        public int DepartureCountryId { get; set; }
        public int DepartureCityId { get; set; }
        public int DestinationCountryId { get; set; }
        public int DestinationCityId { get; set; }
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
    }
}

