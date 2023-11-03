namespace MaratukAdmin.Entities.Sansejour
{
    public class RoomCategory : BaseDbEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int HotelId { get; set; }

        public Hotel Hotel { get; set; }
    }
}
