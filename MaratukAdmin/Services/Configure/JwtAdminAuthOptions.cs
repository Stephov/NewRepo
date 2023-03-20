namespace MaratukAdmin.Services.Configure
{
    public class JwtAdminAuthOptions
    {
        public static JwtAdminAuthOptions Default = new JwtAdminAuthOptions
        {
            SigningKey = null,
            ValidateActor = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidAudiences = string.Empty,
            ValidIssuers = string.Empty
        };

        public bool ValidateIssuerSigningKey { get; set; }

        public string SigningKey { get; set; }

        public bool ValidateIssuer { get; set; }

        public string ValidIssuers { get; set; }

        public bool ValidateAudience { get; set; }

        public string ValidAudiences { get; set; }

        public bool ValidateLifetime { get; set; }

        public bool ValidateActor { get; set; }
    }
}

