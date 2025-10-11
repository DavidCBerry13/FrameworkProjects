using DavidBerry.Framework.Util;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DavidBerry.Framework.Tests.Util;

public class EmbeddedResourceUtilTests
{



    [Fact]
    public void ReadEmbeddedResourceTextFile_CorrectlyReadsTextFile()
    {
        // Arrange
        var assembly = typeof(EmbeddedResourceUtilTests).Assembly;
        var filename = "EmbeddedResources.ExampleFile.txt";

        // Act
        var contents = assembly.ReadEmbeddedResourceTextFile(filename);

        // Assert
        contents.ShouldBe("This is a test embedded resource file.");
    }


    [Fact]
    public void ReadEmbeddedResourceBinaryFile_CorrectlyReadsBinaryFile()
    {
        // Arrange
        var assembly = typeof(EmbeddedResourceUtilTests).Assembly;
        var filename = "EmbeddedResources.dotnet-logo.png";

        // Act
        var bytes = assembly.ReadEmbeddedResourceBinaryFile(filename);

        // Assert
        using (var sha256 = SHA256.Create())
        {
            var checksum = sha256.ComputeHash(bytes);
            checksum.ToHexadecimalString().ShouldBe("8FCF6F6CD575C0F8C643691765A7DB2A4B3B104BFBFF34646555F5CCFFDB2895");
        }
    }


}




