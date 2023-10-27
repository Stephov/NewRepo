namespace MaratukAdmin.Dto.Response
{
    public class AgencyAuthenticationResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public string name { get; set; }
        public string Role { get; set; }
        public int Itn { get; set; } 
        public  int AgentId { get; set; }
    }
}
