using System.Security.Principal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MaratukAdmin.Entities
{
    public class AgencyUser
    {
        public int Id { get; set; }
        public string? AgencyName { get; set; }
        public string? FullCompanyName { get; set; }
        //todo add user lastname first namZ
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public string? CompanyLocation { get; set; }
        public string? CompanyLegalAddress { get; set; }
        public int Itn { get; set; }
        public string? BankAccountNumber { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string HashId { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public bool IsActivated { get; set; }
        public int IsAproved { get; set; }
        public string Role { get; set; }
    }
}
