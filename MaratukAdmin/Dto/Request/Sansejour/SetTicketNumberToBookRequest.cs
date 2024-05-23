namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class SetTicketNumberToBookRequest
    {
        public string OrderNumber { get; set; }
        public List<Ticket> Tikets { get; set; }
    }

    public class Ticket
    {
        public int BookId { get; set; }
        public string TicketNumber { get; set; }

        public Ticket()
        {
            BookId = 0;
            TicketNumber = string.Empty;
        }
    }

    
    
}
