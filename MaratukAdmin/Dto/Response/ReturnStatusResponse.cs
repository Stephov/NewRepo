namespace MaratukAdmin.Dto.Response
{
    public class ReturnStatusResponse
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }

        public ReturnStatusResponse()
        {
            StatusCode = 0;
            StatusMessage = string.Empty;
        }
    }
}
