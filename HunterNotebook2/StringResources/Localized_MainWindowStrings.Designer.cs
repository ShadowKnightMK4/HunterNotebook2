﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HunterNotebook2.StringResources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Localized_MainWindowStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Localized_MainWindowStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HunterNotebook2.StringResources.Localized_MainWindowStrings", typeof(Localized_MainWindowStrings).Assembly);
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
        ///   Looks up a localized string similar to There was error pasting the formated Text..
        /// </summary>
        internal static string MainWindow_Clipboard_PasteFailure {
            get {
                return ResourceManager.GetString("MainWindow_Clipboard_PasteFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Save Changes?&quot;.
        /// </summary>
        internal static string MainWindow_MessageTitle_SaveChanges {
            get {
                return ResourceManager.GetString("MainWindow_MessageTitle_SaveChanges", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warning: There were some errors in loaded the state. Reverting to default settings this time..
        /// </summary>
        internal static string MainWindow_OnShow_ErrorInConfigStateOnLoadMessage {
            get {
                return ResourceManager.GetString("MainWindow_OnShow_ErrorInConfigStateOnLoadMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warning: State of Application did not load ok. Reverting to default settings this time..
        /// </summary>
        internal static string MainWindow_OnShow_FailToLoadConfigStateMessage {
            get {
                return ResourceManager.GetString("MainWindow_OnShow_FailToLoadConfigStateMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Plugin could not load plugins that handle saving / loaded various file types.
        /// </summary>
        internal static string MainWindow_PluginMessages_PluginFormatLoadFail {
            get {
                return ResourceManager.GetString("MainWindow_PluginMessages_PluginFormatLoadFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} -- {1}{2}.
        /// </summary>
        internal static string MainWindow_Title_HasContextFormatString {
            get {
                return ResourceManager.GetString("MainWindow_Title_HasContextFormatString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} -- No Context.
        /// </summary>
        internal static string MainWindow_Title_NoContextFormatString {
            get {
                return ResourceManager.GetString("MainWindow_Title_NoContextFormatString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This file may have unsaved changed. Are you sure you wish to start fresh? If so then changes will be lost..
        /// </summary>
        internal static string NewMenu_PromptChangesText {
            get {
                return ResourceManager.GetString("NewMenu_PromptChangesText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to save config file. Settings not preserved.
        /// </summary>
        internal static string Shutdown_FailureToSaveConfigMessage {
            get {
                return ResourceManager.GetString("Shutdown_FailureToSaveConfigMessage", resourceCulture);
            }
        }
    }
}
