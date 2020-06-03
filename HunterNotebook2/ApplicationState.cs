using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace HunterNotebook2
{
    /// <summary>
    /// Used to hold user's preferences and settings. Saved as Xml and reloaded as needed
    /// </summary>
    public class ApplicationState
    {

      
        /// <summary>
        /// try all locations locations
        /// </summary>
        /// <returns></returns>
         public static string GetConfigLocation()
        {
            string option;
            Stream JustExist = null;
            option = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HNotebook.config.xml");
            try
            {
                using ( JustExist = File.Open(option, FileMode.OpenOrCreate))
                {
                    return option;
                }
            }
            catch (UnauthorizedAccessException)
            {
                option = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HNotebook.config.xml");
                try
                {
                    using ( JustExist = File.Open(option, FileMode.OpenOrCreate))
                    {
                        return option;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    option = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HNotebook.config.xml");
                    try
                    {
                        using ( JustExist = File.Open(option, FileMode.OpenOrCreate))
                        {
                            return option;
                        }
                    }
                    catch (IOException)
                    {
                        option = string.Empty;
                    }
                    finally
                    {

                    }

                }
            }
            finally
            {
                JustExist?.Dispose();
            }

            return option;
        }

        public void SaveConfig()
        {
            string target = GetConfigLocation();
            using (StreamWriter s = new StreamWriter(target))
            {
                XmlSerializer WriteMe = new XmlSerializer(typeof(ApplicationState));
                WriteMe.Serialize(s, this);
            }
        }

        public static void SaveConfig(ApplicationState state)
        {
            state?.SaveConfig();
        }

        public static ApplicationState LoadConfig()
        {
            string target = GetConfigLocation();
            using (StreamReader s = new StreamReader(target))
            {
                XmlSerializer ReadMe = new XmlSerializer(typeof(ApplicationState));
                //return (ApplicationState)ReadMe.Deserialize( s);
                using (var SaferXmlRead = XmlReader.Create(s))
                {
                    return (ApplicationState)ReadMe.Deserialize(SaferXmlRead);
                }
            }

        }
        /// <summary>
        /// Last word wrap prefernece
        /// </summary>
        public bool WordWrapFlag;

        /// <summary>
        /// If set then the file is saved to logged in user's desktop on shutdown
        /// </summary>
        public bool SaveToDesktopOnShutdownFlag;
        /// <summary>
        /// If true then app offers to reload changed file
        /// </summary>
        public bool ReloadOnChangeDetect;

        public bool DropClipboard_FormatMenuItems;



        #region Prefered User save location
        public enum UserDocumentPreference
        {
            /// <summary>
            /// My Docments
            /// </summary>
            DefaultUserDocumentLocation = 0,
            /// <summary>
            /// Hardcoded_Userdoc i sued
            /// </summary>
            UseHardcodedPath = 1,

            /// <summary>
            /// Special_UserDocument is used
            /// </summary>
            ForcedSpecialState = 2,
        }

        /// <summary>
        /// Specifies the UserLocation PReference
        /// </summary>
        public UserDocumentPreference DefaultUserLocation;
        /// <summary>
        /// Contains a  hard coded string to default to
        /// </summary>
        public string Hardcoded_UserDocument;
        /// <summary>
        /// Points to a const that specifies the folder to user
        /// </summary>
        public Environment.SpecialFolder Special_UserDocument;


        /// <summary>
        /// get the user's prefered document location based on other settings in the class
        /// </summary>
        [XmlIgnore]
        
        public string UserDocumentLocation
        {
            get
            {
                switch (DefaultUserLocation)
                {
                    case UserDocumentPreference.DefaultUserDocumentLocation:
                        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    case UserDocumentPreference.ForcedSpecialState:
                        return Environment.GetFolderPath(Special_UserDocument);
                    case UserDocumentPreference.UseHardcodedPath:
                        return Hardcoded_UserDocument;
                    default:
                        throw new NotSupportedException("Additional possibilites for the UserDocument are not done yet. Current Mode " + Enum.GetName(typeof(UserDocumentPreference), DefaultUserLocation)  + " is not supported");
                }
            }
        }

        #endregion


        /// <summary>
        /// On load before first shown this contains state load errors. MainWindow.Shown evals this 
        /// </summary>
        [XmlIgnore]
        public List<Exception> LoadErrors = new List<Exception>();
    

    }
}
