using DavidBerry.Framework.Functional;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DavidBerry.Framework.Tests.Functional
{
    public class ResultTests
    {
        [Fact]
        public void ResultSuccess_IsSuccessTrue_WithNoErrorObjectAsError()
        {
            Result result = Result.Success();

            result.IsSuccess.ShouldBeTrue();
            result.Error.ShouldBeOfType<NoError>();
        }

        [Fact]
        public void ResultFailure_IsSuccessReturnsFalse()
        {
            Result result = Result.Failure("Things went terribly wrong");

            result.IsSuccess.ShouldBeFalse();
        }

        [Fact]
        public void ResultFailureWithMessage_ReturnsErrorObject()
        {
            string errorMessage = "Things went terribly wrong";
            Result result = Result.Failure(errorMessage);

            result.Error.ShouldBeOfType<Error>();
            result.Error.Message.ShouldBe(errorMessage);
        }


        [Fact]
        public void WhenResultFailureCalledWithErrorObject_ErrorObjectIsReturned()
        {
            string errorMessage = "That data was not valid";
            Result result = Result.Failure(new InvalidDataError(errorMessage));

            result.Error.ShouldBeOfType<InvalidDataError>();
            result.Error.Message.ShouldBe(errorMessage);
        }



        [Fact]
        public void GenericResultSuccess_UsingPrimitive_IsSuccessTrue_WithNoErrorObjectAsError()
        {
            var returnValue = 32;
            var result = Result.Success(returnValue);

            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldBe(32);
            result.Error.ShouldBeOfType<NoError>();
        }


        [Fact]
        public void GenericResultSuccess_IsSuccessTrue_WithNoErrorObjectAsError()
        {

            var result = Result.Success(new SampleEmployee("John", "Smith", "Developer"));

            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldBeOfType<SampleEmployee>();
            result.Error.ShouldBeOfType<NoError>();
        }



        [Fact]
        public void When_ResultFailureOfTCalled_IsSuccess_IsFalse()
        {
            var errorMessage = "Something went wrong";
            var result = Result.Failure<int>(errorMessage);

            result.IsSuccess.ShouldBeFalse();
        }


        [Fact]
        public void When_ResultFailureOfTCalled_Value_IsDefaultValue()
        {
            var errorMessage = "Something went wrong";
            var result = Result.Failure<int>(errorMessage);

            result.Value.ShouldBe(default);
        }


        [Fact]
        public void When_ResultFailureOfTCalledWithMessage_ErrorCreatedWithMessage()
        {
            var errorMessage = "Something went wrong";
            var result = Result.Failure<int>(errorMessage);

            result.Error.Message.ShouldBe(errorMessage);
            result.Error.Message.ShouldNotBeOfType<NoError>();
        }


        [Fact]
        public void When_ResultFailureOfTCalledWithCustomError_CustomErrorClassReturned()
        {
            var errorMessage = "I could not find that employee";
            var result = Result.Failure<int>(new ObjectNotFoundError(errorMessage));

            result.Error.ShouldBeOfType<ObjectNotFoundError>();
            result.Error.Message.ShouldBe(errorMessage);
        }


        class SampleEmployee
        {
            public SampleEmployee(string firstName, string lastName, string title)
            {
                FirstName = firstName;
                LastName = lastName;
                Title = title;
            }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Title { get; set; }
        }


    }
}
