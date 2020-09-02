namespace LanguageAudioTool
{
    partial class ConfigureInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureInput));
            this.btnNext = new System.Windows.Forms.Button();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.btnAddFile = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDragDrop = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(194, 451);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(112, 42);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.button1_Click);
            // 
            // lstFiles
            // 
            this.lstFiles.AllowDrop = true;
            this.lstFiles.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.HorizontalScrollbar = true;
            this.lstFiles.ItemHeight = 16;
            this.lstFiles.Location = new System.Drawing.Point(6, 18);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstFiles.Size = new System.Drawing.Size(465, 244);
            this.lstFiles.TabIndex = 1;
            this.lstFiles.SelectedIndexChanged += new System.EventHandler(this.lstFiles_SelectedIndexChanged);
            this.lstFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1_DragDrop);
            this.lstFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
            this.lstFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
            // 
            // btnAddFile
            // 
            this.btnAddFile.Location = new System.Drawing.Point(82, 300);
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(126, 39);
            this.btnAddFile.TabIndex = 2;
            this.btnAddFile.Text = "Add file...";
            this.btnAddFile.UseVisualStyleBackColor = true;
            this.btnAddFile.Click += new System.EventHandler(this.btnAddFile_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(268, 300);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(126, 39);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "Remove selected";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDragDrop);
            this.groupBox1.Controls.Add(this.lstFiles);
            this.groupBox1.Controls.Add(this.btnRemove);
            this.groupBox1.Controls.Add(this.btnAddFile);
            this.groupBox1.Location = new System.Drawing.Point(12, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 354);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "List of files";
            // 
            // lblDragDrop
            // 
            this.lblDragDrop.AllowDrop = true;
            this.lblDragDrop.AutoSize = true;
            this.lblDragDrop.BackColor = System.Drawing.SystemColors.Window;
            this.lblDragDrop.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDragDrop.Location = new System.Drawing.Point(94, 137);
            this.lblDragDrop.Name = "lblDragDrop";
            this.lblDragDrop.Size = new System.Drawing.Size(289, 19);
            this.lblDragDrop.TabIndex = 4;
            this.lblDragDrop.Text = "< Drag and Drop files here >";
            this.lblDragDrop.DragDrop += new System.Windows.Forms.DragEventHandler(this.label1_DragDrop);
            this.lblDragDrop.DragEnter += new System.Windows.Forms.DragEventHandler(this.label1_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(145, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 24);
            this.label1.TabIndex = 5;
            this.label1.Text = "Step 1/4 : Input file(s)";
            // 
            // ConfigureInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 511);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnNext);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConfigureInput";
            this.Text = "Language Audio Tool v0.1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Launcher_FormClosing);
            this.Load += new System.EventHandler(this.Launcher_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Button btnAddFile;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblDragDrop;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}

