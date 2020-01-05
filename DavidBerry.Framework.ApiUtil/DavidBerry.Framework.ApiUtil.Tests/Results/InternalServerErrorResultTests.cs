﻿using DavidBerry.Framework.ApiUtil.Results;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Framework.ApiUtil.Tests.Results
{
    public class InternalServerErrorResultTests
    {

        [Fact]
        public void InternalServerErrorResultHasStatusCode500()
        {
            // Arrange
            var result = new InternalServerErrorResult();

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public void InternalServerErrorObjectResultHasStatusCode500()
        {
            // Arrange
            var result = new InternalServerErrorObjectResult("Some Message");

            // Assert
            Assert.Equal(500, result.StatusCode);
        }


    }
}
