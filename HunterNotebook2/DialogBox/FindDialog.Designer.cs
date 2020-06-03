namespace HunterNotebook2.DialogBox
{
    partial class FindDialog
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
            this.components = new System.ComponentModel.Container();
            this.TextBoxSearchString = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CheckBoxEnableRegularExpressions = new System.Windows.Forms.CheckBox();
            this.CheckBoxEnableWildcards = new System.Windows.Forms.CheckBox();
            this.CheckBoxEnableCaseSensitive = new System.Windows.Forms.CheckBox();
            this.ButtonFindNext = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.CheckBoxLoopToStart = new System.Windows.Forms.CheckBox();
            this.ButtonFindLast = new System.Windows.Forms.Button();
            this.ButtonFindFirst = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TextBoxSearchString
            // 
            this.TextBoxSearchString.Location = new System.Drawing.Point(40, 30);
            this.TextBoxSearchString.Name = "TextBoxSearchString";
            this.TextBoxSearchString.Size = new System.Drawing.Size(449, 26);
            this.TextBoxSearchString.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.CheckBoxEnableRegularExpressions);
            this.groupBox1.Controls.Add(this.CheckBoxEnableWildcards);
            this.groupBox1.Controls.Add(this.CheckBoxEnableCaseSensitive);
            this.groupBox1.Location = new System.Drawing.Point(40, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(392, 168);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Control";
            // 
            // CheckBoxEnableRegularExpressions
            // 
            this.CheckBoxEnableRegularExpressions.AutoSize = true;
            this.CheckBoxEnableRegularExpressions.Location = new System.Drawing.Point(18, 86);
            this.CheckBoxEnableRegularExpressions.Name = "CheckBoxEnableRegularExpressions";
            this.CheckBoxEnableRegularExpressions.Size = new System.Drawing.Size(214, 24);
            this.CheckBoxEnableRegularExpressions.TabIndex = 2;
            this.CheckBoxEnableRegularExpressions.Text = "Allow Regular Expression";
            this.CheckBoxEnableRegularExpressions.UseVisualStyleBackColor = true;
            // 
            // CheckBoxEnableWildcards
            // 
            this.CheckBoxEnableWildcards.AutoSize = true;
            this.CheckBoxEnableWildcards.Location = new System.Drawing.Point(18, 56);
            this.CheckBoxEnableWildcards.Name = "CheckBoxEnableWildcards";
            this.CheckBoxEnableWildcards.Size = new System.Drawing.Size(137, 24);
            this.CheckBoxEnableWildcards.TabIndex = 2;
            this.CheckBoxEnableWildcards.Text = "Use Wildcards";
            this.CheckBoxEnableWildcards.UseVisualStyleBackColor = true;
            // 
            // CheckBoxEnableCaseSensitive
            // 
            this.CheckBoxEnableCaseSensitive.AutoSize = true;
            this.CheckBoxEnableCaseSensitive.Location = new System.Drawing.Point(18, 26);
            this.CheckBoxEnableCaseSensitive.Name = "CheckBoxEnableCaseSensitive";
            this.CheckBoxEnableCaseSensitive.Size = new System.Drawing.Size(130, 24);
            this.CheckBoxEnableCaseSensitive.TabIndex = 0;
            this.CheckBoxEnableCaseSensitive.Text = "Case Matters";
            this.CheckBoxEnableCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // ButtonFindNext
            // 
            this.ButtonFindNext.Location = new System.Drawing.Point(635, 30);
            this.ButtonFindNext.Name = "ButtonFindNext";
            this.ButtonFindNext.Size = new System.Drawing.Size(93, 43);
            this.ButtonFindNext.TabIndex = 3;
            this.ButtonFindNext.Text = "Find Next";
            this.ButtonFindNext.UseVisualStyleBackColor = true;
            this.ButtonFindNext.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox5);
            this.groupBox2.Controls.Add(this.CheckBoxLoopToStart);
            this.groupBox2.Location = new System.Drawing.Point(40, 247);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(373, 107);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Other";
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(18, 56);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(113, 24);
            this.checkBox5.TabIndex = 5;
            this.checkBox5.Text = "checkBox5";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // CheckBoxLoopToStart
            // 
            this.CheckBoxLoopToStart.AutoSize = true;
            this.CheckBoxLoopToStart.Location = new System.Drawing.Point(18, 26);
            this.CheckBoxLoopToStart.Name = "CheckBoxLoopToStart";
            this.CheckBoxLoopToStart.Size = new System.Drawing.Size(247, 24);
            this.CheckBoxLoopToStart.TabIndex = 0;
            this.CheckBoxLoopToStart.Text = "Start at Top Upon End of Text";
            this.CheckBoxLoopToStart.UseVisualStyleBackColor = true;
            // 
            // ButtonFindLast
            // 
            this.ButtonFindLast.Location = new System.Drawing.Point(635, 80);
            this.ButtonFindLast.Name = "ButtonFindLast";
            this.ButtonFindLast.Size = new System.Drawing.Size(93, 43);
            this.ButtonFindLast.TabIndex = 5;
            this.ButtonFindLast.Text = "Find Last";
            this.ButtonFindLast.UseVisualStyleBackColor = true;
            // 
            // ButtonFindFirst
            // 
            this.ButtonFindFirst.Location = new System.Drawing.Point(635, 129);
            this.ButtonFindFirst.Name = "ButtonFindFirst";
            this.ButtonFindFirst.Size = new System.Drawing.Size(93, 39);
            this.ButtonFindFirst.TabIndex = 6;
            this.ButtonFindFirst.Text = "Find First";
            this.ButtonFindFirst.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(18, 116);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(245, 28);
            this.comboBox1.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(269, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(37, 28);
            this.button1.TabIndex = 8;
            this.button1.Text = ". . .";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // FindDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 395);
            this.Controls.Add(this.ButtonFindFirst);
            this.Controls.Add(this.ButtonFindLast);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ButtonFindNext);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.TextBoxSearchString);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindDialog";
            this.ShowIcon = false;
            this.Text = "Find Dialog";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FindDialog_HelpButtonClicked);
            this.Load += new System.EventHandler(this.FindDialog_Load);
            this.Shown += new System.EventHandler(this.FindDialog_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxSearchString;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox CheckBoxEnableRegularExpressions;
        private System.Windows.Forms.CheckBox CheckBoxEnableWildcards;
        private System.Windows.Forms.CheckBox CheckBoxEnableCaseSensitive;
        private System.Windows.Forms.Button ButtonFindNext;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox CheckBoxLoopToStart;
        private System.Windows.Forms.Button ButtonFindLast;
        private System.Windows.Forms.Button ButtonFindFirst;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button1;
    }
}