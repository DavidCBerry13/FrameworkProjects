using DavidBerry.Framework.ResultType;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DavidBerry.Framework.Tests.ResultType
{
    public class ResultTests
    {
        [Fact]
        public void ResultSuccess_IsSuccessTrue_WithNoErrorObjectAsError()
        {
            Result result = Result.Success();

            result.IsSuccess.Should().BeTrue();
            result.Error.Should().BeOfType<NoError>();
        }

        [Fact]
        public void ResultFailure_IsSuccessReturnsFalse()
        {
            Result result = Result.Failure("Things went terribly wrong");

            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void ResultFailureWithMessage_ReturnsErrorObject()
        {
            string errorMessage = "Things went terribly wrong";
            Result result = Result.Failure(errorMessage);

            result.Error.Should().BeOfType<Error>();
            result.Error.Message.Should().Be(errorMessage);
        }


        [Fact]
        public void WhenResultFailureCalledWithErrorObject_ErrorObjectIsReturned()
        {
            string errorMessage = "That data was not valid";
            Result result = Result.Failure(new InvalidDataError(errorMessage));

            result.Error.Should().BeOfType<InvalidDataError>();
            result.Error.Message.Should().Be(errorMessage);
        }



        [Fact]
        public void GenericResultSuccess_UsingPrimitive_IsSuccessTrue_WithNoErrorObjectAsError()
        {
            var returnValue = 32;
            var result = Result.Success<int>(returnValue);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(32);
            result.Error.Should().BeOfType<NoError>();
        }


        [Fact]
        public void GenericResultSuccess_IsSuccessTrue_WithNoErrorObjectAsError()
        {

            var result = Result.Success<SampleEmployee>(new SampleEmployee("John", "Smith", "Developer"));

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<SampleEmployee>();
            result.Error.Should().BeOfType<NoError>();
        }



        [Fact]
        public void When_ResultFailureOfTCalled_IsSuccess_IsFalse()
        {
            var errorMessage = "Something went wrong";
            var result = Result.Failure<int>(errorMessage);

            result.IsSuccess.Should().BeFalse();
        }


        [Fact]
        public void When_ResultFailureOfTCalled_Value_IsDefaultValue()
        {
            var errorMessage = "Something went wrong";
            var result = Result.Failure<int>(errorMessage);

            result.Value.Should().Be(default(int));
        }


        [Fact]
        public void When_ResultFailureOfTCalledWithMessage_ErrorCreatedWithMessage()
        {
            var errorMessage = "Something went wrong";
            var result = Result.Failure<int>(errorMessage);

            result.Error.Message.Should().Be(errorMessage);
            result.Error.Message.Should().NotBeOfType<NoError>();
        }


        [Fact]
        public void When_ResultFailureOfTCalledWithCustomError_CustomErrorClassReturned()
        {
            var errorMessage = "I could not find that employee";
            var result = Result.Failure<int>(new ObjectNotFoundError(errorMessage));

            result.Error.Should().BeOfType<ObjectNotFoundError>();
            result.Error.Message.Should().Be(errorMessage);
        }


        class SampleEmployee
        {
            public SampleEmployee(String firstName, String lastName, String title)
            {
                FirstName = firstName;
                LastName = lastName;
                Title = title;
            }

            public String FirstName { get; set; }
            public String LastName { get; set; }
            public String Title { get; set; }
        }


    }
}
