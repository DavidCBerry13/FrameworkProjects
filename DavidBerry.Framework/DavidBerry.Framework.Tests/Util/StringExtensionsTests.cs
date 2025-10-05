using DavidBerry.Framework.Util;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DavidBerry.Framework.Tests.Util;


public class StringExtensionsTests
{

    [Fact]
    public void StringLongerThanSpecifiedLength_IsTruncated()
    {
        // Arrange
        string value = "This is a test string";

        // Act
        string truncated = value.Truncate(10);

        // Assert
        truncated.ShouldBe("This is a ");
    }


    [Fact]
    public void StringShorterThanSpecifiedLength_IsNotTruncated()
    {
        // Arrange
        string value = "This is a test string";

        // Act
        string truncated = value.Truncate(100);

        // Assert
        truncated.ShouldBe("This is a test string");
    }


    [Fact]
    public void StringEqualToSpecifiedLength_IsNotTruncated()
    {
        // Arrange
        string value = "This is a test string!!";

        // Act
        string truncated = value.Truncate(24);

        // Assert
        truncated.ShouldBe("This is a test string!!");
    }




    [Fact]
    public void ToTitleCase_ConvertsStringToTitleCaseCorrectly_ForEnUsCulture()
    {
        // Arrange
        string value = "this is a test string";
        CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");

        // Act
        string titleCased = value.ToTitleCase(cultureInfo);

        // Assert
        titleCased.ShouldBe("This Is A Test String");
    }

    [Fact]
    public void ToByteArray_EmptyString_ReturnsEmptyByteArray()
    {
        // Arrange
        string value = string.Empty;
        Encoding encoding = Encoding.UTF8;

        // Act
        var result = value.ToByteArray(encoding);

        // Asset
        result.ShouldBe(Array.Empty<byte>());
    }

    [Fact]
    public void ToByteArray_ProperlyEncodesStringWithUtf8()
    {
        // Arrange
        string value = "Chicago Illinois";
        Encoding encoding = Encoding.UTF8;

        // Act
        var result = value.ToByteArray(encoding);

        // Asset
        result.ShouldBe(new byte[] { 0x43, 0x68, 0x69, 0x63, 0x61, 0x67, 0x6f, 0x20, 0x49, 0x6c, 0x6c, 0x69, 0x6e, 0x6f, 0x69, 0x73 });
    }

    [Fact]
    public void ToByteArray_ProperlyEncodesStringWithAscii()
    {
        // Arrange
        string value = "Chicago Illinois";
        Encoding encoding = Encoding.ASCII;
        // Act
        var result = value.ToByteArray(encoding);
        // Asset
        result.ShouldBe(new byte[] { 0x43, 0x68, 0x69, 0x63, 0x61, 0x67, 0x6f, 0x20, 0x49, 0x6c, 0x6c, 0x69, 0x6e, 0x6f, 0x69, 0x73 });
    }

    [Fact]
    public void ToByteArray_ProperlyEncodesStringWithUnicode()
    {
        // Arrange
        string value = "Chicago Illinois";
        Encoding encoding = Encoding.Unicode;

        // Act
        var result = value.ToByteArray(encoding);

        // Asset
        result.ShouldBe(new byte[] { 0x43, 0x00, 0x68, 0x00, 0x69, 0x00, 0x63, 0x00, 0x61, 0x00, 0x67, 0x00, 0x6f, 0x00, 0x20, 0x00, 0x49, 0x00, 0x6c, 0x00, 0x6c, 0x00, 0x69, 0x00, 0x6e, 0x00, 0x6f, 0x00, 0x69, 0x00, 0x73, 0x00 });
    }

    [Fact]
    public void ToByteArray_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string value = null;
        Encoding encoding = Encoding.UTF8;

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => value.ToByteArray(encoding));
    }

    [Fact]
    public void ToByteArray_NullEncoding_ThrowsArgumentNullException()
    {
        // Arrange
        string value = "Chicago Illinois";
        Encoding encoding = null;

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => value.ToByteArray(encoding));
    }

    [Fact]
    public void ToBase64String_ProperlyConvertsStringToBase64()
    {
        // Arrange
        byte[] bytes = new byte[] { 0x43, 0x68, 0x69, 0x63, 0x61, 0x67, 0x6f, 0x20, 0x49, 0x6c, 0x6c, 0x69, 0x6e, 0x6f, 0x69, 0x73 };

        // Act
        var result = bytes.ToBase64String();

        // Asset
        result.ShouldBe("Q2hpY2FnbyBJbGxpbm9pcw==");
    }

    [Fact]
    public void ConvertToString_EmptyByteArray_ReturnsEmptyString()
    {
        // Arrange
        byte[] bytes = Array.Empty<byte>();
        Encoding encoding = Encoding.UTF8;

        // Act
        var result = bytes.ConvertToString(encoding);

        // Asset
        result.ShouldBe(string.Empty);
    }

    [Fact]
    public void ConvertToString_ProperlyDecodesByteArrayWithUtf8()
    {
        // Arrange
        byte[] bytes = new byte[] { 0x43, 0x68, 0x69, 0x63, 0x61, 0x67, 0x6f, 0x20, 0x49, 0x6c, 0x6c, 0x69, 0x6e, 0x6f, 0x69, 0x73 };
        Encoding encoding = Encoding.UTF8;

        // Act
        var result = bytes.ConvertToString(encoding);

        // Asset
        result.ShouldBe("Chicago Illinois");
    }

    [Fact]
    public void ConvertToString_ProperlyDecodesByteArrayWithAscii()
    {
        // Arrange
        byte[] bytes = new byte[] { 0x43, 0x68, 0x69, 0x63, 0x61, 0x67, 0x6f, 0x20, 0x49, 0x6c, 0x6c, 0x69, 0x6e, 0x6f, 0x69, 0x73 };
        Encoding encoding = Encoding.ASCII;

        // Act
        var result = bytes.ConvertToString(encoding);

        // Asset
        result.ShouldBe("Chicago Illinois");
    }

    [Fact]
    public void ConvertToString_ProperlyDecodesByteArrayWithUnicode()
    {
        // Arrange
        byte[] bytes = new byte[] { 0x43, 0x00, 0x68, 0x00, 0x69, 0x00, 0x63, 0x00, 0x61, 0x00, 0x67, 0x00, 0x6f, 0x00, 0x20, 0x00, 0x49, 0x00, 0x6c, 0x00, 0x6c, 0x00, 0x69, 0x00, 0x6e, 0x00, 0x6f, 0x00, 0x69, 0x00, 0x73, 0x00 };
        Encoding encoding = Encoding.Unicode;

        // Act
        var result = bytes.ConvertToString(encoding);

        // Asset
        result.ShouldBe("Chicago Illinois");
    }

}

