namespace MaratukAdmin.Dto.Response
{
    public class FlightCalendarResponse
    {
        public List<CalendarSchedule> FligthDaysInfo { get; set; }
        public List<CalendarSchedule> ReturnedDaysInfo { get; set; }

    }

    public class CalendarSchedule
    {
        public string Days { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
