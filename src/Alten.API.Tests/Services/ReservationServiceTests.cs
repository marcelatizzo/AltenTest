using System;
using System.Linq;
using System.Threading.Tasks;
using Alten.API;
using Alten.API.Controllers;
using Alten.API.Models;
using Alten.API.Services;
using Alten.API.Tests.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Alten.API.Tests.Services;

[TestFixture]
public class ReservationServiceTests
{
    private IReservationService service;

    [SetUp]
    public void SetUp()
    {
        var dbContext = DatabaseSetup.SetUpInMemoryDb();
        service = new ReservationService(dbContext);
    }

    [Test]
    public void Test1()
    {
        var controller = new ReservationController(service);
        var task = controller.Get();
        task.Wait();
        var result = task.Result;

        Assert.True(result.Any());
    }
}