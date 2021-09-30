using Alten.API.Models;
using Alten.API.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Alten.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _service;

    public ReservationController(IReservationService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody]Reservation reservation)
    {
        try
        {
            reservation.Id = 0;

            var periodValidator = new ReservationPeriodValidator(_service);
            var validationResult = periodValidator.Validate(reservation);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState, null);
                return BadRequest(ModelState);
            }

            await _service.AddReservation(reservation);
            return Ok();
        }
        catch(ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody]Reservation reservation)
    {
        try
        {
            reservation.Id = id;

            var periodValidator = new ReservationPeriodValidator(_service);
            var validationResult = periodValidator.Validate(reservation);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState, null);
                return BadRequest(ModelState);
            }

            await _service.ChangeReservation(id, reservation);
            return Ok();
        }
        catch(ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteReservation(id);
        return Ok();
    }

    [HttpGet]
    public async Task<IEnumerable<Reservation>> Get()
    {
        return await _service.GetReservations();
    }

    [HttpGet("{id}")]
    public async Task<Reservation> Get(int id)
    {
        return await _service.GetReservation(id);
    }

    [HttpGet("/check/{accomodationStart}/{accomodationEnd}")]
    public async Task<bool> CheckAvailability(DateTime accomodationStart, DateTime accomodationEnd)
    {
        return  await _service.CheckPeriodAvailability(accomodationStart, accomodationEnd);
    }
}