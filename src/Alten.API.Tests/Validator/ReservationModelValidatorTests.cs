using System;
using Alten.API.Models;
using NUnit.Framework;

namespace Alten.API.Tests.Validator;

[TestFixture]
public class ReservationModelValidatorTests
{
    private ReservationModelValidator modelValidator;

    [SetUp]
    public void SetUp()
    {
        modelValidator = new ReservationModelValidator();
    }

    [Test]
    public void HappyPath()
    {
        var reservation = new Reservation 
        { 
            Id = 1,
            GuestName = "Guest Name",
            AccomodationStart = DateTime.Today.AddDays(2),
            AccomodationEnd = DateTime.Today.AddDays(4)
        };

        var result = modelValidator.Validate(reservation);

        Assert.True(result.IsValid, result.ToString());
    }

    [Test]
    public void ValidateGuestName_Empty()
    {
        var reservation = new Reservation 
        { 
            Id = 1,
            GuestName = "",
            AccomodationStart = DateTime.Today.AddDays(6),
            AccomodationEnd = DateTime.Today.AddDays(8)
        };

        var result = modelValidator.Validate(reservation);

        Assert.False(result.IsValid);
    }

    [Test]
    public void ValidateGuestName_Over300Characters()
    {
        var reservation = new Reservation 
        { 
            Id = 1,
            GuestName = new string('a', 301),
            AccomodationStart = DateTime.Today.AddDays(6),
            AccomodationEnd = DateTime.Today.AddDays(8)
        };

        var result = modelValidator.Validate(reservation);

        Assert.False(result.IsValid);
    }

    [Test]
    public void ValidateGuestName_Equal300Characters()
    {
        var reservation = new Reservation 
        { 
            Id = 1,
            GuestName = new string('a', 300),
            AccomodationStart = DateTime.Today.AddDays(6),
            AccomodationEnd = DateTime.Today.AddDays(8)
        };

        var result = modelValidator.Validate(reservation);

        Assert.True(result.IsValid, result.ToString());
    }
    
    [Test]
    public void Validate_AccomodationStart_ShouldBeAtLeast1DayAfterToday()
    {
        var reservation = new Reservation 
        { 
            Id = 1,
            GuestName = "Guest Name",
            AccomodationStart = DateTime.Today,
            AccomodationEnd = DateTime.Today.AddDays(2)
        };

        var result = modelValidator.Validate(reservation);

        Assert.False(result.IsValid);
    }
    
    [Test]
    public void Validate_AccomodationStart_ShouldBeLessThan30DaysInAdvance()
    {
        var reservation = new Reservation 
        { 
            Id = 1,
            GuestName = "Guest Name",
            AccomodationStart = DateTime.Today.AddDays(31),
            AccomodationEnd = DateTime.Today.AddDays(31)
        };

        var result = modelValidator.Validate(reservation);

        Assert.False(result.IsValid);
    }
    
    [Test]
    public void Validate_AccomodationEnd_ShouldNotBePriorToAccomodationStart()
    {
        var reservation = new Reservation 
        { 
            Id = 1,
            GuestName = "Guest Name",
            AccomodationStart = DateTime.Today.AddDays(2),
            AccomodationEnd = DateTime.Today.AddDays(1)
        };

        var result = modelValidator.Validate(reservation);

        Assert.False(result.IsValid);
    }
    
    [Test]
    public void Validate_AccomodationEnd_ShouldBeAtMax3Days()
    {
        var reservation = new Reservation 
        { 
            Id = 1,
            GuestName = "Guest Name",
            AccomodationStart = DateTime.Today.AddDays(1),
            AccomodationEnd = DateTime.Today.AddDays(4)
        };

        var result = modelValidator.Validate(reservation);

        Assert.False(result.IsValid);
    }
}