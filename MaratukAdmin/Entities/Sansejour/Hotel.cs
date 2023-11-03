namespace MaratukAdmin.Entities.Sansejour
{
    public class Hotel : BaseDbEntity
    {

        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public int? HotelCategoryId { get; set; }

        //public HotelCategory HotelCategory { get; set; }
    }
}
