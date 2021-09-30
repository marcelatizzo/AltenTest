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
    private static readonly DateTime baseDate = DateTime.Today;
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
            AccomodationStart = baseDate.AddDays(6),
            AccomodationEnd = baseDate.AddDays(8)
        };

        var result = periodValidator.Validate(reservationB);

        Assert.True(result.IsValid);
    }

    private void SeedData(ApiDbContext dbContext)
    {
        dbContext.Reservation.RemoveRange(dbContext.Reservation);
        dbContext.SaveChanges();

        dbContext.Reservation.Add(new Reservation { Id = 1, GuestName = "B", AccomodationStart = baseDate.AddDays(6), AccomodationEnd = baseDate.AddDays(8) });
        dbContext.Reservation.Add(new Reservation { Id = 2, GuestName = "C", AccomodationStart = baseDate.AddDays(11), AccomodationEnd = baseDate.AddDays(11) });
        
        dbContext.SaveChanges();
    }

    private static IEnumerable<TestScenario> PeriodScenarios()
    {
        // Case 1 - Should succeed:  | A |-----
        //                           -----| B |
        yield return new TestScenario("Case 1 - Period A before period B", baseDate.AddDays(4), baseDate.AddDays(5), true);

        // Case 2 - Should fail:     -| A |----
        //                           -----| B |
        yield return new TestScenario("Case 2 - Period A before and touching period B", baseDate.AddDays(5), baseDate.AddDays(6), false);

        // Case 3 - Should fail:     --| A |---
        //                           -----| B |
        yield return new TestScenario("Case 3 - Period A before and overlapping period B", baseDate.AddDays(5), baseDate.AddDays(7), false);

        // Case 4 - Should fail:     -|  A  |--
        //                           -|  B  |--
        yield return new TestScenario("Case 4 - Period A equal period B", baseDate.AddDays(6), baseDate.AddDays(8), false);

        // Case 5 - Should fail:     -|   A   |
        //                           ---| B |--
        yield return new TestScenario("Case 5 - Period A embrace period B", baseDate.AddDays(5), baseDate.AddDays(9), false);

        // Case 6 - Should fail:     ---| A |--
        //                           -|   B   |
        yield return new TestScenario("Case 6 - Period A inside period B", baseDate.AddDays(7), baseDate.AddDays(7), false);

        // Case 7 - Should fail:     ---| A |--
        //                           -| B |----
        yield return new TestScenario("Case 7 - Period A after and overlapping period B", baseDate.AddDays(7), baseDate.AddDays(9), false);

        // Case 8 - Should fail:     -----| A |
        //                           -| B |----
        yield return new TestScenario("Case 8 - Period A after and touching period B", baseDate.AddDays(8), baseDate.AddDays(9), false);

        // Case 9 - Should succeed:  -----| A |
        //                           | C |-----
        yield return new TestScenario("Case 9 - Period A after period C", baseDate.AddDays(12), baseDate.AddDays(13), true);

        // Case 10 - Should succeed: -----| A |-----
        //                           | B |-----| C |
        yield return new TestScenario("Case 10 - Period A between periods B and C", baseDate.AddDays(9), baseDate.AddDays(10), true);
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