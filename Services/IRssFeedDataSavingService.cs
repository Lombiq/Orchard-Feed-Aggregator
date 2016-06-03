using Orchard;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.RssReader.Services
{
    public interface IRssFeedDataSavingService : IDependency
    {
        IList<string> GetAccessibleContentItemStorageNames(string contentType);
    }
}