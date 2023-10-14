﻿namespace MaratukAdmin.Entities
{
    public class SearchResultFunction
    {
        public int FlightId { get; set; }
        public int AgeFrom { get; set; }
        public int AgeUpTo { get; set; }
        public double Bruto { get; set; }
        public int FlightTimeMinute { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DestinationAirportCode { get; set; }
       
    }

    public class SearchResultFunctionTwoWay 
    {
        public int FlightId { get; set; }
        public int AgeFrom { get; set; }
        public int AgeUpTo { get; set; }
        public double Bruto { get; set; }
        public int FlightTimeMinute { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DestinationAirportCode { get; set; }
    }
}
