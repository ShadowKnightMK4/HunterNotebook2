using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileFormatHandler;
using System.IO;
using System.Resources;

[assembly: NeutralResourcesLanguage("en")]

namespace HunterNotebook2
{
    static class Program
    {
        //static GenericHandler fileFormats;
        static FormatHandler FileFormats;
        static ApplicationState CurrentState;

        /// <summary>
        /// load the config file
        /// </summary>
        public static void FetchConfigFile()
        {
            try
            {
                CurrentState = ApplicationState.LoadConfig();
            }
            catch (InvalidOperationException e)
            {
                // Welcome, Everything  is fine. (just use defualt)
                CurrentState = new ApplicationState();
                CurrentState.LoadErrors.Add(e);

                if (InternalFlags.WipeConfigOnError)
                {
                    File.Delete(ApplicationState.GetConfigLocation());
                }
            }
            finally
            {
                if (File.Exists(ApplicationState.GetConfigLocation()))
                {
                    if (InternalFlags.ExploreConfigLocationOnLoad)
                    {
                        using (System.Diagnostics.Process Exploreme = new System.Diagnostics.Process())
                        {
                            Exploreme.StartInfo = new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe"),
                                Arguments = Path.GetDirectoryName(ApplicationState.GetConfigLocation())
                            };
                            Exploreme.Start();
                        }
                    }
                }
            }
        }
        public static void FormatChecker(out FormatHandler Formats)
        //public static void FormatChecker(out GenericHandler Formats)
        {
            /*fileFormats = new GenericHandler();
            Formats = fileFormats;*/
            FileFormats = new FormatHandler();
            Formats = FileFormats;
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //FormatChecker(out fileFormats);
            FormatChecker(out FileFormats);
            FetchConfigFile();
            using (var MainWindow = new MainWindowFormat())
            {
                MainWindow.FileFormatPlugins = FileFormats;
                MainWindow.CurrentState = CurrentState;
                Application.Run(MainWindow);
            }
        }
    }
}
