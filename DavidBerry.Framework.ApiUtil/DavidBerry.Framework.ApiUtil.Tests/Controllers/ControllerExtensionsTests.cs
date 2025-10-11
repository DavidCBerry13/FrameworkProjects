using DavidBerry.Framework.ApiUtil.Controllers;
using DavidBerry.Framework.ApiUtil.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;


namespace DavidBerry.Framework.ApiUtil.Tests.Controllers;


public class ControllerExtensionsTests
{

    [Fact]
    public void ForbiddenExtensionMethod_ReturnsForbiddenResult()
    {
        // Arrange
        var mockController = new Mock<ControllerBase>();
        mockController.CallBase = true;

        // Act
        var result = mockController.Object.Forbidden();

        // Assert
        result.ShouldBeOfType<ForbiddenResult>();
    }


    [Fact]
    public void ForbiddenExtensionMethodWithValue_ReturnsForbiddenResultWithSameValue()
    {
        // Arrange
        var mockController = new Mock<ControllerBase>();
        mockController.CallBase = true;
        string message = "You do not have permission to access this resource.";

        // Act
        var result = mockController.Object.Forbidden(message);

        // Assert
        result.ShouldBeOfType<ForbiddenObjectResult>();
        ((ForbiddenObjectResult)result).Value.ShouldBe(message);
    }



    [Fact]
    public void InternalServerErrorExtensionMethod_ReturnsInternalServerErrorResult()
    {
        // Arrange
        var mockController = new Mock<ControllerBase>();
        mockController.CallBase = true;

        // Act
        var result = mockController.Object.InternalServerError();

        // Assert
        result.ShouldBeOfType<InternalServerErrorResult>();
    }



    [Fact]
    public void InternalServerErrorExtensionMethodWithValue_Returns_InternalServerErrorObjectResult_WithValue()
    {
        // Arrange
        var mockController = new Mock<ControllerBase>();
        mockController.CallBase = true;
        string message = "It all went terribly, terribly wrong!";

        // Act
        var result = mockController.Object.InternalServerError(message);

        // Assert
        result.ShouldBeOfType<InternalServerErrorObjectResult>();
        ((InternalServerErrorObjectResult)result).Value.ShouldBe(message);
    }

}