namespace MaratukAdmin.Entities.Sansejour
{
    public class BookInvoiceData : BaseDbEntity
    {
        public string OrderNumber { get; set; }
        public string? InvoiceOption { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PassportType { get; set; }
        public string? PassportNumber { get; set; }
        public string? Citizenship { get; set; }
        public string? ResidentialAddress { get; set; }
    }
}
