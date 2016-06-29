using System.Collections.Generic;
using System.Linq;

namespace System.Xml.Linq
{
    internal static class XDocumentExtensions
    {
        /// <summary>
        /// Gets the first descendant node with the given name.
        /// This is necessary for the node retrieval because the various namespaces in feeds.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="nodeName">The name of the selected descendant node.</param>
        public static XElement GetDescendantNodeByName(this XElement parentNode, string nodeName)
        {
            return parentNode.Descendants().FirstOrDefault(node => node.Name.LocalName == nodeName);
        }

        /// <summary>
        /// Gets all descendant nodes with the given name.
        /// This is necessary for the multiple node retrieval because the various namespaces in feeds.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="nodeName">The name of the selected descendant nodes.</param>
        public static IEnumerable<XElement> GetDescendantNodesByName(this XElement parentNode, string nodeName)
        {
            return parentNode.Elements().Where(element => element.Name.LocalName == nodeName);
        }

        public static bool ElementContainsNodeWithName(this XElement xElement, string name)
        {
            return xElement.Elements().Any(element => element.Name.LocalName == name);
        }

        /// <summary>
        /// Tries to retrieve the nodes in the following format: DescendantNode1/DescendantNode2/...
        /// With the last fragment multiple nodes can be selected.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="path">The path to the descendant node, relative to the parent node.</param>
        /// <param name="selectedNodes">The result nodes.</param>
        public static bool TryGetNodesByPath(this XElement parentNode, string path, out List<XElement> selectedNodes)
        {
            selectedNodes = new List<XElement>();
            var splitPath = path.Split('/');
            var selectedNode = parentNode;
            for (int i = 0; i < splitPath.Length; i++)
            {
                if (i < splitPath.Length - 2)
                {
                    selectedNode = GetDescendantNodeByName(selectedNode, splitPath[i]);
                }
                else
                {
                    selectedNodes.AddRange(GetDescendantNodesByName(selectedNode, splitPath[i]));
                }

                if (selectedNode == null) return false;
            }

            return selectedNodes.Any();
        }
    }
}