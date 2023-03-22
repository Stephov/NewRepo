namespace MaratukAdmin.Dto.Response
{
    public class AuthenticationResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public string name { get; set; }
    }
}
