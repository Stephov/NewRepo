﻿using MaratukAdmin.Entities;

namespace MaratukAdmin.Dto.Request
{
    public class ScheduleRequest
    {
        public DateTime FlightStartDate { get; set; }
        public DateTime FlightEndDate { get; set; }
        public int[] DayOfWeek { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
