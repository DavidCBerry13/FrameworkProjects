using DavidBerry.Framework.Util;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DavidBerry.Framework.Tests.Util
{
    public class DataRangeTests
    {

        [Theory]
        [InlineData(0,10)]
        [InlineData(-10, 10)]
        public void Int32_ConstructorAcceptsValidArguments(int minimum, int maximum)
        {
            DataRange<int> range = new DataRange<int>(minimum, maximum);

            range.Minimum.ShouldBe(minimum);
            range.Maximum.ShouldBe(maximum);
        }


        [Fact]
        public void Int32_RangeWithMaximumLessThanMinimumCannotBeConstructredUsing()
        {
            Assert.Throws<ArgumentException>(() => new DataRange<int>(5, 0));
        }


        [Fact]
        public void Int32_RangeWithSameMaximumAndMinimumCannotBeConstructed()
        {
            Assert.Throws<ArgumentException>(() => new DataRange<int>(0, 0));
        }

        [Fact]
        public void Int32_ContainsValue_ShouldReturnTrue_ForMinimumValueInRange()
        {
            DataRange<int> range = new DataRange<int>(0, 100);

            var result = range.ContainsValue(0);

            result.ShouldBeTrue();
        }

        [Fact]
        public void Int32_ContainsValue_ShouldReturnTrue_ForMaximumValueInRange()
        {
            DataRange<int> range = new DataRange<int>(0, 100);

            var result = range.ContainsValue(100);

            result.ShouldBeTrue();
        }

        [Fact]
        public void Int32_ContainsValue_ShouldReturnTrue_ForValueInRange()
        {
            DataRange<int> range = new DataRange<int>(0, 100);

            var result = range.ContainsValue(45);

            result.ShouldBeTrue();
        }

        [Fact]
        public void Int32_ContainsValue_ShouldReturnFalse_ForValueLowerThanRange()
        {
            DataRange<int> range = new DataRange<int>(25, 100);

            var result = range.ContainsValue(24);

            result.ShouldBeFalse();
        }


        [Fact]
        public void Int32_ContainsValue_ShouldReturnFalse_ForValueGreaterThanRange()
        {
            DataRange<int> range = new DataRange<int>(0, 100);

            var result = range.ContainsValue(101);

            result.ShouldBeFalse();
        }




        [Fact]
        public void Int32_ContainsRange_ShouldReturnTrue_ForRangeWithSameMinAndMax()
        {
            DataRange<int> range = new DataRange<int>(0, 100);
            DataRange<int> other = new DataRange<int>(0, 100);

            var result = range.ContainsRange(other);

            result.ShouldBeTrue();
        }

        [Fact]
        public void Int32_ContainsRange_ShouldReturnTrue_ForRangeWithSameMinAndLowerMax()
        {
            DataRange<int> range = new DataRange<int>(0, 100);
            DataRange<int> other = new DataRange<int>(0, 50);

            var result = range.ContainsRange(other);

            result.ShouldBeTrue();
        }

        [Fact]
        public void Int32_ContainsRange_ShouldReturnTrue_ForRangeWithGreaterMinAndSameMax()
        {
            DataRange<int> range = new DataRange<int>(0, 100);
            DataRange<int> other = new DataRange<int>(25, 100);

            var result = range.ContainsRange(other);

            result.ShouldBeTrue();
        }


        [Fact]
        public void Int32_ContainsRange_ShouldReturnTrue_ForRangeFullyContained()
        {
            DataRange<int> range = new DataRange<int>(0, 100);
            DataRange<int> other = new DataRange<int>(25, 75);

            var result = range.ContainsRange(other);

            result.ShouldBeTrue();
        }

        [Fact]
        public void Int32_ContainsRange_ShouldReturnFalse_ForRangeWithLowerMinAndGreaterMax()
        {
            DataRange<int> range = new DataRange<int>(10, 20);
            DataRange<int> other = new DataRange<int>(5, 25);

            var result = range.ContainsRange(other);

            result.ShouldBeFalse();
        }


        [Fact]
        public void Int32_ContainsRange_ShouldReturnFalse_ForLowerOverlappingRange()
        {
            DataRange<int> range = new DataRange<int>(10, 20);
            DataRange<int> other = new DataRange<int>(5, 15);

            var result = range.ContainsRange(other);

            result.ShouldBeFalse();
        }


        [Fact]
        public void Int32_ContainsRange_ShouldReturnFalse_ForHigherOverlappingRange()
        {
            DataRange<int> range = new DataRange<int>(10, 20);
            DataRange<int> other = new DataRange<int>(15, 25);

            var result = range.ContainsRange(other);

            result.ShouldBeFalse();
        }



        [Fact]
        public void Int32_ContainsRange_ShouldReturnFalse_ForRangeWithSameMinGreaterMax()
        {
            DataRange<int> range = new DataRange<int>(10, 20);
            DataRange<int> other = new DataRange<int>(10, 25);

            var result = range.ContainsRange(other);

            result.ShouldBeFalse();
        }


        [Fact]
        public void Int32_ContainsRange_ShouldReturnFalse_ForRangeWithLowerMinSameMax()
        {
            DataRange<int> range = new DataRange<int>(10, 20);
            DataRange<int> other = new DataRange<int>(5, 20);

            var result = range.ContainsRange(other);

            result.ShouldBeFalse();
        }













        [Fact]
        public void Int32_IsInsideRange_ShouldReturnTrue_ForRangeWithSameMinAndMax()
        {
            DataRange<int> range = new DataRange<int>(0, 100);
            DataRange<int> other = new DataRange<int>(0, 100);

            var result = range.IsInsideRange(other);

            result.ShouldBeTrue();
        }

        [Fact]
        public void Int32_IsInsideRange_ShouldReturnTrue_ForRangeWithSameMinAndLowerMax()
        {
            DataRange<int> range = new DataRange<int>(0, 100);
            DataRange<int> other = new DataRange<int>(0, 50);

            var result = range.IsInsideRange(other);

            result.ShouldBeFalse();
        }

        [Fact]
        public void Int32_IsInsideRange_ShouldReturnTrue_ForRangeWithGreaterMinAndSameMax()
        {
            DataRange<int> range = new DataRange<int>(0, 100);
            DataRange<int> other = new DataRange<int>(25, 100);

            var result = range.IsInsideRange(other);

            result.ShouldBeFalse();
        }


        [Fact]
        public void Int32_IsInsideRange_ShouldReturnTrue_ForRangeFullyContained()
        {
            DataRange<int> range = new DataRange<int>(0, 100);
            DataRange<int> other = new DataRange<int>(25, 75);

            var result = range.IsInsideRange(other);

            result.ShouldBeFalse();
        }

        [Fact]
        public void Int32_IsInsideRange_ShouldReturnFalse_ForRangeWithLowerMinAndGreaterMax()
        {
            DataRange<int> range = new DataRange<int>(10, 20);
            DataRange<int> other = new DataRange<int>(5, 25);

            var result = range.IsInsideRange(other);

            result.ShouldBeTrue();
        }


        [Fact]
        public void Int32_IsInsideRange_ShouldReturnFalse_ForLowerOverlappingRange()
        {
            DataRange<int> range = new DataRange<int>(10, 20);
            DataRange<int> other = new DataRange<int>(5, 15);

            var result = range.IsInsideRange(other);

            result.ShouldBeFalse();
        }


        [Fact]
        public void Int32_IsInsideRange_ShouldReturnFalse_ForHigherOverlappingRange()
        {
            DataRange<int> range = new DataRange<int>(10, 20);
            DataRange<int> other = new DataRange<int>(15, 25);

            var result = range.IsInsideRange(other);

            result.ShouldBeFalse();
        }



        [Fact]
        public void Int32_IsInsideRange_ShouldReturnFalse_ForRangeWithSameMinGreaterMax()
        {
            DataRange<int> range = new DataRange<int>(10, 20);
            DataRange<int> other = new DataRange<int>(10, 25);

            var result = range.IsInsideRange(other);

            result.ShouldBeTrue();
        }


        [Fact]
        public void Int32_IsInsideRange_ShouldReturnFalse_ForRangeWithLowerMinSameMax()
        {
            DataRange<int> range = new DataRange<int>(10, 20);
            DataRange<int> other = new DataRange<int>(5, 20);

            var result = range.IsInsideRange(other);

            result.ShouldBeTrue();
        }

        [Fact]
        public void Int32_ToString_ReturnsProperlyFormattedString()
        {
            DataRange<int> range = new DataRange<int>(10, 20);

            var result = range.ToString();

            result.ShouldBe("[10-20]");
        }




    }
}
