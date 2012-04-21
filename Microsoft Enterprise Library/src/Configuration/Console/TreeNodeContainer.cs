//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    /// <devdoc>
    /// Represents a lookup table of ConfigurationTreeNode objects indexed by their contained configuration node's ID.
    /// </devdoc>
    class TreeNodeContainer
    {
        private Hashtable treeNodes;

        public TreeNodeContainer()
        {
            treeNodes = new Hashtable();
        }

        /// <devdoc>
        /// Adds the specified ConfigurationTreeNode to the container.
        /// </devdoc>
        public void AddTreeNode(ConfigurationTreeNode treeNode)
        {
            treeNodes[treeNode.ConfigurationNode.Id] = treeNode;
        }

        /// <devdoc>
        /// Looks up the ConfigurationTreeNode indexed by the specified ID.
        /// </devdoc>
        public ConfigurationTreeNode GetTreeNode(Guid id)
        {
            return (ConfigurationTreeNode) treeNodes[id];
        }
    }
}