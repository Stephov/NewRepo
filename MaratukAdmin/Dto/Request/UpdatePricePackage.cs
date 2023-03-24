using MaratukAdmin.Entities;

namespace MaratukAdmin.Dto.Request
{
    public class UpdatePricePackage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public int CountryID { get; set; }
    }
}
