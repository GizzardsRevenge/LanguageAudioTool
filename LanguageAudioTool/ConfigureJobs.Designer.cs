namespace LanguageAudioTool
{
    partial class ConfigureJobs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureJobs));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numBeep = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.btnBeep = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numSilenceSeconds = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numChunkSpeed = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.lstActions = new System.Windows.Forms.ListBox();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBeep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSilenceSeconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChunkSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.numBeep);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.btnBeep);
            this.groupBox1.Controls.Add(this.btnDown);
            this.groupBox1.Controls.Add(this.btnUp);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numSilenceSeconds);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numChunkSpeed);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.btnClearAll);
            this.groupBox1.Controls.Add(this.btnDeleteSelected);
            this.groupBox1.Controls.Add(this.lstActions);
            this.groupBox1.Location = new System.Drawing.Point(32, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 307);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "For each section of audio...";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(123, 270);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 12);
            this.label8.TabIndex = 25;
            this.label8.Text = "second beep";
            // 
            // numBeep
            // 
            this.numBeep.DecimalPlaces = 1;
            this.numBeep.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numBeep.Location = new System.Drawing.Point(65, 268);
            this.numBeep.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numBeep.Name = "numBeep";
            this.numBeep.Size = new System.Drawing.Size(52, 21);
            this.numBeep.TabIndex = 24;
            this.numBeep.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 270);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 23;
            this.label9.Text = "Play a";
            // 
            // btnBeep
            // 
            this.btnBeep.Location = new System.Drawing.Point(242, 265);
            this.btnBeep.Name = "btnBeep";
            this.btnBeep.Size = new System.Drawing.Size(75, 23);
            this.btnBeep.TabIndex = 22;
            this.btnBeep.Text = "Add";
            this.btnBeep.UseVisualStyleBackColor = true;
            this.btnBeep.Click += new System.EventHandler(this.btnBeep_Click);
            // 
            // btnDown
            // 
            this.btnDown.Enabled = false;
            this.btnDown.Location = new System.Drawing.Point(101, 162);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 21;
            this.btnDown.Text = "↓ Move Dn";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Enabled = false;
            this.btnUp.Location = new System.Drawing.Point(20, 162);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.TabIndex = 20;
            this.btnUp.Text = "↑ Move Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(242, 236);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(118, 241);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "seconds of silence";
            // 
            // numSilenceSeconds
            // 
            this.numSilenceSeconds.Location = new System.Drawing.Point(60, 238);
            this.numSilenceSeconds.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numSilenceSeconds.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSilenceSeconds.Name = "numSilenceSeconds";
            this.numSilenceSeconds.Size = new System.Drawing.Size(52, 21);
            this.numSilenceSeconds.TabIndex = 17;
            this.numSilenceSeconds.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 240);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "Play";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(242, 207);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(182, 212);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "% speed";
            // 
            // numChunkSpeed
            // 
            this.numChunkSpeed.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numChunkSpeed.Location = new System.Drawing.Point(123, 210);
            this.numChunkSpeed.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numChunkSpeed.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numChunkSpeed.Name = "numChunkSpeed";
            this.numChunkSpeed.Size = new System.Drawing.Size(53, 21);
            this.numChunkSpeed.TabIndex = 13;
            this.numChunkSpeed.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 211);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "Play the section at ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "Do these actions:";
            // 
            // btnClearAll
            // 
            this.btnClearAll.Location = new System.Drawing.Point(323, 162);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(123, 23);
            this.btnClearAll.TabIndex = 10;
            this.btnClearAll.Text = "Clear all";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnDeleteSelected
            // 
            this.btnDeleteSelected.Enabled = false;
            this.btnDeleteSelected.Location = new System.Drawing.Point(194, 162);
            this.btnDeleteSelected.Name = "btnDeleteSelected";
            this.btnDeleteSelected.Size = new System.Drawing.Size(123, 23);
            this.btnDeleteSelected.TabIndex = 9;
            this.btnDeleteSelected.Text = "Delete selected";
            this.btnDeleteSelected.UseVisualStyleBackColor = true;
            this.btnDeleteSelected.Click += new System.EventHandler(this.btnDeleteSelected_Click);
            // 
            // lstActions
            // 
            this.lstActions.FormattingEnabled = true;
            this.lstActions.ItemHeight = 12;
            this.lstActions.Location = new System.Drawing.Point(20, 32);
            this.lstActions.Name = "lstActions";
            this.lstActions.Size = new System.Drawing.Size(426, 124);
            this.lstActions.TabIndex = 0;
            this.lstActions.SelectedIndexChanged += new System.EventHandler(this.lstActions_SelectedIndexChanged);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(108, 499);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(122, 43);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "< Prev";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(270, 499);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(122, 43);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(129, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(242, 24);
            this.label10.TabIndex = 7;
            this.label10.Text = "Step 3/4 : Define actions";
            // 
            // ConfigureJobs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 575);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "ConfigureJobs";
            this.ShowIcon = false;
            this.Text = "Language Audio Tool v0.1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBeep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSilenceSeconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChunkSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstActions;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnDeleteSelected;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numSilenceSeconds;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numChunkSpeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numBeep;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnBeep;
        private System.Windows.Forms.Label label10;
    }
}