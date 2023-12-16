using MaratukAdmin.Extensions;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Concrete;
using MaratukAdmin.Middlewares;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Concrete;
using MaratukAdmin.Services;
using MaratukAdmin.Services.Configure;
using MaratukAdmin.Utils;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using System.Text;




var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddRequiredAppSettings();

// Add services to the container.


builder.Services.AddAuthentication(
    CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();
builder.Services.AddSingleton(sp =>
{
    return new SigningCredentials(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AdminAuth:JWT:SigningKey"])),
        SecurityAlgorithms.HmacSha256);
});




builder.Services.AddHttpClient();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSwaggerGen(s =>
{
    s.EnableAnnotations();
});


builder.Services.AddLocalJWTAdminAuth(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddCustomSwagger(builder.Configuration);



builder.Services.AddProblemDetails(builder.Configuration, builder.Environment);
builder.Services.AddManagers(builder.Configuration);
builder.Services.AddRepositories(builder.Configuration);




builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<JwtTokenService>();



builder.Services.AddAuthentication("AdminScheme");

builder.Services.AddDbContexts(builder.Configuration);


builder.Services.AddServicesOptions(builder.Configuration);

builder.Services.AddDistributedMemoryCache();       // To use IDistributedCache
builder.Services.AddHttpContextAccessor();          // This serice allows to get access to current HttpContext

/*builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin2",
        builder => builder.WithOrigins("https://16.171.143.175:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});*/

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://13.53.168.66", "http://13.51.156.155")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();



// Configure the HTTP request pipeline.

app.UseCors("AllowSpecificOrigin");
/*app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 401)
    {
        var customResponse = new Response
        {
            Message = "Unauthorized access",
            StatusCode = 401
        };

        var json = JsonConvert.SerializeObject(customResponse);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(json);
    }
});
*/



app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCertificateForwarding();
app.UseCustomSwagger(builder.Configuration);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//app.UseCors();


app.MapHealthChecks("/healthcheck");
app.MapControllers();



app.Run();
