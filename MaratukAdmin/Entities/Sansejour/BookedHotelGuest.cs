namespace MaratukAdmin.Entities.Sansejour
{
    public class BookedHotelGuest : BaseDbEntity
    {
        public string OrderNumber { get; set; }
        public int IsAdult { get; set; }
        public int GuestType { get; set; }
        public int GenderId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDay { get; set; }
        public string Email { get; set; }
        public string Passport { get; set; }
        public DateTime PassportExpiryDate { get; set; }
    }
}
