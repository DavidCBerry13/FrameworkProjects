using System;
using System.Collections.Generic;
using System.Text;
using DavidBerry.Framework.Util;
using Shouldly;
using Xunit;

namespace DavidBerry.Framework.Tests.Util
{
    public class CollectionExtensionsTests
    {
        [Fact]
        public void TestNullListReturnsTrueFor_IsNullOrEmpty()
        {
            List<string> states = null;

            var empty = states.IsNullOrEmpty();

            empty.ShouldBeTrue();
        }

        [Fact]
        public void TestEmptyListReturnsTrueFor_IsNullOrEmpty()
        {
            List<string> states = new List<string>();

            var empty = states.IsNullOrEmpty();

            empty.ShouldBeTrue();
        }

        [Fact]
        public void TestListWithElementsReturnsFalseFor_IsNullOrEmpty()
        {
            List<string> states = new List<string>() { "California", "New York", "Virginia", "Washington", "Wisconsin" };

            var empty = states.IsNullOrEmpty();

            empty.ShouldBeFalse();
        }

        [Fact]
        public void TestNullDictionaryReturnsTrueFor_IsNullOrEmpty()
        {
            Dictionary<string, string> states = null;

            var empty = states.IsNullOrEmpty();

            empty.ShouldBeTrue();
        }


        [Fact]
        public void TestEmptyDictionaryReturnsTrueFor_IsNullOrEmpty()
        {
            Dictionary<string, string> states = new Dictionary<string, string>();

            var empty = states.IsNullOrEmpty();

            empty.ShouldBeTrue();
        }


        [Fact]
        public void TestDictionaryWithElementsReturnsFalseFor_IsNullOrEmpty()
        {
            Dictionary<string, string> states = new Dictionary<string, string>()
            {
                { "WI", "Wisconsin" },
                { "IL", "Illinois" },
                { "MN", "Minnesota" },
                { "MI", "Michigan" },
                { "IA", "Iowa" },
            };

            var empty = states.IsNullOrEmpty();

            empty.ShouldBeFalse();
        }

    }
}
