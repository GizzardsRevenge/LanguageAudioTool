namespace LanguageAudioTool
{
    partial class ConfigureSections
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureSections));
            this.label1 = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radRemove = new System.Windows.Forms.RadioButton();
            this.radPlayOnceFullSpeed = new System.Windows.Forms.RadioButton();
            this.radHandleAsNormal = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numAudioSectionLength = new System.Windows.Forms.NumericUpDown();
            this.radAudioDef3 = new System.Windows.Forms.RadioButton();
            this.radAudioDef2 = new System.Windows.Forms.RadioButton();
            this.radAudioDef1 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAudioSectionLength)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(117, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(266, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Step 2/4 : How to subdivide";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(290, 451);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(112, 42);
            this.btnNext.TabIndex = 7;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radRemove);
            this.groupBox2.Controls.Add(this.radPlayOnceFullSpeed);
            this.groupBox2.Controls.Add(this.radHandleAsNormal);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 222);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(477, 143);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "First section of audio";
            // 
            // radRemove
            // 
            this.radRemove.AutoSize = true;
            this.radRemove.Location = new System.Drawing.Point(20, 112);
            this.radRemove.Name = "radRemove";
            this.radRemove.Size = new System.Drawing.Size(77, 16);
            this.radRemove.TabIndex = 4;
            this.radRemove.Text = "Remove it";
            this.radRemove.UseVisualStyleBackColor = true;
            this.radRemove.CheckedChanged += new System.EventHandler(this.radRemove_CheckedChanged);
            // 
            // radPlayOnceFullSpeed
            // 
            this.radPlayOnceFullSpeed.AutoSize = true;
            this.radPlayOnceFullSpeed.Location = new System.Drawing.Point(20, 90);
            this.radPlayOnceFullSpeed.Name = "radPlayOnceFullSpeed";
            this.radPlayOnceFullSpeed.Size = new System.Drawing.Size(167, 16);
            this.radPlayOnceFullSpeed.TabIndex = 3;
            this.radPlayOnceFullSpeed.Text = "Play once, at full speed";
            this.radPlayOnceFullSpeed.UseVisualStyleBackColor = true;
            this.radPlayOnceFullSpeed.CheckedChanged += new System.EventHandler(this.radPlayOnceFullSpeed_CheckedChanged);
            // 
            // radHandleAsNormal
            // 
            this.radHandleAsNormal.AutoSize = true;
            this.radHandleAsNormal.Checked = true;
            this.radHandleAsNormal.Location = new System.Drawing.Point(20, 68);
            this.radHandleAsNormal.Name = "radHandleAsNormal";
            this.radHandleAsNormal.Size = new System.Drawing.Size(287, 16);
            this.radHandleAsNormal.TabIndex = 2;
            this.radHandleAsNormal.TabStop = true;
            this.radHandleAsNormal.Text = "Perform the same actions as a normal section";
            this.radHandleAsNormal.UseVisualStyleBackColor = true;
            this.radHandleAsNormal.CheckedChanged += new System.EventHandler(this.radHandleAsNormal_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(20, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "other sections";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(18, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(443, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "If the first section is an intro you may wish to handle it differently to";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numAudioSectionLength);
            this.groupBox1.Controls.Add(this.radAudioDef3);
            this.groupBox1.Controls.Add(this.radAudioDef2);
            this.groupBox1.Controls.Add(this.radAudioDef1);
            this.groupBox1.Location = new System.Drawing.Point(12, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 86);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "An audio section is...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(140, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "seconds";
            // 
            // numAudioSectionLength
            // 
            this.numAudioSectionLength.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numAudioSectionLength.Location = new System.Drawing.Point(79, 39);
            this.numAudioSectionLength.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numAudioSectionLength.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numAudioSectionLength.Name = "numAudioSectionLength";
            this.numAudioSectionLength.Size = new System.Drawing.Size(55, 21);
            this.numAudioSectionLength.TabIndex = 5;
            this.numAudioSectionLength.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // radAudioDef3
            // 
            this.radAudioDef3.AutoSize = true;
            this.radAudioDef3.Location = new System.Drawing.Point(20, 60);
            this.radAudioDef3.Name = "radAudioDef3";
            this.radAudioDef3.Size = new System.Drawing.Size(179, 16);
            this.radAudioDef3.TabIndex = 4;
            this.radAudioDef3.Text = "entire file is one section";
            this.radAudioDef3.UseVisualStyleBackColor = true;
            this.radAudioDef3.CheckedChanged += new System.EventHandler(this.radAudioDef3_CheckedChanged);
            // 
            // radAudioDef2
            // 
            this.radAudioDef2.AutoSize = true;
            this.radAudioDef2.Location = new System.Drawing.Point(20, 39);
            this.radAudioDef2.Name = "radAudioDef2";
            this.radAudioDef2.Size = new System.Drawing.Size(53, 16);
            this.radAudioDef2.TabIndex = 3;
            this.radAudioDef2.Text = "every";
            this.radAudioDef2.UseVisualStyleBackColor = true;
            this.radAudioDef2.CheckedChanged += new System.EventHandler(this.radAudioDef2_CheckedChanged);
            // 
            // radAudioDef1
            // 
            this.radAudioDef1.AutoSize = true;
            this.radAudioDef1.Checked = true;
            this.radAudioDef1.Location = new System.Drawing.Point(20, 18);
            this.radAudioDef1.Name = "radAudioDef1";
            this.radAudioDef1.Size = new System.Drawing.Size(257, 16);
            this.radAudioDef1.TabIndex = 2;
            this.radAudioDef1.TabStop = true;
            this.radAudioDef1.Text = "separated by several seconds of silence";
            this.radAudioDef1.UseVisualStyleBackColor = true;
            this.radAudioDef1.CheckedChanged += new System.EventHandler(this.radAudioDef1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(98, 451);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 42);
            this.button1.TabIndex = 10;
            this.button1.Text = "< Prev";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // ConfigureSections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 511);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConfigureSections";
            this.Text = "Language Audio Tool v0.1";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAudioSectionLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radRemove;
        private System.Windows.Forms.RadioButton radPlayOnceFullSpeed;
        private System.Windows.Forms.RadioButton radHandleAsNormal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numAudioSectionLength;
        private System.Windows.Forms.RadioButton radAudioDef3;
        private System.Windows.Forms.RadioButton radAudioDef2;
        private System.Windows.Forms.RadioButton radAudioDef1;
        private System.Windows.Forms.Button button1;
    }
}