using DavidBerry.Framework.Util;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DavidBerry.Framework.Tests.Util
{
    public class ChecksumExtensionsTests
    {



        [Fact]
        public void GetChecksum_FromByteArray_ReturnsCorrectChecksum_ForMd5()
        {
            // Arrange
            byte[] data = Encoding.UTF8.GetBytes("This is a test string");

            // Act
            var checksum = data.GetChecksum(ChecksumAlgorithm.MD5);

            // Assert
            checksum.ToHexadecimalString().ShouldBe("C639EFC1E98762233743A75E7798DD9C");
        }

        [Fact]
        public void GetChecksum_FromByteArray_ReturnsCorrectChecksum_ForSha1()
        {
            // Arrange
            byte[] data = Encoding.UTF8.GetBytes("This is a test string");

            // Act
            var checksum = data.GetChecksum(ChecksumAlgorithm.SHA1);

            // Assert
            checksum.ToHexadecimalString().ShouldBe("E2F67C772368ACDEEE6A2242C535C6CC28D8E0ED");
        }

        [Fact]
        public void GetChecksum_FromByteArray_ReturnsCorrectChecksum_ForSha256()
        {
            // Arrange
            byte[] data = Encoding.UTF8.GetBytes("This is a test string");

            // Act
            var checksum = data.GetChecksum(ChecksumAlgorithm.SHA256);

            // Assert
            checksum.ToHexadecimalString().ShouldBe("717AC506950DA0CCB6404CDD5E7591F72018A20CBCA27C8A423E9C9E5626AC61");
        }


        [Fact]
        public void GetChecksum_FromByteArray_ReturnsCorrectChecksum_ForSha384()
        {
            // Arrange
            byte[] data = Encoding.UTF8.GetBytes("This is a test string");

            // Act
            var checksum = data.GetChecksum(ChecksumAlgorithm.SHA384);

            // Assert
            checksum.ToHexadecimalString().ShouldBe("9BD1F75EB75C8FFAD8F4B4C67C8F14DB32CC3D4177B942334ABD47F9E02E35B371D599CB4796185D7410E808F046E119");
        }


        [Fact]
        public void GetChecksum_FromByteArray_ReturnsCorrectChecksum_ForSha512()
        {
            // Arrange
            byte[] data = Encoding.UTF8.GetBytes("This is a test string");

            // Act
            var checksum = data.GetChecksum(ChecksumAlgorithm.SHA512);

            // Assert
            checksum.ToHexadecimalString().ShouldBe("B8EE69B29956B0B56E26D0A25C6A80713C858CF2902A12962AAD08D682345646B2D5F193BBE03997543A9285E5932F34BAF2C85C89459F25BA1CF43C4410793C");
        }


        [Fact]
        public void GetChecksum_FromByteArray_ReturnsCorrectChecksum_ForSha3_256()
        {
            // Arrange
            byte[] data = Encoding.UTF8.GetBytes("This is a test string");

            // Act
            var checksum = data.GetChecksum(ChecksumAlgorithm.SHA3_256);

            // Assert
            checksum.ToHexadecimalString().ShouldBe("22CDA3A2D2053F0A81FBF89AC531F724989F2308F0A2F29D894B07B4FEC0320E");
        }


        [Fact]
        public void GetChecksum_FromByteArray_ReturnsCorrectChecksum_ForSha3_384()
        {
            // Arrange
            byte[] data = Encoding.UTF8.GetBytes("This is a test string");

            // Act
            var checksum = data.GetChecksum(ChecksumAlgorithm.SHA3_384);

            // Assert
            checksum.ToHexadecimalString().ShouldBe("E31B4363776B1BC445BB5EE749ADDCB19F4CF22457FC36376D06CFE150D1E5C9D963A1D50E3AA8E65737E7C26BF817AE");
        }


        [Fact]
        public void GetChecksum_FromByteArray_ReturnsCorrectChecksum_ForSha3_512()
        {
            // Arrange
            byte[] data = Encoding.UTF8.GetBytes("This is a test string");

            // Act
            var checksum = data.GetChecksum(ChecksumAlgorithm.SHA3_512);

            // Assert
            checksum.ToHexadecimalString().ShouldBe("B8841178726F493E85FD41ED891CEC32B57781E707162ABF954C0D5E1C713262B84B37C68229017C698344CD8C7E0B4E7C1496DF7C303CFEA9F201A4DE9D12B2");
        }

    }
}
