using MaratukAdmin.Entities;

namespace MaratukAdmin.Dto.Request
{
    public class AddPriceBlockRequest
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
        public int PriceBlockStateId { get; set; }

    }
}
