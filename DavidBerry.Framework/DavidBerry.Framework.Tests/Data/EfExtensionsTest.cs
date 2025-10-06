using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DavidBerry.Framework;
using DavidBerry.Framework.Data;
using DavidBerry.Framework.Domain;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace DavidBerry.Framework.Tests.Data
{
    public class EfExtensionsTest
    {

        [Fact]
        public void ObjectStateNew_ConvertedToEfStateAdded()
        {
            var efSate = EfExtensions.ConvertToEFState(ObjectState.NEW);

            efSate.ShouldBe(EntityState.Added);
        }

        [Fact]
        public void ObjectStateModified_ConvertedToEfStateModified()
        {
            var efSate = EfExtensions.ConvertToEFState(ObjectState.MODIFIED);

            efSate.ShouldBe(EntityState.Modified);
        }


        [Fact]
        public void ObjectStateDeleted_ConvertedToEfStateDelated()
        {
            var efSate = EfExtensions.ConvertToEFState(ObjectState.DELETED);

            efSate.ShouldBe(EntityState.Deleted);
        }


        [Fact]
        public void ObjectStateUnchangedd_ConvertedToEfStateUnchanged()
        {
            var efSate = EfExtensions.ConvertToEFState(ObjectState.UNCHANGED);

            efSate.ShouldBe(EntityState.Unchanged);
        }

    }
}
