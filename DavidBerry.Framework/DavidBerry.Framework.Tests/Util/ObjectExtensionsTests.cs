using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DavidBerry.Framework.Util;
using Shouldly;
using Xunit;

namespace DavidBerry.Framework.Tests.Util;



public class ObjectExtensionsTests
{

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }


    [Fact]
    public void As_CorrectlyCastsAnObjectToCorrectType()
    {
        // Arrange
        object obj = new Person() { Name = "John", Age = 30 };

        // Act
        var person = obj.As<Person>();

        // Assert
        person.ShouldBeOfType<Person>();
        person.Name.ShouldBe("John");
        person.Age.ShouldBe(30);
    }


    [Fact]
    public void As_ReturnsNullWhenIncorrectTypeGiven()
    {
        // Arrange
        object obj = new string("hello World");

        // Act
        var person = obj.As<Person>();

        // Assert
        person.ShouldBeNull();
    }

}

