using Alten.API.Models;

namespace Alten.API.Services;

public interface IReservationService
{
    Task<List<Reservation>> GetReservations();
    Task<Reservation> GetReservation(int id);
    Task AddReservation(Reservation reservation);
    Task ChangeReservation(int id, Reservation reservation);
    Task DeleteReservation(int id);
    Task<bool> CheckPeriodAvailability(DateTime accomodationStart, DateTime accomodationEnd);
}