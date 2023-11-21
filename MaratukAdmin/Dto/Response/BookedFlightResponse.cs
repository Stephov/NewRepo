namespace MaratukAdmin.Dto.Response
{
    public class BookedFlightResponse
    { 
        public List<BookedUserInfo> bookedUsers { get; set; }
        public int Id { get; set; }
        public string OrderNumber { get; set; }    
        public DateTime DateOfOrder { get; set; }
        public string ToureTypeId { get; set; }
        public int? HotelId { get; set; }
        public string? TicketNumber { get; set; }
        public int OrderStatusId { get; set; } = 1;
        public double TotalPrice { get; set; }
        public string Rate { get; set; }
        public int AgentId { get; set; }

        public double TotalPriceAmd { get; set; }
        public int PassengersCount { get; set; }
        public DateTime TourStartDate { get; set; }
        public DateTime? TourEndDate { get; set; }
        public DateTime? DeadLine { get; set; }
        public double Paid { get; set; } = 0;
        public int MaratukAgentId { get; set; }
        public string? MaratukAgentName { get; set; }
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public double? Dept { get; set; }
        public int StartFlightId { get; set; }
        public int? EndFlightId { get; set; }
    }
    public class BookedFlightResponseFinal
    {
        public int DeptUSD { get; set; }
        public int DeptEUR { get; set; }

        public List<BookedFlightResponse> bookedFlightResponses { get; set; }
    }

    public class BookedUserInfo
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDay { get; set; }
        public string Email { get; set; }
        public string Passport { get; set; }
        public DateTime PasportExpiryDate { get; set; }
        public string GenderName { get; set; }

    }
}
