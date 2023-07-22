namespace MaratukAdmin.Models
{
    public class PricePaskageCountry
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public List<PricePackageCity> Cityes { get; set; }

    }
}
