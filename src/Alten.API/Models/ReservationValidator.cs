using Alten.API.Services;
using FluentValidation;

namespace Alten.API.Models;

public class ReservationValidator : AbstractValidator<Reservation>
{
    public ReservationValidator(IReservationService service)
    {

        RuleFor(r => r.GuestName)
            .NotEmpty().WithMessage("The Guest Name is required.")
            .Length(1, 300).WithMessage("The Guest Name cannot be more than 300 characters.");

        RuleFor(r => r.AccomodationStart)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("The Accomodation Start is required.")
            .GreaterThan(DateTime.Now).WithMessage("The Accomodation should start at lead 1 day after the booking.")
            .LessThanOrEqualTo(DateTime.Now.AddDays(30)).WithMessage("The room cannot be reserved more than 30 days in advance.");

        RuleFor(r => r.AccomodationEnd)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("The Accomodation End is required.")
            .GreaterThanOrEqualTo(r => r.AccomodationStart).WithMessage("The Accomodation End should be greater than or equal to the Accomodation Start")
            .LessThan(r => r.AccomodationStart.AddDays(3)).WithMessage("The period of accomodation should be between 1 and 3 days");

        RuleFor(r => r)
            .CustomAsync(async (reservation, context, cancelationToken) => 
            {
                if (!await service.CheckPeriodAvailability(reservation.AccomodationStart, reservation.AccomodationEnd))
                {
                    context.AddFailure("Selected period is not available.");
                }
            });
    }
}
