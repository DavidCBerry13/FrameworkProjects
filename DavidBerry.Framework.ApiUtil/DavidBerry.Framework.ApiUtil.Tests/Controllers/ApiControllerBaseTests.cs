using AutoMapper;
using DavidBerry.Framework.ApiUtil.Controllers;
using DavidBerry.Framework.ApiUtil.Models;
using DavidBerry.Framework.Exceptions;
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



    [Fact]
    public void MapErrorResult_ReturnsBadRequest_WhenResultIsFailure_WithInvalidDataError()
    {
        // Arrange
        Mock<ILogger<ApiControllerBase>> mockLogger = new Mock<ILogger<ApiControllerBase>>();
        Mock<IMapper> mockMapper = new Mock<IMapper>();
        Mock<ApiControllerBase> mockController = new Mock<ApiControllerBase>(mockLogger.Object, mockMapper.Object);
        mockController.CallBase = true;
        Result result = Result.Failure(new InvalidDataError("Invalid data"));

        // Act
        var actionResult = mockController.Object.MapErrorResult<TestEntity, TestModel>(result);

        // Assert
        actionResult.ShouldBeOfType<BadRequestObjectResult>();

        var badRequestResult = actionResult as BadRequestObjectResult;
        badRequestResult.Value.ShouldBeOfType<ApiMessageModel>();

        var apiMessageModel = badRequestResult.Value as ApiMessageModel;
        apiMessageModel.Message.ShouldBe("Invalid data");

    }


    [Fact]
    public void MapErrorResult_ReturnsNotFound_WhenResultIsFailure_WithObjectNotFoundError()
    {
        // Arrange
        Mock<ILogger<ApiControllerBase>> mockLogger = new Mock<ILogger<ApiControllerBase>>();
        Mock<IMapper> mockMapper = new Mock<IMapper>();
        Mock<ApiControllerBase> mockController = new Mock<ApiControllerBase>(mockLogger.Object, mockMapper.Object);
        mockController.CallBase = true;
        Result result = Result.Failure(new ObjectNotFoundError("The data was nowhere we looked"));

        // Act
        var actionResult = mockController.Object.MapErrorResult<TestEntity, TestModel>(result);

        // Assert
        actionResult.ShouldBeOfType<NotFoundObjectResult>();

        var notFoundResult = actionResult as NotFoundObjectResult;
        notFoundResult.Value.ShouldBeOfType<ApiMessageModel>();

        var apiMessageModel = notFoundResult.Value as ApiMessageModel;
        apiMessageModel.Message.ShouldBe("The data was nowhere we looked");

    }



    [Fact]
    public void MapErrorResult_ReturnsCreateObjectExistsConflictErrorResult_WhenResultIsFailure_WithObjectAlreadyExistsError()
    {
        // Arrange
        TestEntity testEntity = new TestEntity() { Id = 1, Name = "I already exist" };

        Mock<ILogger<ApiControllerBase>> mockLogger = new Mock<ILogger<ApiControllerBase>>();
        Mock<IMapper> mockMapper = new Mock<IMapper>();
        mockMapper.Setup(x => x.Map<TestEntity, TestModel>(testEntity)).Returns(new TestModel() { Id = 1, Name = "I already exist" });

        Mock<ApiControllerBase> mockController = new Mock<ApiControllerBase>(mockLogger.Object, mockMapper.Object);
        mockController.CallBase = true;
        Result result = Result.Failure(new ObjectAlreadyExistsError<TestEntity>("The object already exists", testEntity));

        // Act
        var actionResult = mockController.Object.MapErrorResult<TestEntity, TestModel>(result);

        // Assert
        actionResult.ShouldBeOfType<ConflictObjectResult>();

        var conflictResult = actionResult as ConflictObjectResult;
        conflictResult.Value.ShouldBeOfType<ConcurrencyErrorModel<TestModel>>();

        var concurrencyErrorModel = conflictResult.Value as ConcurrencyErrorModel<TestModel>;
        concurrencyErrorModel.Message.ShouldBe("The object already exists");

        concurrencyErrorModel.CurrentObject.ShouldBeOfType<TestModel>();
        concurrencyErrorModel.CurrentObject.Id.ShouldBe(1);
        concurrencyErrorModel.CurrentObject.Name.ShouldBe("I already exist");
    }


    [Fact]
    public void MapErrorResult_ReturnsConcurrencyConflict_WhenResultIsFailure_WithConcurrencyError()
    {
        // Arrange
        TestEntity testEntity = new TestEntity() { Id = 1, Name = "Concurrency Error" };

        Mock<ILogger<ApiControllerBase>> mockLogger = new Mock<ILogger<ApiControllerBase>>();
        Mock<IMapper> mockMapper = new Mock<IMapper>();
        mockMapper.Setup(x => x.Map<TestEntity, TestModel>(testEntity)).Returns(new TestModel() { Id = 1, Name = "Concurrency Error" });

        Mock<ApiControllerBase> mockController = new Mock<ApiControllerBase>(mockLogger.Object, mockMapper.Object);
        mockController.CallBase = true;
        Result result = Result.Failure(new ConcurrencyError<TestEntity>("Someone else updated the object", testEntity));

        // Act
        var actionResult = mockController.Object.MapErrorResult<TestEntity, TestModel>(result);

        // Assert
        actionResult.ShouldBeOfType<ConflictObjectResult>();

        var conflictResult = actionResult as ConflictObjectResult;
        conflictResult.Value.ShouldBeOfType<ConcurrencyErrorModel<TestModel>>();

        var concurrencyErrorModel = conflictResult.Value as ConcurrencyErrorModel<TestModel>;
        concurrencyErrorModel.Message.ShouldBe("Someone else updated the object");

        concurrencyErrorModel.CurrentObject.ShouldBeOfType<TestModel>();
        concurrencyErrorModel.CurrentObject.Id.ShouldBe(1);
        concurrencyErrorModel.CurrentObject.Name.ShouldBe("Concurrency Error");
    }


    [Fact]
    public void CreateConcurrencyConflictErrorResult_CreatesConflictObjectResult_WhenPassedConcurrencyException()
    {
        // Arrange
        TestEntity testEntity = new TestEntity() { Id = 1, Name = "Concurrency Error" };

        Mock<ILogger<ApiControllerBase>> mockLogger = new Mock<ILogger<ApiControllerBase>>();
        Mock<IMapper> mockMapper = new Mock<IMapper>();
        mockMapper.Setup(x => x.Map<TestEntity, TestModel>(testEntity)).Returns(new TestModel() { Id = 1, Name = "Concurrency Error" });

        Mock<ApiControllerBase> mockController = new Mock<ApiControllerBase>(mockLogger.Object, mockMapper.Object);
        mockController.CallBase = true;

        var concurrencyException = new ConcurrencyException<TestEntity>("Error Message", testEntity);

        // Act
        var actionResult = mockController.Object.CreateConcurrencyConflictErrorResult<TestModel, TestEntity>(concurrencyException);

        // Assert
        actionResult.ShouldBeOfType<ConflictObjectResult>();

        var conflictResult = actionResult as ConflictObjectResult;
        conflictResult.Value.ShouldBeOfType<ConcurrencyErrorModel<TestModel>>();

        var errorModel = conflictResult.Value as ConcurrencyErrorModel<TestModel>;
        errorModel.Message.ShouldBe("Error Message");
        errorModel.CurrentObject.ShouldBeOfType<TestModel>();
        errorModel.CurrentObject.Id.ShouldBe(1);
        errorModel.CurrentObject.Name.ShouldBe("Concurrency Error");
    }


    [Fact]
    public void CreateConcurrencyConflictErrorResult_CreatesConflictObjectResult_WhenPassedConcurrencyError()
    {
        // Arrange
        TestEntity testEntity = new TestEntity() { Id = 1, Name = "Concurrency Error" };

        Mock<ILogger<ApiControllerBase>> mockLogger = new Mock<ILogger<ApiControllerBase>>();
        Mock<IMapper> mockMapper = new Mock<IMapper>();
        mockMapper.Setup(x => x.Map<TestEntity, TestModel>(testEntity)).Returns(new TestModel() { Id = 1, Name = "Concurrency Error" });

        Mock<ApiControllerBase> mockController = new Mock<ApiControllerBase>(mockLogger.Object, mockMapper.Object);
        mockController.CallBase = true;

        var concurrencyError = new ConcurrencyError<TestEntity>("Error Message", testEntity);

        // Act
        var actionResult = mockController.Object.CreateConcurrencyConflictErrorResult<TestEntity, TestModel>(concurrencyError);

        // Assert
        actionResult.ShouldBeOfType<ConflictObjectResult>();

        var conflictResult = actionResult as ConflictObjectResult;
        conflictResult.Value.ShouldBeOfType<ConcurrencyErrorModel<TestModel>>();

        var errorModel = conflictResult.Value as ConcurrencyErrorModel<TestModel>;
        errorModel.Message.ShouldBe("Error Message");
        errorModel.CurrentObject.ShouldBeOfType<TestModel>();
        errorModel.CurrentObject.Id.ShouldBe(1);
        errorModel.CurrentObject.Name.ShouldBe("Concurrency Error");
    }


    [Fact]
    public void CreateObjectExistsConflictErrorResult_CreatesConflictObjectResult_WhenPassedObjectAlreadyExistsError()
    {
        // Arrange
        TestEntity testEntity = new TestEntity() { Id = 1, Name = "Object Exists Error" };

        Mock<ILogger<ApiControllerBase>> mockLogger = new Mock<ILogger<ApiControllerBase>>();
        Mock<IMapper> mockMapper = new Mock<IMapper>();
        mockMapper.Setup(x => x.Map<TestEntity, TestModel>(testEntity)).Returns(new TestModel() { Id = 1, Name = "Object Exists Error" });

        Mock<ApiControllerBase> mockController = new Mock<ApiControllerBase>(mockLogger.Object, mockMapper.Object);
        mockController.CallBase = true;

        var objectExistsError = new ObjectAlreadyExistsError<TestEntity>("Error Message", testEntity);

        // Act
        var actionResult = mockController.Object.CreateObjectExistsConflictErrorResult<TestEntity, TestModel>(objectExistsError);

        // Assert
        actionResult.ShouldBeOfType<ConflictObjectResult>();

        var conflictResult = actionResult as ConflictObjectResult;
        conflictResult.Value.ShouldBeOfType<ConcurrencyErrorModel<TestModel>>();

        var errorModel = conflictResult.Value as ConcurrencyErrorModel<TestModel>;
        errorModel.Message.ShouldBe("Error Message");
        errorModel.CurrentObject.ShouldBeOfType<TestModel>();
        errorModel.CurrentObject.Id.ShouldBe(1);
        errorModel.CurrentObject.Name.ShouldBe("Object Exists Error");
    }

}