using System.Xml.Serialization;

namespace MaratukAdmin.Entities.Sansejour
{
    public class SyncSejourAccomodationType : BaseDbEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDescribed { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
