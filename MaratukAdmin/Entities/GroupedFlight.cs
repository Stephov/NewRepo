﻿namespace MaratukAdmin.Entities
{
    public class GroupedFlight
    {
        public int Id { get; set; }
        public string DepartureCountryName { get; set; }
        public int DepartureCountryId { get; set; }
        public string DepartureCityName { get; set; }
        public int DepartureCityId { get; set; }
        public string DepartureAirportName { get; set; }
        public string DepartureAirportCode { get; set; }
        public List<Destination> Destination { get; set; }
    }

    public class Destination
    {
        public int FlightId { get; set; }
        public int PriceBlockId { get; set; }
        public string DestinationCountryName { get; set; }
        public int DestinationCountryId { get; set; }
        public string DestinationCityName { get; set; }
        public int DestinationCityId { get; set; }
        public string DestinationAirportName { get; set; }
        public string DestinationAirportCode { get; set; }
        public List<DateInfo> Date { get; set; }
    }

    public class DateInfo
    {
        public int FligthId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DayOfWeek { get; set; }
        public double? Price { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
