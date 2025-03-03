using Google.Apis.Sheets.v4.Data;
using MatriculationExamsServer.services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

// גישה לתצורה
var Configuration = builder.Configuration;
string sheetName = "matriculationexams";
string encodedSheetName = Uri.EscapeDataString(sheetName);
// Add services to the container.
builder.Services.AddSingleton(sp => new GoogleSheetApiService(
    credentialsPath: "matriculationexams-9a2568638e31.json",
    applicationName: sheetName
));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<LoginService>();
builder.Services.AddSingleton<AuthenticationService>();
builder.Services.AddSingleton<ColorService>();



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["Jwt:Issuer"],
            ValidAudience = Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]))
        };
    });


var app = builder.Build();
app.UseCors(builder => builder.WithOrigins("http://localhost:4200", "https://matriculationexam.onrender.com")
            .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials());
    // Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();
//app.UseCors("AllowAllOrigins");
app.MapControllers();

app.Run();
