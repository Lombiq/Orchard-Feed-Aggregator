using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Helpers
{
    /// <summary>
    /// Helper for parsing dates.
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Parses the given string to date. This is neccessary because the simple tryparse can't parse the
        /// date strings which ends with "EDT".
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <param name="date">The parsed date.</param>
        /// <returns>True if parsing was successful.</returns>
        public static bool TryParseDateTime(string dateString, out DateTime date)
        {
            // Checking if the simple tryparse can parse the date.
            date = DateTime.MinValue;
            if (DateTime.TryParse(dateString, out date))
            {
                return true;
            }

            var predefinedTimeZones = new Dictionary<string, string>
            {
                { "EDT", "-0400" },
                { "EST", "-0500" },
                { "HST", "-1000" },
                { "HAST", "-1000" },
                { "AKDT", "-0800" },
                { "PDT", "-0700" },
                { "CDT", "-0500" }
            };
            // If the given string end with a predefined key, then try parse it manually.
            foreach (var timeZone in predefinedTimeZones)
            {
                if (dateString.Substring(dateString.Length - timeZone.Key.Length) == timeZone.Key)
                {
                    if (DateTime.TryParse(dateString.Substring(0, dateString.Length - timeZone.Key.Length) + " " + timeZone.Value, out date))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}