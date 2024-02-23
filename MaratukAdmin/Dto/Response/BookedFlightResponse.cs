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
        public string HotelName { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public string RoomCode { get; set; } = string.Empty;
        public string BoardDesc { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public string? TicketNumber { get; set; }
        public int OrderStatusId { get; set; } = 1;
        public double TotalPrice { get; set; }
        public string Rate { get; set; }
        public int AgentId { get; set; }
        public int BookStatusForClient { get; set; }
        public string BookStatusForClientName { get; set; }
        public string AgentName { get; set; }

        public double TotalPriceAmd { get; set; }
        public int PassengersCount { get; set; }
        public DateTime TourStartDate { get; set; }
        public DateTime? TourEndDate { get; set; }
        public DateTime? DeadLine { get; set; }
        public double Paid { get; set; } = 0;
        public int MaratukAgentId { get; set; }
        public int BookStatusForMaratuk { get; set; }
        public string BookStatusForMaratukName { get; set; }
        public string? MaratukAgentName { get; set; }
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public double? Dept { get; set; }
        public int StartFlightId { get; set; }
        public int? EndFlightId { get; set; }
        public string? Comments { get; set; } = string.Empty;
    }

    public class BookedFlightResponseForMaratuk
    {
        public List<BookedUserInfoForMaratuk> bookedUsers { get; set; }
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime DateOfOrder { get; set; }
        public string ToureTypeId { get; set; }
        public int? HotelId { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public string BoardDesc { get; set; } = string.Empty;
        public string RoomCode { get; set; } = string.Empty;
        public string? TicketNumber { get; set; }
        public int OrderStatusId { get; set; } = 1;
        public double TotalPrice { get; set; }
        public string Rate { get; set; }
        public int AgentId { get; set; }
        public int BookStatusForClient { get; set; }
        public string BookStatusForClientName { get; set; }
        public string AgentName { get; set; }

        public double TotalPriceAmd { get; set; }
        public int PassengersCount { get; set; }
        public DateTime TourStartDate { get; set; }
        public DateTime? TourEndDate { get; set; }
        public DateTime? DeadLine { get; set; }
        public double Paid { get; set; } = 0;
        public int MaratukAgentId { get; set; }
        public int MaratukHotelAgentId { get; set; }
        public int BookStatusForMaratuk { get; set; }
        public string BookStatusForMaratukName { get; set; }
        public string? MaratukAgentName { get; set; }
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public double? Dept { get; set; }
        public int StartFlightId { get; set; }
        public int? EndFlightId { get; set; }
        public string? Comments { get; set; } = string.Empty;
    }
    public class BookedFlightResponseFinal
    {
        public int DeptUSD { get; set; }
        public int DeptEUR { get; set; }

        public List<BookedFlightResponse> bookedFlightResponses { get; set; }
    }

    public class BookedFlightResponseFinalForMaratukAgent
    {
        public int DeptUSD { get; set; }
        public int DeptEUR { get; set; }
        public int TotalPages { get; set; }
        public List<BookedFlightResponseForMaratuk> bookedFlightResponses { get; set; }
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

    public class BookedUserInfoForMaratuk
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDay { get; set; }
        public string Email { get; set; }
        public string Passport { get; set; }
        public DateTime PasportExpiryDate { get; set; }
        public string GenderName { get; set; }

    }

    public class BookedUserInfoForMaratukRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDay { get; set; }
        public string Email { get; set; }
        public string Passport { get; set; }
        public DateTime PasportExpiryDate { get; set; }
        public int GenderId { get; set; }

    }
}
