﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using PluginSystem;
using HunterNotebook2.DialogBox;
using System.IO;
using System.Globalization;
using System.Resources;
using HunterNotebook2.StringResources;
using System.Text.RegularExpressions;

namespace HunterNotebook2
{
    public partial class MainWindowFormat : Form
    {
        public MainWindowFormat()
        {
            InitializeComponent();
            CurrentFile = new FileDescription
            {
                NoSource = true
            };
        }

        /// <summary>
        /// Current User State of app. Does not poin to loaded file
        /// </summary>
        public ApplicationState CurrentState { get; set; }

        public FileDescription CurrentFile { get; set; }
        /// <summary>
        /// instance of class containing the current plugins
        /// </summary>
        //public FileFormatHandler.GenericHandler FileFormatPlugins;
        public FormatHandler FileFormatPlugins { get; set; }

        public SimpleToolHandler SimpleToolPlugin { get; set; }

        BackgroundWorker CheckSyntaxWorker = new BackgroundWorker();
        /// <summary>
        /// Contains loaded string resource data
        /// </summary>
        ResourceManager MainWindow_ResourceStrings;

        ResourceManager General_ResourceStrings;

        /// <summary>
        /// holds the generic Zoom Text Template
        /// </summary>
        string Zoom_StatusBarText;
        /// <summary>
        /// Holds the Generic Word Wrap Template
        /// </summary>
        string WordWrap_StatusBarText;

        /// <summary>
        /// possible valeues for SavedStartControl_Text
        /// </summary>
        enum SavedStartControl_Key
        {
            StatusbarText_Zoom = 0,
            StatusbarText_Wordwrap = 1,
            MenuItem_Text_SearchSubMenu = 2,
            MenuItem_Text_WebSearch = 3
        }
        Dictionary<SavedStartControl_Key, string> SavedStartControl_Text = new Dictionary<SavedStartControl_Key, string>();


        #region ToolKit_Functions
        /// <summary>
        /// routien to update a statusbar toolstrip easily
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="format"></param>
        /// <param name="Argument"></param>
        private void ToolKit_SetSimpleToolStripText(ToolStripStatusLabel Target, string format, object Argument)
        {
            Target.Text = string.Format(CultureInfo.CurrentCulture, format, Argument);
        }

        /// <summary>
        /// Returns TrueText if val is true and FalseText if value is false
        /// </summary>
        /// <param name="val">check this</param>
        /// <param name="TrueText">This is returned if val is true</param>
        /// <param name="FalseText">This is returned in val is false</param>
        /// <returns>Returns TrueText if val is true and FalseText otherwise</returns>
        private static string Toolkit_ToggleToString(bool val, string TrueText, string FalseText)
        {
            if (val)
            {
                return TrueText;
            }
            return FalseText;
        }

        /// <summary>
        /// Common Get get from clipboard and put in the RichText Window. Respects the DropClipboard value in user settings
        /// </summary>
        private void ToolKit_FetchClipboard(bool Rtf)
        {
            if (CurrentState.DropClipboard_FormatMenuItems == true)
            {
                Rtf = false;
            }

            if (Rtf)
            {
                string Text;
                bool FailedToWork = false;
                if (Clipboard.ContainsText(TextDataFormat.Rtf))
                {
                    Text = Clipboard.GetText(TextDataFormat.Rtf);
                    try
                    {
                        MainWindowRichText.SelectedRtf = Text;
                    }
                    finally
                    {
                        if (FailedToWork)
                        {
                            MessageBox.Show(MainWindow_ResourceStrings.GetString("MainWindow_Clipboard_PasteFailure", CultureInfo.CurrentUICulture));
                        }
                    }
                }
                else
                {
                    Text = Clipboard.GetText(TextDataFormat.UnicodeText);
                    MainWindowRichText.SelectedText = Text;
                }

            }
            else
            {
                Text = Clipboard.GetText(TextDataFormat.UnicodeText);
                MainWindowRichText.SelectedText = Text;
            }

        }

        /// <summary>
        /// Common Thing to set clipboard
        /// </summary>
        /// <param name="Rtf">RTF or Not</param>
        /// <param name="ClearSelection">Do We clear selected text?</param>
        private void ToolKit_ClipboardSetConditional(bool Rtf, bool ClearSelection)
        {
            if (CurrentState.DropClipboard_FormatMenuItems && (Rtf == true))
            {
                CurrentState.DropClipboard_FormatMenuItems = false;
                ToolKit_ClipboardSetConditional(false, ClearSelection);
                CurrentState.DropClipboard_FormatMenuItems = true;
                return;
            }
            if (CurrentState.DropClipboard_FormatMenuItems && (Rtf == false))
            {
                Clipboard.SetText(MainWindowRichText.SelectedText);
                MainWindowRichText.SelectedText = string.Empty;
            }
            else
            {
                Clipboard.SetText(MainWindowRichText.SelectedRtf, TextDataFormat.Rtf);
                MainWindowRichText.SelectedRtf = string.Empty;
            }
        }

        /// <summary>
        /// used by the Website menu items. Launch process pointed string.format(format, FormatArgument)
        /// and use shellargument as the actual argument to the app
        /// </summary>
        /// <param name="format"></param>
        /// <param name="FormatArgument"></param>
        /// <param name="ShellArgument"></param>
        private void ToolKit_LaunchExternal(string format, object FormatArgument, string ShellArgument)
        {
            using (Process P = new Process())
            {
                P.StartInfo = new ProcessStartInfo(string.Format(CultureInfo.InvariantCulture, format, FormatArgument))
                {
                    Arguments = ShellArgument
                };
                P.Start();
            }
        }
        #endregion

        /// <summary>
        /// Anything here that needs set initial variables can go here.
        /// This is ran befure checking internal config or the user preferneces
        /// </summary>
        private void SafeSetup()
        {
            SavedStartControl_Text.Add(SavedStartControl_Key.StatusbarText_Zoom, ZoomStatusBarText_ToolStrip.Text);
            SavedStartControl_Text.Add(SavedStartControl_Key.StatusbarText_Wordwrap, WordWrapStatusBarText_ToolStrip.Text);
            SavedStartControl_Text.Add(SavedStartControl_Key.MenuItem_Text_SearchSubMenu, searchToolStripMenuItem.Text);
            SavedStartControl_Text.Add(SavedStartControl_Key.MenuItem_Text_WebSearch, webToolStripMenuItem.Text);
            Zoom_StatusBarText = ZoomStatusBarText_ToolStrip.Text;
            WordWrap_StatusBarText = WordWrapStatusBarText_ToolStrip.Text;
            ToolKit_SetSimpleToolStripText(ZoomStatusBarText_ToolStrip, Zoom_StatusBarText, MainWindowRichText.ZoomFactor);
            ToolKit_SetSimpleToolStripText(WordWrapStatusBarText_ToolStrip, WordWrap_StatusBarText, Toolkit_ToggleToString(MainWindowRichText.WordWrap, "On", "Off"));
        }

        /// <summary>
        /// look at current settings saved and update the GUI to match
        /// </summary>
        private void SyncGuiToConfig()
        {
            wordWrapToolStripMenuItem.Checked = CurrentState.WordWrapFlag;
            saveToDesktopOnShutdownToolStripMenuItem.Checked = CurrentState.SaveToDesktopOnShutdownFlag;
            askToReloadOnChangeToolStripMenuItem.Checked = CurrentState.ReloadOnChangeDetect;


            if (CurrentFile == null)
            {
                Text = string.Format(CultureInfo.InvariantCulture, MainWindow_ResourceStrings.GetString("MainWindow_Title_NoContextFormatString",
                    CultureInfo.CurrentUICulture), Assembly.GetExecutingAssembly().GetName().Name);
            }
            else
            {
                string Changed;
                if (CurrentFile.Changed)
                {
                    Changed = "*";
                }
                else
                {
                    Changed = string.Empty;
                }

                Text = string.Format(CultureInfo.InvariantCulture,
                    MainWindow_ResourceStrings.GetString("MainWindow_Title_HasContextFormatString", CultureInfo.CurrentUICulture),
                    Assembly.GetExecutingAssembly().GetName().Name, 
                    CurrentFile.SourceLocation, Changed);
            }

            AdjustZoom(MainWindowRichText.ZoomFactor);


        }



        private void MainWindowFormat_Load(object sender, EventArgs e)
        {
            SafeSetup();
            MainWindow_ResourceStrings = new ResourceManager(typeof(Localized_MainWindowStrings));
            General_ResourceStrings = new ResourceManager(typeof(Localized_GenericDialog));
            {
                SyncGuiToConfig();
            }




        }


        /// <summary>
        /// Toggle the ask to reload state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AskToReloadOnChangeToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            CurrentState.ReloadOnChangeDetect = askToReloadOnChangeToolStripMenuItem.Checked;
            // don't forget to enable or disable backbround worker as needed
        }

        /// <summary>
        /// toggle the save to desktop on Windows Shutdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveToDesktopOnShutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentState.SaveToDesktopOnShutdownFlag = saveToDesktopOnShutdownToolStripMenuItem.Checked;

        }


        /// <summary>
        /// This looops thru the loaded plugins and populates the SimpleTools Menu Item. Should nothing be loaded. It hides the menu item instead
        /// </summary>
        private void LinkDyamicSimpleTools()
        {
            simpleToolToolStripMenuItem.DropDownItems.Clear();

            foreach (InstancedSimpleToolPlugin Tool in SimpleToolPlugin.GetPlugins())
            {

              var Item =  simpleToolToolStripMenuItem.DropDownItems.Add(Tool.GetMenuItemName(), null, GenericSimpleTool_OnClick);
                Item.Tag = Tool.GetMenuItemCommand();
            }

            if (simpleToolToolStripMenuItem.DropDownItems.Count == 0)
            {
                simpleToolToolStripMenuItem.Visible = false;
            }
        }

        /// <summary>
        /// Simple Tool Exec().  Get the text, replace the { } stuff with what the tool wants and launch it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// {0} becomes the current file (or untitled.txt) if never saved.
        /// {1} becomes the currently selected text (or an empty string) if nother is selected
        /// {2} becomes the *entire* contents of the text box
        /// </remarks>
        private void GenericSimpleTool_OnClick(object sender, EventArgs e)
        {
            ToolStripItem That = (ToolStripItem)sender;
            string tag = (string)That.Tag;
            if (tag.Contains("{0}"))
            {
                tag = tag.Replace("{0}", CurrentFile.SourceLocation);
            }

            if (tag.Contains("{1}"))
            {
                tag = tag.Replace("{1}", MainWindowRichText.SelectedText);
            }
            if (tag.Contains("{2}"))
            {
                tag = tag.Replace("{2}", MainWindowRichText.Text);
            }
            
            using (Process cmd = new Process())
            {
                cmd.StartInfo = new ProcessStartInfo();
                cmd.StartInfo.FileName = tag;
                cmd.Start();
            }
        }

        /// <summary>
        /// This loops thru the loaded plugins and dynamicly recreates their menu with each call.
        /// The class that handles the format is stored in the Tag value of the ToolStripItems.
        /// The Generic Event on click fetchs the tag and either opens or saves the file based on what to do
        /// </summary>
        private void LinkDyamicFileFormatMenus()
        {
            // clear the sub menus
            saveAsToolStripMenuItem.DropDownItems.Clear();
            openToolStripMenuItem.DropDownItems.Clear();


            // loop thru each instanced iFormat and make a save and load menu item for it under each area
            foreach (InstancedIFormat2 Format in FileFormatPlugins.GetPlugins())
            {
                var SaveItemEntry=  saveAsToolStripMenuItem.DropDownItems.Add(Format.GetShortName(), null, GenericFile_SaveMenuOnClick);
                var OpenItemEntry = openToolStripMenuItem.DropDownItems.Add(Format.GetShortName(), null, GenericFile_OpenMenuOnClick);

                // don't forget the tags
                SaveItemEntry.Tag = OpenItemEntry.Tag = Format;
            }
        }

        /// <summary>
        /// Wipe the loaded file from the app 
        /// </summary>
        private void ClearLoadedFile()
        {
            CurrentFile = new FileDescription();
            MainWindowRichText.Clear();
            MainWindowRichText.ClearUndo();
            SyncGuiToConfig();
        }


        /// <summary>
        /// Generic File Format on Open event. Common to each menu item under the open menu
        /// 
        /// </summary>
        /// <remarks>The Tag should be an Instanced_IFormat class instance</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenericFile_OpenMenuOnClick(object sender, EventArgs e)
        {

            // Unbox and show a dlg 
            ToolStripMenuItem unbox = (ToolStripMenuItem)sender;
            using (FileDialog dlg = WindowsFilePlugin.GetDialog((InstancedIFormat2)unbox.Tag, CurrentState, WindowsFilePlugin.FileDialogMode.UseOpenDialog))
            {
                var result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
               
                    WindowsFilePlugin.GenericFormatLoad(dlg.FileName, (InstancedIFormat2)unbox.Tag, MainWindowRichText, CurrentFile);
                    SyncGuiToConfig();
                    MainWindowRichText.TextChanged += MainWindowRichText_TextChanged;
                }

            }
        }

        /// <summary>
        /// Generic File Format on Save event. Common to each menu item under the save menu
        /// 
        /// </summary>
        /// <remarks>The Tag should be an Instanced_IFormat class instance</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenericFile_SaveMenuOnClick(object sender, EventArgs e)
        {
            ToolStripMenuItem unbox = (ToolStripMenuItem)sender;
            using (FileDialog dlg = WindowsFilePlugin.GetDialog((InstancedIFormat2) unbox.Tag, CurrentState, WindowsFilePlugin.FileDialogMode.UseSaveDialog))
            {
                var result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    WindowsFilePlugin.GenericFormatSave(dlg.FileName, (InstancedIFormat2)unbox.Tag, MainWindowRichText,  CurrentFile);
                    SyncGuiToConfig();
                    MainWindowRichText.TextChanged += MainWindowRichText_TextChanged;
                }
                
            }
        }

        /// <summary>
        /// shows the window. Do any final preready
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowFormat_Shown(object sender, EventArgs e)
        {
            StringBuilder errmsgs = new StringBuilder();
            if (CurrentState == null)
            {
                CurrentState = new ApplicationState();
                errmsgs.AppendLine(MainWindow_ResourceStrings.GetString("MainWindow_OnShow_FailToLoadConfigStateMessage", CultureInfo.CurrentUICulture));
            }
            else
            {
                if (CurrentState.LoadErrors.Count > 0)
                {
                    errmsgs.AppendLine(MainWindow_ResourceStrings.GetString("MainWindow_OnShow_ErrorInConfigStateOnLoadMessage", CultureInfo.CurrentUICulture));
                    foreach (Exception err in CurrentState.LoadErrors)
                    {
                        errmsgs.AppendLine(err.Message);
                    }
                }
            }


            if (SimpleToolPlugin == null)
            {
                errmsgs.AppendLine("No Simple Tools Found");
            }
            else
            {
                LinkDyamicSimpleTools();
            }
            if (FileFormatPlugins == null)
            {
                errmsgs.AppendLine(MainWindow_ResourceStrings.GetString("MainWindow_PluginMessages_PluginFormatLoadFail", CultureInfo.CurrentUICulture));
            }
            else
            {
                // generate the open and save menuitems for each loaded plugin
                LinkDyamicFileFormatMenus();
            }
            // show error if non empty
            if (errmsgs.Length > 0)
            {
                MessageBox.Show(errmsgs.ToString(),
                    General_ResourceStrings.GetString("General_NoticeTitle", CultureInfo.CurrentUICulture),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            TextWindowMode = CurrentTextMode.DynamicHighlighter;

            MakeRichTextWindow();
        
        }


        #region Edit Menu Items
        /// <summary>
        /// Cut text with no formating "Cut"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolKit_ClipboardSetConditional(false, true);
        }

        /// <summary>
        /// if checked the edit standard menu items already disregard formatting.
        /// if false formatting is preserved.  THis is the "Drop Formatting" Item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DropFormattingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentState.DropClipboard_FormatMenuItems = dropFormattingToolStripMenuItem.Checked;
        }

        
        /// <summary>
        /// A default: double clicking copies with No Formatting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyToolStripMenuItem_DoubleClick(object sender, EventArgs e)
        {
            ToolKit_ClipboardSetConditional(false, false);
        }

        /// <summary>
        /// Menu Event Click for "Copy Text Only" - Copy selected text only to the clipboard with no formatting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyTextOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolKit_ClipboardSetConditional(false, false);
        }

        /// <summary>
        /// "Copy With Formatting" Menu Event - Copy Selected Rich Text to the clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyFormattingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolKit_ClipboardSetConditional(true, false) ;
        }

        
        /// <summary>
        /// Select all text in the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWindowRichText.SelectAll();
        }

        /// <summary>
        /// clear selected text in the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWindowRichText.SelectedText = string.Empty;
            
        }
        


       

        /// <summary>
        /// Menu On Click for "Cut Text Only" to cut selected text away with no formatting. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutTextOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolKit_ClipboardSetConditional(false, true);
        }

        /// <summary>
        /// Menu On Click for "Cut With Formatting" to cut selected text as rich text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutWithFormattingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolKit_ClipboardSetConditional(true, true);
        }

        /// <summary>
        ///  Menu event for "Cut" TODO: Cut with no formatting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutToolStripMenuItem_DoubleClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Paste with Text Only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasteTextOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolKit_FetchClipboard(false);
        }

        private void PasteWithFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolKit_FetchClipboard(true);
        }
        #endregion

        #region Format Sub Menu Events
        /// <summary>
        /// menu button to display the choose font dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int start, len;
            using (var FontForm = new ChooseFontForm())
            {
                if (FontForm.ShowDialog() == DialogResult.OK)
                {
                    if (FontForm.AffectAllText)
                    {
                        start = MainWindowRichText.SelectionStart;
                        len = MainWindowRichText.SelectionLength;
                        MainWindowRichText.SelectAll();
                        MainWindowRichText.SelectionFont = FontForm.Result;
                        MainWindowRichText.SelectionStart = start;
                        MainWindowRichText.SelectionLength = len ;
                    }
                    else
                    {
                        MainWindowRichText.SelectionFont = FontForm.Result;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefaultLevelTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Start = MainWindowRichText.SelectionStart;
            var Len = MainWindowRichText.SelectionLength;
            MainWindowRichText.SelectAll();
            MainWindowRichText.SelectionFont = new Font("Times New Roman", 12.0f);

            MainWindowRichText.SelectionStart = Start;
            MainWindowRichText.SelectionLength = Len;
        }

        /// <summary>
        /// Menu Button to Level text with last chosen font
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LastChosenFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DefaultLevelTextToolStripMenuItem_Click(sender, e);
        }
        #endregion

        #region Search Options


        /// <summary>
        /// Preform some formatted on text before sending it to WWW
        /// </summary>
        /// <param name="template"></param>
        /// <param name="text"></param>
        private void Filtered_WebLaunch(string template, string text)
        {
            text = text.Replace(' ', '+');
            ToolKit_LaunchExternal(template, text, null);
        }


        /// <summary>
        /// google selected text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Google_SelectedTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filtered_WebLaunch("https://www.google.com/search?q={0}", MainWindowRichText.SelectedText);
        }

        /// <summary>
        /// bing selected text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bing_SelectedTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filtered_WebLaunch("https://www.bing.com/search?q={0}", MainWindowRichText.SelectedText);
        }

        /// <summary>
        /// Theasurus selected text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThesaurusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filtered_WebLaunch("https://www.thesaurus.com/browse/{0}", MainWindowRichText.SelectedText);
        }

        /// <summary>
        /// dictionary selected text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DictionaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filtered_WebLaunch("https://www.dictionary.com/browse/{0}", MainWindowRichText.SelectedText);
        }
        #endregion

        #region Visual Options

        /// <summary>
        /// Adjust the zoom of the main Window Rich Text, update gui
        /// </summary>
        /// <param name="NewValue"></param>
        /// <returns></returns>
        private float AdjustZoom(float NewValue)
        {
            float old;
            old = MainWindowRichText.ZoomFactor;
            MainWindowRichText.ZoomFactor = NewValue;
            ZoomStatusBarText_ToolStrip.Text = string.Format(CultureInfo.CurrentCulture, Zoom_StatusBarText, NewValue);

            return old;
        }

        /// <summary>
        /// update the word wrap, the statusbar and the currentstate class on check changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordWrapToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            MainWindowRichText.WordWrap = wordWrapToolStripMenuItem.Checked;
            CurrentState.WordWrapFlag = wordWrapToolStripMenuItem.Checked;

            WordWrapStatusBarText_ToolStrip.Text = string.Format(CultureInfo.CurrentCulture, WordWrap_StatusBarText, Toolkit_ToggleToString(MainWindowRichText.WordWrap, "On", "Off"));
        }

        /// <summary>
        /// "Increase Zoom" Event - increaze existing zoom by 5%
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IncreaseZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdjustZoom(MainWindowRichText.ZoomFactor * 1.05f);
        }

        /// <summary>
        /// "Decrease Zoom" Evnet - decrease zoom by 5%
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DecreaseZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdjustZoom(MainWindowRichText.ZoomFactor * 0.95f);
        }

        /// <summary>
        /// "Reset Zoom" Event - resent zoom to 100%
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdjustZoom(1.0f);
        }
        /// <summary>
        /// Show the dialog to enable fine tuning the zoom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var FancyBox = new ChooseZoom())
            {
                FancyBox.ZoomFactor = MainWindowRichText.ZoomFactor;

                if (FancyBox.ShowDialog(MainWindowRichText) == DialogResult.OK)
                {
                    MainWindowRichText.ZoomFactor = FancyBox.ZoomFactor;
                }
            }
        }

        #endregion

      

        private bool SaveConfigFile()
        {
            bool result = true;
            // TODO: some kind of debug check to ensure we don't go down in flames
            CurrentState.SaveConfig();
            return result;
        }
        private void MainWindowFormat_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.WindowsShutDown:
                    {
                        if (CurrentState.SaveToDesktopOnShutdownFlag)
                        {
                            // save the file.
                        }
                        break;
                    }
                default:
                    {
                        // nothing 
                        break;
                    }
            }

            // finally save the config file
            if (!SaveConfigFile() && (e.CloseReason != CloseReason.WindowsShutDown))
            {
                MessageBox.Show(MainWindow_ResourceStrings.GetString("Shutdown_FailureToSaveConfigMessage", CultureInfo.CurrentUICulture));
            }
        }

       

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentFile.Changed)
            {
                DialogResult yesno = MessageBox.Show(MainWindow_ResourceStrings.GetString("NewMenu_PromptChangesText", CultureInfo.CurrentUICulture),
                                                     General_ResourceStrings.GetString("General_WarningTitle", CultureInfo.CurrentUICulture),
                                                     MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (yesno == DialogResult.Yes)
                {
                    ClearLoadedFile();
                    MainWindowRichText.TextChanged += MainWindowRichText_TextChanged;
                }
            }
        }

        private void BingSpellcheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var Fancy = new DialogBox.SpellCheckDeluxDialog())
            {
                Fancy.ShowDialog();
            }
        }

        DialogBox.FindDialog Search = new DialogBox.FindDialog();
        private void FindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Search == null)
            {
                Search = new FindDialog
                {
                    SearchThis = MainWindowRichText
                };
                Search.Show();
            }
            else
            {
                Search.SearchThis = MainWindowRichText;
                Search.Show();
            }
        }

        private void SearchToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (DesignMode == true)
            {
                return;
            }
            if ( (MainWindowRichText.SelectedText.Length > 0) && (MainWindowRichText.SelectedText != null))
            {
                googleToolStripMenuItem.Enabled =
                bingToolStripMenuItem.Enabled =
                thesaurusToolStripMenuItem.Enabled =
                webToolStripMenuItem.Enabled =
                dictionaryToolStripMenuItem.Enabled = true;
                webToolStripMenuItem.Text = string.Format(CultureInfo.InvariantCulture, SavedStartControl_Text[SavedStartControl_Key.MenuItem_Text_WebSearch], string.Empty);
            }
            else
            {
                googleToolStripMenuItem.Enabled =
                bingToolStripMenuItem.Enabled =
                thesaurusToolStripMenuItem.Enabled =
                webToolStripMenuItem.Enabled =
                dictionaryToolStripMenuItem.Enabled = false;
                webToolStripMenuItem.Text = string.Format(CultureInfo.InvariantCulture, SavedStartControl_Text[SavedStartControl_Key.MenuItem_Text_WebSearch], "(Highlight Text First)"); 
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name;
            if (CurrentFile.Changed)
            {
                if (CurrentFile.NoSource == true)
                {
                    name = "This unsaved file";
                }
                else
                {
                    if (! string.IsNullOrEmpty(CurrentFile.SourceLocation))
                    {
                        name = Path.GetFileName(CurrentFile.SourceLocation);
                    }
                    else
                    {
                        name = "unknown file";
                    }
                }

                switch (MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "{0} has been changed. Save changes before exit?", name), MainWindow_ResourceStrings.GetString("MainWindow_MessageTitle_SaveChanges", CultureInfo.CurrentUICulture), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Cancel:
                        return;
                    // break;
                    case DialogResult.Yes:
                        {
                            break;
                        }
                    case DialogResult.No:
                        {
                            break;
                        }
                }

                Close();
            }
        }


        

        private void MainWindowRichText_KeyPress(object sender, KeyPressEventArgs e)
        {
   
        }

        private void aboutAppToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void alwaysOnTopToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = alwaysOnTopToolStripMenuItem.Checked;
        }
    }
}
