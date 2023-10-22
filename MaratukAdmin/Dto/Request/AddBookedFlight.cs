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
        public int AgentId { get; set; }

        public double TotalPrice { get; set; }
        public double Rate { get; set; }
        public double TotalPriceAmd { get; set; }
        public int PassengersCount { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }

        public int MaratukAgentId { get; set; }
        public int CountryId { get; set; }
        public int FlightId { get; set; }

    }
}
