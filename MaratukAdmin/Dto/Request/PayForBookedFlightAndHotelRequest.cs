namespace MaratukAdmin.Dto.Request
{
    public class PayForBookedFlightAndHotelRequest
    {
        public string OrderNumber { get; set; }
        public double Sum { get; set; }
        public string Currency { get; set; }
        public int PaymentType { get; set; }
        public int PayerId { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
