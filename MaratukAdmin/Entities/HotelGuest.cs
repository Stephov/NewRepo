namespace MaratukAdmin.Entities
{
    public class HotelGuest
    {

        public int GenderId { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public DateTime BirthDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Passport { get; set; }
        public DateTime PasportExpiryDate { get; set; }
    }
}
