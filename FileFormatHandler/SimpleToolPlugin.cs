using GenericPlugin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace PluginSystem
{
    /// <summary>
    /// A temple tool lives and its trigged within the ExternalTools Menu. They optionally are passed 
    /// </summary>
    public class InstancedSimpleToolPlugin:  InstancedPluginContainer 
    {
        public enum PreferredLocation
        {
            /// <summary>
            /// There is no preference
            /// </summary>
            Undefined = 0,
            /// <summary>
            /// The Static Tools Menu
            /// </summary>
            Default = 1,
            /// <summary>
            /// The Web {0} Menu
            /// </summary>
            WebEntry = 2

        }
        /// <summary>
        /// gets the text that will appear on the menu.
        /// </summary>
        /// <returns></returns>
        public virtual string GetMenuItemName()
        {
            return (string)HandlerType.GetMethod("GetMenuItemName").Invoke(Handler, Array.Empty<object>());
        }

        /// <summary>
        /// get the base command (pre template processing) that will be ran with the menu.
        /// </summary>
        /// <returns></returns>
        public virtual string GetMenuItemCommand()
        {
        return  (string)     HandlerType.GetMethod("GetMenuItemCommand").Invoke(Handler, Array.Empty<object>());
        }
      

        /// <summary>
        /// return an integer that specifies where to place the menu.
        /// </summary>
        /// <returns></returns>
        public virtual PreferredLocation GetMenuPreferedLocation()
        {
            var TargetMethod = HandlerType.GetMethod("GetMenuPreferedLocation");
            if (TargetMethod == null)
            {
                return PreferredLocation.Default;
            }
            return (PreferredLocation)TargetMethod.Invoke(Handler, null);
        }

    }


    public class SimpleToolHandler : DynamicClassLoader<InstancedSimpleToolPlugin>
    {
        public SimpleToolHandler() : base("Simple Tool Domain Container", false, "\\SimpleTools")
        {
            ScanFolder("\\SimpleTools", new PluginFilterCheck(SimpleToolHandlerFilter), null, new TargetClassLoadName(SimpleToolTypeCheck), "SIMPLETOOL");
        }

        private bool SimpleToolTypeCheck(string name, TypeInfo Data)
        {
            name = name.ToUpper(CultureInfo.InvariantCulture);

            if (name.Contains("NOEXPORT"))
            {
                return false;
            }

            if (name.StartsWith("SIMPLETOOL", StringComparison.InvariantCulture) == false)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// I really only case if it exists, ends with .DLL and is not a directory
        /// </summary>
        /// <param name="Info"></param>
        /// <returns></returns>
        private bool SimpleToolHandlerFilter(FileInfo Info)
        {
            if (!Info.Exists)
            {
                return false;
            }
            if (Info.FullName.ToUpperInvariant().EndsWith(".DLL", StringComparison.InvariantCultureIgnoreCase) == false)
            {
                return false;
            }
            if (Info.Attributes.HasFlag( FileAttributes.Directory))
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }
    }

}
