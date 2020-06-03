using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

namespace HunterNotebook2.DialogBox
{
    public partial class ChooseFontForm : Form
    {
        public ChooseFontForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// container for dialog specific strings
        /// </summary>
        ResourceManager LocalizeStrings;
        /// <summary>
        /// container for the generial strings shared
        /// </summary>
        ResourceManager GenericStrings;


        public Font Result { get; set; } = null;
        public float ResultSize { get; set; } = 12;

        public bool AffectAllText = true;
        private void ChooseFontForm_Load(object sender, EventArgs e)
        {
            LocalizeStrings = new ResourceManager(typeof(StringResources.Localized_ChooseFontDialogStrings));
            GenericStrings = new ResourceManager(typeof(StringResources.Localized_GenericDialog));
            int TargetIndex = 0;
           
            foreach (FontFamily Familiar in FontFamily.Families)
            {
                ComboBoxEasyChooseFont.Items.Add(Familiar.Name);
                if (Familiar.Name.Contains("Times New Roman"))
                {
                    TargetIndex = ComboBoxEasyChooseFont.Items.Count;
                }
            }

            Text = LocalizeStrings.GetString("ChooseFont_Titlebar", CultureInfo.CurrentUICulture);
            RichTextBox_ExampleText.Text = LocalizeStrings.GetString("ChooseFont_ExampleString", CultureInfo.CurrentCulture);
            RadioButtonChangeSelectedTextFont.Text = LocalizeStrings.GetString("ChooseFont_TextAction_SelectedText", CultureInfo.CurrentUICulture);
            RadioButtonChangeAllTextFont.Text = LocalizeStrings.GetString("ChooseFont_TextAction_AllText", CultureInfo.CurrentUICulture);
            GroupboxEffectText.Text = LocalizeStrings.GetString("ChooseFont_HowToHandleText", CultureInfo.CurrentUICulture);
            ComboBoxEasyChooseFont.SelectedIndex = TargetIndex;
            RadioButtonChangeAllTextFont.Checked = true;
        }

        private void ComboBoxEasyChooseFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeText(ComboBoxEasyChooseFont.SelectedItem.ToString());

        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            Result = new Font(ComboBoxEasyChooseFont.SelectedItem.ToString(), ResultSize);
            AffectAllText = RadioButtonChangeAllTextFont.Checked;
    
            DialogResult = DialogResult.OK;

        }

        private void ChangeText(string Target)
        {
            try
            {
                using (var FontThing = new FontFamily(Target))
                {
                    if (RadioButtonChangeAllTextFont.Checked == true)
                    {
                        RichTextBox_ExampleText.SelectAll();
                    }
                    RichTextBox_ExampleText.SelectionFont = new Font(FontThing, 12);

                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show(LocalizeStrings.GetString("ChooseFont_FontPreviewFailure", CultureInfo.CurrentUICulture),
                                GenericStrings.GetString("General_ErrorTitle", CultureInfo.CurrentUICulture),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ButtonShowFontCommonDialog_Click(object sender, EventArgs e)
        {
            using (var Dlg = new FontDialog())
            {
                Dlg.ShowColor = false;
                Dlg.ShowEffects =false;
                Dlg.AllowScriptChange = false;
                if (Dlg.ShowDialog() == DialogResult.OK) 
                {
                    ComboBoxEasyChooseFont.SelectedIndex = ComboBoxEasyChooseFont.Items.IndexOf(Dlg.Font.Name);
                    Result = new Font(Dlg.Font, Dlg.Font.Style);
                    ResultSize = Dlg.Font.SizeInPoints;
                }
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            ResultSize = 0;
            Result = null;
            DialogResult = DialogResult.Cancel;
            AffectAllText = false;
            Close();
        }
    }
}
