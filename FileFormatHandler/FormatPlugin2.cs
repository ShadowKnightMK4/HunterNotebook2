using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Globalization;
using GenericPlugin;

namespace PluginSystem
{
    public class InstancedIFormat2 : InstancedPluginContainer
    {/*
        public object Handler;
        public AppDomain Domain;
        public Assembly LoadedAssembly;
        public Type HandlerType;
        */

        public void ReadData(StreamReader Source, StreamWriter Output, out bool ContainsRtfTags)
        {
            // a cludge, I would not figure out to get out args to work this way in the remote class
            ContainsRtfTags = false;
            HandlerType.GetMethod("ReadData").Invoke(Handler, new object[] { Source, Output, ContainsRtfTags });
        }

        public string GetPreferredExtension()
        {
            return (string) HandlerType.GetMethod("GetPreferredExtension").Invoke(Handler, Array.Empty<object>());
        }
        public void WriteData(StreamReader Source, StreamWriter Output)
        {
            HandlerType.GetMethod("WriteData").Invoke(Handler, new object[] { Source, Output });
        }

        public string GetDialogBoxExt()
        {
            return (string)HandlerType.GetMethod("GetDialogBoxExt").Invoke(Handler, Array.Empty<object>());
        }

        public string GetShortName()
        {
            return (string)HandlerType.GetMethod("GetShortName").Invoke(Handler, Array.Empty<object>());
        }

        public string GetFriendlyName()
        {
            return (string)HandlerType.GetMethod("GetFriendlyName").Invoke(Handler, Array.Empty<object>());
        }
    }

    public class FormatHandler : DynamicClassLoader<InstancedIFormat2>
    {

        private bool PluginStringCheck(FileInfo Info)
        {
            string Generical;
            Generical = Info.FullName.ToUpperInvariant();
            if (Generical.EndsWith(".DLL", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }

        
        public FormatHandler(): base("Hunter notebook2 generic template", false, null)
        {
            
            ScanFolder("\\FormatPlugins", new PluginFilterCheck(PluginStringCheck), null, new TargetClassLoadName(FilterClassesToSpecs), null); ;
        }

        /// <summary>
        /// Filter out classes that do not meet what we are looking for.
        /// </summary>
        /// <param name="name">name of the loaded class</param>
        /// <param name="Data">The Type info of the considered classs</param>
        /// <returns>true if the class meets our needs and false if it does not</returns>
        /// <remarks> 
        /// This routine checked if IFORMAT is somewhere in the class name and does Not end with NOEXPORT.
        /// It uses FilterClassesCheck_Routines() to check method info <see cref="FilterClassesCheck_Routines(TypeInfo)"/>
        /// </remarks>
        private bool FilterClassesToSpecs(string name, TypeInfo Data)
        {

            name = name.ToUpper(CultureInfo.InvariantCulture);
            if (name.Contains("IFORMAT") == false)
            {
                return false;
            }

            if (name.EndsWith("NOEXPORT", StringComparison.CurrentCulture) == true)
            {
                return false;
            }

            if (Data.IsGenericType)
            {
                return false;
            }




            return FilterClassesCheck_Routines(Data);
        }

        /// <summary>
        /// Utilities routine to get class methods.
        /// </summary>
        /// <param name="IncludeParent">if false then just returns methods defined in the passed class type
        ///                             if true then this routine makes a list of all methods in the class including any parent classes
        ///                             </param>
        /// <param name="Info">type of the class to check</param>
        /// <returns></returns>
        

        // helper for FIlterClasses callback - checks if the TypeData contaisn the iFormat Specs
        private bool FilterClassesCheck_Routines(TypeInfo Data)
        {
            int VerifyCheck = 0;
            ParameterInfo[] ArgInfo;
            List<MethodInfo> Info = InstancedPluginContainer.GetClassMethods(true, Data);
            foreach (MethodInfo RoutineInfo in Info)
            {
                switch (RoutineInfo.Name)
                {
                    case "ReadData":
                        if (!RoutineInfo.IsPublic)
                        {
                            continue;
                        }
                        if (RoutineInfo.ReturnType != typeof(void))
                        {
                            continue;
                        }

                        if (RoutineInfo.IsStatic == true)
                        {
                            continue;
                        }
                        ArgInfo = RoutineInfo.GetParameters();
                        if (ArgInfo.Length != 3)
                        {
                            continue;
                        }
                        if (ArgInfo[0].ParameterType !=  typeof(StreamReader))
                        {
                            continue;
                        }

                        if (ArgInfo[1].ParameterType != typeof(StreamWriter))
                        {
                            continue;
                        }

                        // TODO: Check argument for reference bool
                        /*
                        if (ArgInfo[2].ParameterType != Boolean.get)
                        {
                            continue;
                        }*/

                        VerifyCheck++;

                        
                        break;
                    case "GetPreferredExtension":
                        if (!RoutineInfo.IsPublic)
                        {
                            continue;
                        }

                        if (RoutineInfo.IsStatic)
                        {
                            continue;
                        }
                        if (RoutineInfo.ReturnType != typeof(string))
                        {
                            continue;
                        }
                        ArgInfo = RoutineInfo.GetParameters();

                        if (ArgInfo.Length != 0)
                        {
                            continue;
                        }
                        VerifyCheck++;
                        break;
                    case "WriteData":
                        if (!RoutineInfo.IsPublic)
                        {
                            continue;
                        }

                        if (RoutineInfo.IsStatic)
                        {
                            continue;
                        }

                        if (RoutineInfo.ReturnType != typeof(void))
                        {
                            continue;
                        }

                        ArgInfo = RoutineInfo.GetParameters();

                        if (ArgInfo.Length != 2)
                        {
                            continue;
                        }

                        if (ArgInfo[0].ParameterType != typeof(StreamReader))
                        {
                            continue;
                        }

                        if (ArgInfo[1].ParameterType != typeof(StreamWriter))
                        {
                            continue;
                        }
                        VerifyCheck++;
                        break;
                    case "GetDialogBoxExt":
                        if (RoutineInfo.IsPublic)
                        {
                            if (RoutineInfo.ReturnType == typeof(string))
                            {
                                VerifyCheck++;
                            }
                        }
                        break;
                    case "GetShortName":
                        if (RoutineInfo.IsPublic)
                        {
                            if (RoutineInfo.ReturnType == typeof(string))
                            {
                                VerifyCheck++;
                            }
                        }
                        break;
                    case "GetFriendlyName":
                        if (RoutineInfo.IsPublic)
                        {
                            if (RoutineInfo.ReturnType == typeof(string))
                            {
                                VerifyCheck++;
                            }
                        }
                        break;

                }

            }
            if (VerifyCheck >= 5)
            {
                return true;
            }
            return false;
        }
      
    }
}
