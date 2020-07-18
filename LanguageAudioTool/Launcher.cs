using System;
using System.Windows.Forms;
using System.IO;
using NAudio.Wave;
using NAudio.MediaFoundation;

namespace LanguageAudioTool
{
    public partial class Launcher : Form
    {
        //public const int kTwoHundredKBPS = 200000;
        //public const int kOneMinuteAudio = kTwoHundredKBPS * 60;
        //public const int kDefaultArraySize = kOneMinuteAudio * 5;

        public Launcher()
        {
            InitializeComponent();

            MediaFoundationApi.Startup();
            DoMp3EncoderCheck();
        }

        private void DoMp3EncoderCheck()
        {
            var mediaType = MediaFoundationEncoder.SelectMediaType(
                            AudioSubtypes.MFAudioFormat_MP3,
                            new WaveFormat(44100, 1), 0);

            if (mediaType == null) // Can't use faster Windows 8 encoding
            {
                // Can't encode
                if (Properties.Settings.Default.ShowWindows7Warn)
                {
                    Windows7Warn warnDialog = new Windows7Warn();
                    DialogResult dr = warnDialog.ShowDialog();

                    if (dr == DialogResult.Ignore)
                        Properties.Settings.Default.ShowWindows7Warn = false;
                }
                FileWorker.UseLameMp3Encoding = true;
            }
            else
                FileWorker.UseLameMp3Encoding = false;
        }

        //DEBUG
        private void DEBUGPopulate()
        {
            MusicFile mf = new MusicFile("d:\\SCRATCH\\Little2.mp3");
            lstFiles.Items.Add(mf);
            UpdateDragDropMessage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Visible = false;

            Configure configDlg = new Configure(lstFiles.Items);
            configDlg.ShowDialog();

            Visible = true;
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
            {
                MusicFile mf = new MusicFile(file);
                lstFiles.Items.Add(mf);
            }

            UpdateDragDropMessage();
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            // Load settings, using defaults if not set
            String folderName = Properties.Settings.Default.MusicFileFolder;

            if (String.IsNullOrWhiteSpace(folderName))
            {
                // Use default music folder
                folderName = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                // And save it back to the property afterwards
                Properties.Settings.Default.MusicFileFolder = folderName;
            }
        }

        private void Launcher_FormClosing(object sender, FormClosingEventArgs e)
        {
            MediaFoundationApi.Shutdown();
            Properties.Settings.Default.Save();
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (String.IsNullOrEmpty(Properties.Settings.Default.MusicFileFolder))
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic);
            else
                dialog.InitialDirectory = Properties.Settings.Default.MusicFileFolder;

            dialog.Filter = "MP3 files|*.mp3|All files|*.*";
            dialog.CheckFileExists = true;
            dialog.Multiselect = true;
            DialogResult dr = dialog.ShowDialog();

            if (dr == DialogResult.OK && dialog.FileNames.Length > 0)
            {
                string[] files = dialog.FileNames;
                Properties.Settings.Default.MusicFileFolder = Path.GetDirectoryName(files[0]);

                foreach (string file in files)
                {
                    MusicFile mf = new MusicFile(file);
                    lstFiles.Items.Add(mf);
                }

                UpdateDragDropMessage();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection selectedItems = new ListBox.SelectedObjectCollection(lstFiles);
            selectedItems = lstFiles.SelectedItems;

            if (lstFiles.SelectedIndex != -1)
            {
                for (int i = selectedItems.Count - 1; i >= 0; i--)
                    lstFiles.Items.Remove(selectedItems[i]);
            }

            UpdateDragDropMessage();
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                btnRemove_Click(sender, e);
        }

        private void label1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void label1_DragDrop(object sender, DragEventArgs e)
        {
            listBox1_DragDrop(sender, e);
        }

        private void UpdateDragDropMessage()
        {
            lblDragDrop.Visible = (lstFiles.Items.Count == 0);
            btnNext.Enabled = (lstFiles.Items.Count > 0);
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = (lstFiles.SelectedIndex >= 0);    
        }
    }
}
