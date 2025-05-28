using System.Net.Http;
using Microsoft.Extensions.Configuration; 
using System; 
using SanityBackend.Services; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Configure HttpClient for SanityService
builder.Services.AddHttpClient<SanityService>(client =>
{
    // BaseAddress will be set in SanityService constructor using IConfiguration
    // No need to set it here if you're passing IConfiguration to the service
});
// Register SanityService as a scoped service
builder.Services.AddScoped<SanityService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();