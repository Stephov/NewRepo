using MaratukAdmin.Utils;
using System.ComponentModel;

namespace MaratukAdmin.Dto.Request
{
    public class PayForBookedFlightAndHotelRequest
    {
        public string OrderNumber { get; set; }
        public string PaymentNumber { get; set; }
        public double? SumAMD { get; set; }
        public string? Currency { get; set; }
        public double? SumInCurrency { get; set; }
        public double? PaidAMD { get; set; }
        public double? PaidInCurrency { get; set; }
        [DefaultValue("D")]
        public string? PaymentType { get; set; } = Enums.enumBookPaymentTypes.D.ToString();
        
        [DefaultValue((int)Enums.enumBookPaymentStatuses.InProcess)] 
        //public int PaymentStatus { get; set; } = (int)Enums.enumBookPaymentStatuses.InProcess;
        //public string PaymentStatus { get; set; } = Enums.enumBookPaymentStatuses.InProcess.ToString();
        public int PaymentStatus { get; set; } = (int)Enums.enumBookPaymentStatuses.InProcess;
        
        public int? PayerId { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
