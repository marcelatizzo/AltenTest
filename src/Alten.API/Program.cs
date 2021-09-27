using Alten.API.Models;
using Alten.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddDbContext<ApiDbContext>(option => option.Use(builder.Configuration.GetConnectionString("db")));

var app = builder.Build();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHealthChecks("/status");

app.MapGet("/reservations", async (IReservationService service) => 
    await service.GetReservations());

app.MapGet("/reservations/{id}", async (int id, IReservationService service) => 
    await service.GetReservation(id));

app.MapGet("/reservations/check/{accomodationStart}/{accomodationEnd}", async (DateTime accomodationStart, DateTime accomodationEnd, IReservationService service) =>
    await service.CheckPeriodAvailability(accomodationStart, accomodationEnd));

app.MapPost("/reservations", async (Reservation reservation, IReservationService service) => 
    await service.AddReservation(reservation));

app.MapPut("/reservations/{id}", async (int id, Reservation reservation, IReservationService service) =>
    await service.ChangeReservation(id, reservation));

app.MapDelete("/reservations/{id}", async (int id, IReservationService service) =>
    await service.DeleteReservation(id));

app.Run();
