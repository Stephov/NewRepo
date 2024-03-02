using MaratukAdmin.Entities;

namespace MaratukAdmin.Dto.Request
{
    public class AddBookedFlight
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDay { get; set; }
        public string Email { get; set; }
        public string Passport { get; set; }
        public DateTime PasportExpiryDate { get; set; }

        public int AgentId { get; set; }

        public double TotalPrice { get; set; }
        public String Rate { get; set; }
        public double TotalPriceAmd { get; set; }
        public int PassengersCount { get; set; }
        public DateTime TourStartDate { get; set; }
        public DateTime? TourEndDate { get; set; }

        public int MaratukFlightAgentId { get; set; }
        public int? MaratukHotelAgentId { get; set; }
        public int CountryId { get; set; }
        public int StartFlightId { get; set; }
        public int? EndFlightId { get; set; }
        public int GenderId { get; set; }
        public int PassengerTypeId { get; set; }

    }
}
