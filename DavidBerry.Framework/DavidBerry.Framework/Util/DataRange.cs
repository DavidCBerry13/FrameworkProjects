using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Util
{
    /// <summary>
    /// The Range class.
    /// </summary>
    /// <remarks>
    /// Code adapted from https://stackoverflow.com/questions/5343006/is-there-a-c-sharp-type-for-representing-an-integer-range/5343033#5343033
    /// </remarks>
    /// <typeparam name="T">Generic parameter.</typeparam>
    public class DataRange<T> where T : IComparable<T>
    {

        public DataRange(T minimum, T maximum)
        {
            if (minimum.CompareTo(maximum) >= 0)
                throw new ArgumentException($"The minimum {minimum} must be less than the maximum {maximum}");

            Minimum = minimum;
            Maximum = maximum;
        }


        /// <summary>
        /// Gets the minimum value of the range
        /// </summary>
        public T Minimum { get; private set; }

        /// <summary>
        /// Gets the maximum value of the range
        /// </summary>
        public T Maximum { get; private set; }

        /// <summary>
        /// Presents the Range in readable format.
        /// </summary>
        /// <returns>String representation of the Range</returns>
        public override string ToString()
        {
            return string.Format("[{0}-{1}]", this.Minimum, this.Maximum);
        }


        /// <summary>
        ///     Determines if the provided value is inside the range.
        /// </summary>
        /// <param name="value">The value to test</param>
        /// <returns>True if the value is inside Range, else false</returns>
        public bool ContainsValue(T value)
        {
            return (this.Minimum.CompareTo(value) <= 0) && (value.CompareTo(this.Maximum) <= 0);
        }

        /// <summary>Determines if this Range is inside the bounds of another range.</summary>
        /// <param name="Range">The parent range to test on</param>
        /// <returns>True if range is inclusive, else false</returns>
        public bool IsInsideRange(DataRange<T> range)
        {
            return range.ContainsValue(this.Minimum) && range.ContainsValue(this.Maximum);
        }

        /// <summary>Determines if another range is inside the bounds of this range.</summary>
        /// <param name="Range">The child range to test</param>
        /// <returns>True if range is inside, else false</returns>
        public bool ContainsRange(DataRange<T> range)
        {
            return this.ContainsValue(range.Minimum) && this.ContainsValue(range.Maximum);
        }
    }
}
