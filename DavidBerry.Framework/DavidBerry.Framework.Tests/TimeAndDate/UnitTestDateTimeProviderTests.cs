using DavidBerry.Framework.TimeAndDate;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DavidBerry.Framework.Tests.TimeAndDate
{
    public class UnitTestDateTimeProviderTests
    {

        [Fact]
        public void CurrentDateTime_ReturnsCurrentDateTime()
        {
            // Arrange
            var targetDateTime = new DateTime(2019, 12, 1, 11, 30, 00);
            UnitTestDateTimeProvider dateTimeProvider = new UnitTestDateTimeProvider(targetDateTime);

            // Act
            var result = dateTimeProvider.CurrentDateTime;

            // Assert
            result.Year.ShouldBe(2019);
            result.Month.ShouldBe(12);
            result.Day.ShouldBe(1);
            result.Hour.ShouldBe(11);
            result.Minute.ShouldBe(30);
            result.Second.ShouldBe(0);

            result.Kind.ShouldBe(DateTimeKind.Local);
        }



        [Fact]
        public void CurrentUtcDateTime_ReturnsCurrentUtcDateTime()
        {
            // Arrange
            var targetDateTime = new DateTime(2019, 12, 1, 22, 30, 00, DateTimeKind.Local);
            UnitTestDateTimeProvider dateTimeProvider = new UnitTestDateTimeProvider(targetDateTime);
            DateTime expectedDateTime = targetDateTime.ToUniversalTime();

            // Act
            var result = dateTimeProvider.CurrentUtcDateTime;

            // Assert
            result.Year.ShouldBe(expectedDateTime.Year);
            result.Month.ShouldBe(expectedDateTime.Month);
            result.Day.ShouldBe(expectedDateTime.Day);
            result.Hour.ShouldBe(expectedDateTime.Hour);
            result.Minute.ShouldBe(expectedDateTime.Minute);
            result.Second.ShouldBe(expectedDateTime.Second);

            result.Kind.ShouldBe(DateTimeKind.Utc);
        }



        [Fact]
        public void Today_ReturnsToday()
        {
            // Arrange
            var targetDateTime = new DateTime(2019, 12, 1, 11, 30, 00);
            UnitTestDateTimeProvider dateTimeProvider = new UnitTestDateTimeProvider(targetDateTime);

            // Act
            var result = dateTimeProvider.Today;

            // Assert
            result.Year.ShouldBe(2019);
            result.Month.ShouldBe(12);
            result.Day.ShouldBe(1);
            result.Hour.ShouldBe(0);
            result.Minute.ShouldBe(0);
            result.Second.ShouldBe(0);
            result.Kind.ShouldBe(DateTimeKind.Local);
        }


    }
}
