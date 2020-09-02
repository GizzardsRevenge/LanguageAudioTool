using System;
using System.Windows.Forms;

namespace LanguageAudioTool
{
    public partial class ConfigureOutput : Form
    {
        public bool OneOutputPerInput
        {
            get { return radPerSection.Checked; }
        }

        public string OutputFolder
        {
            get { return txtDestFolder.Text; }
        }

        public ConfigureOutput()
        {
            InitializeComponent();

            // Load settings
            radPerSection.Checked = Properties.Settings.Default.OneOutputPerInput;
            radPerInput.Checked = !radPerSection.Checked;

            txtDestFolder.Text = Properties.Settings.Default.OutputFolder;

            UpdateWarningLabel();
        }

        private void radPerInput_CheckedChanged(object sender, EventArgs e)
        {
            UpdateWarningLabel();
        }

        private void radPerSection_CheckedChanged(object sender, EventArgs e)
        {
            UpdateWarningLabel();
        }

        private void UpdateWarningLabel()
        {
            lblWarn1.Visible = radPerSection.Checked;
            lblWarn2.Visible = radPerSection.Checked;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.SelectedPath = txtDestFolder.Text;
            dialog.ShowNewFolderButton = true;
            dialog.Description = "Output folder for created files";
            DialogResult dr = dialog.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
                txtDestFolder.Text = dialog.SelectedPath;
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // Save settings
            Properties.Settings.Default.OutputFolder = txtDestFolder.Text;
            Properties.Settings.Default.OneOutputPerInput = radPerSection.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}