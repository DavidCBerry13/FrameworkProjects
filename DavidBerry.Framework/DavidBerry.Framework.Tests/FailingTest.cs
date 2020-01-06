using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DavidBerry.Framework.Tests
{
    public class FailingTest
    {

        [Fact]
        public void AlwaysFails()
        {
            Assert.True(false);
        }

    }
}
