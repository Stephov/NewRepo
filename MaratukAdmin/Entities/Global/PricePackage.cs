namespace MaratukAdmin.Entities.Global
{
    public class PricePackage : BaseDbEntity
    {
        public string Name { get; set; }
        public string NameEng { get; set; }
        public int CountryId { get; set; }//create aviakompania
    }
}
