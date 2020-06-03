using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace FileFormatHandler
{

    [Serializable]
    internal class AssemblyResolver_DynamicClassLoader
    {
        /// <summary>
        /// needs to resolve loading plugins in an instanced AppDomain / Assembly assembly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Assembly Probe_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.LoadFile(args.Name);
        }

        
    }
    /// <summary>
    /// The generic plugin scanner / loader.
    /// </summary>
    public class DynamicClassLoader<T>: IDisposable where T : InstancedPluginContainer, new()
    {


        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ScanNow">set to true to perform the scan for the plugins now</param>
        /// <param name="StartLocation">use null for executing asssembly director\\plugins</param>
        public DynamicClassLoader(string AppDomainNameTemplate, bool ScanNow, string StartLocation)
        {
            //            Ini("Hunternotebook File Handler Domain Parent");
            Ini(AppDomainNameTemplate);

            if (ScanNow)
            {
                // this call loads *all* exported types in the folder's assemblies
                ScanFolder(StartLocation, null, null, null, null);
            }
        }



        

        /// <summary>
        /// common ini that is shared with all constructers
        /// </summary>
        private void Ini(string GenericDomain)
        {
            AppDomainSetup Setup;
            Setup = AppDomain.CurrentDomain.SetupInformation;
            if (string.IsNullOrEmpty(GenericDomain))
            {
                GenericDomain = Assembly.GetExecutingAssembly().FullName + " Plugin App Domain Parent ";
            }

            FormatContainer = AppDomain.CreateDomain(GenericDomain, AppDomain.CurrentDomain.Evidence, Setup);
            LoadedFileHandliers = new List<T>();
        }
#endregion
        #region tool routines to easy code readability 

        /// <summary>
        /// EAIS,   also makes it easier to read the code I've written
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Friendly"></param>
        /// <param name="Target"></param>
        private static void Tool_DuplicateDomain(AppDomain Source, string Friendly,  out AppDomain Target)
        {
            Target = AppDomain.CreateDomain(Friendly, Source.Evidence, Source.SetupInformation);
        }

     
        #endregion
        #region Disposal and finalizer
        protected virtual void Dispose(bool ManagedAlso)
        {
            if (ManagedAlso)
            {
                foreach (InstancedPluginContainer container in LoadedFileHandliers)
                {
                    container.Dispose();
                }
                LoadedFileHandliers.Clear();
            }
            
            IsDisposed = true;
        }

        ~DynamicClassLoader()
        {
            Dispose(false);
        }
        /// <summary>
        /// if disposed, this is true.
        /// </summary>
        public bool IsDisposed { get; protected set; }

        /// <summary>
        /// Keep in mind, this also disposes of all loaded dlls and plugins
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        /// <summary>
        /// Used to control if an assembly is ok to reture to the caller. return true if yes, return false is not
        /// DOES NOT PREVENT LOADING the assembly in the probe. It just tells the routine to not use this one.
        /// </summary>
        /// <param name="VerifyAgainst">The loaded assembly check</param>
        /// <returns></returns>
        public delegate bool AssemblyCheck(Assembly VerifyAgainst);


        #region load plugin 
        /// <summary>
        /// Load the instanced class named name at Dlllocation
        /// </summary>
        /// <param name="Name">The exported type must match this</param>
        /// <param name="DllLocation">Dll or Assembly to check</param>
        /// <returns>Instance of <typeparamref name="T"/> if sucessfull or null if not</returns>
        public T LoadPlugin(string Name, string DllLocation)
        {
            List<T> _;
            string SubName = "Container for " + Name + " plugin";
            return LoadPlugin(null, Name, null, null, DllLocation, SubName, 0, out _);
        }

        /// <summary>
        /// Load all instances of exported classes that contain Name
        /// </summary>
        /// <param name="Name">the name to check againt</param>
        /// <param name="DllLocation">Assembly to dll to load from</param>
        /// <returns></returns>
        public List<T> LoadAllPlugins(string Name, string DllLocation)
        {
            string SubName = "Container for plugins contained in " + Path.GetFileNameWithoutExtension(DllLocation);
            T first = (LoadPlugin(null, Name, null, null, DllLocation, SubName, -1, out List<T> NoJunk));
            NoJunk.Add(first);
            return NoJunk;
        }

        public List<T> LoadAllPlugins(TargetClassLoadName ClassNameFilter, string DllLocation)
        {
            string SubName = "Container for plugins contained in " + Path.GetFileNameWithoutExtension(DllLocation);
            T first = (LoadPlugin(ClassNameFilter, string.Empty, null, null, DllLocation, SubName, 1, out List<T> NoJunk));
            NoJunk.Add(first);
            return NoJunk;
        }
        /// <summary>
        /// Load an instanced class that the callback routine approves
        /// </summary>
        /// <param name="CheckName">delege to examine with. </param>
        /// <param name="DllLocation">assembly or dll to load from</param>
        /// <returns></returns>
        public T LoadPlugin(TargetClassLoadName CheckName, string DllLocation)
        {
            string SubName = "Container for " + CheckName + " plugin";
            return LoadPlugin(CheckName, null, null, null, DllLocation, SubName, 0, out List<T> _);
        }



        /// <summary>
        /// the generic load routine for the system. The exported routines lead here
        /// </summary>
        /// <param name="TargetFormatClassCheck">specific null to skip this check. Called to see if the exported type is a match <seealso cref="TargetClassLoadName"/></param>
        /// <param name="TargetFormatClassString">if TargetFormatClassCheck is null checked class names are compaired against it</param>
        /// <param name="VerifyCallback">prevent unwanted assemblys from being used. <seealso cref="AssemblyCheck"/></param>
        /// <param name="VerifyAssemblyString">if Verifycallback is null, this is case sensisite compared with the FullName of the loaded assembly. No match means no go</param>
        /// <param name="DllLocation">load the dll from here</param>
        /// <param name="SubDomain">name of AppDomain created from the parent subdomain</param>
        /// <param name="MaxOverflow">load nore than MaxOverflow if > 0. Set negative to disable cap</param>
        /// <param name="Overflow">on exit contained additional loaded and instanced types</param>
        /// <returns></returns>
        private T LoadPlugin(TargetClassLoadName TargetFormatClassCheck,
                              string TargetFormatClassString,
                                                    AssemblyCheck VerifyCallback,
                                                    string VerifyAssemblyString,
                                                          string DllLocation,
                                                          string SubDomain,
                                                          int MaxOverflow,
                                                          out List<T> Overflow)
        {
            T Iterate = null;
            T ret = null;
            Overflow = null;
            if (MaxOverflow < 0)
            {
                Overflow = new List<T>();
            }
            /*
             * redundant but get point accross
            if (MaxOverflow == 0)
            {
                Overflow = null;
            }*/

            if (MaxOverflow >= 1)
            {
                Overflow = new List<T>(MaxOverflow);
            }

            Tool_DuplicateDomain(FormatContainer, SubDomain, out AppDomain Probe);

            AssemblyResolver_DynamicClassLoader tiny = new AssemblyResolver_DynamicClassLoader();
            Probe.AssemblyResolve += new ResolveEventHandler(tiny.Probe_AssemblyResolve);

            Assembly Dll = Probe.Load(DllLocation);

            if (VerifyCallback != null)
            {
                if (VerifyCallback(Dll) == false)
                {
                    throw new InvalidOperationException("Dll: " + Path.GetFileName(DllLocation) + " did not pass VerifyCheck");
                }
            }
            else
            {
                if (VerifyAssemblyString != null)
                {
                    if (Dll.FullName.Contains(VerifyAssemblyString) == false)
                    {
                        throw new InvalidOperationException("Dll: " + Path.GetFileName(DllLocation) + " did not pass VerifyCheck");
                    }
                }
            }


            foreach (Type DllType in  Dll.GetExportedTypes())
            {
                
                /*
                 * FUTURE ME: PAST ME WAS SLEEPY.
                 * This code is indended to check 3 scenerios for a match
                 * 
                 *  #1
                 * TargetFormatClassCheck delegate is not null and returns true on call 
                 * 
                 * #2
                 *  TargetFormatClassCheck delegate is null, and the string is contained within the type's fullname if not zero
                 *  
                 *  #3
                 *  both TargetClassCheck delegate is null and the string is null (this matches any)
                 *  
                 *  
                 *  Reason at the time is to consalodate the instance creation code on  match 
                 *  YOUR THANK ME LATER FOR THIS COMMENT,
                 *  PAST ME
                 *  
			Follow Up:  Catch Generic Routines and things man or the .net runtime was not ment to instance in a Try Catch block
                 */
                    if (((TargetFormatClassCheck != null) &&
                       (TargetFormatClassCheck(DllType.FullName, DllType.GetTypeInfo()) == true)) 
                        ||
                        ((TargetFormatClassString != null)) && ((TargetFormatClassString != null) && (DllType.FullName.Contains(TargetFormatClassString) == true)) 
                       ||
                       (TargetFormatClassString == null) && (TargetFormatClassCheck == null))
                    {
                        // it's a match
                        Iterate = new T();
                        {

                            Iterate.Domain = Probe;
                            Iterate.Handler = Activator.CreateInstance(DllType);
                            Iterate.HandlerType = DllType;
                            Iterate.LoadedAssembly = Dll;
                        };
                        if (ret == null)
                        {
                            ret = Iterate;
                            if (MaxOverflow == 0)
                            {
                                break;
                            }
                            else
                            {
                                if (MaxOverflow > 0)
                                {
                                    MaxOverflow--;
                                }
                            }
                        }
                        else
                        {
                            
                            Overflow.Add(Iterate);
                            MaxOverflow--;
                        }
                    }
               
            }


            if (ret == null)
            {
                // no matches
                AppDomain.Unload(Probe);
                Overflow.Clear();
                Overflow = null;
            }
            return ret;
        }




        #endregion

        #region Defined delegates and Callback
        /// <summary>
        /// callback. Return true if the passed Info is ok to be possibly loaded as a plugin.
        /// </summary>
        /// <param name="Info">the dll or file to check against</param>
        /// <returns>true for it to match and false to prevent said dll from being loaded</returns>
        public delegate bool PluginFilterCheck(FileInfo Info);

        /// <summary>
        /// Deleagate that should return true if the passed name and TypeInfo matchs what one is looking for
        /// </summary>
        /// <param name="name">name of the class type to check</param>
        /// <param name="Data">TypeInfo for that calss</param>
        /// <returns></returns>
        public delegate bool TargetClassLoadName(string name, TypeInfo Data);
        #endregion

        #region Scanning Plugin Folder

        
        private enum ScanPluginFolderAction
        {

        }




        /// <summary>
        /// get a list of Dlls / assemblies that match something to load
        /// </summary>
        /// <param name="TargetFolder">Check this folder</param>
        /// <param name="FilterCheck">if set, defines a routine that returns true if file object is ok to procedue with</param>
        /// <param name="PluginStringContains">only valid if FilterCheck=null, FileObjects checked contain this string within their name</param>
        /// <returns></returns>
        private List<string> GetValidExternTargets(string TargetFolder,
                                      PluginFilterCheck FilterCheck,
                                      string PluginStringContains
                                      )
        {
            List<string> OkDlls = new List<string>();
            if (string.IsNullOrEmpty(TargetFolder))
            {
                TargetFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }

            if (TargetFolder.Length > 2)
            {
                if (TargetFolder.StartsWith("\\", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    if (TargetFolder.StartsWith("\\\\", StringComparison.InvariantCultureIgnoreCase) == false)
                    {
                        TargetFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TargetFolder ;
                    }
                }
            }
            if (Directory.Exists(TargetFolder) == false)
            {
                throw new DirectoryNotFoundException(TargetFolder);
            }

            foreach (string sInfo in Directory.GetFiles(TargetFolder))
            {
                FileInfo Info = new FileInfo(sInfo);
                if (FilterCheck == null)
                {
                    if (string.IsNullOrEmpty(PluginStringContains))
                    {
                        OkDlls.Add(sInfo);
                    }
                    else
                    {
                        if (sInfo.Contains(PluginStringContains))
                        {
                            OkDlls.Add(sInfo);
                        }
                    }
                }
                else
                {
                    if (FilterCheck(Info) == true)
                    {
                        OkDlls.Add(sInfo);
                    }
                }

            }
            return OkDlls;
        }



        /// <summary>
        /// Scan the folder and instance all classes that dlls contain within them
        /// </summary>
        /// <param name="TargetFolder">Load in this folder for shared libraries / dlls</param>
        /// <param name="SharedLibraryContains">any potential plugin to load must have this in the dll name</param>
        /// <param name="ClassNameContains">only instance classes with this string somewhere in the same</param>
        public void ScanFolder(string TargetFolder, 
                               PluginFilterCheck CheckMe, 
                               string SharedLibraryContains, 
                               TargetClassLoadName CheckClassInfo,
                               string ClassNameContains)
        {
            var Targets = GetValidExternTargets(TargetFolder, CheckMe, SharedLibraryContains);
            if (Targets.Count > 0)
            {
                if (CheckClassInfo == null)
                {
                    foreach (string TargetSharedLib in Targets)
                    {
                        LoadedFileHandliers.AddRange(LoadAllPlugins(ClassNameContains, TargetSharedLib));
                    }
                }
                else
                {
                    foreach (string TargetShareLib in Targets)
                    {
                        LoadedFileHandliers.AddRange(LoadAllPlugins(CheckClassInfo, TargetShareLib));
                    }
                }
            }
        }


        #endregion

  

        /// <summary>
        /// isolate the plugins from the app itself
        /// </summary>
        private AppDomain FormatContainer;


        /// <summary>
        /// After contruction, contains a collection of instanced plugins read to be called.
        /// </summary>
        private List<T> LoadedFileHandliers;

        public List<T> GetPlugins()
        {
            return LoadedFileHandliers;
        }
    }

    /// <summary>
    /// This bare minimum for the loaded plugin for this system.
    /// To use: Redrive from this class and call the Generic routines with this.
    /// </summary>
    public class InstancedPluginContainer : IDisposable
    {
        #region Required Public Access Fields
        /// <summary>
        /// Holds the remote assembly created object.
        /// </summary>
        public object Handler { get; set; }
        /// <summary>
        /// Holds the domain the object resides in
        /// </summary>
        public AppDomain Domain { get; set; }
        /// <summary>
        /// Holds the Assembly the instanced Handler is from
        /// </summary>
        public Assembly LoadedAssembly { get; set; }
        /// <summary>
        /// Holds Type Info for the instanced Handler.
        /// </summary>
        public Type HandlerType { get; set; }
        #endregion

        /// <summary>
        /// Helper routine to get either only the passed type's exported routines or get *all* routines including base
        /// </summary>
        /// <param name="IncludeParent">false is same as Inco.DeclaredMethod. True means include inherited methods also</param>
        /// <param name="Info">class Type to examine</param>
        /// <returns></returns>
        public static List<MethodInfo> GetClassMethods(bool IncludeParent, TypeInfo Info)
        {
            if (Info == null)
            {
                throw new ArgumentNullException(nameof(Info));
            }
            if (IncludeParent == false)
            {
                return (List<MethodInfo>)Info.DeclaredMethods;
            }
            else
            {
                List<MethodInfo> ret = new List<MethodInfo>();
                if (Info.BaseType != null)
                {
                    ret.AddRange(GetClassMethods(true, Info.BaseType.GetTypeInfo()));
                }
                ret.AddRange(Info.DeclaredMethods);
                return ret;
            }
        }

        /// <summary>
        /// Unload the AppDomain on cleanup
        /// </summary>
        protected bool ReleaseOnCleanup {  get;  set; }

        #region iDispose
        protected virtual void Dispose(bool ManToo)
        {
            if (ManToo) 
            {
                AppDomain.Unload(Domain);
                Domain = null;
                HandlerType = null;
                LoadedAssembly = null;

            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        ~InstancedPluginContainer()
        {
            Dispose(false);
            
        }
        #endregion
    }

}

