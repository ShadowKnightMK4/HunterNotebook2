namespace HunterNotebook2.DialogBox
{
    partial class ChooseZoom
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
            this.ButtonOk = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ComboBoxChooseZoom = new System.Windows.Forms.ComboBox();
            this.VScrollBarTick = new System.Windows.Forms.VScrollBar();
            this.CheckBoxRememberZoom = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ButtonOk
            // 
            this.ButtonOk.Location = new System.Drawing.Point(663, 13);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.Size = new System.Drawing.Size(114, 31);
            this.ButtonOk.TabIndex = 0;
            this.ButtonOk.Text = "Ok";
            this.ButtonOk.UseVisualStyleBackColor = true;
            this.ButtonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Location = new System.Drawing.Point(663, 50);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(114, 31);
            this.ButtonCancel.TabIndex = 1;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ComboBoxChooseZoom
            // 
            this.ComboBoxChooseZoom.FormattingEnabled = true;
            this.ComboBoxChooseZoom.Items.AddRange(new object[] {
            "0.001",
            "0.2",
            "0.4",
            "0.6",
            "0.8",
            "1.0",
            "2.0",
            "3.0",
            "4.0",
            "6.0",
            "8.0",
            "100.0"});
            this.ComboBoxChooseZoom.Location = new System.Drawing.Point(12, 16);
            this.ComboBoxChooseZoom.Name = "ComboBoxChooseZoom";
            this.ComboBoxChooseZoom.Size = new System.Drawing.Size(216, 28);
            this.ComboBoxChooseZoom.TabIndex = 2;
            this.ComboBoxChooseZoom.Validating += new System.ComponentModel.CancelEventHandler(this.ComboBoxChooseZoom_Validating);
            // 
            // VScrollBarTick
            // 
            this.VScrollBarTick.Location = new System.Drawing.Point(240, 16);
            this.VScrollBarTick.Name = "VScrollBarTick";
            this.VScrollBarTick.Size = new System.Drawing.Size(26, 28);
            this.VScrollBarTick.TabIndex = 3;
            this.VScrollBarTick.Scroll += new System.Windows.Forms.ScrollEventHandler(this.VScrollBarTick_Scroll);
            // 
            // CheckBoxRememberZoom
            // 
            this.CheckBoxRememberZoom.AutoSize = true;
            this.CheckBoxRememberZoom.Location = new System.Drawing.Point(560, 103);
            this.CheckBoxRememberZoom.Name = "CheckBoxRememberZoom";
            this.CheckBoxRememberZoom.Size = new System.Drawing.Size(188, 24);
            this.CheckBoxRememberZoom.TabIndex = 4;
            this.CheckBoxRememberZoom.Text = "Remember this Zoom";
            this.CheckBoxRememberZoom.UseVisualStyleBackColor = true;
            // 
            // ChooseZoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 207);
            this.Controls.Add(this.CheckBoxRememberZoom);
            this.Controls.Add(this.VScrollBarTick);
            this.Controls.Add(this.ComboBoxChooseZoom);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChooseZoom";
            this.Text = "Choose Zoom";
            this.Load += new System.EventHandler(this.ChooseZoom_Load);
            this.Shown += new System.EventHandler(this.ChooseZoom_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChooseZoom_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonOk;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.VScrollBar VScrollBarTick;
        public System.Windows.Forms.CheckBox CheckBoxRememberZoom;
        public System.Windows.Forms.ComboBox ComboBoxChooseZoom;
    }
}