namespace MaratukAdmin.Entities.Global
{
    public class Airport : BaseDbEntity
    {
        public string Name { get; set; }
        public int CityId { get; set; }
        public string Code { get; set; }
    }
}
