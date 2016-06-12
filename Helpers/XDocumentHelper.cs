using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Lombiq.FeedAggregator.Helpers
{
    public static class XDocumentHelper
    {
        /// <summary>
        /// This is necessary for the node getting because the various namespaces in feeds.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static XElement GetDescendantNodeByName(XElement parentNode, string nodeName)
        {
            return parentNode.Descendants().FirstOrDefault(node => node.Name.LocalName == nodeName);
        }

        /// <summary>
        /// This is necessary for the multiple node getting because the various namespaces in feeds.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static IEnumerable<XElement> GetDescendantNodesByName(XElement parentNode, string nodeName)
        {
            return parentNode.Elements().Where(element => element.Name.LocalName == nodeName);
        }

        public static bool ElementContainsNodeWithName(XElement xElement, string name)
        {
            return xElement.Elements().Any(element => element.Name.LocalName == name);
        }
    }
}