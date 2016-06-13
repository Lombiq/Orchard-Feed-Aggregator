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
        /// Gets the first descendant node with the given name.
        /// This is necessary for the node getting because the various namespaces in feeds.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="nodeName">The name of the selected descendant node.</param>
        public static XElement GetDescendantNodeByName(XElement parentNode, string nodeName)
        {
            return parentNode.Descendants().FirstOrDefault(node => node.Name.LocalName == nodeName);
        }

        /// <summary>
        /// Gets all descendant nodes with the given name.
        /// This is necessary for the multiple node getting because the various namespaces in feeds.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="nodeName">The name of the selected descendant nodes.</param>
        public static IEnumerable<XElement> GetDescendantNodesByName(XElement parentNode, string nodeName)
        {
            return parentNode.Elements().Where(element => element.Name.LocalName == nodeName);
        }

        public static bool ElementContainsNodeWithName(XElement xElement, string name)
        {
            return xElement.Elements().Any(element => element.Name.LocalName == name);
        }

        /// <summary>
        /// Tries getting the node in the following format: DescendantNode1/DescendantNode2/...
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="path">The path to the descendant node, relative to the prent node.</param>
        /// <param name="selectedNode">The result node.</param>
        public static bool TryGetNodeByPath(XElement parentNode, string path, out XElement selectedNode)
        {
            selectedNode = parentNode;
            foreach (var pathFragment in path.Split('/'))
            {
                selectedNode = GetDescendantNodeByName(selectedNode, pathFragment);
                if (selectedNode == null) return false;
            }

            return true;
        }
    }
}