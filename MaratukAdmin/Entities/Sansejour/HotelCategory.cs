namespace MaratukAdmin.Entities.Sansejour
{
    public class HotelCategory : BaseDbEntity
    {
        //public HotelCategory()
        //{
        //    Hotels = new List<Hotel>();
        //}
        public string Name { get; set; }
        public float StarCount { get; set; }

        //public virtual List<Hotel> Hotels { get; set; }
    }
}
