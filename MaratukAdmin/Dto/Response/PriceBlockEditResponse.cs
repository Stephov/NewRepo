using MaratukAdmin.Dto.Request;

namespace MaratukAdmin.Dto.Response
{
    public class PriceBlockEditResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PricelBlockTypeId { get; set; }
        public int PricePackageId { get; set; }
        public int ServiceClassId { get; set; }
        public int SeasonId { get; set; }
        public int PartnerId { get; set; }
        public int CurrencyId { get; set; }
        public string Comments { get; set; }
        public int TarifId { get; set; }
    }


}

