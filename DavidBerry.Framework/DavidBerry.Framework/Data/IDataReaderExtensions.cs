using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Data;

/// <summary>
/// Class defining useful extension methods for the IDataReader interface.
/// </summary>
/// <remarks>
/// The extension methods fall into two categories in this interface.  First are extension methods
/// for dealing with null values in the database, both to check if a value is null and return an appropriate
/// null value from IDataReader and to retrn a default value when a database value is null.  The second
/// category of extension methods are those that allow you to specify the column name rather than the
/// ordinal position of the column in the result set.
/// </remarks>
public static class IDataReaderExtensions
{

    /// <summary>
    /// Gets the Type object of the column with the given name
    /// </summary>
    /// <param name="name">A string of the name of the column in the result set</param>
    /// <returns>A Type object of the datatype of the specified column</returns>
    public static Type GetFieldType(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return reader.GetFieldType(ordinal);
    }




    #region Get Data Methods - Boolean Values

    /// <summary>
    /// Gets a boolean value from the column with the given name from this IDataReader
    /// </summary>
    /// <remarks>
    /// This extension method allow you to specify the column name rather than the ordinal of the column.
    /// Internally, it looks up the column name for you
    /// </remarks>
    /// <param name="reader">A IDataReader that is the object this method is an extension for</param>
    /// <param name="name">A string of the column name to get the value from</param>
    /// <returns>A bool of the column value</returns>
    public static bool GetBoolean(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return reader.GetBoolean(ordinal);
    }


    /// <summary>
    /// Gets a nullable bool value from the column with the given ordinal index
    /// </summary>
    /// <remarks>
    /// Internally, this method handles calling the IDataReader.IsDbNull() method and if this returns
    /// true, a null value will be returned.
    /// </remarks>
    /// <param name="reader">A IDataReader that is the object this method is an extension for</param>
    /// <param name="i">An into fo the column ordinal</param>
    /// <returns>A nullable bool of the column value</returns>
    public static bool? GetBooleanNullable(this IDataReader reader, int i)
    {
        return reader.IsDBNull(i)
            ? null
            : reader.GetBoolean(i);
    }


    /// <summary>
    /// Gets a nullable bool value from the column with the given name
    /// </summary>
    /// <param name="reader">A IDataReader that is the object this method is an extension for</param>
    /// <param name="name">A string of the name of the column</param>
    /// <returns>A nullable bool value that represents the value for this column</returns>
    public static bool? GetBooleanNullable(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetBooleanNullable(reader, ordinal);
    }


    /// <summary>
    /// Gets a boolean value from the given column, returning the provided default value if the value in the database is null
    /// </summary>
    /// <param name="reader">A IDataReader object of the data reader to get the value from</param>
    /// <param name="i">An int of the index of the column</param>
    /// <param name="defaultValue">A bool of the default value to use if the value for this column is NULL</param>
    /// <returns>A bool of the value of the column to use (if the value was null in the DB, the provided default value is used)</returns>
    public static bool GetBooleanWithDefault(this IDataReader reader, int i, bool defaultValue)
    {
        if (reader.IsDBNull(i))
        {
            return defaultValue;
        }

        return reader.GetBoolean(i);
    }


    /// <summary>
    /// Gets a boolean value from the given column, returning the provided default value if the value in the database is null
    /// </summary>
    /// <param name="reader">A IDataReader object of the data reader to get the value from</param>
    /// <param name="name">A string of the name of the column to get the value from</param>
    /// <param name="defaultValue">A bool of the default value </param>
    /// <returns>A bool of the value of the column to use (if the value was null in the DB, the provided default value is used)</returns>
    public static bool GetBooleanWithDefault(this IDataReader reader, string name, bool defaultValue)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetBooleanWithDefault(reader, ordinal, defaultValue);
    }


    #endregion


    #region region Get Data Methods - Char Values

    /// <summary>
    /// Gets the char value for the named
    /// </summary>
    /// <param name="reader">A IDataReader object of the data reader to get the value from</param>
    /// <param name="name">A string of the name of the column</param>
    /// <returns>A char of the value in the column</returns>
    public static char GetChar(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return reader.GetChar(ordinal);
    }


    /// <summary>
    /// Gets a nullable char for the column at the given index
    /// </summary>
    /// <param name="reader">A IDataReader object of the data reader to get the value from</param>
    /// <param name="i">An int of the column index to get the value from</param>
    /// <returns>A nullable char.  If the value in the database is a null, null will be returned</returns>
    public static char? GetCharNullable(this IDataReader reader, int i)
    {
        if (reader.IsDBNull(i))
            return null;

        return reader.GetChar(i);
    }


    /// <summary>
    /// Gets a nullable char value for the named column
    /// </summary>
    /// <param name="reader">A IDataReader object of the data reader to get the value from</param>
    /// <param name="name">A string of the name of the column</param>
    /// <returns>A nullable char of the value in the database.  If the database value is null, null is returned</returns>
    public static char? GetCharNullable(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetCharNullable(reader, ordinal);
    }


    /// <summary>
    /// Get a char value from the database for the column at the given index, returning the default value
    /// if the value in the database is null
    /// </summary>
    /// <param name="reader">A IDataReader object of the data reader to get the value from</param>
    /// <param name="i">An int of the column index to get the value from</param>
    /// <param name="defaultValue">A char of the default value to use if the value in the column is null</param>
    /// <returns>A char of the value in the database or the default value if the database value was null</returns>
    public static char GetCharWithDefault(this IDataReader reader, int i, char defaultValue)
    {
        if (reader.IsDBNull(i))
            return defaultValue;

        return reader.GetChar(i);
    }


    /// <summary>
    /// Get a char value from the database for the named column, returning the default value if the value in the database is null
    /// </summary>
    /// <param name="reader">A IDataReader object of the data reader to get the value from</param>
    /// <param name="name">A string of the name of the column</param>
    /// <param name="defaultValue">A char of the default value to use if the value in the column is null</param>
    /// <returns>A char of the value in the database or the default value if the database value was null</returns>
    public static char GetCharWithDefault(this IDataReader reader, string name, char defaultValue)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetCharWithDefault(reader, ordinal, defaultValue);
    }

    #endregion


    #region Accessor Methods - string Values

    /// <summary>
    /// Gets the string value for the column with the given name
    /// </summary>
    /// <param name="reader">A IDataReader object of the data reader to get the value from</param>
    /// <param name="name">A string of the column name to get the value from</param>
    /// <returns>A string of the value in the column</returns>
    public static string GetString(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return reader.GetString(ordinal);
    }


    /// <summary>
    /// Get a string that can be null if the value in the database is null
    /// </summary>
    /// <param name="reader">A IDataReader object of the data reader to get the value from</param>
    /// <param name="index">An int of the column index of the column to read data from</param>
    /// <returns>A string of the value in the column.  If the database contains null, then null is returned</returns>
    public static string GetStringNullable(this IDataReader reader, int index)
    {
        if (reader.IsDBNull(index))
            return null;

        return reader.GetString(index);
    }


    /// <summary>
    /// Get a string that can be null if the value in the database is null
    /// </summary>
    /// <param name="reader">A IDataReader object of the data reader to get the value from</param>
    /// <param name="name">A string of the column name</param>
    /// <returns>A string of the value in the column.  If the database contains null, then null is returned</returns>
    public static string GetStringNullable(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetStringNullable(reader, ordinal);
    }


    /// <summary>
    /// Get a string value from the column at the given index, returning the provided default value if the
    /// value in the database is null
    /// </summary>
    /// <param name="reader">A IDataReader object of the data reader to get the value from</param>
    /// <param name="index">An int of the index of the column to read</param>
    /// <param name="defaultValue">A string of a default value to use if the database value is null</param>
    /// <returns>A string of the database value, or the default value if the database value was null</returns>
    public static string GetStringWithDefault(this IDataReader reader, int index, string defaultValue)
    {
        if (reader.IsDBNull(index))
            return defaultValue;

        return reader.GetString(index);
    }


    /// <summary>
    /// Get a string value from the column with the given name, returning the provided default value if the
    /// value in the database is null
    /// </summary>
    /// <param name="reader">A IDataReader object of the data reader to get the value from</param>
    /// <param name="name">A string of the name of the column</param>
    /// <param name="defaultValue">A string of a default value to use if the database value is null</param>
    /// <returns>A string of the database value, or the default value if the database value was null</returns>
    public static string GetStringWithDefault(this IDataReader reader, string name, string defaultValue)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetStringWithDefault(reader, ordinal, defaultValue);
    }


    #endregion




    #region Accessor Methods - Int32 Values


    public static int GetInt32(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return reader.GetInt32(ordinal);
    }


    public static int? GetInt32Nullable(this IDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal))
            return null;

        return reader.GetInt32(ordinal);
    }


    public static int? GetInt32Nullable(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetInt32Nullable(reader, ordinal);
    }



    public static int GetInt32WithDefault(this IDataReader reader, int ordinal, int defaultValue)
    {
        if (reader.IsDBNull(ordinal))
            return defaultValue;

        return reader.GetInt32(ordinal);
    }


    public static int GetInt32WithDefault(this IDataReader reader, string name, int defaultValue)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetInt32WithDefault(reader, ordinal, defaultValue);
    }


    #endregion


    #region Accessor Methods - Int16 Values

    /// <summary>
    /// Get the short (Int16) value from the column with the given name
    /// </summary>
    /// <param name="reader">A IDataReader object of the data reader to get the value from</param>
    /// <param name="name">A string of the name of the column</param>
    /// <returns>A short (int16) of the value in the column</returns>
    public static short GetInt16(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return reader.GetInt16(ordinal);
    }


    /// <summary>
    /// Get a nullable short value for the column with the given index
    /// </summary>
    /// <param name="reader">A IDataReader object of the data reader to get the value from</param>
    /// <param name="ordinal">An int of the column index of the column to read</param>
    /// <returns>A nullable short value.  If the value in the database is null, null will be returned</returns>
    public static short? GetInt16Nullable(this IDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal))
            return null;

        return reader.GetInt16(ordinal);
    }


    public static short? GetInt16Nullable(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetInt16Nullable(reader, ordinal);
    }



    public static short GetInt16WithDefault(this IDataReader reader, int ordinal, short defaultValue)
    {
        if (reader.IsDBNull(ordinal))
            return defaultValue;

        return reader.GetInt16(ordinal);
    }


    public static short GetInt16WithDefault(this IDataReader reader, string name, short defaultValue)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetInt16WithDefault(reader, ordinal, defaultValue);
    }


    #endregion


    #region Accessor Methods - Int64 Values


    public static long GetInt64(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return reader.GetInt64(ordinal);
    }


    public static long? GetInt64Nullable(this IDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal))
            return null;

        return reader.GetInt64(ordinal);
    }


    public static long? GetInt64Nullable(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetInt64Nullable(reader, ordinal);
    }



    public static long GetInt64WithDefault(this IDataReader reader, int ordinal, long defaultValue)
    {
        if (reader.IsDBNull(ordinal))
            return defaultValue;

        return reader.GetInt64(ordinal);
    }


    public static long GetInt64WithDefault(this IDataReader reader, string name, long defaultValue)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetInt64WithDefault(reader, ordinal, defaultValue);
    }


    #endregion



    #region Accessor Methods - float Values

    /// <summary>
    /// Get the float value from the column with the given name
    /// </summary>
    /// <param name="reader">An IDataReader of the data reader for the result set</param>
    /// <param name="name">A string of the column name containing the float</param>
    /// <returns>A float of the value contained in the column</returns>
    public static float GetFloat(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return reader.GetFloat(ordinal);
    }


    /// <summary>
    /// Get the float value from the column with the given position or if the current row contains a null
    /// for this column, return null
    /// </summary>
    /// <param name="reader">An IDataReader of the data reader for the result set</param>
    /// <param name="ordinal">An int of the ordinal position in the result set of the column to read</param>
    /// <returns>A nullable float value representing the value read from the database</returns>
    public static float? GetFloatNullable(this IDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal))
            return null;

        return reader.GetFloat(ordinal);
    }

    /// <summary>
    /// Get the float value from the column with the given name or if the current row contains a null
    /// for this column, return null
    /// </summary>
    /// <param name="reader">An IDataReader of the data reader for the result set</param>
    /// <param name="name">A string of the column name containing the float</param>
    /// <returns>A nullable float value representing the value read from the database</returns>
    public static float? GetFloatNullable(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetFloatNullable(reader, ordinal);
    }


    /// <summary>
    /// Get the float value from the column in the given ordinal position in the result set, returning
    /// the given default value if the value in the database is null
    /// </summary>
    /// <param name="reader">An IDataReader of the data reader for the result set</param>
    /// <param name="ordinal">An int of the ordinal position in the result set of the column to read</param>
    /// <param name="defaultValue">A float of the default value to return if the value in the database is null</param>
    /// <returns>A float of either the value stored inthe database or the default value if the database contains null</returns>
    public static float GetFloatWithDefault(this IDataReader reader, int ordinal, float defaultValue)
    {
        if (reader.IsDBNull(ordinal))
            return defaultValue;

        return reader.GetFloat(ordinal);
    }


    /// <summary>
    /// Get the float value from the column with the given name, returning
    /// the given default value if the value in the database is null
    /// </summary>
    /// <param name="reader">An IDataReader of the data reader for the result set</param>
    /// <param name="name">A string of the column name containing the float</param>
    /// <param name="defaultValue">A float of the default value to return if the value in the database is null</param>
    /// <returns>A float of either the value stored inthe database or the default value if the database contains null</returns>
    public static float GetFloatWithDefault(this IDataReader reader, string name, float defaultValue)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetFloatWithDefault(reader, ordinal, defaultValue);
    }


    #endregion


    #region Accessor Methods - double Values

    /// <summary>
    /// Get the double value in the named column in the result set
    /// </summary>
    /// <param name="reader">An IDataReader of the data reader for the result set</param>
    /// <param name="name">A string of the column name containing the double</param>
    /// <returns>A double of the value in the column</returns>
    public static double GetDouble(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return reader.GetDouble(ordinal);
    }


    public static double? GetDoubleNullable(this IDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal))
            return null;

        return reader.GetDouble(ordinal);
    }


    public static double? GetDoubleNullable(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetDoubleNullable(reader, ordinal);
    }



    public static double GetDoubleWithDefault(this IDataReader reader, int ordinal, double defaultValue)
    {
        if (reader.IsDBNull(ordinal))
            return defaultValue;

        return reader.GetDouble(ordinal);
    }


    public static double GetDoubleWithDefault(this IDataReader reader, string name, double defaultValue)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetDoubleWithDefault(reader, ordinal, defaultValue);
    }


    #endregion





    #region Accessor Methods - Decimal Values


    public static decimal GetDecimal(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return reader.GetDecimal(ordinal);
    }


    public static decimal? GetDecimalNullable(this IDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal))
            return null;

        return reader.GetDecimal(ordinal);
    }


    public static decimal? GetDecimalNullable(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetDecimalNullable(reader, ordinal);
    }



    public static decimal GetDecimalWithDefault(this IDataReader reader, int ordinal, decimal defaultValue)
    {
        if (reader.IsDBNull(ordinal))
            return defaultValue;

        return reader.GetDecimal(ordinal);
    }


    public static decimal GetDecimalWithDefault(this IDataReader reader, string name, decimal defaultValue)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetDecimalWithDefault(reader, ordinal, defaultValue);
    }


    #endregion


    #region Get Data Methods - DateTime


    public static DateTime GetDateTime(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return reader.GetDateTime(ordinal);
    }


    public static DateTime? GetDateTimeNullable(this IDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal))
            return null;

        return reader.GetDateTime(ordinal);
    }


    public static DateTime? GetDateTimeNullable(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetDateTimeNullable(reader, ordinal);
    }


    public static DateTime GetDateTimeWithDefault(this IDataReader reader, int ordinal, DateTime defaultValue)
    {
        if (reader.IsDBNull(ordinal))
            return defaultValue;

        return reader.GetDateTime(ordinal);
    }


    public static DateTime GetDateTimeWithDefault(this IDataReader reader, string name, DateTime defaultValue)
    {
        int ordinal = reader.GetOrdinal(name);
        return GetDateTimeWithDefault(reader, ordinal, defaultValue);
    }



    #endregion


    #region Accessor Methods - Object Values


    public static Object GetValue(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return reader.GetValue(ordinal);
    }

    #endregion


    #region Accessor Methods - Byte Values


    public static byte GetByte(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return reader.GetByte(ordinal);
    }



    #endregion


    #region Get Data Methods - GUID Values

    /// <summary>
    /// Gets a Guid value from the datareader.
    /// </summary>
    /// <remarks>
    /// Returns Guid.Empty for null.
    /// </remarks>
    /// <param name="name">Name of the column containing the value.</param>
    public static System.Guid GetGuid(this IDataReader reader, string name)
    {
        int ordinal = reader.GetOrdinal(name);
        return reader.GetGuid(ordinal);
    }


    #endregion

}

