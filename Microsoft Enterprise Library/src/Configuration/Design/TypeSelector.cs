//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	class TypeSelector
    {
        private TreeNode rootNode;
        private Type currentType;
        private TreeNode currentTypeTreeNode;
        private Type baseType_;
        private Type configurationType;
        private TreeView treeView;
        private TypeSelectorIncludes flags;
        private bool includeBaseType;
        private bool includeAbstractTypes;
        private bool includeNonPublicTypes;
        private bool includeAllInterfaces;
        private Hashtable loadedAssemblies;

		public TypeSelector(Type currentType, Type baseType, TreeView treeView) : this(currentType, baseType, TypeSelectorIncludes.None, null, treeView)
        {
        }

        public TypeSelector(Type currentType, Type baseType, TypeSelectorIncludes flags, TreeView treeView)
            :this(currentType, baseType, flags, null, treeView)
        {
        }
		
		public TypeSelector(Type currentType, Type baseType, TypeSelectorIncludes flags, Type configurationType, TreeView treeView)
        {
            if (treeView == null)
            {
                throw new ArgumentNullException("treeView");
            }
            if (baseType == null)
            {
                throw new ArgumentNullException("typeToVerify");
            }
            loadedAssemblies = new Hashtable();
            this.configurationType = configurationType;
            this.treeView = treeView;
            this.currentType = currentType;
            this.baseType_ = baseType;
            this.flags = flags;
            this.includeAbstractTypes = IsSet(TypeSelectorIncludes.AbstractTypes);
            this.includeAllInterfaces = IsSet(TypeSelectorIncludes.Interfaces);
            this.includeNonPublicTypes = IsSet(TypeSelectorIncludes.NonpublicTypes);
            this.includeBaseType = IsSet(TypeSelectorIncludes.BaseType);

            LoadTypes(baseType);
        }

		public Type TypeToVerify
        {
            get { return this.baseType_; }
        }

		public bool LoadTreeView(Assembly assembly)
        {
            if (loadedAssemblies.Contains(assembly.FullName))
            {
                return true;
            }

            TreeNodeTable nodeTable = new TreeNodeTable(assembly);
            Type[] types = LoadTypesFromAssembly(assembly);
            if (types == null)
            {
                return false;
            }

            LoadValidTypes(types, nodeTable);

            return AddTypesToTreeView(nodeTable, assembly);
        }

        private bool AddTypesToTreeView(TreeNodeTable nodeTable, Assembly assembly)
        {
            if (nodeTable.AssemblyNode.Nodes.Count > 0)
            {
                this.rootNode.Nodes.Add(nodeTable.AssemblyNode);
                this.rootNode.ExpandAll();
                loadedAssemblies[assembly.FullName] = 1;
                return true;
            }
            else
            {
                return false;
            }
        }

        private static Type[] LoadTypesFromAssembly(Assembly assembly)
        {
            Type[] types = null;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException)
            {
            }
            return types;
        }

        private void LoadValidTypes(Type[] types, TreeNodeTable nodeTable)
        {
            if (types == null)
            {
                return;
            }

            foreach (Type t in types)
            {
                if (IsTypeValid(t))
                {
                    TreeNode newNode = nodeTable.AddType(t);
                    if (t == this.currentType)
                    {
                        this.currentTypeTreeNode = newNode;
                    }
                }
                //LoadValidTypes(t.GetNestedTypes(), nodeTable);
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="selectedType"></param>
		/// <returns></returns>
        public bool IsTypeValid(Type selectedType)
        {
            bool valid = false;

            if (includeAllInterfaces && selectedType.IsInterface)
            {
                valid = true;
            }
            else if (selectedType == baseType_)
            {
                valid = includeBaseType;
            }
            else
            {
                valid = baseType_.IsAssignableFrom(selectedType);

                if (valid)
                {
                    if ((!includeAbstractTypes) && (selectedType.IsAbstract || selectedType.IsInterface))
                    {
                        valid = false;
                    }
                }

                if (valid)
                {
                    if (!(selectedType.IsPublic) && !(selectedType.IsNestedPublic) && (!includeNonPublicTypes))
                    {
                        valid = false;
                    }
                }

                if (valid && configurationType != null)
                {
                    object[] configurationElementTypeAttributes = selectedType.GetCustomAttributes(typeof(ConfigurationElementTypeAttribute), true);
                    if (configurationElementTypeAttributes.Length == 0)
                    {
                        valid = false;
                    }
                    else
                    {
                        ConfigurationElementTypeAttribute configElementTypeAttribute = (ConfigurationElementTypeAttribute)configurationElementTypeAttributes[0];
                        if (configurationType != configElementTypeAttribute.ConfigurationType)
                        {
                            valid = false;
                        }
                    }
                }
            }
            return valid;
        }

        private bool IsSet(TypeSelectorIncludes compareFlag)
        {
            return ((this.flags & compareFlag) == compareFlag);
        }

        private void LoadTypes(Type baseType)
        {
            TreeNode treeNode = new TreeNode(String.Empty, -1, -1);
            if (baseType.IsInterface)
            {
                if (configurationType != null)
                {
					treeNode.Text = string.Format(CultureInfo.CurrentUICulture, Resources.TypeSelectorInterfaceRootNodeTextWithConfigurationType, baseType.FullName, configurationType.FullName);
                }
                else
                {
					treeNode.Text = string.Format(CultureInfo.CurrentUICulture, Resources.TypeSelectorInterfaceRootNodeText, baseType.FullName);
                }
            }
            else if (baseType.IsClass)
            {
                if (configurationType != null)
                {
                    treeNode.Text = string.Format(CultureInfo.CurrentUICulture, Resources.TypeSelectorClassRootNodeTextWithConfigurationType, baseType.FullName, configurationType.FullName);
                }
                else
                {
					treeNode.Text = string.Format(CultureInfo.CurrentUICulture, Resources.TypeSelectorClassRootNodeText, baseType.FullName);
                }
            }
            this.treeView.Nodes.Add(treeNode);
            this.rootNode = new TreeNode(Resources.AssembliesLabelText, 0, 1);
            treeNode.Nodes.Add(rootNode);
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                this.LoadTreeView(assembly);
            }
            treeNode.ExpandAll();
            this.treeView.SelectedNode = this.currentTypeTreeNode;
        }

        /// <devdoc>
        /// Represents the table of tree nodes by assembly type.
        /// </devdoc>
        private class TreeNodeTable
        {
            private Hashtable namespaceTable;
            private TreeNode assemblyNode;

            public TreeNodeTable(Assembly assembly)
            {
                this.namespaceTable = new Hashtable();
                this.assemblyNode = new TreeNode(assembly.GetName().Name, (int)TypeImages.Assembly, (int)TypeImages.Assembly);
            }

            public TreeNode AssemblyNode
            {
                get { return this.assemblyNode; }
            }

            public TreeNode AddType(Type t)
            {
                TreeNode namespaceNode = GetNamespaceNode(t);
                TreeNode typeNode = new TreeNode(t.Name, (int)TypeImages.Class, (int)TypeImages.Class);
                typeNode.Tag = t;
                namespaceNode.Nodes.Add(typeNode);
                return typeNode;
            }

            private TreeNode GetNamespaceNode(Type t)
            {
                TreeNode namespaceNode = null;
                if (t.Namespace == null)
                {
                    namespaceNode = this.assemblyNode;
                }
                else if (this.namespaceTable.ContainsKey(t.Namespace))
                {
                    namespaceNode = (TreeNode)this.namespaceTable[t.Namespace];
                }
                else
                {
                    namespaceNode = new TreeNode(t.Namespace, (int)TypeImages.Namespace, (int)TypeImages.Namespace);
                    this.assemblyNode.Nodes.Add(namespaceNode);
                    this.namespaceTable.Add(t.Namespace, namespaceNode);
                }
                return namespaceNode;
            }
        }
    }
}