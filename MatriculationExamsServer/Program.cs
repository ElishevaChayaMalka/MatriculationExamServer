using Google.Apis.Sheets.v4.Data;
using MatriculationExamsServer.services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
string sheetName = "matriculationexams";
string encodedSheetName = Uri.EscapeDataString(sheetName);



// Add services to the container.
builder.Services.AddSingleton(sp => new GoogleSheetApiService(
    credentialsPath: "matriculationexams-4c178fbf1707.json",
    applicationName: sheetName
));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
