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
            await dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == id);

        public async Task<bool> CheckPeriodAvailability(DateTime accomodationStart, DateTime accomodationEnd) =>
            await dbContext.Reservations.AllAsync(r =>
                (r.AccomodationStart < accomodationStart || r.AccomodationStart > accomodationEnd) &&
                (r.AccomodationEnd < accomodationStart && r.AccomodationEnd > accomodationEnd) &&
                (r.AccomodationStart >= accomodationStart || r.AccomodationEnd <= accomodationEnd));

        public async Task<List<Reservation>> GetReservations() => 
            await dbContext.Reservations.ToListAsync();

        public async Task AddReservation(Reservation reservation)
        {
            await ValidateReservation(reservation);
            
            dbContext.Reservations.Add(reservation);
            await dbContext.SaveChangesAsync();
        }

        public async Task ChangeReservation(int id, Reservation newData)
        {
            await ValidateReservation(newData);

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

            dbContext.Reservations.Remove(reservation);
            await dbContext.SaveChangesAsync();
        }

        private async Task ValidateReservation(Reservation reservation)
        {
            if (reservation.AccomodationEnd < reservation.AccomodationStart)
            {
                throw new ArgumentException("The accomodation period is invalid.");
            }

            var daysToAccomodationStart = reservation.AccomodationStart.Subtract(DateTime.Now).TotalDays;
            if (daysToAccomodationStart < 1 || daysToAccomodationStart > 30)
            {
                throw new ArgumentException("The accomodation start date is invalid");                
            }

            if (reservation.AccomodationEnd.Subtract(reservation.AccomodationStart).TotalDays > 3)
            {
                throw new ArgumentException("The period infomed is longer then the allowed.");
            }

            // if (!(await CheckPeriodAvailability(reservation.AccomodationStart, reservation.AccomodationEnd)))
            // {
            //     throw new ArgumentException("The accomodation period informed is not available.");
            // }
        }
    }
}