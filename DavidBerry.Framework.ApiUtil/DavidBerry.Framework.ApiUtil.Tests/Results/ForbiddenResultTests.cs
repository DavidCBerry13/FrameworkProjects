using DavidBerry.Framework.ApiUtil.Results;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Framework.ApiUtil.Tests.Results
{
    public class ForbiddenResultTests
    {

        [Fact]
        public void ForbiddenResultHasStatusCode403()
        {
            // Arrange
            var result = new ForbiddenResult();

            // Assert
            Assert.Equal(403, result.StatusCode);
        }

        [Fact]
        public void ForbiddenObjectResultHasStatusCode403()
        {
            // Arrange
            var result = new ForbiddenObjectResult("Some Message");

            // Assert
            Assert.Equal(403, result.StatusCode);
        }

    }
}
