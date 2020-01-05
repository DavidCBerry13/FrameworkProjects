using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using DavidBerry.Framework;
using DavidBerry.Framework.Data;
using DavidBerry.Framework.Domain;
using Microsoft.EntityFrameworkCore;

namespace DavidBerry.Framework.Tests.Data
{
    public class EfExtensionsTest
    {

        [Fact]
        public void ObjectStateNew_ConvertedToEfStateAdded()
        {
            var efSate = EfExtensions.ConvertToEFState(ObjectState.NEW);

            efSate.Should().Be(EntityState.Added);
        }

        [Fact]
        public void ObjectStateModified_ConvertedToEfStateModified()
        {
            var efSate = EfExtensions.ConvertToEFState(ObjectState.MODIFIED);

            efSate.Should().Be(EntityState.Modified);
        }


        [Fact]
        public void ObjectStateDeleted_ConvertedToEfStateDelated()
        {
            var efSate = EfExtensions.ConvertToEFState(ObjectState.DELETED);

            efSate.Should().Be(EntityState.Deleted);
        }


        [Fact]
        public void ObjectStateUnchangedd_ConvertedToEfStateUnchanged()
        {
            var efSate = EfExtensions.ConvertToEFState(ObjectState.UNCHANGED);

            efSate.Should().Be(EntityState.Unchanged);
        }

    }
}
