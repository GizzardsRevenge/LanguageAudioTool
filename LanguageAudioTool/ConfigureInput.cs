using System;
using System.Windows.Forms;
using System.IO;
using NAudio.Wave;
using NAudio.MediaFoundation;
using System.Runtime.Remoting.Messaging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace LanguageAudioTool
{
    public partial class ConfigureInput : Form
    {
        ConfigureOutput _outputDlg = new ConfigureOutput();
        ConfigureJobs _jobsDlg = new ConfigureJobs();
        ConfigureSections _sectionsDlg = new ConfigureSections();
        private FileWorker _currentFileWorker;
        private bool _copyStarted = false;

        public ConfigureInput()
        {
            InitializeComponent();

            backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;

            MediaFoundationApi.Startup();
            DoMp3EncoderCheck();

            // Set the dialogs' start positions
            _outputDlg.StartPosition = FormStartPosition.Manual;
            _jobsDlg.StartPosition = FormStartPosition.Manual;
            _sectionsDlg.StartPosition = FormStartPosition.Manual;

            //DEBUGPopulate();
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
            MusicFile mf = new MusicFile("d:\\SCRATCH\\Little.mp3");
            lstFiles.Items.Add(mf);
            UpdateDragDropMessage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckFilesAreValid())
                Stage2(Location);
        }

        private bool CheckFilesAreValid()
        {
            foreach (MusicFile file in lstFiles.Items)
            {
                if (!file.CheckIsValid())
                {
                    string warning = String.Format("This file could not be read:{0}{1}{0}If it is a valid mp3 file, check it is not zero length", Environment.NewLine, file.Filename);
                    MessageBox.Show(warning, "Cannot process file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private void Stage2(Point location)
        {
            Visible = false;
            _sectionsDlg.Location = location;
            DialogResult dr = _sectionsDlg.ShowDialog();

            if (dr == DialogResult.OK)
                Stage3(_sectionsDlg.Location);
            else
            {
                Stage1(_sectionsDlg.Location);
            }
        }

        private void Stage1(Point location)
        {
            Location = location;
            Visible = true;
        }

        private void Stage3(Point location)
        {
            Visible = false;
            _jobsDlg.Location = location;
            DialogResult dr = _jobsDlg.ShowDialog();

            if (dr == DialogResult.OK)
                Stage4(_jobsDlg.Location);
            else if (dr == DialogResult.No)
                Stage2(_jobsDlg.Location);
            else
                Stage1(_jobsDlg.Location);
        }


        private void Stage4(Point location)
        {
            Visible = false;
            _outputDlg.Location = location;
            DialogResult dr = _outputDlg.ShowDialog(this);

            if (dr == DialogResult.OK)
                DoTheActualWork();
            else if (dr == DialogResult.No)
                Stage3(_outputDlg.Location);
            else
                Stage1(_outputDlg.Location);
        }

        private void DoTheActualWork()
        {
            if (SanityChecks())
            {
                int fileIndex = 1; // Counting from 1 (for user display)
                List<Job> actions = new List<Job>();

                foreach (MusicFile file in lstFiles.Items)
                {
                    file.ProcessIntoSections(_sectionsDlg.SectionDefinitionProp, _sectionsDlg.SectionByDurationDuration);

                    foreach (Job job in _jobsDlg.Jobs)
                        actions.Add(job);

                    InvokeIfRequired(Copying.Instance, delegate
                    {
                        Copying.Instance.InfoText = String.Format("Processing file: {0}/{1}", fileIndex++, lstFiles.Items.Count);
                    });

                    _currentFileWorker = new FileWorker(file.Sections, actions, file, _sectionsDlg.FirstSectionBehaviour, _outputDlg.OutputFolder);

                    if (backgroundWorker.IsBusy != true)
                    {
                        backgroundWorker.RunWorkerAsync();
                    }

                    DialogResult dr = Copying.Instance.ShowDialog();

                    if (dr == DialogResult.Cancel)
                    {
                        backgroundWorker.CancelAsync();
                    }
                }
            }
            else
            {
                // Bit of a hack...right now we know that dialog 4 is the right one to show to resolve the sanity check fail
                Stage4(Location);
            }
        }

        public static void InvokeIfRequired(Control control, MethodInvoker action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }

        private bool SanityChecks()
        {
            // Check that the folder is not the same as any of the files'
            string outputFolder = _outputDlg.OutputFolder;

            foreach (MusicFile file in lstFiles.Items)
            {
                if (SameDirectory(System.IO.Path.GetDirectoryName(file.Filename), outputFolder))
                {
                    MessageBox.Show(@"The output folder is the same as one or more input files' folder.
This is a beta version of this software, and as a safety precaution, the output files cannot go into the same folder as original files.
This is to prevent possible loss or corruption of data.", "Aborted", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private bool SameDirectory(string path1, string path2)
        {
            return (
                0 == String.Compare(
                    System.IO.Path.GetFullPath(path1).TrimEnd('\\'),
                    System.IO.Path.GetFullPath(path2).TrimEnd('\\'),
                    StringComparison.InvariantCultureIgnoreCase))
                ;
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

        private void BackgroundWorker_DoWork(object sender, EventArgs e)
        {
            _copyStarted = true;

             InvokeIfRequired(Copying.Instance, delegate
             {
                 Copying.Instance.InfoText = "Processing file: 1/" + lstFiles.Items.Count.ToString();
                 //"INITIAL TEXT";// With this dummy text, we know whether the progress box ever got updated
             });

             InvokeIfRequired(Copying.Instance, delegate
             {
                 Copying.Instance.ProgressBarValue = 0;
             });

            _currentFileWorker.DoWork(_outputDlg.OneOutputPerInput);
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, EventArgs e)
        {
            if (_copyStarted)
            {
                Stage1(_outputDlg.Location);
                _copyStarted = false;
                Copying.Instance.Close();
                Process.Start(_outputDlg.OutputFolder);
            }
        }
    }
}
