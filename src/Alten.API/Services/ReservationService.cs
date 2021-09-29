using Alten.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Alten.API.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApiDbContext dbContext;

        public ReservationService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Reservation> GetReservation(int id) => 
            await dbContext.Reservation.FirstOrDefaultAsync(r => r.Id == id);

        public async Task<bool> CheckPeriodAvailability(DateTime accomodationStart, DateTime accomodationEnd) =>
            await dbContext.Reservation.AllAsync(r =>
                (r.AccomodationStart < accomodationStart || r.AccomodationStart > accomodationEnd) &&
                (r.AccomodationEnd < accomodationStart || r.AccomodationEnd > accomodationEnd) &&
                (r.AccomodationStart >= accomodationStart || r.AccomodationEnd <= accomodationEnd));

        public async Task<List<Reservation>> GetReservations() => 
            await dbContext.Reservation.ToListAsync();

        public async Task AddReservation(Reservation reservation)
        {
            await ValidatePeriodDisponibility(reservation);
            
            dbContext.Reservation.Add(reservation);
            await dbContext.SaveChangesAsync();
        }

        public async Task ChangeReservation(int id, Reservation newData)
        {
            await ValidatePeriodDisponibility(newData);

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

        private async Task ValidatePeriodDisponibility(Reservation reservation)
        {
            if (!(await CheckPeriodAvailability(reservation.AccomodationStart, reservation.AccomodationEnd)))
            {
                throw new ArgumentException("The accomodation period informed is not available.");
            }
        }
    }
}