using System;
using Alten.API;
using Alten.API.Models;
using Alten.API.Services;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;

namespace Alten.API.Tests.Data;

public static class DatabaseSetup
{
    public static ApiDbContext SetUpInMemoryDb()
    {
        var dbOptions = new DbContextOptionsBuilder<ApiDbContext>()
            .UseInMemoryDatabase(databaseName: "Hotel")
            .Options;

        var dbContext = new ApiDbContext(dbOptions);

        DatabaseSetup.SeedData(dbContext);

        return dbContext;
    }

    private static void SeedData(ApiDbContext dbContext)
    {
        var baseDate = DateTime.Today;

        dbContext.Reservation.Add(new Reservation { GuestName = "Guest 1", AccomodationStart = baseDate.AddDays(5), AccomodationEnd = baseDate.AddDays(7) });
        
        dbContext.SaveChanges();
    }
}
