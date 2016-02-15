namespace Me.HDSerialNumber.CS
{
    partial class FormMain
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.TextBoxPerf = new System.Windows.Forms.TextBox();
            this.ButtonDriveInfo = new System.Windows.Forms.Button();
            this.TextBoxDisque = new System.Windows.Forms.TextBox();
            this.ButtonWmi = new System.Windows.Forms.Button();
            this.comboBoxDriveLetter = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // TextBoxPerf
            // 
            this.TextBoxPerf.Location = new System.Drawing.Point(2, 118);
            this.TextBoxPerf.Name = "TextBoxPerf";
            this.TextBoxPerf.ReadOnly = true;
            this.TextBoxPerf.Size = new System.Drawing.Size(423, 20);
            this.TextBoxPerf.TabIndex = 9;
            // 
            // ButtonDriveInfo
            // 
            this.ButtonDriveInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ButtonDriveInfo.Location = new System.Drawing.Point(145, 3);
            this.ButtonDriveInfo.Name = "ButtonDriveInfo";
            this.ButtonDriveInfo.Size = new System.Drawing.Size(147, 23);
            this.ButtonDriveInfo.TabIndex = 8;
            this.ButtonDriveInfo.Text = "Win32API serial number";
            this.ButtonDriveInfo.UseVisualStyleBackColor = true;
            this.ButtonDriveInfo.Click += new System.EventHandler(this.ButtonDriveInfo_Click);
            // 
            // TextBoxDisque
            // 
            this.TextBoxDisque.Location = new System.Drawing.Point(2, 32);
            this.TextBoxDisque.Multiline = true;
            this.TextBoxDisque.Name = "TextBoxDisque";
            this.TextBoxDisque.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxDisque.Size = new System.Drawing.Size(423, 80);
            this.TextBoxDisque.TabIndex = 7;
            // 
            // ButtonWmi
            // 
            this.ButtonWmi.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ButtonWmi.Location = new System.Drawing.Point(2, 3);
            this.ButtonWmi.Name = "ButtonWmi";
            this.ButtonWmi.Size = new System.Drawing.Size(137, 23);
            this.ButtonWmi.TabIndex = 6;
            this.ButtonWmi.Text = "WMI serial number";
            this.ButtonWmi.UseVisualStyleBackColor = true;
            this.ButtonWmi.Click += new System.EventHandler(this.ButtonWmi_Click);
            // 
            // comboBoxDriveLetter
            // 
            this.comboBoxDriveLetter.FormattingEnabled = true;
            this.comboBoxDriveLetter.Location = new System.Drawing.Point(298, 5);
            this.comboBoxDriveLetter.Name = "comboBoxDriveLetter";
            this.comboBoxDriveLetter.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDriveLetter.TabIndex = 10;
            this.comboBoxDriveLetter.SelectedIndexChanged += new System.EventHandler(this.comboBoxDriveLetter_SelectedIndexChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 145);
            this.Controls.Add(this.comboBoxDriveLetter);
            this.Controls.Add(this.TextBoxPerf);
            this.Controls.Add(this.ButtonDriveInfo);
            this.Controls.Add(this.TextBoxDisque);
            this.Controls.Add(this.ButtonWmi);
            this.Name = "FormMain";
            this.Text = "HDSerialNumber";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.TextBox TextBoxPerf;
        internal System.Windows.Forms.Button ButtonDriveInfo;
        internal System.Windows.Forms.TextBox TextBoxDisque;
        internal System.Windows.Forms.Button ButtonWmi;
        private System.Windows.Forms.ComboBox comboBoxDriveLetter;
    }
}

