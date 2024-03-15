namespace MaratukAdmin.Entities.Sansejour
{
    public class BookPayment : BaseDbEntity
    {
        public string OrderNumber { get; set; }
        public string PaymentNumber { get; set; }
        public double SumAMD { get; set; }
        public string Currency { get; set; }
        public double SumInCurrency { get; set; }
        public double PaidAMD { get; set; }
        public double PaidInCurrency { get; set; }
        public string PaymentType { get; set; }
        public int PaymentStatus { get; set; }
        public int PayerId { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
