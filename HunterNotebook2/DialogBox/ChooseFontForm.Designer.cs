namespace HunterNotebook2.DialogBox
{
    partial class ChooseFontForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseFontForm));
            this.ComboBoxEasyChooseFont = new System.Windows.Forms.ComboBox();
            this.RichTextBox_ExampleText = new System.Windows.Forms.RichTextBox();
            this.ButtonOk = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.GroupboxEffectText = new System.Windows.Forms.GroupBox();
            this.RadioButtonChangeAllTextFont = new System.Windows.Forms.RadioButton();
            this.RadioButtonChangeSelectedTextFont = new System.Windows.Forms.RadioButton();
            this.ButtonShowFontCommonDialog = new System.Windows.Forms.Button();
            this.GroupboxEffectText.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComboBoxEasyChooseFont
            // 
            this.ComboBoxEasyChooseFont.FormattingEnabled = true;
            resources.ApplyResources(this.ComboBoxEasyChooseFont, "ComboBoxEasyChooseFont");
            this.ComboBoxEasyChooseFont.Name = "ComboBoxEasyChooseFont";
            this.ComboBoxEasyChooseFont.SelectedIndexChanged += new System.EventHandler(this.ComboBoxEasyChooseFont_SelectedIndexChanged);
            // 
            // RichTextBox_ExampleText
            // 
            resources.ApplyResources(this.RichTextBox_ExampleText, "RichTextBox_ExampleText");
            this.RichTextBox_ExampleText.Name = "RichTextBox_ExampleText";
            // 
            // ButtonOk
            // 
            resources.ApplyResources(this.ButtonOk, "ButtonOk");
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.UseVisualStyleBackColor = true;
            this.ButtonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // ButtonCancel
            // 
            resources.ApplyResources(this.ButtonCancel, "ButtonCancel");
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // GroupboxEffectText
            // 
            this.GroupboxEffectText.Controls.Add(this.RadioButtonChangeAllTextFont);
            this.GroupboxEffectText.Controls.Add(this.RadioButtonChangeSelectedTextFont);
            resources.ApplyResources(this.GroupboxEffectText, "GroupboxEffectText");
            this.GroupboxEffectText.Name = "GroupboxEffectText";
            this.GroupboxEffectText.TabStop = false;
            // 
            // RadioButtonChangeAllTextFont
            // 
            resources.ApplyResources(this.RadioButtonChangeAllTextFont, "RadioButtonChangeAllTextFont");
            this.RadioButtonChangeAllTextFont.Name = "RadioButtonChangeAllTextFont";
            this.RadioButtonChangeAllTextFont.TabStop = true;
            this.RadioButtonChangeAllTextFont.UseVisualStyleBackColor = true;
            // 
            // RadioButtonChangeSelectedTextFont
            // 
            resources.ApplyResources(this.RadioButtonChangeSelectedTextFont, "RadioButtonChangeSelectedTextFont");
            this.RadioButtonChangeSelectedTextFont.Name = "RadioButtonChangeSelectedTextFont";
            this.RadioButtonChangeSelectedTextFont.TabStop = true;
            this.RadioButtonChangeSelectedTextFont.UseVisualStyleBackColor = true;
            // 
            // ButtonShowFontCommonDialog
            // 
            resources.ApplyResources(this.ButtonShowFontCommonDialog, "ButtonShowFontCommonDialog");
            this.ButtonShowFontCommonDialog.Name = "ButtonShowFontCommonDialog";
            this.ButtonShowFontCommonDialog.UseVisualStyleBackColor = true;
            this.ButtonShowFontCommonDialog.Click += new System.EventHandler(this.ButtonShowFontCommonDialog_Click);
            // 
            // ChooseFontForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ButtonShowFontCommonDialog);
            this.Controls.Add(this.GroupboxEffectText);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOk);
            this.Controls.Add(this.RichTextBox_ExampleText);
            this.Controls.Add(this.ComboBoxEasyChooseFont);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ChooseFontForm";
            this.Load += new System.EventHandler(this.ChooseFontForm_Load);
            this.GroupboxEffectText.ResumeLayout(false);
            this.GroupboxEffectText.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox ComboBoxEasyChooseFont;
        private System.Windows.Forms.RichTextBox RichTextBox_ExampleText;
        private System.Windows.Forms.Button ButtonOk;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.GroupBox GroupboxEffectText;
        private System.Windows.Forms.RadioButton RadioButtonChangeAllTextFont;
        private System.Windows.Forms.RadioButton RadioButtonChangeSelectedTextFont;
        private System.Windows.Forms.Button ButtonShowFontCommonDialog;
    }
}