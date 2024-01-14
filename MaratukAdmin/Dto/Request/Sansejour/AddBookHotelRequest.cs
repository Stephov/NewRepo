using MaratukAdmin.Entities;

namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class AddBookHotelRequest
    {
        public List<HotelGuest> Guests { get; set; }
        public string HotelCode { get; set; }
        public int RoomId { get; set; }
        public string RoomCode { get; set; }

        public int AgentId { get; set; }

        public double TotalPrice { get; set; }
        public String Rate { get; set; }
        public double TotalPriceAmd { get; set; }
        public int GuestsCount { get; set; }
        public DateTime TourStartDate { get; set; }
        public DateTime? TourEndDate { get; set; }

        public int MaratukAgentId { get; set; }
        public int CountryId { get; set; }

        public DateTime? DeadLine { get; set; }
        public double Paid { get; set; } = 0;
        public double? Dept { get; set; }

    }
}
