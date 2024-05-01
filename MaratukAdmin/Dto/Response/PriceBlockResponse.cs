namespace MaratukAdmin.Dto.Response
{
    public class PriceBlockResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PriceBlockType { get; set; }
        public string PricePackageId { get; set; }
        public string ServiceClassId { get; set; }
        public string PriceBlockStateId { get; set; }
        public string TripTypeId { get; set; }
        public int TripDays { get; set; }
        public bool OnlyFligth { get; set; }
        public decimal Comission { get; set; }

    }
}
