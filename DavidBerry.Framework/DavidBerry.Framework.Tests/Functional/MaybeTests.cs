using DavidBerry.Framework.Functional;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DavidBerry.Framework.Tests.Functional
{
    public class MaybeTests
    {

        [Fact]
        public void CanCreateMaybeObjectWithValueIsObject()
        {
            Customer customer = new Customer() { Id = 1, FirstName = "John", LastName = "Smith" };

            var value = Maybe.Create<Customer>(customer);

            //value.ShouldNotBeNull();
            value.ShouldBeOfType<Maybe<Customer>>();
        }

        [Fact]
        public void CanCreateMaybeObjectWithValueAsNull()
        {
            var value = Maybe.Create<Customer>(null);

            //value.ShouldNotBeNull();
            value.ShouldBeOfType<Maybe<Customer>>();
        }

        [Fact]
        public void HasValueIsTrueForMaybeObjectWithValue()
        {
            Customer customer = new Customer() { Id = 1, FirstName = "John", LastName = "Smith" };

            var maybe = Maybe.Create(customer);

            maybe.HasValue.ShouldBeTrue();
            maybe.HasNoValue.ShouldBeFalse();
        }

        [Fact]
        public void HasValueIsFalseForMaybeObjectWithoutValue()
        {
            var maybe = Maybe.Create<Customer>(null);

            maybe.HasValue.ShouldBeFalse();
            maybe.HasNoValue.ShouldBeTrue();
        }


        [Fact]
        public void ValuePropertyContainsTheValueObject()
        {
            Customer customer = new Customer() { Id = 1, FirstName = "John", LastName = "Smith" };

            var maybe = Maybe.Create(customer);

            maybe.Value.Id.ShouldBe(1);
            maybe.Value.FirstName.ShouldBe("John");
            maybe.Value.LastName.ShouldBe("Smith");
        }

        [Fact]
        public void WhenMaybeObjectContainsValueHasValueFunctionCalledInBind()
        {
            Customer customer = new Customer() { Id = 1, FirstName = "John", LastName = "Smith" };

            var maybe = Maybe.Create(customer);

            var firstName = maybe.Match(
                (customer) => customer.FirstName,
                () => "None"
            );

            firstName.ShouldBe("John");
        }


        [Fact]
        public void WhenMaybeObjectContainsNoValueNoValueFunctionCalledInBind()
        {
            var maybe = Maybe.Create<Customer>(null);

            var firstName = maybe.Match(
                (customer) => customer.FirstName,
                () => "None"
            );

            firstName.ShouldBe("None");
        }



        [Fact]
        public void MaybeObjectIsCreatedImplicitelyWithValue()
        {
            Maybe<string> maybe = "Some Value";

            maybe.ShouldBeOfType<Maybe<string>>();
            maybe.HasValue.ShouldBeTrue();
            maybe.HasNoValue.ShouldBeFalse();
            maybe.Value.ShouldBe("Some Value");
        }


        [Fact]
        public void MaybeObjectIsCreatedImplicitelyWithNull()
        {
            // Local Function to Test
            //Maybe<string> ReturnsNull()
            //{
            //    return null;
            //}

            Maybe<string> maybe = SomeFunction(true);

            //maybe.ShouldBeOfType<Maybe<string>>();
            maybe.HasValue.ShouldBeFalse();
            maybe.HasNoValue.ShouldBeTrue();
            maybe.Value.ShouldBeNull();
        }



        public Maybe<string> SomeFunction(bool returnNull)
        {
            if ( returnNull) return null;

            return "Not Null";
        }




    }




    class Customer
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }

}
