using Alten.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Alten.API.Services;

public class ReservationService : IReservationService
{
    private readonly ApiDbContext dbContext;

    public ReservationService(ApiDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Reservation> GetReservation(int id) => 
        await dbContext.Reservation.FirstOrDefaultAsync(r => r.Id == id);

    public async Task<bool> CheckPeriodAvailability(DateTime accomodationStart, DateTime accomodationEnd, int reservationIdToIgnore=0) =>
        await dbContext.Reservation.AllAsync(r =>
            (r.Id == reservationIdToIgnore) ||
            (   (r.AccomodationStart < accomodationStart || r.AccomodationStart > accomodationEnd) &&
                (r.AccomodationEnd < accomodationStart || r.AccomodationEnd > accomodationEnd) &&
                (r.AccomodationStart >= accomodationStart || r.AccomodationEnd <= accomodationEnd)));

    public async Task<List<Reservation>> GetReservations() => 
        await dbContext.Reservation.ToListAsync();

    public async Task AddReservation(Reservation reservation)
    {
        dbContext.Reservation.Add(reservation);
        await dbContext.SaveChangesAsync();
    }

    public async Task ChangeReservation(int id, Reservation newData)
    {
        var reservation = await GetReservation(id);
        reservation.GuestName = newData.GuestName;
        reservation.AccomodationStart = newData.AccomodationStart;
        reservation.AccomodationEnd = newData.AccomodationEnd;

        dbContext.Entry(reservation).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteReservation(int id)
        {
        var reservation = await GetReservation(id);

        dbContext.Reservation.Remove(reservation);
        await dbContext.SaveChangesAsync();
    }
}
