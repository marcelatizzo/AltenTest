using Alten.API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.AddDbContext<ApiDbContext>(ServiceLifetime.Transient, ServiceLifetime.Scoped);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHealthChecks("/status");

app.MapGet("/reservations", async (ApiDbContext db) => 
    db.Reservations.ToListAsync());

app.MapGet("/reservations/{id}", async (int id, ApiDbContext db) => 
    db.Reservations.FirstOrDefaultAsync(r => r.Id == id));

app.MapPost("/reservations", async (Reservation reservation, ApiDbContext db) =>
    {
       db.Reservations.Add(reservation);
       await db.SaveChangesAsync();

       return reservation; 
    });

app.MapPut("/reservations/{id}", async (int id, Reservation reservation, ApiDbContext db) =>
    {
        db.Entry(reservation).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return reservation;
    });

app.MapDelete("/reservations/{id}", async (int id, ApiDbContext db) =>
    {
        var reservation = await db.Reservations.FirstOrDefaultAsync(r => r.Id == id);

        db.Reservations.Remove(reservation);
        await db.SaveChangesAsync();
        return;
    });

app.Run();
