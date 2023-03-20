using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MaratukAdmin.Services.Configure
{
    public static class JwtAdminAuthConfiguration
    {
        public static IServiceCollection AddLocalJWTAdminAuth(this IServiceCollection services, IConfiguration configuration)
        {
            JwtAdminAuthOptions options = configuration.GetSection("AdminAuth:JWT").Get<JwtAdminAuthOptions>();
            if (options == null)
            {
                options = JwtAdminAuthOptions.Default;
            }

            string[] audiences = ((options.ValidAudiences != null) ? options.ValidAudiences.Split(',') : new string[0]);
            string[] issuers = ((options.ValidIssuers != null) ? options.ValidIssuers.Split(',') : new string[0]);
            if (string.IsNullOrWhiteSpace(options.SigningKey))
            {
                throw new Exception("No Signing Key Specified for JWT");
            }

            services.AddAuthentication("Bearer").AddJwtBearer("AdminScheme", delegate (JwtBearerOptions jwtBearerOptions)
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = options.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey ?? "N/A")),
                    ValidateIssuer = options.ValidateIssuer,
                    ValidIssuers = issuers,
                    ValidateAudience = options.ValidateAudience,
                    ValidAudiences = audiences,
                    ValidateLifetime = options.ValidateLifetime,
                    ValidateActor = options.ValidateActor,
                    ClockSkew = TimeSpan.Zero
                };
            });
            return services;
        }
    }
}
