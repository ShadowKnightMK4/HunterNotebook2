using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Resources;

namespace HunterNotebook2.DialogBox
{
    public partial class FindDialog : Form
    {
        public FindDialog()
        {
            InitializeComponent();
        }

        public TextBoxBase SearchThis { get; set;  } = null;

        struct BakedSettings
        {
            public bool EnableCase;
            public bool EnableWildcard;
            public bool EnableRegular;
            public string BakedString;
            public string BakedSearch;
        }

        Regex SearchTextCode;
        BakedSettings Settings = new BakedSettings();
        ResourceManager LocalizedStrings;

        private void FindDialog_Load(object sender, EventArgs e)
        {
            LocalizedStrings = new ResourceManager(typeof(StringResources.Localized_FindDialogStrings));
            ButtonFindFirst.Text = LocalizedStrings.GetString("FindDialog_FindFirstButtonTitle", CultureInfo.CurrentUICulture);
            ButtonFindNext.Text = LocalizedStrings.GetString("FindDialog_FindNextButtonTitle", CultureInfo.CurrentUICulture);
            ButtonFindLast.Text = LocalizedStrings.GetString("FindDialog_FindLastButtonTitle", CultureInfo.CurrentUICulture);

            CheckBoxEnableCaseSensitive.Text = LocalizedStrings.GetString("FindDialog_Checkbox_CaseMatters", CultureInfo.CurrentUICulture);
            CheckBoxEnableRegularExpressions.Text = LocalizedStrings.GetString("FindDialog_Checkbox_AllowUnfilteredExpression", CultureInfo.CurrentUICulture);
            CheckBoxEnableWildcards.Text = LocalizedStrings.GetString("FindFialog_Checkbox_AllowWildcard", CultureInfo.CurrentUICulture);
            CheckBoxLoopToStart.Text = LocalizedStrings.GetString("FindDialog_Checkbox_RestartAtTop", CultureInfo.CurrentUICulture);

        }


        private string FindPatternSetup()
        {
            if (CheckBoxEnableRegularExpressions.Checked)
            {
                return TextBoxSearchString.Text;
               }
            else
            {
                if (CheckBoxEnableWildcards.Checked)
                {
                    string tmp = Regex.Escape(TextBoxSearchString.Text);
                    tmp = tmp.Replace("\\*", ".*");
                    tmp = tmp.Replace("\\?", ".");
                    tmp = string.Format(CultureInfo.InvariantCulture, "^{0}+$", tmp);


                    return tmp;
                }
                else 
                {
                    return string.Format(CultureInfo.InvariantCulture, "^{0}+$", TextBoxSearchString.Text);                }
            }
        }
        private void FindSetup()
        {
            RegexOptions Opts = RegexOptions.Compiled;

            if (CheckBoxEnableCaseSensitive.Checked == false)
            {
                Opts |= RegexOptions.IgnoreCase;
            }

            //if ((CheckBoxEnableWildcards.Checked == true) || (CheckBoxEnableRegularExpressions.Checked == true))
            {
                SearchTextCode = new Regex(FindPatternSetup(), Opts);
            }

            Settings.EnableCase = CheckBoxEnableCaseSensitive.Checked;
            Settings.EnableWildcard = CheckBoxEnableWildcards.Checked;
            Settings.EnableRegular = CheckBoxEnableRegularExpressions.Checked;
            Settings.BakedString = SearchThis.Text;
            Settings.BakedSearch = FindPatternSetup();


            

        }
        private void FindDialog_Shown(object sender, EventArgs e)
        {
            if (SearchThis == null)
            {
                foreach (Control fail in this.Controls)
                {
                    fail.Enabled = false;
                }
            }
            else
            {

                foreach (Control fail in this.Controls)
                {
                    fail.Enabled = true;
                }
            }
        }

        private static void SetPos(int pos, TextBoxBase bas)
        {
            bas.SelectionStart = pos;
        }

        int lastPos = 0;


   
        private void button2_Click(object sender, EventArgs e)
        {
            FindSetup();
            if (SearchTextCode != null)
            {
                Match result = SearchTextCode.Match(SearchThis.Text);
                if (result.Success == false)
                {
                    MessageBox.Show(LocalizedStrings.GetString("FindDialog_NoResultsMessage", CultureInfo.CurrentUICulture) );
                }
                else
                {
                    lastPos = result.Index;
                    SetPos(result.Index, SearchThis);
                }

            }
        }

        private void FindDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogBox.DisplayMessageResource.MessageBox_ShowResource("HunterNotebook2.DialogBox.FindDialogHelp.txt", "Help", Encoding.UTF8);
        }
    }
}
