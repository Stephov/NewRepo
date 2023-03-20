namespace MaratukAdmin.Entities.Global
{
    public class City : BaseDbEntity
    {
        public City()
        {
            Airports = new List<Airport>();
        }

        public string Name { get; set; }
        public string NameEng { get; set; }
        public int CountryId { get; set; }

        public virtual List<Airport> Airports { get; set; }

    }
}
