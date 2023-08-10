namespace MaratukAdmin.Dto.Request
{
    public class AgencyAgentCredentialsRequest
    {
        public int AgencyItn { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
