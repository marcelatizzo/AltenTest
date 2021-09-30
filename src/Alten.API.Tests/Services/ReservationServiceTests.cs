using System;
using System.Linq;
using System.Threading.Tasks;
using Alten.API;
using Alten.API.Controllers;
using Alten.API.Models;
using Alten.API.Services;
using Alten.API.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Alten.API.Tests.Services;

[TestFixture]
public class ReservationServiceTests
{
    private IReservationService service;
    private ReservationController controller;

    [SetUp]
    public void SetUp()
    {
        var dbContext = DatabaseSetup.SetUpInMemoryDb();
        service = new ReservationService(dbContext);
        controller = new ReservationController(service);
    }

    [Test]
    public void Test1()
    {
        var task = controller.Get();
        task.Wait();
        var result = task.Result;

        Assert.True(result.Any());
    }

    [Test]
    public void Test2()
    {
        DateTime accomodationDate = DateTime.Today.AddDays(1); 
        var add1Task = controller.Add(new Reservation { GuestName = "Guest 1", AccomodationStart = accomodationDate, AccomodationEnd = accomodationDate });
        add1Task.Wait();
        var add2Task = controller.Add(new Reservation { GuestName = "Guest 2", AccomodationStart = accomodationDate, AccomodationEnd = accomodationDate });
        add2Task.Wait();
        
        var result = add2Task.Result;

        //Assert.AreEqual(BadRequestResult, result);
    }
}