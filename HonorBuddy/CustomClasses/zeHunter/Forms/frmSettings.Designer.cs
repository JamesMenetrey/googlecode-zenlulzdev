namespace zeHunter.Forms
{
    partial class frmSettings
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
            this.pgHunter = new System.Windows.Forms.PropertyGrid();
            this.btnSaveAndExit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabConfigurations = new System.Windows.Forms.TabPage();
            this.tabUpdate = new System.Windows.Forms.TabPage();
            this.labUpdateCurrentVersion = new System.Windows.Forms.Label();
            this.labUpdateNewVersion = new System.Windows.Forms.Label();
            this.btnPatchnote = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.tabControl.SuspendLayout();
            this.tabConfigurations.SuspendLayout();
            this.tabUpdate.SuspendLayout();
            this.SuspendLayout();
            // 
            // pgHunter
            // 
            this.pgHunter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgHunter.Location = new System.Drawing.Point(0, 3);
            this.pgHunter.Name = "pgHunter";
            this.pgHunter.Size = new System.Drawing.Size(484, 356);
            this.pgHunter.TabIndex = 0;
            // 
            // btnSaveAndExit
            // 
            this.btnSaveAndExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAndExit.Location = new System.Drawing.Point(331, 406);
            this.btnSaveAndExit.Name = "btnSaveAndExit";
            this.btnSaveAndExit.Size = new System.Drawing.Size(95, 23);
            this.btnSaveAndExit.TabIndex = 1;
            this.btnSaveAndExit.Text = "Save and Close";
            this.btnSaveAndExit.UseVisualStyleBackColor = true;
            this.btnSaveAndExit.Click += new System.EventHandler(this.btnSaveAndExit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(432, 406);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(12, 411);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "Developped by ZenLulz - ";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabConfigurations);
            this.tabControl.Controls.Add(this.tabUpdate);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(495, 388);
            this.tabControl.TabIndex = 4;
            // 
            // tabConfigurations
            // 
            this.tabConfigurations.Controls.Add(this.pgHunter);
            this.tabConfigurations.Location = new System.Drawing.Point(4, 22);
            this.tabConfigurations.Name = "tabConfigurations";
            this.tabConfigurations.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfigurations.Size = new System.Drawing.Size(487, 362);
            this.tabConfigurations.TabIndex = 0;
            this.tabConfigurations.Text = "Configurations";
            this.tabConfigurations.UseVisualStyleBackColor = true;
            // 
            // tabUpdate
            // 
            this.tabUpdate.Controls.Add(this.labUpdateCurrentVersion);
            this.tabUpdate.Controls.Add(this.labUpdateNewVersion);
            this.tabUpdate.Controls.Add(this.btnPatchnote);
            this.tabUpdate.Controls.Add(this.btnUpdate);
            this.tabUpdate.Controls.Add(this.label6);
            this.tabUpdate.Controls.Add(this.label4);
            this.tabUpdate.Controls.Add(this.label3);
            this.tabUpdate.Controls.Add(this.label2);
            this.tabUpdate.Location = new System.Drawing.Point(4, 22);
            this.tabUpdate.Name = "tabUpdate";
            this.tabUpdate.Padding = new System.Windows.Forms.Padding(3);
            this.tabUpdate.Size = new System.Drawing.Size(487, 272);
            this.tabUpdate.TabIndex = 1;
            this.tabUpdate.Text = "Update";
            this.tabUpdate.UseVisualStyleBackColor = true;
            // 
            // labUpdateCurrentVersion
            // 
            this.labUpdateCurrentVersion.AutoSize = true;
            this.labUpdateCurrentVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labUpdateCurrentVersion.Location = new System.Drawing.Point(110, 65);
            this.labUpdateCurrentVersion.Name = "labUpdateCurrentVersion";
            this.labUpdateCurrentVersion.Size = new System.Drawing.Size(184, 17);
            this.labUpdateCurrentVersion.TabIndex = 8;
            this.labUpdateCurrentVersion.Text = "<labUpdateCurrentVersion>";
            // 
            // labUpdateNewVersion
            // 
            this.labUpdateNewVersion.AutoSize = true;
            this.labUpdateNewVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labUpdateNewVersion.Location = new System.Drawing.Point(125, 87);
            this.labUpdateNewVersion.Name = "labUpdateNewVersion";
            this.labUpdateNewVersion.Size = new System.Drawing.Size(164, 17);
            this.labUpdateNewVersion.TabIndex = 7;
            this.labUpdateNewVersion.Text = "<labUpdateNewVersion>";
            // 
            // btnPatchnote
            // 
            this.btnPatchnote.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPatchnote.Location = new System.Drawing.Point(249, 239);
            this.btnPatchnote.Name = "btnPatchnote";
            this.btnPatchnote.Size = new System.Drawing.Size(123, 27);
            this.btnPatchnote.TabIndex = 6;
            this.btnPatchnote.Text = "Patchnote";
            this.btnPatchnote.UseVisualStyleBackColor = true;
            this.btnPatchnote.Click += new System.EventHandler(this.btnPatchnote_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(120, 239);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(123, 27);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 183);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(406, 17);
            this.label6.TabIndex = 4;
            this.label6.Text = "Press on the following buttons to update or read the patchnote.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Version available :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Current version: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(293, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "A new version of ZeHunter is available.";
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.Location = new System.Drawing.Point(138, 411);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(53, 17);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Donate";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 437);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSaveAndExit);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(535, 385);
            this.Name = "frmSettings";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WINDOW_TITLE";
            this.tabControl.ResumeLayout(false);
            this.tabConfigurations.ResumeLayout(false);
            this.tabUpdate.ResumeLayout(false);
            this.tabUpdate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgHunter;
        private System.Windows.Forms.Button btnSaveAndExit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabConfigurations;
        private System.Windows.Forms.TabPage tabUpdate;
        private System.Windows.Forms.Button btnPatchnote;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labUpdateCurrentVersion;
        private System.Windows.Forms.Label labUpdateNewVersion;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}