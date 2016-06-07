using Lombiq.RssReader.Models;
using Orchard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.RssReader.Services
{
    public interface IFeedManager : IDependency
    {
        /// <summary>
        /// Returns true if feed type getting was successful.
        /// If the feed is invalid then it counts as unsuccesful getting.
        /// </summary>
        /// <param name="rssSyncProfilePart">The RssSyncProfilePart</param>
        /// <param name="feedType">The type of the feed.</param>
        /// <returns></returns>
        bool TryGetValidFeedType(RssSyncProfilePart rssSyncProfilePart, out string feedType);
    }
}