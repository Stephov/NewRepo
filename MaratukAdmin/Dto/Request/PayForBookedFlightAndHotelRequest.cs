using MaratukAdmin.Utils;
using System.ComponentModel;

namespace MaratukAdmin.Dto.Request
{
    public class PayForBookedFlightAndHotelRequest
    {
        public string OrderNumber { get; set; }
        public string PaymentNumber { get; set; }
        public double SumAMD { get; set; }
        public string Currency { get; set; }
        public double SumInCurrency { get; set; }
        //[JsonConverter(typeof(StringEnumConverter))]
        [DefaultValue("D")]
        public string PaymentType { get; set; }
        [DefaultValue(Enums.enumBookPaymentStatuses.InProcess)]
        public int PaymentStatus { get; set; }
        public int PayerId { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
