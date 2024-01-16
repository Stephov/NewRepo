namespace MaratukAdmin.Entities.Sansejour
{
    public class BookedHotel : BaseDbEntity
    {
        public string OrderNumber { get; set; }
        //public int AgentId { get; set; }
        //public DateTime DateOfOrder { get; set; }
        public string ToureTypeId { get; set; }
        public int? HotelId { get; set; }
        public string? HotelCode { get; set; }
        public int SejourRateId { get; set; }

        public string? Room { get; set; }
        public string? RoomCode { get; set; }
        public int OrderStatusId { get; set; } = 1;
        //public string Rate { get; set; }
        public double TotalPrice { get; set; }
        public double TotalPriceAmd { get; set; }
        public int GuestsCount { get; set; }
        public DateTime? DeadLine { get; set; }
        public double Paid { get; set; } = 0;
        //public int MaratukAgentId { get; set; }
        public int CountryId { get; set; }
        public double? Dept { get; set; }
        public DateTime TourStartDate { get; set; }
        public DateTime? TourEndDate { get; set; }


    }
}
