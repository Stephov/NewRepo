﻿namespace MaratukAdmin.Entities.Sansejour
{
    public class BookedHotel : BaseDbEntity
    {
        public string OrderNumber { get; set; }
        //public int AgentId { get; set; }
        //public DateTime DateOfOrder { get; set; }
        public string ToureTypeId { get; set; }
        public int? HotelId { get; set; }
        public string? HotelCode { get; set; }
        public string? Board { get; set; }
        public string? BoardDesc { get; set; }
        public int SejourRateId { get; set; }

        public string? Room { get; set; }
        public string? RoomCode { get; set; }
        public int OrderStatusId { get; set; } = 1;
        //public string Rate { get; set; }
        public double? Price { get; set; }
        public double HotelTotalPrice { get; set; }
        public double HotelTotalPriceAmd { get; set; }
        public int GuestsCount { get; set; }
        public DateTime? DeadLine { get; set; }
        public double Paid { get; set; } = 0;
        //public int MaratukAgentId { get; set; }
        public int CountryId { get; set; }
        public double? Dept { get; set; }
        public DateTime AccomodationStartDate { get; set; }
        public DateTime? AccomodationEndDate { get; set; }
        public int? AccomodationDaysCount { get; set; }
        public bool? LateCheckout { get; set; }
        public int HotelAgentId { get; set; }
        public int BookStatusForClient { get; set; }
        public int MaratukHotelAgentId { get; set; }
        public int BookStatusForMaratuk { get; set; }
    }
}
