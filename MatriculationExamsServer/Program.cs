using Google.Apis.Sheets.v4.Data;
using MatriculationExamsServer.services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
var builder = WebApplication.CreateBuilder(args);

// ���� ������
var Configuration = builder.Configuration;
//string sheetName = "matriculationexams";
//string encodedSheetName = Uri.EscapeDataString(sheetName);
//// Add services to the container.
//builder.Services.AddSingleton(sp => new GoogleSheetApiService(
//    credentialsPath: "matriculationexams-9a2568638e31.json",
//    applicationName: sheetName
//));
string jsonCredentials = Environment.GetEnvironmentVariable("GOOGLE_CREDENTIALS");
GoogleCredential credential = GoogleCredential.FromJson(jsonCredentials)
    .CreateScoped(SheetsService.Scope.Spreadsheets);

var service = new SheetsService(new BaseClientService.Initializer()
{
    HttpClientInitializer = credential,
    ApplicationName = "matriculationexams"
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<LoginService>();
builder.Services.AddSingleton<AuthenticationService>();
builder.Services.AddSingleton<ColorService>();
builder.Services.AddScoped<GoogleSheetApiService>();



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
app.UseCors(builder => builder.WithOrigins("http://localhost:4200", "https://matriculationexam.onrender.com", "https://matriculationexamserver.onrender.com")
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
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");
app.MapControllers();
app.Urls.Add("http://0.0.0.0:5000");
app.Run();
