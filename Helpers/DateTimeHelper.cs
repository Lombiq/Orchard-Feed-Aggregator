using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Helpers
{
    /// <summary>
    /// Helper for parsing dates.
    /// </summary>
    public class DateTimeHelper
    {
        /// <summary>
        /// Parses the given string to date. This is neccessary because the simple tryparse can't parse the
        /// date strings which ends with "EDT".
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <param name="date">The parsed date.</param>
        /// <returns>True if the parse was successful.</returns>
        public static bool TryGetDateTime(string dateString, out DateTime date)
        {
            // Checking if the simple tryparse can parse the date.
            date = new DateTime();
            if (DateTime.TryParse(dateString, out date))
            {
                return true;
            }

            // If the given string ends with "EDT", then parse it manually.
            if (dateString.Substring(dateString.Length - 3) == "EDT")
            {
                if (DateTime.TryParse(dateString.Substring(0, dateString.Length - 3) + " -0400", out date))
                {
                    return true;
                }
            }

            return false;
        }
    }
}