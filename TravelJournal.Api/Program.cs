using Microsoft.EntityFrameworkCore;
using TravelJournal.Application.Interfaces;
using TravelJournal.Application.Services;
using TravelJournal.Domain.Interfaces;
using TravelJournal.Infrastructure.Data;
using TravelJournal.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<RouteDbContext>(options =>
    options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IRouteService, RouteService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();