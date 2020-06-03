using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace FileFormatHandler
{

    
    /// <summary>
    /// File format handler.
    /// Respecial for scanning plugn folders and loading plugins that work support file formats
    /// 
    /// This leads into a bit of Windows with the FileDialogExt() routine in the defination, however
    /// it is intended to be platform agnostic 
    /// </summary>
    [Serializable]
    public class GenericHandler
    {


        #region Constructors
        /// <summary>
        /// Check the defualt plugin folder (currently CD) and load any built in plugins also
        /// </summary>
        public GenericHandler()
        {
            Ini();
            ScanPluginFolder(null);
        }


        

        /// <summary>
        /// common ini that is shared with all constructers
        /// </summary>
        private void Ini()
        {
            AppDomainSetup Setup = new AppDomainSetup();
            Setup = AppDomain.CurrentDomain.SetupInformation;
            FormatContainer = AppDomain.CreateDomain("Hunternotebook File Handler Domain Parent", AppDomain.CurrentDomain.Evidence, Setup);
            LoadedFileHandliers = new List<Instanced_IFormat>();
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


        

        /// <summary>
        /// Can Target Dll and load all classes that match the iformatspec into the thing.
        /// </summary>
        /// <param name="TargetFormatClass">class to load</param>
        /// <param name="DllLocation">dll to load from</param>
        /// <param name="Probe">used to store dll when checking the class out. Does not hold final load. Recommanded to unload domain once done batching callers to this routine</param>
        /// <returns>Instanced_iFormat read to be called from in the caller</returns>
        public List<Instanced_IFormat> LoadFormats(string TargetFormatClass, string DllLocation, string DomainFriendly)
        {
            List<Instanced_IFormat> ret = new List<Instanced_IFormat>();
            Instanced_IFormat ThisHandler = new Instanced_IFormat();

            AppDomain Probe;
            Tool_DuplicateDomain(AppDomain.CurrentDomain, "Format Container for " + DomainFriendly, out Probe);

            Probe.AssemblyResolve += new ResolveEventHandler(Probe_AssemblyResolve);
            Assembly DllContainer = Probe.Load(DllLocation);
            if (TargetFormatClass == null)
            {
                TargetFormatClass = string.Empty;
            }
            {
                foreach (Type ExportedType in DllContainer.GetExportedTypes())
                {
                    // TODO: adjust matching code or make a more virgoes orutine for this
                    if (ExportedType.Name.Contains("iFormat") && (ExportedType.Name.Contains(TargetFormatClass)) && (ExportedType.Name.Contains("NOEXPORT") == false))
                    {
                        // is match. 
                        ThisHandler.Domain = Probe;
                        ThisHandler.LoadedAssembly = DllContainer;
                        ThisHandler.Handler =  Activator.CreateInstance(ExportedType, new object[] { });
                        ThisHandler.HandlerType = ExportedType;
                        ret.Add(ThisHandler);

                        ThisHandler = new Instanced_IFormat();

                    }
                }
            }

         
            return ret;
        }

      
        private Assembly Probe_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.LoadFile(args.Name);
        }

        #region Scanning Plugin Folder

        private class ScanError
        {
            public ScanError()
            {

            }
            public ScanError(Exception E, string File)
            {
                Triggered = E;
                FileName = File;
            }
            public Exception Triggered;
            public string FileName;
        }
        private void ScanPluginFolder(string TargetFolder)
        {
            List<ScanError> ScanErrors = new List<ScanError>();
            if (string.IsNullOrEmpty(TargetFolder))
            {
                ///set target folder to current assembly's running location
                TargetFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); 
            }
            else
            {
                if (TargetFolder.Length > 2)
                {
                    if (TargetFolder.StartsWith("\\\\")) // possible UNC path. don't assume sub directory
                    {
                        
                    }
                    else
                    {
                        if (TargetFolder.StartsWith("\\"))
                        {
                            TargetFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TargetFolder;
                        }
                    }
                }
            }


            foreach (string Name in Directory.EnumerateFiles(TargetFolder, "*.dll", SearchOption.TopDirectoryOnly))
            {
                string possName =Name;
                FileInfo FileData = null;
                try
                {
                    FileData = new FileInfo(possName);
                }
                catch (IOException e)
                {
                    // just save the erros until the end;
                    ScanErrors.Add(new ScanError(e, possName));
                }

                if (FileData != null)
                {
                    LoadedFileHandliers.AddRange(this.LoadFormats(null, possName, "PluginContainer"));
                }
            }
        }

        #endregion

        #region Intenal Format Support
        private void AddInternalFormat()
        {

        }
        #endregion


        /// <summary>
        /// isolate the plugins from the app itself
        /// </summary>
        private AppDomain FormatContainer;


        /// <summary>
        /// After contruction, contains a collection of instanced plugins read to be called.
        /// </summary>
        private List<Instanced_IFormat> LoadedFileHandliers;

        public List<Instanced_IFormat> GetPlugins()
        {
            return LoadedFileHandliers;
        }
    }


    /// <summary>
    /// a format that was locaded
    /// </summary>
    public class Instanced_IFormat : iFormat
    {
        public object Handler;
        public AppDomain Domain;
        public Assembly LoadedAssembly;
        public Type HandlerType;
        public void ReadData(StreamReader Source, StreamWriter Output, out bool ContainsRtfTags)
        {
            ContainsRtfTags = false;
            HandlerType.GetMethod("ReadData").Invoke(Handler, new object[] { Source, Output, ContainsRtfTags });
        }

        public void WriteData(StreamReader Source, StreamWriter Output)
        {
            HandlerType.GetMethod("WriteData").Invoke(Handler, new object[] { Source, Output });
        }

        public string GetDialogBoxExt()
        {
            return (string)HandlerType.GetMethod("GetDialogBoxExt").Invoke(Handler, new object[] { });
        }

        public string GetShortName()
        {
            return (string)HandlerType.GetMethod("GetShortName").Invoke(Handler, new object[] { });
        }

        public string GetFriendlyName()
        {
            return (string)HandlerType.GetMethod("GetFriendlyName").Invoke(Handler, new object[] { });
        }
    }

    [Obsolete("")]
    public interface iFormat
    {
        /// <summary>
        /// Read the file in whatever format it and write the text to target
        /// </summary>
        /// <param name="Source">Source (likely a file) to read from</param>
        /// <param name="Target">Target to write too (the app assigns this to the main window)</param>
        /// <param name="AssignRtf">Set if the Target output contains RTF tags</param>
        void ReadData(StreamReader Source, StreamWriter Output, out bool ContainsRtfTags);

        /// <summary>
        /// Read the text from source and encode it in the file format
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Target"></param>
        void WriteData(StreamReader Source, StreamWriter Output);

    }
}

