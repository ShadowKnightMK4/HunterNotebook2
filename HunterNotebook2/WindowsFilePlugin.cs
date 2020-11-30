using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PluginSystem;
using System.IO;
using System.Resources;
using System.Globalization;
namespace HunterNotebook2
{
    /// <summary>
    /// The Generical handler of FileFormatHandler is intended to be C# platform agnostic.
    /// 
    /// This class is to be what the app uses to integrate the thing into the app
    /// </summary>
    public static class WindowsFilePlugin
    {

        public static void ShowExceptionOnLoad(Exception e)
        {
            string msg;
            if (e == null)
            {
                msg = string.Empty;
            }
            else
            {
                msg = e.Message;
            }
            MessageBox.Show(msg, StringResources.GetString("WindowFilePlugin_ErrorMsgOnOpen", CultureInfo.CurrentUICulture) , MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public enum FileDialogMode
        {
            UseOpenDialog = 1,
            UseSaveDialog = 2
        }

        /// <summary>
        /// Construct a filename for the prompt message
        /// </summary>
        private static string GetSourceFileName(FileDescription Desc)
        {
            StringBuilder ret = new StringBuilder();
            if (Desc.NoSource)
            {
                ret.Append("Untitled");
            }
            else
            {
                if (string.IsNullOrEmpty(Desc.SourceLocation) == false)
                {
                    ret.Append(Path.GetFileName(Desc.SourceLocation));
                }
                else
                {
                    ret.Append("unknown");
                }
            }
            return ret.ToString();
        }


        /// <summary>
        /// load from the sourcefile using the handler and place the resulting text into the FinalOut RichText
        /// </summary>
        /// <param name="SourceFile"></param>
        /// <param name="Handler"></param>
        /// <param name="FinalOut"></param>
        public static void GenericFormatLoad(string SourceFile, InstancedIFormat2 Handler, RichTextBox FinalOut,  FileDescription GuiUpdate)
        {
            
            if (GuiUpdate == null)
            {
                GuiUpdate = new FileDescription();
            }
            if (Handler == null)
            {
                throw new ArgumentNullException(nameof(Handler));
            }
            if (FinalOut == null)
            {
                throw new ArgumentNullException(nameof(FinalOut));
            }

            try
            {
                using (StreamReader source = new StreamReader(SourceFile))
                {
                    using (var GenericStream = new MemoryStream())
                    {
                        using (StreamWriter Target = new StreamWriter(GenericStream))
                        {
                            Handler.ReadData(source, Target, out bool RTFFormat);

                            Target.Flush();
                            Target.BaseStream.Position = 0;

                            byte[] Data = new byte[Target.BaseStream.Length];
                            Target.BaseStream.Read(Data, 0, (int)Target.BaseStream.Length);

                            FinalOut.Clear();
                            FinalOut.ClearUndo();
                            FinalOut.SelectAll();

                            if (RTFFormat)
                            {
                                FinalOut.SelectedRtf = Encoding.Unicode.GetString(Data);
                                
                            }
                            else
                            {
                                FinalOut.SelectedText = Encoding.ASCII.GetString(Data);
                            }

                            GuiUpdate.NoSource = false;
                            GuiUpdate.SourceLocation = SourceFile;
                            GuiUpdate.Changed = false;
                            GuiUpdate.Format = Handler;
                        }
                    }


                }
            }
            catch (IOException e)
            {
                ShowExceptionOnLoad(e);
            }
        }

        private static ResourceManager StringResources;
        public static FileDialog GetDialog(InstancedIFormat2 ThisOne, ApplicationState CurrentState, FileDialogMode Mode)
        {
            if (StringResources == null)
            {
                StringResources = new ResourceManager(typeof(StringResources.Localized_GenericDialog));
            }
            FileDialog ret = null;
            if (CurrentState == null)
            {
                throw new ArgumentNullException(nameof(CurrentState));
            }
            if (ThisOne == null)
            {
                throw new ArgumentNullException(nameof(ThisOne));
            }
            else
            {
                if (Mode.HasFlag(FileDialogMode.UseOpenDialog))
                {
                    ret = new OpenFileDialog
                    {
                        Title = StringResources.GetString("WindowFilePlugin_OpenFragment", CultureInfo.CurrentUICulture)
                    };
                }
                else
                {
                    if (Mode.HasFlag(FileDialogMode.UseSaveDialog))
                    {
                        ret = new SaveFileDialog
                        {
                            Title = StringResources.GetString("WindowFilePlugin_SaveFragment", CultureInfo.CurrentUICulture)
                        };
                    }
                }

                if (ret == null)
                {
                    throw new NotImplementedException(StringResources.GetString("WindowFilePlugin_UnsupportedMode", CultureInfo.CurrentUICulture));
                }
                ret.InitialDirectory = CurrentState.UserDocumentLocation;

                ret.Title += ThisOne.GetFriendlyName();
                ret.Filter = ThisOne.GetDialogBoxExt();
                if (ret.Filter.EndsWith("|", StringComparison.InvariantCultureIgnoreCase))
                {
                    ret.Filter += "All Files (*.*)|*.*";
                }
                else
                {
                    ret.Filter += "|All Files (*.*)|*.*";
                }

                return ret;
            }
        }

        public static void GenericFormatSave(string fileName, InstancedIFormat2 tag, RichTextBox mainWindowRichText, FileDescription currentFile)
        {
            
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            if (mainWindowRichText == null)
            {
                throw new ArgumentNullException(nameof(mainWindowRichText));
            }
            if (currentFile == null)
            {
                throw new ArgumentNullException(nameof(currentFile));
            }
            try
            {
                using (StreamWriter TargetFile = new StreamWriter(fileName))
                {
                    MemoryStream BufferForData = new MemoryStream();
                    {
                        bool RTFFormat = false;
                        byte[] WindowData = Encoding.UTF8.GetBytes(mainWindowRichText.Text);
                        BufferForData.Write(WindowData, 0, WindowData.Length);
                        BufferForData.Flush();
                        BufferForData.Position = 0;

                        using (StreamReader SourceData = new StreamReader(BufferForData))
                        {
                            tag.WriteData(SourceData, TargetFile);
                            // Why StreamReader closes here I DON'T get it but *shrugs*
                        }
                        currentFile.NoSource = false;
                        currentFile.SourceLocation = fileName;
                        currentFile.Changed = false;
                    }
                }
            }
            catch (IOException e)
            {
                ShowExceptionOnLoad(e);
            }
        }
    }
}
