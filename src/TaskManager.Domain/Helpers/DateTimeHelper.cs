using System;

namespace TaskManager.Domain.Helpers;

public static class DateTimeHelper
{
    private static readonly TimeZoneInfo BrazilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
    
    public static DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, BrazilTimeZone);
    
    public static DateTime ToBrazilTime(DateTime utcDateTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, BrazilTimeZone);
    }
}
