using AutoMapper;
using DavidBerry.Framework.ApiUtil.Controllers;
using DavidBerry.Framework.ApiUtil.Models;
using DavidBerry.Framework.Functional;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DavidBerry.Framework.ApiUtil.Tests.Controllers;

public class ApiControllerBaseTests
{

    [Fact]
    public void MapErrorResult_ReturnsBadRequest_WhenInvalidDataErrorInResult()
    {
        // Arrange
        Mock<ILogger<ApiControllerBase>> mockLogger = new Mock<ILogger<ApiControllerBase>>();
        Mock<IMapper> mockMapper = new Mock<IMapper>();
        Mock<ApiControllerBase> mockController = new Mock<ApiControllerBase>(mockLogger.Object, mockMapper.Object);
        mockController.CallBase = true;
        Result result = Result.Failure(new InvalidDataError("Invalid data"));

        // Act
        var actionResult = mockController.Object.MapErrorResult(result);

        // Assert
        actionResult.ShouldBeOfType<BadRequestObjectResult>();

        var badRequestResult = actionResult as BadRequestObjectResult;
        badRequestResult.Value.ShouldBeOfType<ApiMessageModel>();

        var apiMessageModel = badRequestResult.Value as ApiMessageModel;
        apiMessageModel.Message.ShouldBe("Invalid data");
    }



    [Fact]
    public void MapErrorResult_ReturnsObjectNotFound_WhenObjectNotFoundErrorInResult()
    {
        // Arrange
        Mock<ILogger<ApiControllerBase>> mockLogger = new Mock<ILogger<ApiControllerBase>>();
        Mock<IMapper> mockMapper = new Mock<IMapper>();
        Mock<ApiControllerBase> mockController = new Mock<ApiControllerBase>(mockLogger.Object, mockMapper.Object);
        mockController.CallBase = true;
        Result result = Result.Failure(new ObjectNotFoundError("We looked everywhere, we could not find it"));

        // Act
        var actionResult = mockController.Object.MapErrorResult(result);

        // Assert
        actionResult.ShouldBeOfType<NotFoundObjectResult>();

        var notFoundResult = actionResult as NotFoundObjectResult;
        notFoundResult.Value.ShouldBeOfType<ApiMessageModel>();

        var apiMessageModel = notFoundResult.Value as ApiMessageModel;
        apiMessageModel.Message.ShouldBe("We looked everywhere, we could not find it");
    }



    [Fact]
    public void CreateResponse_CallsSuccessFunction_WhenResultIsSuccess()
    {
        // Arrange
        Mock<ILogger<ApiControllerBase>> mockLogger = new Mock<ILogger<ApiControllerBase>>();
        Mock<IMapper> mockMapper = new Mock<IMapper>();
        Mock<ApiControllerBase> mockController = new Mock<ApiControllerBase>(mockLogger.Object, mockMapper.Object);
        mockController.CallBase = true;

        var entity = new TestEntity { Id = 1, Name = "Test" };
        Result<TestEntity> result = Result.Success(entity);
        bool successFunctionCalled = false;
        Func<TestEntity, ActionResult> successFunction = (e) =>
        {
            successFunctionCalled = true;
            return new OkObjectResult(e);
        };

        // Act
        var actionResult = mockController.Object.CreateResponse<TestEntity, TestModel>(result, successFunction);

        // Assert
        successFunctionCalled.ShouldBeTrue();
        actionResult.ShouldBeOfType<OkObjectResult>();
        var okResult = actionResult as OkObjectResult;
        okResult.Value.ShouldBe(entity);
    }

    [Fact]
    public void CreateResponse_CallsMapErrorResult_WhenResultIsFailure()
    {
        // Arrange
        Mock<ILogger<ApiControllerBase>> mockLogger = new Mock<ILogger<ApiControllerBase>>();
        Mock<IMapper> mockMapper = new Mock<IMapper>();
        Mock<ApiControllerBase> mockController = new Mock<ApiControllerBase>(mockLogger.Object, mockMapper.Object);
        mockController.CallBase = true;
        Result<TestEntity> result = Result.Failure<TestEntity>(new ObjectNotFoundError("Not found"));


        // Act
        var actionResult = mockController.Object.CreateResponse<TestEntity, TestModel>(result, It.IsAny<Func<TestEntity, ActionResult>>());

        // Assert
        actionResult.ShouldBeOfType<NotFoundObjectResult>();

        var notFoundResult = actionResult as NotFoundObjectResult;
        notFoundResult.Value.ShouldBeOfType<ApiMessageModel>();

        var apiMessageModel = notFoundResult.Value as ApiMessageModel;
        apiMessageModel.Message.ShouldBe("Not found");
    }

}