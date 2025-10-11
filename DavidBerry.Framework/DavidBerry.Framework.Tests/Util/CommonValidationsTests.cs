using DavidBerry.Framework.Util;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace DavidBerry.Framework.Tests.Util;

public class CommonValidationsTests
{
    [Theory]
    [InlineData("AL")]
    [InlineData("AK")]
    [InlineData("AZ")]
    [InlineData("AR")]
    [InlineData("CA")]
    [InlineData("CO")]
    [InlineData("CT")]
    [InlineData("DE")]
    [InlineData("FL")]
    [InlineData("GA")]
    [InlineData("HI")]
    [InlineData("ID")]
    [InlineData("IL")]
    [InlineData("IN")]
    [InlineData("IA")]
    [InlineData("KS")]
    [InlineData("KY")]
    [InlineData("LA")]
    [InlineData("ME")]
    [InlineData("MD")]
    [InlineData("MA")]
    [InlineData("MI")]
    [InlineData("MN")]
    [InlineData("MS")]
    [InlineData("MO")]
    [InlineData("MT")]
    [InlineData("NE")]
    [InlineData("NV")]
    [InlineData("NH")]
    [InlineData("NJ")]
    [InlineData("NM")]
    [InlineData("NY")]
    [InlineData("NC")]
    [InlineData("ND")]
    [InlineData("OH")]
    [InlineData("OK")]
    [InlineData("OR")]
    [InlineData("PA")]
    [InlineData("RI")]
    [InlineData("SC")]
    [InlineData("SD")]
    [InlineData("TN")]
    [InlineData("TX")]
    [InlineData("UT")]
    [InlineData("VT")]
    [InlineData("VA")]
    [InlineData("WA")]
    [InlineData("WV")]
    [InlineData("WI")]
    [InlineData("WY")]
    public void US_States_Validation_WorksForAllStateAbbreviations(string state)
    {
        // Act
        var result = Regex.IsMatch(state, DavidBerry.Framework.Util.CommonValidations.US_STATES);

        // Assert
        result.ShouldBeTrue();
    }



    [Theory]
    [InlineData("al")]
    [InlineData("ak")]
    [InlineData("az")]
    [InlineData("ar")]
    [InlineData("ca")]
    [InlineData("co")]
    [InlineData("ct")]
    [InlineData("de")]
    [InlineData("fl")]
    [InlineData("ga")]
    [InlineData("hi")]
    [InlineData("id")]
    [InlineData("il")]
    [InlineData("in")]
    [InlineData("ia")]
    [InlineData("ks")]
    [InlineData("ky")]
    [InlineData("la")]
    [InlineData("me")]
    [InlineData("md")]
    [InlineData("ma")]
    [InlineData("mi")]
    [InlineData("mn")]
    [InlineData("ms")]
    [InlineData("mo")]
    [InlineData("mt")]
    [InlineData("ne")]
    [InlineData("nv")]
    [InlineData("nh")]
    [InlineData("nj")]
    [InlineData("nm")]
    [InlineData("ny")]
    [InlineData("nc")]
    [InlineData("nd")]
    [InlineData("oh")]
    [InlineData("ok")]
    [InlineData("or")]
    [InlineData("pa")]
    [InlineData("ri")]
    [InlineData("sc")]
    [InlineData("sr")]
    [InlineData("tn")]
    [InlineData("tx")]
    [InlineData("ut")]
    [InlineData("vt")]
    [InlineData("va")]
    [InlineData("wa")]
    [InlineData("wv")]
    [InlineData("wi")]
    [InlineData("wy")]
    public void US_States_Validation_ShouldFail_LowerCaseAbbreviations(string state)
    {
        // Act
        var result = Regex.IsMatch(state, DavidBerry.Framework.Util.CommonValidations.US_STATES);

        // Assert
        result.ShouldBeFalse();
    }


    [Theory]
    [InlineData("Alabama")]
    [InlineData("")]
    [InlineData("WYO")]
    [InlineData("California ")]
    [InlineData(" TX")]
    [InlineData("New York")]
    [InlineData("C")]
    public void US_States_Validation_ShouldFail_InvalidAbbreviations(string state)
    {
        // Act
        var result = Regex.IsMatch(state, CommonValidations.US_STATES);

        // Assert
        result.ShouldBeFalse();
    }


    [Theory]
    [InlineData("11201")]
    [InlineData("54911")]
    [InlineData("60062")]
    [InlineData("80401")]
    [InlineData("94130")]
    public void USZipCodes_Validation_WorksForValidZipCodes(string zipCode)
    {
        // Act
        var result = Regex.IsMatch(zipCode, CommonValidations.US_ZIP_CODES);

        // Assert
        result.ShouldBeTrue();
    }



    [Theory]
    [InlineData("1201")]
    [InlineData("54911 ")]
    [InlineData("6066A")]
    [InlineData("ZIPCD")]
    [InlineData("")]
    public void USZipCodes_Validation_FailsOnInvalidZipCodes(string zipCode)
    {
        // Act
        var result = Regex.IsMatch(zipCode, CommonValidations.US_ZIP_CODES);

        // Assert
        result.ShouldBeFalse();
    }


}


