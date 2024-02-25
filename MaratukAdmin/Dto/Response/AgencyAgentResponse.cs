namespace MaratukAdmin.Dto.Response
{
    public class AgencyAgentResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }

    public class AgencyAgentResponseForAcc
    {
        public int Id { get; set; }
        public string AgencyName { get; set; }
        public string FullCompanyName { get; set; }
        public string CompanyLocation { get; set; }
        public string CompanyLegalAddress { get; set; }
        public int Itn { get; set; }
        public string BankAccountNumber { get; set; }
        public string Country { get; set; }
        public int CountryId { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string FullName { get; set; }
        public string email { get; set; }
        public int IsApproved { get; set; }
        public string IsApprovStatusName { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? RejectedDate { get; set; }

    }
}
