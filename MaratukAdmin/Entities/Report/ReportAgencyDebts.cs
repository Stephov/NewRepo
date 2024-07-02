using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Entities.Report
{
    public class ReportAgencyDebts
    {
        public DateTime? FlightDate { get; set; }
        public string? FlightNumber { get; set; }
        public List<AgencyDebtDetails>? AgencyDebts { get; set; }
    }

    public class AgencyDebtDetails
    {
        public string? AgencyName { get; set; }
        public string? Currency { get; set; }
        public double? Debt { get; set; }
        public double? Paid { get; set; }
        public double? TotalAmount { get; set; }
        public double? PaidAMD { get; set; }
    }
}
