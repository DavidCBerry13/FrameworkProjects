using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Util;

public interface ITimePeriod
{

    public DateTime StartTime { get; }

    public DateTime EndTime { get; }

}



public static class ITimePeriodExtensions
{
    /// <summary>
    /// Checks if this time period overlaps with another time period
    /// </summary>
    /// <param name="period1">The first time period</param>
    /// <param name="period2">A TimePeriod to see if check for overlap with</param>
    /// <returns></returns>
    public static bool Overlaps(this ITimePeriod period1, ITimePeriod period2)
    {
        return period1.StartTime < period2.EndTime && period2.StartTime < period1.EndTime;
    }


    public static bool Overlaps(this ITimePeriod period1, DateTime startTime, DateTime endTime)
    {
        return period1.StartTime < endTime && startTime < period1.EndTime;
    }

    public static bool Contains(this ITimePeriod period, DateTime dateTime)
    {
        return period.StartTime <= dateTime && dateTime <= period.EndTime;
    }

    public static TimeSpan Duration(this ITimePeriod period)
    {
        return period.EndTime - period.StartTime;
    }

    public static bool IsInside(this ITimePeriod innerPeriod, ITimePeriod outerPeriod)
    {
        return outerPeriod.StartTime <= innerPeriod.StartTime && innerPeriod.EndTime <= outerPeriod.EndTime;
    }


}