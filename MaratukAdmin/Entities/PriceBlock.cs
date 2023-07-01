namespace MaratukAdmin.Entities
{
    public class PriceBlock : BaseDbEntity
    {
        public string Name { get; set; }
        public int PricelBlockTypeId { get; set; }
        public int PricePackageId { get; set; }
        public int ServiceClassId { get; set; }
        public int SeasonId { get; set; }
        public int PartnerId { get; set; }
        public int CurrencyId { get; set; }
        public string Comments { get; set; }
        public int TarifId { get; set; }
        public ICollection<PriceBlockServices> Services { get; set; }
    }
}
