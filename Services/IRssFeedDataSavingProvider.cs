using Lombiq.RssReader.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.RssReader.Services
{
    public interface IRssFeedDataSavingProvider : IDependency
    {
        string ProviderType { get; }
        void Save(IRssFeedDataSavingProviderContext rssFeedDataSavingProviderContext);
    }
}