namespace MaratukAdmin.Dto.Request.Sansejour
{
    public class BookedInfoFlightPartRequest
    {
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int AgentId { get; set; }
        public int MaratukAgentId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int StartFlightId { get; set; }
        public bool GroupByStartFlightId { get; set; } = false;
        //public int EndFlightId { get; set; }
    }
}
