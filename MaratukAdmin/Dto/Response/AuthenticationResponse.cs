namespace MaratukAdmin.Dto.Response
{
    public class AuthenticationResponse
    {
        public int Id { get; set; }//TODO
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public string name { get; set; }
        public string Role { get; set; }
    }
}
