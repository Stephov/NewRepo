﻿namespace MaratukAdmin.Dto.Response
{
    public class ScheduleInfoResponse
    {
        public DateTime FlightStartDate { get; set; }
        public DateTime FlightEndDate { get; set; }
        public string[] DayOfWeek { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}

