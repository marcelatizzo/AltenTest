using System;
using System.Collections;
using System.Collections.Generic; 
using Alten.API;
using Alten.API.Models;
using Alten.API.Services;
using Alten.API.Tests.Data;
using NUnit.Framework;

namespace Alten.API.Tests.Validator;

[TestFixture]
public class ReservationPeriodValidationTests
{
    private IReservationService service;
    private ReservationPeriodValidator periodValidator;

    [SetUp]
    public void SetUp()
    {
        var dbContext = DatabaseSetup.SetUpInMemoryDb();
        SeedData(dbContext);

        service = new ReservationService(dbContext);

        periodValidator = new ReservationPeriodValidator(service);
    }

    [TestCaseSource(nameof(PeriodScenarios))]
    public void TestCases(TestScenario scenario)
    {
        var reservationA = new Reservation
        {
            Id = 0,
            GuestName = "A",
            AccomodationStart = scenario.AccomodationStart,
            AccomodationEnd = scenario.AccomodationEnd
        };

        var result = periodValidator.Validate(reservationA);

        Assert.AreEqual(scenario.ExpectedResult, result.IsValid, scenario.CaseScenario);
    }

    [Test]
    public void IgnoreSameReservation()
    {
        var reservationB = new Reservation 
        { 
            Id = 1,
            GuestName = "B",
            AccomodationStart = DateTime.Today.AddDays(6),
            AccomodationEnd = DateTime.Today.AddDays(8)
        };

        var result = periodValidator.Validate(reservationB);

        Assert.True(result.IsValid);
    }

    private void SeedData(ApiDbContext dbContext)
    {
        dbContext.Reservation.RemoveRange(dbContext.Reservation);
        dbContext.SaveChanges();

        dbContext.Reservation.Add(new Reservation { Id = 1, GuestName = "B", AccomodationStart = DateTime.Today.AddDays(6), AccomodationEnd = DateTime.Today.AddDays(8) });
        dbContext.Reservation.Add(new Reservation { Id = 2, GuestName = "C", AccomodationStart = DateTime.Today.AddDays(11), AccomodationEnd = DateTime.Today.AddDays(11) });
        
        dbContext.SaveChanges();
    }

    private static IEnumerable<TestScenario> PeriodScenarios()
    {
        // Case 1 - Should succeed:  | A |-----
        //                           -----| B |
        yield return new TestScenario("Case 1 - Period A before period B", DateTime.Today.AddDays(4), DateTime.Today.AddDays(5), true);

        // Case 2 - Should fail:     -| A |----
        //                           -----| B |
        yield return new TestScenario("Case 2 - Period A before and touching period B", DateTime.Today.AddDays(5), DateTime.Today.AddDays(6), false);

        // Case 3 - Should fail:     --| A |---
        //                           -----| B |
        yield return new TestScenario("Case 3 - Period A before and overlapping period B", DateTime.Today.AddDays(5), DateTime.Today.AddDays(7), false);

        // Case 4 - Should fail:     -|  A  |--
        //                           -|  B  |--
        yield return new TestScenario("Case 4 - Period A equal period B", DateTime.Today.AddDays(6), DateTime.Today.AddDays(8), false);

        // Case 5 - Should fail:     -|   A   |
        //                           ---| B |--
        yield return new TestScenario("Case 5 - Period A embrace period B", DateTime.Today.AddDays(5), DateTime.Today.AddDays(9), false);

        // Case 6 - Should fail:     ---| A |--
        //                           -|   B   |
        yield return new TestScenario("Case 6 - Period A inside period B", DateTime.Today.AddDays(7), DateTime.Today.AddDays(7), false);

        // Case 7 - Should fail:     ---| A |--
        //                           -| B |----
        yield return new TestScenario("Case 7 - Period A after and overlapping period B", DateTime.Today.AddDays(7), DateTime.Today.AddDays(9), false);

        // Case 8 - Should fail:     -----| A |
        //                           -| B |----
        yield return new TestScenario("Case 8 - Period A after and touching period B", DateTime.Today.AddDays(8), DateTime.Today.AddDays(9), false);

        // Case 9 - Should succeed:  -----| A |
        //                           | C |-----
        yield return new TestScenario("Case 9 - Period A after period C", DateTime.Today.AddDays(12), DateTime.Today.AddDays(13), true);

        // Case 10 - Should succeed: -----| A |-----
        //                           | B |-----| C |
        yield return new TestScenario("Case 10 - Period A between periods B and C", DateTime.Today.AddDays(9), DateTime.Today.AddDays(10), true);
    }

    public class TestScenario
    {
        public TestScenario(
            string caseScenario,
            DateTime accomodationStart,
            DateTime accomodationEnd,
            bool expectedResult)
        {
            CaseScenario = caseScenario;
            AccomodationStart = accomodationStart;
            AccomodationEnd = accomodationEnd;
            ExpectedResult = expectedResult;
        }

        public string CaseScenario { get; }
        public DateTime AccomodationStart { get; }
        public DateTime AccomodationEnd { get; }
        public bool ExpectedResult { get; }
    }
}