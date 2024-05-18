
namespace MaratukAdmin.Entities.Report
{
    public class ReportTouristInfoHotel : IReportTouristInfo
    {
        public DateTime? Date { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public string? HotelManager { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string? Tiitle { get; set; }
        public double? Summa { get; set; }
        public string? Currency { get; set; }
        public double? CurrencyRate { get; set; }
    }
}
