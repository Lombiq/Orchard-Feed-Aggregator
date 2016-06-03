using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.RssReader.Models
{
    public interface IRssFeedDataSavingProviderContext
    {
        IContent Content { get; }

        string Data { get; }

        string ProviderTypeName { get; }
    }
}