﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Application Configuration.
        /// </summary>
        internal static string ApplicationNodeName {
            get {
                return ResourceManager.GetString("ApplicationNodeName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Assemblies.
        /// </summary>
        internal static string AssembliesLabelText {
            get {
                return ResourceManager.GetString("AssembliesLabelText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to load the assembly. The error message is &apos;{0}&apos;..
        /// </summary>
        internal static string AssemblyLoadFailedErrorMessage {
            get {
                return ResourceManager.GetString("AssemblyLoadFailedErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to General.
        /// </summary>
        internal static string CategoryGeneral {
            get {
                return ResourceManager.GetString("CategoryGeneral", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Close the current application configuration..
        /// </summary>
        internal static string CloseApplicationUICommandLongText {
            get {
                return ResourceManager.GetString("CloseApplicationUICommandLongText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Close Application.
        /// </summary>
        internal static string CloseApplicationUICommandText {
            get {
                return ResourceManager.GetString("CloseApplicationUICommandText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} at path {1}.\n{2}\n\n.
        /// </summary>
        internal static string ConfigurationErrorToString {
            get {
                return ResourceManager.GetString("ConfigurationErrorToString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Configuration Files(web.config, *.exe.config, app.config)|web.config;*.exe.config;app.config|All Files(*.*)|*.*.
        /// </summary>
        internal static string ConfigurationFileDialogFilter {
            get {
                return ResourceManager.GetString("ConfigurationFileDialogFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path to the application configuration file..
        /// </summary>
        internal static string ConfigurationFilePathDescription {
            get {
                return ResourceManager.GetString("ConfigurationFilePathDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Configuration Sources.
        /// </summary>
        internal static string ConfigurationSourceNodeName {
            get {
                return ResourceManager.GetString("ConfigurationSourceNodeName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add configuration sources to the application.
        /// </summary>
        internal static string ConfigurationSourceUICommandLongText {
            get {
                return ResourceManager.GetString("ConfigurationSourceUICommandLongText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Configuration Sources.
        /// </summary>
        internal static string ConfigurationSourceUICommandText {
            get {
                return ResourceManager.GetString("ConfigurationSourceUICommandText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The custom attributes collection contains multiple entries of the key &apos;{0}&apos;..
        /// </summary>
        internal static string CustomAttributesDuplicateKeyError {
            get {
                return ResourceManager.GetString("CustomAttributesDuplicateKeyError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The custom attribute at position {0} is empty..
        /// </summary>
        internal static string CustomAttributesKeyNullError {
            get {
                return ResourceManager.GetString("CustomAttributesKeyNullError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Application.
        /// </summary>
        internal static string DefaultApplicationName {
            get {
                return ResourceManager.GetString("DefaultApplicationName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file &apos;{0}&apos; could not be opened..
        /// </summary>
        internal static string ErrorFileCouldNotBeOpened {
            get {
                return ResourceManager.GetString("ErrorFileCouldNotBeOpened", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gets or sets if Event Logging is enabled..
        /// </summary>
        internal static string EventLoggingEnabledDescription {
            get {
                return ResourceManager.GetString("EventLoggingEnabledDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can only add IHierarchyNode objects to this container..
        /// </summary>
        internal static string ExceptionComponentNotHierarchyNode {
            get {
                return ResourceManager.GetString("ExceptionComponentNotHierarchyNode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type must be a ConfigurationNode..
        /// </summary>
        internal static string ExceptionConfigNodeExpected {
            get {
                return ResourceManager.GetString("ExceptionConfigNodeExpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not save one or more of the application files..
        /// </summary>
        internal static string ExceptionFilesNotSaved {
            get {
                return ResourceManager.GetString("ExceptionFilesNotSaved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name {0} is invalid for the component..
        /// </summary>
        internal static string ExceptionInvalidComponentName {
            get {
                return ResourceManager.GetString("ExceptionInvalidComponentName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The lowerBound value &apos;{0}&apos; is not less than or equal to the upperBound value &apos;{1}&apos;..
        /// </summary>
        internal static string ExceptionLowerBoundOutOfRange {
            get {
                return ResourceManager.GetString("ExceptionLowerBoundOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The configuration design manger {0} has a dependency on {1} that could not be loaded..
        /// </summary>
        internal static string ExceptionManagerDependencyNotFound {
            get {
                return ResourceManager.GetString("ExceptionManagerDependencyNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is not a valid base type attribute for the selector..
        /// </summary>
        internal static string ExceptionNoBaseTypeAttribute {
            get {
                return ResourceManager.GetString("ExceptionNoBaseTypeAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The node already exists in the collection..
        /// </summary>
        internal static string ExceptionNodeAlreadyInCollection {
            get {
                return ResourceManager.GetString("ExceptionNodeAlreadyInCollection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A node type map is not registered for the configuration type {0}..
        /// </summary>
        internal static string ExceptionNodeMapNotRegistered {
            get {
                return ResourceManager.GetString("ExceptionNodeMapNotRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A sibling node with the name &apos;{0}&apos; already exists..
        /// </summary>
        internal static string ExceptionNodeNameAlreadyExists {
            get {
                return ResourceManager.GetString("ExceptionNodeNameAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The reference type for the node was not set.  Please add the ReferenceTypeAttribute to your property..
        /// </summary>
        internal static string ExceptionNoRefTypeAttribute {
            get {
                return ResourceManager.GetString("ExceptionNoRefTypeAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{0}&apos; must be assignable from the type &apos;{1}&apos;..
        /// </summary>
        internal static string ExceptionNotAssignableType {
            get {
                return ResourceManager.GetString("ExceptionNotAssignableType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can only re-order sibling nodes..
        /// </summary>
        internal static string ExceptionOnlyReorderSiblings {
            get {
                return ResourceManager.GetString("ExceptionOnlyReorderSiblings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value can not be null or string or empty..
        /// </summary>
        internal static string ExceptionStringNullOrEmpty {
            get {
                return ResourceManager.GetString("ExceptionStringNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{0}&apos; must be a type of ConfigurationNode..
        /// </summary>
        internal static string ExceptionTypeNotConfigurationNode {
            get {
                return ResourceManager.GetString("ExceptionTypeNotConfigurationNode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type {0} is not valid..
        /// </summary>
        internal static string ExceptionTypeNotValid {
            get {
                return ResourceManager.GetString("ExceptionTypeNotValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value for {0} can not be null..
        /// </summary>
        internal static string ExceptionValueNullMessage {
            get {
                return ResourceManager.GetString("ExceptionValueNullMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add the file configuration source element..
        /// </summary>
        internal static string FileConfigurationSourceUICommandLongText {
            get {
                return ResourceManager.GetString("FileConfigurationSourceUICommandLongText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File Configuration Source.
        /// </summary>
        internal static string FileConfigurationSourceUICommandText {
            get {
                return ResourceManager.GetString("FileConfigurationSourceUICommandText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gets or sets the file to use for the source..
        /// </summary>
        internal static string FileSourceFileDescription {
            get {
                return ResourceManager.GetString("FileSourceFileDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All Files(*.*)|*.*.
        /// </summary>
        internal static string GenericFileFilter {
            get {
                return ResourceManager.GetString("GenericFileFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Save File.
        /// </summary>
        internal static string GenericSaveFile {
            get {
                return ResourceManager.GetString("GenericSaveFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Instrumentation.
        /// </summary>
        internal static string InstrumentationNodeName {
            get {
                return ResourceManager.GetString("InstrumentationNodeName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add instrumentation to the application.
        /// </summary>
        internal static string InstrumentationUICommandLongText {
            get {
                return ResourceManager.GetString("InstrumentationUICommandLongText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Instrumentation.
        /// </summary>
        internal static string InstrumentationUICommandText {
            get {
                return ResourceManager.GetString("InstrumentationUICommandText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Key: {0}, Value: {1}.
        /// </summary>
        internal static string KeyValueEditorFormat {
            get {
                return ResourceManager.GetString("KeyValueEditorFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The length of the string {0} must not exceed {1}..
        /// </summary>
        internal static string MaxLengthExceededErrorMessage {
            get {
                return ResourceManager.GetString("MaxLengthExceededErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The string {0} must have a length of at least {1}..
        /// </summary>
        internal static string MinLengthExceededErrorMessage {
            get {
                return ResourceManager.GetString("MinLengthExceededErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Passwords do not match.
        /// </summary>
        internal static string MismatchedPasswordCaption {
            get {
                return ResourceManager.GetString("MismatchedPasswordCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The passwords do not match. Passwords are case sensitive (e.g. &quot;password&quot; does not equal &quot;PassWord&quot;). Please try again..
        /// </summary>
        internal static string MismatchedPasswordMessage {
            get {
                return ResourceManager.GetString("MismatchedPasswordMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Move Down.
        /// </summary>
        internal static string MoveDownMenuItemText {
            get {
                return ResourceManager.GetString("MoveDownMenuItemText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Move currently selected node down.
        /// </summary>
        internal static string MoveDownStatusText {
            get {
                return ResourceManager.GetString("MoveDownStatusText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Move Up.
        /// </summary>
        internal static string MoveUpMenuItemText {
            get {
                return ResourceManager.GetString("MoveUpMenuItemText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Move currently selected node up.
        /// </summary>
        internal static string MoveUpStatusText {
            get {
                return ResourceManager.GetString("MoveUpStatusText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gets or sets the name of the node..
        /// </summary>
        internal static string NodeNameDescription {
            get {
                return ResourceManager.GetString("NodeNameDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (none).
        /// </summary>
        internal static string None {
            get {
                return ResourceManager.GetString("None", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No Types Found In Assembly..
        /// </summary>
        internal static string NoTypesFoundInAssemblyCaption {
            get {
                return ResourceManager.GetString("NoTypesFoundInAssemblyCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There were no types found in the assembly &apos;{0}&apos; that implement or inherit from the base type &apos;{1}&apos;..
        /// </summary>
        internal static string NoTypesFoundInAssemblyErrorMessage {
            get {
                return ResourceManager.GetString("NoTypesFoundInAssemblyErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Open Application.
        /// </summary>
        internal static string OpenApplicationCaption {
            get {
                return ResourceManager.GetString("OpenApplicationCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One or more errors occurred while trying to open the configuration..
        /// </summary>
        internal static string OpenApplicationErrorMessage {
            get {
                return ResourceManager.GetString("OpenApplicationErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Save of Read-Only File.
        /// </summary>
        internal static string OverwriteFileCaption {
            get {
                return ResourceManager.GetString("OverwriteFileCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file {0} cannot be saved because it is write-protected.\n\nDo you want to remove the write-protection and overwrite the file in its current location..
        /// </summary>
        internal static string OverwriteFileMessage {
            get {
                return ResourceManager.GetString("OverwriteFileMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gets or sets if Performance Counters are enabled..
        /// </summary>
        internal static string PerformanceCountersEnabledDescription {
            get {
                return ResourceManager.GetString("PerformanceCountersEnabledDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value {0} does not match the regular expression..
        /// </summary>
        internal static string RegExErrorMessage {
            get {
                return ResourceManager.GetString("RegExErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Remove current node.
        /// </summary>
        internal static string RemoveUICommandLongText {
            get {
                return ResourceManager.GetString("RemoveUICommandLongText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Remove.
        /// </summary>
        internal static string RemoveUICommandText {
            get {
                return ResourceManager.GetString("RemoveUICommandText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Save Application.
        /// </summary>
        internal static string SaveApplicationCaption {
            get {
                return ResourceManager.GetString("SaveApplicationCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One or more errors occurred while trying to save the configuration..
        /// </summary>
        internal static string SaveApplicationErrorMessage {
            get {
                return ResourceManager.GetString("SaveApplicationErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One or more errors occurred while trying to save the configuration. Would you still like to close the application without saving?.
        /// </summary>
        internal static string SaveApplicationErrorRequestMessage {
            get {
                return ResourceManager.GetString("SaveApplicationErrorRequestMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application has not been saved, would you like to save before closing?.
        /// </summary>
        internal static string SaveApplicationRequest {
            get {
                return ResourceManager.GetString("SaveApplicationRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gets or sets the default configuration source..
        /// </summary>
        internal static string SelectedSourceDescription {
            get {
                return ResourceManager.GetString("SelectedSourceDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to System Configuration Source.
        /// </summary>
        internal static string SystemConfigurationSourceElementNodeCreationText {
            get {
                return ResourceManager.GetString("SystemConfigurationSourceElementNodeCreationText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Adds the system configuration source element..
        /// </summary>
        internal static string SystemConfigurationSourceUICommandLongText {
            get {
                return ResourceManager.GetString("SystemConfigurationSourceUICommandLongText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to System ConfigurationSource.
        /// </summary>
        internal static string SystemConfigurationSourceUICommandText {
            get {
                return ResourceManager.GetString("SystemConfigurationSourceUICommandText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gets or sets the type for the element..
        /// </summary>
        internal static string TypeNameDescription {
            get {
                return ResourceManager.GetString("TypeNameDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Classes that inherit from {0}.
        /// </summary>
        internal static string TypeSelectorClassRootNodeText {
            get {
                return ResourceManager.GetString("TypeSelectorClassRootNodeText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Classes that inherit from {0} and have a ConfigurationElementType of &apos;{1}&apos;.
        /// </summary>
        internal static string TypeSelectorClassRootNodeTextWithConfigurationType {
            get {
                return ResourceManager.GetString("TypeSelectorClassRootNodeTextWithConfigurationType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Types that implement {0}.
        /// </summary>
        internal static string TypeSelectorInterfaceRootNodeText {
            get {
                return ResourceManager.GetString("TypeSelectorInterfaceRootNodeText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Types that implement {0} and have a ConfigurationElementType of &apos;{1}&apos;.
        /// </summary>
        internal static string TypeSelectorInterfaceRootNodeTextWithConfigurationType {
            get {
                return ResourceManager.GetString("TypeSelectorInterfaceRootNodeTextWithConfigurationType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The selected type must be a subclass of {0}.
        /// </summary>
        internal static string TypeSubclassErrorMsg {
            get {
                return ResourceManager.GetString("TypeSubclassErrorMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name {0} should be unique within the application block..
        /// </summary>
        internal static string UniqueNameErrorMessage {
            get {
                return ResourceManager.GetString("UniqueNameErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Validate current node..
        /// </summary>
        internal static string ValidateUICommandLongText {
            get {
                return ResourceManager.GetString("ValidateUICommandLongText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Validate.
        /// </summary>
        internal static string ValidateUICommandText {
            get {
                return ResourceManager.GetString("ValidateUICommandText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Validation.
        /// </summary>
        internal static string ValidationCaption {
            get {
                return ResourceManager.GetString("ValidationCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One or more validation errors occurred..
        /// </summary>
        internal static string ValidationErrorsMessage {
            get {
                return ResourceManager.GetString("ValidationErrorsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The property {0} is invalid for {1}. Message: {2}.
        /// </summary>
        internal static string ValidationErrorToString {
            get {
                return ResourceManager.GetString("ValidationErrorToString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value for {0} is not in the specified range..
        /// </summary>
        internal static string ValueNotInRangeErrorMessage {
            get {
                return ResourceManager.GetString("ValueNotInRangeErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value for {0} is not outside of the specified range..
        /// </summary>
        internal static string ValueOutsideRangeErrorMessage {
            get {
                return ResourceManager.GetString("ValueOutsideRangeErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gets or sets if WMI is enabled..
        /// </summary>
        internal static string WMIEnabledDescription {
            get {
                return ResourceManager.GetString("WMIEnabledDescription", resourceCulture);
            }
        }
    }
}
