using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using DavidBerry.Framework.Data;
using System.Data;
using FluentAssertions;

namespace DavidBerry.Framework.Tests.Data
{
    public class EfUnitOfWorkTests
    {

        [Fact]
        public void VerifyUnitOfWorkCallsContextSaveChanges()
        {
            // Arrange
            Mock<DbContext> mockContext = new Mock<DbContext>();
            
            // Act
            EfUnitOfWork<DbContext> uow = new EfUnitOfWork<DbContext>(mockContext.Object);
            uow.CommitChanges();

            // Assert
            mockContext.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }


        [Fact]
        public void VerifyDbUpdateConcurrencyException_TranslatedTo_DBConcurrencyException()
        {
            // Arrange
            Mock<DbContext> mockContext = new Mock<DbContext>();
            mockContext.Setup(ctx => ctx.SaveChanges())
                .Throws<DbUpdateConcurrencyException>();

            // Act and Assert
            EfUnitOfWork<DbContext> uow = new EfUnitOfWork<DbContext>(mockContext.Object);
            var exception = Assert.Throws<DBConcurrencyException>(() => uow.CommitChanges());
            exception.InnerException.Should().BeOfType<DbUpdateConcurrencyException>();
        }

    }
}
