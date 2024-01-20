using MaratukAdmin.Entities.Global;
using MaratukAdmin.Entities.Sansejour;

namespace MaratukAdmin.Dto.Response.Sansejour
{
    public class BookedInfoFlightPartResponse
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string OrderNumber { get; set; }
        public DateTime TourStartDate { get; set; }
        public DateTime TourEndDate { get; set; }
        public string? AgentStatus { get; set; }
        public string? MaratukAgentStatus { get; set; }
        public string? Paid { get; set; }
        public double Summa { get; set; }
        public int StartFlightId { get; set; }
        public int EndFlightId { get; set; }
    }
}
