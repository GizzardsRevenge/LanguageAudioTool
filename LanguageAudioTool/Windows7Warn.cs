using System;
using System.Drawing;
using System.Windows.Forms;

namespace LanguageAudioTool
{
    public partial class Windows7Warn : Form
    {
        public Windows7Warn()
        {
            InitializeComponent();

            pictureBox1.Image = SystemIcons.Information.ToBitmap();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                DialogResult = DialogResult.Ignore;
            else
                DialogResult = DialogResult.OK;

            Close();
        }
    }
}
