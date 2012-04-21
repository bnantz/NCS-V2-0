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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    /// <devdoc>
    /// Represents a factory for creating ConfigurationTreeNode objects.
    /// </devdoc>
    class TreeNodeFactory
    {
        private static object syncRoot = new Object();
        private static object lockObj = new Object();
        private static TreeNodeFactory instance;

        private TreeNodeContainer treeNodeContainer;
        private Hashtable treeNodeLookup;
        private ConfigurationNodeImageContainer imageContainer;

        private TreeNodeFactory()
        {
            this.treeNodeContainer = new TreeNodeContainer();
            this.treeNodeLookup = new Hashtable();
            this.treeNodeLookup.Add(typeof(ConfigurationNode), typeof(ConfigurationTreeNode));
        }

        /// <devdoc>
        /// Looks up the ConfigurationTreeNode indexed by the specified ID.
        /// </devdoc>
        public static ConfigurationTreeNode GetTreeNode(Guid id)
        {
            return Current.treeNodeContainer.GetTreeNode(id);
        }

        /// <devdoc>
        /// The singleton instance to use with static methods.
        /// </devdoc>
        public static TreeNodeFactory Current
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new TreeNodeFactory();
                        }

                    }
                }
                return instance;
            }
        }

        public static void SetImageContainer(ConfigurationNodeImageContainer imageContainer)
        {
            lock (lockObj)
            {
                Current.imageContainer = imageContainer;
            }

        }

        /// <devdoc>
        /// Creates a new intance of a ConfigurationTreeNode class and sets its ImageIndex and SelectedImageIndex property values.
        /// </devdoc>
        public static ConfigurationTreeNode Create(ConfigurationNode node)
        {
            Type nodeType = node.GetType();
            Type treeNodeType = GetTreeNodeType(nodeType);
            ConfigurationTreeNode treeNode = (ConfigurationTreeNode) Activator.CreateInstance(treeNodeType, new object[] {node});
            if (Current.imageContainer != null)
            {
                treeNode.ImageIndex = Current.imageContainer.GetImageIndex(nodeType);
                treeNode.SelectedImageIndex = Current.imageContainer.GetSelectedImageIndex(nodeType);
            }
            if (Current.treeNodeContainer != null)
            {
                Current.treeNodeContainer.AddTreeNode(treeNode);
            }
            return treeNode;
        }

        private static Type GetTreeNodeType(Type nodeType)
        {
            Type t = (Type) Current.treeNodeLookup[nodeType];
            if (t != null)
            {
                return t;
            }
            else
            {
                return GetTreeNodeType(nodeType.BaseType);
            }
        }
    }
}