using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HunterNotebook2.DialogBox
{
    public partial class ChooseZoom : Form
    {
        public ChooseZoom()
        {
            InitializeComponent();
        }

        public float ZoomFactor { get; set; } = 1.0f;


        private void ButtonOk_Click(object sender, EventArgs e)
        {
            float val;

            {
                try
                {
                    val = float.Parse(ComboBoxChooseZoom.Text, 
                                      NumberStyles.AllowDecimalPoint,
                                      CultureInfo.InvariantCulture) ;
                } 
                catch (FormatException)
                {
                    val = 0;
                    MessageBox.Show(ZoomDialogResourceStrings.GetString("ZoomDialog_EmptyZoomValueMessage", CultureInfo.CurrentUICulture),
                                    GenericDialogStrings.GetString("General_ErrorTitle", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (val != 0)
                {
                    //                    Target.ZoomFactor = val;
                    ZoomFactor = val;
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }



        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ChooseZoom_Shown(object sender, EventArgs e)
        {
  
                ButtonOk.Enabled =
                ComboBoxChooseZoom.Enabled =
                VScrollBarTick.Enabled =
                CheckBoxRememberZoom.Enabled = true;

        }

        private void VScrollBarTick_Scroll(object sender, ScrollEventArgs e)
        {
            ComboBoxChooseZoom.Text = (((float)e.NewValue) / 10) .ToString(CultureInfo.InvariantCulture);
        }

        private void ComboBoxChooseZoom_Validating(object sender, CancelEventArgs e)
        {
            float text = 0;
            try 
            {
                text = float.Parse(ComboBoxChooseZoom.Text, NumberStyles.Float, CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
                string message = ZoomDialogResourceStrings.GetString("ZoomDialog_BadZoomStringFormatMessage", CultureInfo.CurrentUICulture);

                e.Cancel = true;
                MessageBox.Show(string.Format(CultureInfo.CurrentCulture, message, ComboBoxChooseZoom.Text));
            }
            if ((text <= 0.0125) || (text >= 64))
            {
                e.Cancel = true;
                MessageBox.Show(ZoomDialogResourceStrings.GetString("ZoomDialog_OutOfBoundsZoomValueMessage", CultureInfo.CurrentUICulture));
            }
        }

        private void ChooseZoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (ModifierKeys.HasFlag(Keys.Control))
            if (true)
            {
                if (((int)(Keys.Escape) == e.KeyChar))
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            }
        }

        ResourceManager GenericDialogStrings;
        ResourceManager ZoomDialogResourceStrings;

        private void ChooseZoom_Load(object sender, EventArgs e)
        {
            ZoomDialogResourceStrings = new ResourceManager(typeof(StringResources.Localized_ZoomDialogStrings));
            GenericDialogStrings = new ResourceManager(typeof(StringResources.Localized_GenericDialog));
            Tag = ZoomDialogResourceStrings.GetString("ZoomDialog_TitleText", CultureInfo.CurrentUICulture);
            CheckBoxRememberZoom.Text = ZoomDialogResourceStrings.GetString("ZoomDialog_RememberZoomCheckbox", CultureInfo.CurrentUICulture);
            ButtonOk.Text = GenericDialogStrings.GetString("General_OkButtonText", CultureInfo.CurrentUICulture);
            ButtonCancel.Text = GenericDialogStrings.GetString("General_CancelButtonText", CultureInfo.CurrentUICulture);
        }
    }
}
