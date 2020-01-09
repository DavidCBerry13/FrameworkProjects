using DavidBerry.Framework.Functional;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DavidBerry.Framework.Tests.Functional
{
    public class MaybeTests
    {

        [Fact]
        public void CanConstructMaybeObjectWithValueIsObject()
        {
            Customer customer = new Customer() { Id = 1, FirstName = "John", LastName = "Smith" };

            var value = Maybe.Create(customer);

            value.Should().NotBeNull();
        }

        [Fact]
        public void CanConstructMaybeObjectWithValueAsNull()
        {
            var value = Maybe.Create<Customer>(null);

            value.Should().NotBeNull();
        }

        [Fact]
        public void HasValueIsTrueForMaybeObjectWithValue()
        {
            Customer customer = new Customer() { Id = 1, FirstName = "John", LastName = "Smith" };

            var maybe = Maybe.Create(customer);

            maybe.HasValue.Should().BeTrue();
        }

        [Fact]
        public void HasValueIsFalseForMaybeObjectWithoutValue()
        {
            var maybe = Maybe.Create<Customer>(null);

            maybe.HasValue.Should().BeFalse();
        }


        [Fact]
        public void ValuePropertyContainsTheValueObject()
        {
            Customer customer = new Customer() { Id = 1, FirstName = "John", LastName = "Smith" };

            var maybe = Maybe.Create(customer);

            maybe.Value.Id.Should().Be(1);
            maybe.Value.FirstName.Should().Be("John");
            maybe.Value.LastName.Should().Be("Smith");
        }

        [Fact]
        public void WhenMaybeObjectContainsValueHasValueFunctionCalledInBind()
        {
            Customer customer = new Customer() { Id = 1, FirstName = "John", LastName = "Smith" };

            var maybe = Maybe.Create(customer);

            var firstName = maybe.Eval(
                (customer) => customer.FirstName,
                () => "None"
            );

            firstName.Should().Be("John");
        }


        [Fact]
        public void WhenMaybeObjectContainsNoValueNoValueFunctionCalledInBind()
        {
            var maybe = Maybe.Create<Customer>(null);

            var firstName = maybe.Eval(
                (customer) => customer.FirstName,
                () => "None"
            );

            firstName.Should().Be("None");
        }


    }




    class Customer
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }

}
