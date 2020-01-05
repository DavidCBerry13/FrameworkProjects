using DavidBerry.Framework.TimeAndDate;
using FluentAssertions;
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
            result.Year.Should().Be(2019);
            result.Month.Should().Be(12);
            result.Day.Should().Be(1);
            result.Hour.Should().Be(11);
            result.Minute.Should().Be(30);
            result.Second.Should().Be(0);

            result.Kind.Should().Be(DateTimeKind.Local);
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
            result.Year.Should().Be(expectedDateTime.Year);
            result.Month.Should().Be(expectedDateTime.Month);
            result.Day.Should().Be(expectedDateTime.Day);
            result.Hour.Should().Be(expectedDateTime.Hour);
            result.Minute.Should().Be(expectedDateTime.Minute);
            result.Second.Should().Be(expectedDateTime.Second);

            result.Kind.Should().Be(DateTimeKind.Utc);
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
            result.Year.Should().Be(2019);
            result.Month.Should().Be(12);
            result.Day.Should().Be(1);
            result.Hour.Should().Be(0);
            result.Minute.Should().Be(0);
            result.Second.Should().Be(0);
            result.Kind.Should().Be(DateTimeKind.Local);
        }


    }
}
