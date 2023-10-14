using MaratukAdmin.Entities;

namespace MaratukAdmin.Dto.Request
{
    public class SearchFlightResult
    {
        public int FlightOneId { get; set; }
        public int? FlightTwoId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public int Infant { get; set; }
    }
}
