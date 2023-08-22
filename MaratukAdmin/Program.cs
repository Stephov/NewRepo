using MaratukAdmin.Extensions;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Concrete;
using MaratukAdmin.Middlewares;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Concrete;
using MaratukAdmin.Services;
using MaratukAdmin.Services.Configure;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.AddSwaggerGen();


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

/*builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));*/

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();



// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCertificateForwarding();
app.UseCustomSwagger(builder.Configuration);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//app.UseCors("corsapp");
app.UseCors("AllowSpecificOrigin");

app.MapHealthChecks("/healthcheck");
app.MapControllers();



app.Run();
