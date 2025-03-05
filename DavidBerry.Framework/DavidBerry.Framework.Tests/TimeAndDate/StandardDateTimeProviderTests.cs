using DavidBerry.Framework.TimeAndDate;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DavidBerry.Framework.Tests.TimeAndDate
{
    public class StandardDateTimeProviderTests
    {

        [Fact]
        public void CurrentDateTime_ReturnsCurrentDateTime()
        {
            // Arrange
            StandardDateTimeProvider dateTimeProvider = new StandardDateTimeProvider();
            DateTime controlTime = DateTime.Now;

            // Act
            var result = dateTimeProvider.CurrentDateTime;
            
            // Assert
            result.Should().BeCloseTo(controlTime, 100);
            result.Kind.Should().Be(DateTimeKind.Local);
        }

        [Fact]
        public void CurrentUtcDateTime_ReturnsCurrentUtcDateTime()
        {
            // Arrange
            StandardDateTimeProvider dateTimeProvider = new StandardDateTimeProvider();
            DateTime controlTime = DateTime.UtcNow;

            // Act
            var result = dateTimeProvider.CurrentUtcDateTime;

            // Assert
            result.Should().BeCloseTo(controlTime, 100);
            result.Kind.Should().Be(DateTimeKind.Utc);
        }


        [Fact]
        public void Today_ReturnsToday()
        {
            // Arrange
            StandardDateTimeProvider dateTimeProvider = new StandardDateTimeProvider();
            DateTime controlTime = DateTime.Today;

            // Act
            var result = dateTimeProvider.Today;

            // Assert
            result.Should().Be(controlTime);
            result.Kind.Should().Be(DateTimeKind.Local);
        }



    }
}
