using Alten.API.Services;
using FluentValidation;

namespace Alten.API.Models;

public class ReservationPeriodValidator : AbstractValidator<Reservation>
{
    public ReservationPeriodValidator(IReservationService service)
    {
        RuleFor(r => r)
            .CustomAsync(async (reservation, context, cancelationToken) => 
            {
                if (!await service.CheckPeriodAvailability(reservation.AccomodationStart, reservation.AccomodationEnd, reservation.Id))
                {
                    context.AddFailure("AccomodationPeriod", "Selected period is not available.");
                }
            });
    }
}