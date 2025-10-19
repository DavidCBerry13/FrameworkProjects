using DavidBerry.Framework.Util;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DavidBerry.Framework.Tests.Util;

public class ITimePeriodExtensionsTests
{



    private class TimePeriod : ITimePeriod
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimePeriod(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }


    [Fact]
    public void TestOverlapsReturnsTrueWhenPeriodsAreTheSame()
    {
        // Arrange
        var period1 = new TimePeriod(DateTime.Parse("2024-10-18T11:00:00"), DateTime.Parse("2024-10-18T13:00:00"));
        var period2 = new TimePeriod(DateTime.Parse("2024-10-18T11:00:00"), DateTime.Parse("2024-10-18T13:00:00"));

        // Act
        bool overlaps = period1.Overlaps(period2); // Should be true

        // Assert
        overlaps.ShouldBeTrue();
    }


    [Fact]
    public void TestOverlapsReturnsTrueWhenPeriodOneEndsDuringPeriodTwo()
    {
        // Arrange
        var period1 = new TimePeriod(DateTime.Parse("2024-10-18T11:00:00"), DateTime.Parse("2024-10-18T14:00:00"));
        var period2 = new TimePeriod(DateTime.Parse("2024-10-18T12:00:00"), DateTime.Parse("2024-10-18T16:00:00"));

        // Act
        bool overlaps = period1.Overlaps(period2); // Should be true

        // Assert
        overlaps.ShouldBeTrue();
    }



    [Fact]
    public void TestOverlapsReturnsTrueWhenPeriodTwoEndsDuringPeriodOne()
    {
        // Arrange
        var period1 = new TimePeriod(DateTime.Parse("2024-10-18T11:00:00"), DateTime.Parse("2024-10-18T14:00:00"));
        var period2 = new TimePeriod(DateTime.Parse("2024-10-18T09:00:00"), DateTime.Parse("2024-10-18T12:00:00"));

        // Act
        bool overlaps = period1.Overlaps(period2); // Should be true

        // Assert
        overlaps.ShouldBeTrue();
    }


    [Fact]
    public void TestOverlapsReturnFalseWhenPeriodOneIsBeforePeriodTwo()
    {
        // Arrange
        var period1 = new TimePeriod(DateTime.Parse("2024-10-18T11:00:00"), DateTime.Parse("2024-10-18T13:00:00"));
        var period2 = new TimePeriod(DateTime.Parse("2024-10-18T14:00:00"), DateTime.Parse("2024-10-18T17:00:00"));

        // Act
        bool overlaps = period1.Overlaps(period2); // Should be true

        // Assert
        overlaps.ShouldBeFalse();
    }



    [Fact]
    public void TestOverlapsReturnFalseWhenPeriodTwoIsBeforePeriodOne()
    {
        // Arrange
        var period1 = new TimePeriod(DateTime.Parse("2024-10-18T11:00:00"), DateTime.Parse("2024-10-18T13:00:00"));
        var period2 = new TimePeriod(DateTime.Parse("2024-10-18T09:00:00"), DateTime.Parse("2024-10-18T10:00:00"));

        // Act
        bool overlaps = period1.Overlaps(period2); // Should be true

        // Assert
        overlaps.ShouldBeFalse();
    }



    [Fact]
    public void TestOverlapsReturnFalseWhenPeriodTwoStartsWhenPeriodOneEndse()
    {
        // Arrange
        var period1 = new TimePeriod(DateTime.Parse("2024-10-18T11:00:00"), DateTime.Parse("2024-10-18T13:00:00"));
        var period2 = new TimePeriod(DateTime.Parse("2024-10-18T13:00:00"), DateTime.Parse("2024-10-18T15:00:00"));

        // Act
        bool overlaps = period1.Overlaps(period2); // Should be true

        // Assert
        overlaps.ShouldBeFalse();
    }


    [Fact]
    public void TestOverlapsReturnFalseWhenPeriodOneStartsWhenPeriodTwoEndse()
    {
        // Arrange
        var period1 = new TimePeriod(DateTime.Parse("2024-10-18T11:00:00"), DateTime.Parse("2024-10-18T13:00:00"));
        var period2 = new TimePeriod(DateTime.Parse("2024-10-18T09:00:00"), DateTime.Parse("2024-10-18T11:00:00"));

        // Act
        bool overlaps = period1.Overlaps(period2); // Should be true

        // Assert
        overlaps.ShouldBeFalse();
    }


    [Fact]
    public void TestContainsResturnsTrueWhenDateTimeIsWithinPeriod()
    {
        // Arrange
        var period = new TimePeriod(DateTime.Parse("2024-10-18T11:00:00"), DateTime.Parse("2024-10-18T13:00:00"));
        var dateTime = DateTime.Parse("2024-10-18T12:00:00");

        // Act
        bool contains = period.Contains(dateTime); // Should be true

        // Assert
        contains.ShouldBeTrue();
    }

    [Fact]
    public void TestContainsResturnsFalseWhenDateTimeIsOutsidePeriod()
    {
        // Arrange
        var period = new TimePeriod(DateTime.Parse("2024-10-18T11:00:00"), DateTime.Parse("2024-10-18T13:00:00"));
        var dateTime = DateTime.Parse("2024-10-18T14:00:00");

        // Act
        bool contains = period.Contains(dateTime); // Should be true

        // Assert
        contains.ShouldBeFalse();
    }

    [Fact]
    public void TestDurationReturnsCorrectTimeSpan()
    {
        // Arrange
        var period = new TimePeriod(DateTime.Parse("2024-10-18T11:00:00"), DateTime.Parse("2024-10-18T13:30:00"));

        // Act
        TimeSpan duration = period.Duration(); // Should be 2 hours and 30 minutes

        // Assert
        duration.ShouldBe(TimeSpan.FromHours(2.5));
    }

    [Fact]
    public void TestIsInsideReturnsTrueWhenPeriodTwoInsidePeriodOne()
    {
        // Arrange
        var period1 = new TimePeriod(DateTime.Parse("2024-10-18T11:00:00"), DateTime.Parse("2024-10-18T16:00:00"));
        var period2 = new TimePeriod(DateTime.Parse("2024-10-18T12:00:00"), DateTime.Parse("2024-10-18T13:00:00"));

        // Act
        bool isInside = period2.IsInside(period1); // Should be true

        // Assert
        isInside.ShouldBeTrue();
    }



    [Fact]
    public void TestIsInsideReturnsFalseWhenPeriodOneEncompassesPeriodTwo()
    {
        // Arrange
        var period1 = new TimePeriod(DateTime.Parse("2024-10-18T11:00:00"), DateTime.Parse("2024-10-18T16:00:00"));
        var period2 = new TimePeriod(DateTime.Parse("2024-10-18T12:00:00"), DateTime.Parse("2024-10-18T13:00:00"));

        // Act
        bool isInside = period1.IsInside(period2); // Should be false

        // Assert
        isInside.ShouldBeFalse();
    }


    [Fact]
    public void TestIsInsideReturnsFalseWhenPeriodsOverlapButAreNotInside()
    {
        // Arrange
        var period1 = new TimePeriod(DateTime.Parse("2024-10-18T11:00:00"), DateTime.Parse("2024-10-18T14:00:00"));
        var period2 = new TimePeriod(DateTime.Parse("2024-10-18T12:00:00"), DateTime.Parse("2024-10-18T16:00:00"));

        // Act
        bool overlaps = period1.IsInside(period2); // Should be False

        // Assert
        overlaps.ShouldBeFalse();
    }


}