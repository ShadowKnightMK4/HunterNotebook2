using HunterNotebook2.SpecializedControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HunterNotebook2
{
    partial class MainWindowFormat
    {

        private enum CurrentTextMode
        {
            StandardRichText = 1,
            DynamicHighlighter = 2
        }


        CurrentTextMode TextWindowMode = CurrentTextMode.StandardRichText;


        /// <summary>
        /// make the dynamic window
        /// </summary>
        private void LinkRichTextWindow()
        {
            MainWindowRichText = new System.Windows.Forms.RichTextBox();
            MainWindowRichText.TextChanged += MainWindowRichText_TextChanged;
            MainWindowRichText.KeyPress += MainWindowRichText_KeyPress;
            MainWindowRichText.VScroll += MainWindowRichText_VScroll;
            MainWindowRichText.HScroll += MainWindowRichText_HScroll;
            
        }

        private void MainWindowFormat_Paint(object sender, PaintEventArgs e)
        {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LinkSyntaxText()
        {
            AutoTextBox tmp = new AutoTextBox();
            tmp.Parent = this;
            tmp.Left = 20;
            tmp.Top
                 = 40;
            /*
            MainWindowRichText.TextChanged += MainWindowRichText_TextChanged;
            MainWindowRichText.KeyPress += MainWindowRichText_KeyPress;
            MainWindowRichText.VScroll += MainWindowRichText_VScroll;
            MainWindowRichText.HScroll += MainWindowRichText_HScroll;
            ((Control)tmp).Paint += MainWindowFormat_Paint1; */
        }

        private void MainWindowFormat_Paint1(object sender, PaintEventArgs e)
        {
            Refresh();
        }

        private void MakeRichTextWindow()
        {
            if (MainWindowRichText != null)
            {
                MainWindowRichText.Dispose();
            }
            switch (TextWindowMode)
            {
                case CurrentTextMode.StandardRichText:
                    LinkRichTextWindow();
                    break;
                case CurrentTextMode.DynamicHighlighter:
                    LinkSyntaxText();
                    
                    break;

            }

            MainWindowRichText.Parent = this;
            MainWindowRichText.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Fires once then turns itself off. SyncGui() adds it. Res09hs8boe for setting changed flag in the AppSetting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowRichText_TextChanged(object sender, EventArgs e)
        {
            CurrentFile.Changed = true;
            MainWindowRichText.TextChanged -= MainWindowRichText_TextChanged;
            SyncGuiToConfig();
            ResetHighlighter();
        }


        private void MainWindowRichText_HScroll(object sender, EventArgs e)
        {
            MainWindowRichText.Invalidate();
        }

        private void MainWindowRichText_VScroll(object sender, EventArgs e)
        {
            MainWindowRichText.Invalidate();
        }
    }
}
