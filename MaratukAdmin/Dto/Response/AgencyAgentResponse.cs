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
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string FullName { get; set; }
        public string email { get; set; }
        public bool IsApproved { get; set; }

    }
}
