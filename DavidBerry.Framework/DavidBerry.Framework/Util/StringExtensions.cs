using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Util;



public static class StringExtensions
{
    /// <summary>
    /// Truncates the given string to the specified maximum length
    /// </summary>
    /// <param name="value">A string to be truncated</param>
    /// <param name="maxLength">The maximum length of the string</param>
    /// <returns>A string of maxlength or less in length</returns>
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }



    /// <summary>
    /// Extension method which will get a Title Case version of the current string
    /// </summary>
    /// <remarks>
    /// This method is really just a convenience wrapper around the TextInfo class.  It uses
    /// the CultureInfo.CurrentCulture as the culture to produce a title case for
    /// </remarks>
    /// <param name="s">The String to convert to Title Case</param>
    /// <returns>A new String in Title Case</returns>
    public static String ToTitleCase(this String s)
    {
        ArgumentNullException.ThrowIfNull(s);

        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

        return textInfo.ToTitleCase(s.ToLower(CultureInfo.CurrentCulture));
    }



    /// <summary>
    /// Extension method which will get a Title Case version of the current string
    /// </summary>
    /// <remarks>
    /// This method is really just a convenience wrapper around the TextInfo class.  It uses
    /// the CultureInfo.CurrentCulture as the culture to produce a title case for
    /// </remarks>
    /// <param name="s">The String to convert to Title Case</param>
    /// <returns>A new String in Title Case</returns>
    public static String ToTitleCase(this String s, CultureInfo cultureInfo)
    {
        ArgumentException.ThrowIfNullOrEmpty(s);
        ArgumentNullException.ThrowIfNull(cultureInfo);

        TextInfo textInfo = cultureInfo.TextInfo;

        return textInfo.ToTitleCase(s.ToLower(CultureInfo.CurrentCulture));
    }


    /// <summary>
    /// Converts the given String to a byte array using the given encoding
    /// </summary>
    /// <param name="s">A String to be converted to a byte array</param>
    /// <param name="encoding">An Encoding object of the encoding to use</param>
    /// <returns>A byte array that represents the string</returns>
    public static byte[] ToByteArray(this String s, Encoding encoding)
    {
        ArgumentNullException.ThrowIfNull(s);
        ArgumentNullException.ThrowIfNull(encoding);

        return encoding.GetBytes(s);
    }

    /// <summary>
    /// Converts the given String to a byte array using a UTF8 encoding
    /// </summary>
    /// <param name="s">A String to be converted to a byte array</param>
    /// <returns>An array of bytes</returns>
    public static byte[] ToByteArray(this String s)
    {
        return StringExtensions.ToByteArray(s, Encoding.UTF8);
    }


    /// <summary>
    /// Converts the given byte array to a String using the given encoding
    /// </summary>
    /// <param name="bytes">A byte array to be converted</param>
    /// <param name="encoding">An encoding to use to decode the byte array</param>
    /// <returns>A String decoded from the byte array</returns>
    public static String ConvertToString(this byte[] bytes, Encoding encoding)
    {
        return encoding.GetString(bytes);
    }


    /// <summary>
    /// Converts the given byte array to a String using a UTF8 encoding
    /// </summary>
    /// <param name="bytes">A byte array to be converted</param>
    /// <returns>A String decoded from the byte array</returns>
    public static String ConvertToString(this byte[] bytes)
    {
        return StringExtensions.ConvertToString(bytes, Encoding.UTF8);
    }


    /// <summary>
    /// Converts the array of bytes into a Hexidecimal String
    /// </summary>
    /// <param name="bytes">An array of bytes to be converted into Hexidecimal</param>
    /// <returns>A Hexadecimal String representation of the byte array</returns>
    public static String ToHexadecimalString(this byte[] bytes)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            sb.Append(string.Format("{0:X2}", b));
        }

        return sb.ToString();
    }


    /// <summary>
    /// Extension method to convert an array of bytes to a Base 64 string
    /// </summary>
    /// <param name="bytes">An array of bytes to be converted</param>
    /// <returns>A Base64 encoded string</returns>
    public static String ToBase64String(this byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }


    /// <summary>
    /// Extension method to convert a Base64 encoded string to an array of bytes
    /// </summary>
    /// <param name="s">A Base64 encoded string</param>
    /// <returns>An array of bytes</returns>
    public static byte[] FromBase64String(this String s)
    {
        return Convert.FromBase64String(s);
    }



}

