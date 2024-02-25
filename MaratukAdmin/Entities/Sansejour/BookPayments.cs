namespace MaratukAdmin.Entities.Sansejour
{
    public class BookPayments : BaseDbEntity
    {
        public string OrderNumber { get; set; }
        public double Sum { get; set; }
        public string Currency { get; set; }
        public int PaymentType { get; set; }
        public int PayerId { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
