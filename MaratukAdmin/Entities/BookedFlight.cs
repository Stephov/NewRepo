namespace MaratukAdmin.Entities
{
    public class BookedFlight : BaseDbEntity
    {
        public string OrderNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDay { get; set; }
        public string Email { get; set; }
        public string Passport { get; set; }
        public int AgentId { get; set; }
        public int AgentStatusId { get; set; }
        public DateTime DateOfOrder { get; set; }
        public string ToureTypeId { get; set; }
        public int? HotelId { get; set; }
        public string? TicketNumber { get; set;}
        public int OrderStatusId { get; set; } = 1;
        public double TotalPrice { get; set; }
        public string Rate { get; set; }
        public double TotalPriceAmd { get; set; }
        public int PassengersCount { get; set; }
        public DateTime TourStartDate { get; set; }
        public DateTime? TourEndDate { get; set; }
        public DateTime? DeadLine { get; set; }
        public DateTime PasportExpiryDate { get; set; }
        public double Paid { get; set; } = 0;
        public int MaratukAgentId { get; set; }
        public int MaratukAgentStatusId { get; set; }
        public int GenderId { get; set; }
        public int CountryId { get; set; }
        public double? Dept { get; set; }

        public int StartFlightId { get; set; }
        public int? EndFlightId { get; set; }
        public string? Comment { get; set; } = string.Empty;
    }
}
