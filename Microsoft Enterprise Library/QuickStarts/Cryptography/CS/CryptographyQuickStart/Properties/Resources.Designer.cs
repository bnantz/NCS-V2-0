﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CryptographyQuickStart.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CryptographyQuickStart.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Decrypted text: {0}.
        /// </summary>
        internal static string DecryptedTextMessage {
            get {
                return ResourceManager.GetString("DecryptedTextMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You should encrypt a text first.
        /// </summary>
        internal static string DecryptErrorMessage {
            get {
                return ResourceManager.GetString("DecryptErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Encrypted text: {0}.
        /// </summary>
        internal static string EncryptedTextMessage {
            get {
                return ResourceManager.GetString("EncryptedTextMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter the text to be encrypted:.
        /// </summary>
        internal static string EncryptInstructionsMessage {
            get {
                return ResourceManager.GetString("EncryptInstructionsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Text to encrypt.
        /// </summary>
        internal static string EncryptTitleMessage {
            get {
                return ResourceManager.GetString("EncryptTitleMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must first generate a hash from text.
        /// </summary>
        internal static string HashErrorMessage {
            get {
                return ResourceManager.GetString("HashErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter the text to be used for generating a hash value:.
        /// </summary>
        internal static string HashInstructionsMessage {
            get {
                return ResourceManager.GetString("HashInstructionsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Generated hash: {0}.
        /// </summary>
        internal static string HashMessage {
            get {
                return ResourceManager.GetString("HashMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Text for hash.
        /// </summary>
        internal static string HashTitleMessage {
            get {
                return ResourceManager.GetString("HashTitleMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to write key file.
        /// </summary>
        internal static string KeyFileErrorTitle {
            get {
                return ResourceManager.GetString("KeyFileErrorTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Original text: {0}.
        /// </summary>
        internal static string OriginalTextMessage {
            get {
                return ResourceManager.GetString("OriginalTextMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The original text has not been tampered.
        /// </summary>
        internal static string TextNotTamperedMessage {
            get {
                return ResourceManager.GetString("TextNotTamperedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The original text has been tampered.
        /// </summary>
        internal static string TextTamperedMessage {
            get {
                return ResourceManager.GetString("TextTamperedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to We are sorry but this quick start is unable to run. It requires a key file to be created to be used for symmetric cryptographic operations, and we can&apos;t create this file.  The most common reason for this is that the quick starts were not installed into their default installation location. If this is true, please edit the configuration file to reflect the installation path. The exception message is: {0}.
        /// </summary>
        internal static string UnableToWriteKeyFileErrorMessage {
            get {
                return ResourceManager.GetString("UnableToWriteKeyFileErrorMessage", resourceCulture);
            }
        }
    }
}
