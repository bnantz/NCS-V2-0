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
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    /// <devdoc>
    /// Maintains the binding between an ConfigurationNode and its icon.
    /// </summary>
    class ConfigurationNodeImageContainer
    {
        private Hashtable selectedImageIndexLookup;
        private Hashtable imageIndexLookup;
        private ImageList imageList;

        public ConfigurationNodeImageContainer(ImageList imageList)
        {
            this.imageList = imageList;
            this.imageIndexLookup = new Hashtable();
            this.selectedImageIndexLookup = new Hashtable();
        }

        /// <devdoc>
        /// Gets the index of the image to display when a ConfigurationNode of the specified type is selected in the ConfigurationTreeView.
        /// </devdoc>
        public int GetSelectedImageIndex(Type configurationNodeType)
        {
            int imageIndex = GetImageIndexInternal(configurationNodeType,
                                                   typeof(SelectedImageAttribute),
                                                   selectedImageIndexLookup);
            if (imageIndex < 0)
            {
                return GetImageIndex(configurationNodeType);
            }
            else
            {
                return imageIndex;
            }
        }

        /// <devdoc>
        /// Gets the index of the image to display when a ConfigurationNodeof the specified type is displayed in the ConfigurationTreeView.
        /// </devdoc>
        public int GetImageIndex(Type configurationNodeType)
        {
            return GetImageIndexInternal(configurationNodeType,
                                         typeof(ImageAttribute),
                                         imageIndexLookup);
        }

        private int GetImageIndexInternal(Type configurationNodeType,
                                          Type imageAttributeType,
                                          Hashtable lookupTable)
        {
            object index = lookupTable[configurationNodeType];
            int imageIndex = -1;
            if (index == null)
            {
                imageIndex = SetImageIndex(configurationNodeType,
                                           imageAttributeType,
                                           lookupTable);
            }
            else
            {
                return (int) index;
            }

            if (imageIndex == -1)
            {
                if (configurationNodeType.BaseType != typeof(object))
                {
                    return GetImageIndexInternal(configurationNodeType.BaseType,
                                                 imageAttributeType,
                                                 lookupTable);
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return imageIndex;
            }
        }

        private int SetImageIndex(Type configurationNodeType,
                                  Type imageAttributeType,
                                  Hashtable lookupTable)
        {
            object[] attributes = configurationNodeType.GetCustomAttributes(imageAttributeType, false);
            if (attributes.Length == 0)
            {
                return -1;
            }
            NodeImageAttribute imageAttribute = (NodeImageAttribute) attributes[0];
            Image image = imageAttribute.GetImage();
            int index = this.imageList.Images.Add(image, Color.Transparent);
            lookupTable[configurationNodeType] = index;
            return index;
        }
    }
}