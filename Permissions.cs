using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission RepopulateFeedSyncProfilePermission = new Permission { Category = "Lombiq Feed Aggregator", Description = "Allows users to repopulate feed sync profile.", Name = "RepopulateFeedSyncProfile" };

        public Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                RepopulateFeedSyncProfilePermission
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new [] { RepopulateFeedSyncProfilePermission }
                }
            };
        }
    }
}