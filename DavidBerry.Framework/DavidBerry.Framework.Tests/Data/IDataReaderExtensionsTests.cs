using DavidBerry.Framework.Data;
using DavidBerry.Framework.Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DavidBerry.Framework.Tests.Data
{
    public class IDataReaderExtensionsTests
    {

        #region GetBoolean

        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void GetBooleanByName_WorksAsExpected_WhenPassingColumnName(bool value)
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetBoolean(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetBoolean(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetBoolean(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void GetBooleanNullable_ReturnsValue_WhenValueNotNull(bool value)
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetBoolean(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetBooleanNullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetBoolean(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Fact]
        public void GetBooleanNullable_ReturnsNull_WhenValueIsNull()
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetBooleanNullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetBoolean(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }



        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void GetBooleanNullableByName_ReturnsValue_WhenValueIsNotNull(bool value)
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetBoolean(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetBooleanNullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetBoolean(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Fact()]
        public void GetBooleanNullableByName_ReturnsNull_WhenValueIsNull()
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);

            // Act
            var result = mockDataReader.Object.GetBooleanNullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetBoolean(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }


        [Theory()]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void GetBooleanWithDefault_ReturnsValue_WhenValueIsNotNull(bool databaseValue, bool defaultValue)
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetBoolean(ordinalPosition)).Returns(databaseValue);

            // Act
            var result = mockDataReader.Object.GetBooleanWithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetBoolean(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }


        [Theory()]
        [InlineData(true)]
        [InlineData(false)]
        public void GetBooleanWithDefault_ReturnsDefault_WhenValueIsNull(bool defaultValue)
        {
            // Data
            int ordinalPosition = 10;


            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetBooleanWithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetBoolean(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }


        [Theory()]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void GetBooleanWithDefaultByName_ReturnsValue_WhenValueIsNotNull(bool databaseValue, bool defaultValue)
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetBoolean(ordinalPosition)).Returns(databaseValue);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetBooleanWithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetBoolean(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }



        [Theory()]
        [InlineData(false)]
        [InlineData(true)]
        public void GetBooleanWithDefaultByName_ReturnsDefault_WhenValueIsNull(bool defaultValue)
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetBooleanWithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetBoolean(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }


        #endregion




        #region Int16

        [Theory()]
        [InlineData(32)]
        [InlineData(-46)]
        public void GetInt16ByName_WorksAsExpected_WhenPassingColumnName(short value)
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetInt16(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetInt16(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetInt16(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Theory()]
        [InlineData(342)]
        [InlineData(-292)]
        public void GetInt16Nullable_ReturnsValue_WhenValueNotNull(short value)
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetInt16(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetInt16Nullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetInt16(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Fact]
        public void GetInt16Nullable_ReturnsNull_WhenValueIsNull()
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetInt16Nullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetInt16(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }



        [Theory()]
        [InlineData(3782)]
        [InlineData(-292)]
        public void GetInt16NullableByName_ReturnsValue_WhenValueIsNotNull(short value)
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetInt16(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetInt16Nullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetInt16(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Fact()]
        public void GetInt16NullableByName_ReturnsNull_WhenValueIsNull()
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);

            // Act
            var result = mockDataReader.Object.GetInt16Nullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetInt16(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }


        [Theory()]
        [InlineData(324, 0)]
        [InlineData(0, 100)]
        public void GetInt16WithDefault_ReturnsValue_WhenValueIsNotNull(short databaseValue, short defaultValue)
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetInt16(ordinalPosition)).Returns(databaseValue);

            // Act
            var result = mockDataReader.Object.GetInt16WithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetInt16(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }


        [Theory()]
        [InlineData(0)]
        [InlineData(100)]
        public void GetInt16WithDefault_ReturnsDefault_WhenValueIsNull(short defaultValue)
        {
            // Data
            int ordinalPosition = 10;


            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetInt16WithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetInt16(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }


        [Theory()]
        [InlineData(100, 0)]
        [InlineData(0, 10)]
        public void GetInt16WithDefaultByName_ReturnsValue_WhenValueIsNotNull(short databaseValue, short defaultValue)
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetInt16(ordinalPosition)).Returns(databaseValue);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetInt16WithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetInt16(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }



        [Theory()]
        [InlineData(0)]
        [InlineData(100)]
        public void GetInt16WithDefaultByName_ReturnsDefault_WhenValueIsNull(short defaultValue)
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetInt16WithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetInt16(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }

        #endregion


        #region Int32

        [Theory()]
        [InlineData(32)]
        [InlineData(-46)]
        public void GetInt32ByName_WorksAsExpected_WhenPassingColumnName(int value)
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetInt32(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetInt32(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetInt32(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Theory()]
        [InlineData(342)]
        [InlineData(-292)]
        public void GetInt32Nullable_ReturnsValue_WhenValueNotNull(int value)
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetInt32(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetInt32Nullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetInt32(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Fact]
        public void GetInt32Nullable_ReturnsNull_WhenValueIsNull()
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetInt32Nullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetInt32(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }



        [Theory()]
        [InlineData(3782)]
        [InlineData(-292)]
        public void GetInt32NullableByName_ReturnsValue_WhenValueIsNotNull(int value)
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetInt32(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetInt32Nullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetInt32(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Fact()]
        public void GetInt32NullableByName_ReturnsNull_WhenValueIsNull()
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);

            // Act
            var result = mockDataReader.Object.GetInt32Nullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetInt32(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }


        [Theory()]
        [InlineData(324, 0)]
        [InlineData(0, 100)]
        public void Getint32WithDefault_ReturnsValue_WhenValueIsNotNull(int databaseValue, int defaultValue)
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetInt32(ordinalPosition)).Returns(databaseValue);

            // Act
            var result = mockDataReader.Object.GetInt32WithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetInt32(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }


        [Theory()]
        [InlineData(0)]
        [InlineData(100)]
        public void GetInt32WithDefault_ReturnsDefault_WhenValueIsNull(int defaultValue)
        {
            // Data
            int ordinalPosition = 10;


            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetInt32WithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetInt32(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }


        [Theory()]
        [InlineData(100, 0)]
        [InlineData(0, 10)]
        public void GetInt32WithDefaultByName_ReturnsValue_WhenValueIsNotNull(int databaseValue, int defaultValue)
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetInt32(ordinalPosition)).Returns(databaseValue);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetInt32WithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetInt32(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }



        [Theory()]
        [InlineData(0)]
        [InlineData(100)]
        public void GetInt32WithDefaultByName_ReturnsDefault_WhenValueIsNull(int defaultValue)
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetInt32WithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetInt32(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }

        #endregion




        #region Int64

        [Theory()]
        [InlineData(32)]
        [InlineData(-46)]
        public void GetInt64ByName_WorksAsExpected_WhenPassingColumnName(long value)
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetInt64(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetInt64(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetInt64(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Theory()]
        [InlineData(342)]
        [InlineData(-292)]
        public void GetInt64Nullable_ReturnsValue_WhenValueNotNull(long value)
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetInt64(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetInt64Nullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetInt64(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Fact]
        public void GetInt64Nullable_ReturnsNull_WhenValueIsNull()
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetInt64Nullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetInt64(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }



        [Theory()]
        [InlineData(3782)]
        [InlineData(-292)]
        public void GetInt64NullableByName_ReturnsValue_WhenValueIsNotNull(long value)
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetInt64(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetInt64Nullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetInt64(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Fact()]
        public void GetInt64NullableByName_ReturnsNull_WhenValueIsNull()
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);

            // Act
            var result = mockDataReader.Object.GetInt64Nullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetInt64(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }


        [Theory()]
        [InlineData(324, 0)]
        [InlineData(0, 100)]
        public void GetInt64WithDefault_ReturnsValue_WhenValueIsNotNull(long databaseValue, long defaultValue)
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetInt64(ordinalPosition)).Returns(databaseValue);

            // Act
            var result = mockDataReader.Object.GetInt64WithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetInt64(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }


        [Theory()]
        [InlineData(0)]
        [InlineData(100)]
        public void GetInt64WithDefault_ReturnsDefault_WhenValueIsNull(long defaultValue)
        {
            // Data
            int ordinalPosition = 10;


            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetInt64WithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetInt64(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }


        [Theory()]
        [InlineData(100, 0)]
        [InlineData(0, 10)]
        public void GetInt64WithDefaultByName_ReturnsValue_WhenValueIsNotNull(long databaseValue, long defaultValue)
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetInt64(ordinalPosition)).Returns(databaseValue);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetInt64WithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetInt64(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }



        [Theory()]
        [InlineData(0)]
        [InlineData(100)]
        public void GetInt64WithDefaultByName_ReturnsDefault_WhenValueIsNull(long defaultValue)
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetInt64WithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetInt64(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }

        #endregion




        #region Float

        [Theory()]
        [InlineData(32.7)]
        [InlineData(-46.9)]
        public void GetFloatByName_WorksAsExpected_WhenPassingColumnName(float value)
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetFloat(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetFloat(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetFloat(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Theory()]
        [InlineData(342.3)]
        [InlineData(-292.3)]
        public void GetFloatNullable_ReturnsValue_WhenValueNotNull(float value)
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetFloat(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetFloatNullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetFloat(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Fact]
        public void GetFloatNullable_ReturnsNull_WhenValueIsNull()
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetFloatNullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetFloat(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }



        [Theory()]
        [InlineData(3782.32)]
        [InlineData(-292.39)]
        public void GetFloatNullableByName_ReturnsValue_WhenValueIsNotNull(float value)
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetFloat(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetFloatNullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetFloat(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Fact()]
        public void GetFloatNullableByName_ReturnsNull_WhenValueIsNull()
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);

            // Act
            var result = mockDataReader.Object.GetFloatNullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetFloat(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }


        [Theory()]
        [InlineData(324, 0.0)]
        [InlineData(0.0, 100.25)]
        public void GetFloatWithDefault_ReturnsValue_WhenValueIsNotNull(float databaseValue, float defaultValue)
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetFloat(ordinalPosition)).Returns(databaseValue);

            // Act
            var result = mockDataReader.Object.GetFloatWithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetFloat(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }


        [Theory()]
        [InlineData(0.0)]
        [InlineData(100.4)]
        public void GetFloatWithDefault_ReturnsDefault_WhenValueIsNull(float defaultValue)
        {
            // Data
            int ordinalPosition = 10;


            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetFloatWithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetFloat(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }


        [Theory()]
        [InlineData(100.82, 0.0)]
        [InlineData(0.0, 10.62)]
        public void GetFloatWithDefaultByName_ReturnsValue_WhenValueIsNotNull(float databaseValue, float defaultValue)
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetFloat(ordinalPosition)).Returns(databaseValue);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetFloatWithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetFloat(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }



        [Theory()]
        [InlineData(0.0)]
        [InlineData(100.50)]
        public void GetFloatWithDefaultByName_ReturnsDefault_WhenValueIsNull(float defaultValue)
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetFloatWithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetFloat(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }

        #endregion


        #region Double

        [Theory()]
        [InlineData(32.7)]
        [InlineData(-46.9)]
        public void GetDoubleByName_WorksAsExpected_WhenPassingColumnName(double value)
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetDouble(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetDouble(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetDouble(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Theory()]
        [InlineData(342.3)]
        [InlineData(-292.3)]
        public void GetDoubleNullable_ReturnsValue_WhenValueNotNull(double value)
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetDouble(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetDoubleNullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetDouble(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Fact]
        public void GetDoubleNullable_ReturnsNull_WhenValueIsNull()
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetDoubleNullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetDouble(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }



        [Theory()]
        [InlineData(3782.32)]
        [InlineData(-292.39)]
        public void GetDoubleNullableByName_ReturnsValue_WhenValueIsNotNull(double value)
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetDouble(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetDoubleNullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetDouble(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Fact()]
        public void GetDoubleNullableByName_ReturnsNull_WhenValueIsNull()
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);

            // Act
            var result = mockDataReader.Object.GetDoubleNullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetDouble(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }


        [Theory()]
        [InlineData(324, 0.0)]
        [InlineData(0.0, 100.25)]
        public void GetDoubleWithDefault_ReturnsValue_WhenValueIsNotNull(double databaseValue, double defaultValue)
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetDouble(ordinalPosition)).Returns(databaseValue);

            // Act
            var result = mockDataReader.Object.GetDoubleWithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetDouble(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }


        [Theory()]
        [InlineData(0.0)]
        [InlineData(100.4)]
        public void GetDoubleWithDefault_ReturnsDefault_WhenValueIsNull(double defaultValue)
        {
            // Data
            int ordinalPosition = 10;


            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetDoubleWithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetDouble(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }


        [Theory()]
        [InlineData(100.82, 0.0)]
        [InlineData(0.0, 10.62)]
        public void GetDoubleWithDefaultByName_ReturnsValue_WhenValueIsNotNull(double databaseValue, double defaultValue)
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetDouble(ordinalPosition)).Returns(databaseValue);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetDoubleWithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetDouble(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }



        [Theory()]
        [InlineData(0.0)]
        [InlineData(100.50)]
        public void GetDoubleWithDefaultByName_ReturnsDefault_WhenValueIsNull(double defaultValue)
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetDoubleWithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetDouble(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }

        #endregion



        #region String

        [Theory()]
        [InlineData("Lorum Ipsum")]
        [InlineData("")]
        public void GetStringByName_WorksAsExpected_WhenPassingColumnName(string databaseValue)
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetString(ordinalPosition)).Returns(databaseValue);

            // Act
            var result = mockDataReader.Object.GetString(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetString(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
        }


        [Theory()]
        [InlineData("Lorum Ipsum")]
        [InlineData("")]
        public void GetStringNullable_ReturnsValue_WhenValueNotNull(string databaseValue)
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetString(ordinalPosition)).Returns(databaseValue);

            // Act
            var result = mockDataReader.Object.GetStringNullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetString(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
        }


        [Fact]
        public void GetStringNullable_ReturnsNull_WhenValueIsNull()
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetStringNullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetString(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }



        [Theory()]
        [InlineData("Lorum Ipsum")]
        [InlineData("")]
        public void GetStringNullableByName_ReturnsValue_WhenValueIsNotNull(string value)
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetString(ordinalPosition)).Returns(value);

            // Act
            var result = mockDataReader.Object.GetStringNullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetString(ordinalPosition), Times.Once);
            result.ShouldBe(value);
        }


        [Fact()]
        public void GetStringNullableByName_ReturnsNull_WhenValueIsNull()
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);

            // Act
            var result = mockDataReader.Object.GetStringNullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetString(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }


        [Theory()]
        [InlineData("Green", "Red")]
        [InlineData("Blue", "")]
        public void GetStringWithDefault_ReturnsValue_WhenValueIsNotNull(string databaseValue, string defaultValue)
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetString(ordinalPosition)).Returns(databaseValue);

            // Act
            var result = mockDataReader.Object.GetStringWithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetString(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }


        [Theory()]
        [InlineData("Orange")]
        [InlineData("")]
        public void GetStringWithDefault_ReturnsDefault_WhenValueIsNull(string defaultValue)
        {
            // Data
            int ordinalPosition = 10;


            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetStringWithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetString(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }


        [Theory()]
        [InlineData("Green", "Red")]
        [InlineData("Blue", "")]
        public void GetStringWithDefaultByName_ReturnsValue_WhenValueIsNotNull(string databaseValue, string defaultValue)
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetString(ordinalPosition)).Returns(databaseValue);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetStringWithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetString(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }



        [Theory()]
        [InlineData("Orange")]
        [InlineData("")]
        public void GetStringWithDefaultByName_ReturnsDefault_WhenValueIsNull(string defaultValue)
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetStringWithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetString(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }

        #endregion



        #region DateTime


        [Fact]
        public void GetDateTimeByName_WorksAsExpected_WhenPassingColumnName()
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;
            DateTime databaseValue = new DateTime(2025, 10, 05, 10, 30, 10);

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetDateTime(ordinalPosition)).Returns(databaseValue);

            // Act
            var result = mockDataReader.Object.GetDateTime(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetDateTime(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
        }


        [Fact]
        public void GetDateTimeNullable_ReturnsValue_WhenValueNotNull()
        {
            // Data
            int ordinalPosition = 10;
            DateTime databaseValue = new DateTime(2025, 10, 05, 10, 30, 10);

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetDateTime(ordinalPosition)).Returns(databaseValue);

            // Act
            var result = mockDataReader.Object.GetDateTimeNullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetDateTime(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
        }


        [Fact]
        public void GetDateTimeNullable_ReturnsNull_WhenValueIsNull()
        {
            // Data
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetDateTimeNullable(ordinalPosition);

            // Assert
            mockDataReader.Verify(x => x.GetDateTime(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }



        [Fact]
        public void GetDateTimeNullableByName_ReturnsValue_WhenValueIsNotNull()
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;
            DateTime databaseValue = new DateTime(2025, 10, 05, 10, 30, 10);

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);
            mockDataReader.Setup(x => x.GetDateTime(ordinalPosition)).Returns(databaseValue);

            // Act
            var result = mockDataReader.Object.GetDateTimeNullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetDateTime(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
        }


        [Fact()]
        public void GetDateTimeNullableByName_ReturnsNull_WhenValueIsNull()
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);

            // Act
            var result = mockDataReader.Object.GetDateTimeNullable(columnName);

            // Assert
            mockDataReader.Verify(x => x.GetDateTime(ordinalPosition), Times.Never);
            result.ShouldBeNull();
        }


        [Fact]
        public void GetDateTimeWithDefault_ReturnsValue_WhenValueIsNotNull()
        {
            // Data
            int ordinalPosition = 10;
            DateTime databaseValue = new DateTime(2025, 10, 05, 10, 30, 10);
            DateTime defaultValue = new DateTime(2000, 1, 1);

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetDateTime(ordinalPosition)).Returns(databaseValue);

            // Act
            var result = mockDataReader.Object.GetDateTimeWithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetDateTime(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }


        [Fact]
        public void GetDateTimeWithDefault_ReturnsDefault_WhenValueIsNull()
        {
            // Data
            int ordinalPosition = 10;
            DateTime defaultValue = new DateTime(2000, 1, 1);

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);

            // Act
            var result = mockDataReader.Object.GetDateTimeWithDefault(ordinalPosition, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetDateTime(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }


        [Fact]
        public void GetDateTimeWithDefaultByName_ReturnsValue_WhenValueIsNotNull()
        {
            // Data
            string columnName = "IsActive";
            int ordinalPosition = 10;
            DateTime databaseValue = new DateTime(2025, 10, 05, 10, 30, 10);
            DateTime defaultValue = new DateTime(2000, 1, 1);

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(false);
            mockDataReader.Setup(x => x.GetDateTime(ordinalPosition)).Returns(databaseValue);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetDateTimeWithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetDateTime(ordinalPosition), Times.Once);
            result.ShouldBe(databaseValue);
            result.ShouldNotBe(defaultValue);
        }



        [Fact]
        public void GetDateTimeWithDefaultByName_ReturnsDefault_WhenValueIsNull()
        {
            // Data
            string columnName = "ColumnName";
            int ordinalPosition = 10;
            DateTime defaultValue = new DateTime(2000, 1, 1);

            // Arrange
            Mock<IDataReader> mockDataReader = new Mock<IDataReader>();
            mockDataReader.Setup(x => x.IsDBNull(ordinalPosition)).Returns(true);
            mockDataReader.Setup(x => x.GetOrdinal(columnName)).Returns(ordinalPosition);


            // Act
            var result = mockDataReader.Object.GetDateTimeWithDefault(columnName, defaultValue);

            // Assert
            mockDataReader.Verify(x => x.GetDateTime(ordinalPosition), Times.Never);
            result.ShouldBe(defaultValue);
        }

        #endregion



    }
}
