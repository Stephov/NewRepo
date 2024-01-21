namespace MaratukAdmin.Dto.Response.Sansejour
{
    public class BookedInfoFlightPartGroupedResponse
    {
        public List<GroupedFlightInfo>? groupedFlightInfo { get; set; }
        public double SummaTotal { get; set; }
    }

    public class GroupedFlightInfo
    {
        public DateTime TourStartDate { get; set; }
        public string? MaratukAgentStatus { get; set; }
        public int StatusCount { get; set; }
        public double Summa { get; set; }
    }

}
