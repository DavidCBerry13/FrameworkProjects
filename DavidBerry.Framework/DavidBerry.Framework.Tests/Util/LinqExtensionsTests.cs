using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using Shouldly;
using DavidBerry.Framework.Util;

namespace DavidBerry.Framework.Tests.Util
{
    public class LinqExtensionsTests
    {


        private class Car
        {
            public string Make { get; set; }
            public string Model { get; set; }
            public string Color { get; set; }
        }

        [Fact]
        public void TestWhereNotExistsSimpleStrings()
        {
            // Arrange
            var colors = new List<string>() { "Red", "Blue", "Green", "White" };
            var target = new List<string>() { "Red", "Green" };

            // Act
            var missingColors = colors.WhereNotExists(target, (x, y) => x == y).ToList();

            // Assert
            missingColors.Count.ShouldBe(2);
            missingColors.ShouldContain("Blue");
            missingColors.ShouldContain("White");
            missingColors.ShouldNotContain("Red");
            missingColors.ShouldNotContain("Green");
        }


        [Fact]
        public void TestWhereNotExistsObjectsNotInStringList()
        {
            // Arrange
            var cars = new List<Car>()
            {
                new Car() { Make = "Ford", Model="Escape", Color = "Silver"},
                new Car() { Make = "Mazda", Model="CX5", Color = "Red"},
                new Car() { Make = "Chevrolet", Model="Corvette", Color = "Yellow"},
                new Car() { Make = "Toyota", Model="Camry", Color = "Green"}
            };
            var colors = new List<string>() { "Red", "Blue", "Green", "White" };


            // Act
            var missingItems = cars.WhereNotExists(colors, (x, y) => x.Color == y).ToList();

            // Assert
            missingItems.Count.ShouldBe(2);
            missingItems.ShouldContain(c => c.Color == "Silver"); // Has a silver car
            missingItems.ShouldContain(c => c.Color == "Yellow");  // Has a yellow car
            missingItems.ShouldNotContain(c => c.Color == "Red"); // No Red Cars
            missingItems.ShouldNotContain(c => c.Color == "Green");  // No Green Cars
        }


        [Fact]
        public void TestWhereNotExistsStringsNotInObjectList()
        {
            // Arrange
            var cars = new List<Car>()
            {
                new Car() { Make = "Ford", Model="Escape", Color = "Silver"},
                new Car() { Make = "Mazda", Model="CX5", Color = "Red"},
                new Car() { Make = "Chevrolet", Model="Corvette", Color = "Yellow"},
                new Car() { Make = "Toyota", Model="Camry", Color = "Green"}
            };
            var colors = new List<string>() { "Red", "Blue", "Green", "White" };


            // Act
            var missingItems = colors.WhereNotExists(cars, (x, y) => x == y.Color).ToList();

            // Assert
            missingItems.Count.ShouldBe(2);
            missingItems.ShouldContain("White"); // There are no white cars
            missingItems.ShouldContain("Blue"); // There are no blue cars
        }



        [Fact]
        public void TestWhereExistsObjectsInStringList()
        {
            // Arrange
            var cars = new List<Car>()
            {
                new Car() { Make = "Ford", Model="Escape", Color = "Silver"},
                new Car() { Make = "Mazda", Model="CX5", Color = "Red"},
                new Car() { Make = "Chevrolet", Model="Corvette", Color = "Yellow"},
                new Car() { Make = "Toyota", Model="Camry", Color = "Green"}
            };
            var colors = new List<string>() { "Red", "Blue", "Green", "White" };


            // Act
            var matchingItems = cars.WhereExists(colors, (x, y) => x.Color == y).ToList();

            // Assert
            matchingItems.Count.ShouldBe(2);
            matchingItems.ShouldNotContain(car => car.Color == "Silver"); // Silver is not in the color list
            matchingItems.ShouldNotContain(car => car.Color == "Yellow"); // Yellow is not in the color list

            matchingItems.ShouldContain(car => car.Color == "Red", "A red car is in the list and red is in the list of colors");
            matchingItems.ShouldContain(car => car.Color == "Green", "A green car is in the list and green is in the list of colors");
        }


        [Fact]
        public void TestWhereExistsStringsInObjectList()
        {
            // Arrange
            var cars = new List<Car>()
            {
                new Car() { Make = "Ford", Model="Escape", Color = "Silver"},
                new Car() { Make = "Mazda", Model="CX5", Color = "Red"},
                new Car() { Make = "Chevrolet", Model="Corvette", Color = "Yellow"},
                new Car() { Make = "Toyota", Model="Camry", Color = "Green"}
            };
            var colors = new List<string>() { "Red", "Blue", "Green", "White" };


            // Act
            var matchingItems = colors.WhereExists(cars, (x, y) => x == y.Color).ToList();

            // Assert
            matchingItems.Count.ShouldBe(2);

            matchingItems.ShouldContain("Red", "A red car is in the list and red is in the list of colors");
            matchingItems.ShouldContain("Green", "A green car is in the list and green is in the list of colors");

            matchingItems.ShouldNotContain("Blue", "because there are no Blue cars");
            matchingItems.ShouldNotContain("White", "because there are no white cars");
        }



    }
}
