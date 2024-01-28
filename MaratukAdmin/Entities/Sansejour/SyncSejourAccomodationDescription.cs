namespace MaratukAdmin.Entities.Sansejour
{
    public class SyncSejourAccomodationDescription : BaseDbEntity
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public double? ChMinAge { get; set; }
        public double? ChMaxAge { get; set; }
        //public int? ChildAge { get; set; }
        
    }
}
