namespace MaratukAdmin.Dto.Request
{
    public class AgencyUserCredentialsRequest
    {
        public string AgencyName { get; set; }
        public string FullCompanyName { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public string CompanyLocation { get; set; }
        public string CompanyLegalAddress { get; set; }
        public int Itn { get; set; }
        public string BankAccountNumber { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }

    public class UpdateAgencyUser
    {
        public int Id { get; set; }
        public string AgencyName { get; set; }
        public string FullCompanyName { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public string CompanyLocation { get; set; }
        public string CompanyLegalAddress { get; set; }
        public int Itn { get; set; }
        public string BankAccountNumber { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

    }
}
