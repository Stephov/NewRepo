namespace MaratukAdmin.Dto.Response.Sansejour
{
    public class BookedInfoFlightPartGroupedResponse
    {
        public DateTime TourStartDate { get; set; }
        public string? MaratukAgentStatus { get; set; }
        public int StatusCount { get; set; }
        public double Summa { get; set; }
        public double SummaTotal { get; set; }
    }

}
