namespace LanguageAudioTool
{
    partial class ConfigureOutput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureOutput));
            this.label1 = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDestFolder = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radPerSection = new System.Windows.Forms.RadioButton();
            this.radPerInput = new System.Windows.Forms.RadioButton();
            this.lblWarn1 = new System.Windows.Forms.Label();
            this.lblWarn2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(122, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(257, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Step 4/4 : Where to output";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(290, 451);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(112, 42);
            this.btnNext.TabIndex = 7;
            this.btnNext.Text = "Do it!";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(98, 451);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(112, 42);
            this.btnPrev.TabIndex = 10;
            this.btnPrev.Text = "< Prev";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "Destination folder:";
            // 
            // txtDestFolder
            // 
            this.txtDestFolder.Location = new System.Drawing.Point(137, 102);
            this.txtDestFolder.Name = "txtDestFolder";
            this.txtDestFolder.Size = new System.Drawing.Size(235, 21);
            this.txtDestFolder.TabIndex = 12;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(378, 100);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 21);
            this.btnBrowse.TabIndex = 13;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radPerSection);
            this.groupBox1.Controls.Add(this.radPerInput);
            this.groupBox1.Location = new System.Drawing.Point(14, 152);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(439, 63);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Destination file";
            // 
            // radPerSection
            // 
            this.radPerSection.AutoSize = true;
            this.radPerSection.Location = new System.Drawing.Point(6, 39);
            this.radPerSection.Name = "radPerSection";
            this.radPerSection.Size = new System.Drawing.Size(221, 16);
            this.radPerSection.TabIndex = 1;
            this.radPerSection.TabStop = true;
            this.radPerSection.Text = "One output file per audio section";
            this.radPerSection.UseVisualStyleBackColor = true;
            this.radPerSection.CheckedChanged += new System.EventHandler(this.radPerSection_CheckedChanged);
            // 
            // radPerInput
            // 
            this.radPerInput.AutoSize = true;
            this.radPerInput.Location = new System.Drawing.Point(6, 18);
            this.radPerInput.Name = "radPerInput";
            this.radPerInput.Size = new System.Drawing.Size(203, 16);
            this.radPerInput.TabIndex = 0;
            this.radPerInput.TabStop = true;
            this.radPerInput.Text = "One output file per input file";
            this.radPerInput.UseVisualStyleBackColor = true;
            this.radPerInput.CheckedChanged += new System.EventHandler(this.radPerInput_CheckedChanged);
            // 
            // lblWarn1
            // 
            this.lblWarn1.AutoSize = true;
            this.lblWarn1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWarn1.ForeColor = System.Drawing.Color.Red;
            this.lblWarn1.Location = new System.Drawing.Point(83, 239);
            this.lblWarn1.Name = "lblWarn1";
            this.lblWarn1.Size = new System.Drawing.Size(335, 12);
            this.lblWarn1.TabIndex = 15;
            this.lblWarn1.Text = "This could create many files in the destination folder!";
            // 
            // lblWarn2
            // 
            this.lblWarn2.AutoSize = true;
            this.lblWarn2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWarn2.ForeColor = System.Drawing.Color.Red;
            this.lblWarn2.Location = new System.Drawing.Point(113, 250);
            this.lblWarn2.Name = "lblWarn2";
            this.lblWarn2.Size = new System.Drawing.Size(275, 12);
            this.lblWarn2.TabIndex = 16;
            this.lblWarn2.Text = "They will be named e.g. InputFilename_001.mp3";
            // 
            // ConfigureOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 511);
            this.Controls.Add(this.lblWarn2);
            this.Controls.Add(this.lblWarn1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtDestFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConfigureOutput";
            this.Text = "Language Audio Tool v0.1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDestFolder;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radPerSection;
        private System.Windows.Forms.RadioButton radPerInput;
        private System.Windows.Forms.Label lblWarn1;
        private System.Windows.Forms.Label lblWarn2;
    }
}