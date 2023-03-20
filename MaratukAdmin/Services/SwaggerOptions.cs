namespace MaratukAdmin.Services
{
    public class SwaggerOptions
    {
        public bool Enabled { get; set; }
        public string JsonRoutePrefix { get; set; }
        public string ApiBasePath { get; set; }
        public string ApiBaseScheme { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public static SwaggerOptions Default = new SwaggerOptions
        {
            Enabled = false,
            JsonRoutePrefix = "",
            ApiBasePath = "/",
            ApiBaseScheme = "http",
            Title = "",
            Description = ""
        };
    }
}
