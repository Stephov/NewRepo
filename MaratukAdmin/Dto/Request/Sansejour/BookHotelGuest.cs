using MaratukAdmin.Entities;

namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class BookHotelGuest
    {
        public List<HotelGuest> Guests { get; set; }
        public string HotelCode { get; set; }
        public string RoomCode { get; set; }

        public int AgentId { get; set; }

        public double TotalPrice { get; set; }
        public String Rate { get; set; }
        public double TotalPriceAmd { get; set; }
        public int PassengersCount { get; set; }
        public DateTime TourStartDate { get; set; }
        public DateTime? TourEndDate { get; set; }

        public int MaratukAgentId { get; set; }
        public int CountryId { get; set; }
        public int StartFlightId { get; set; }
        public int? EndFlightId { get; set; }
    }
}
