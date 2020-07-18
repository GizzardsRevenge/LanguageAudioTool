using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace LanguageAudioTool
{
    public partial class Configure : Form
    {
        private ListBox.ObjectCollection _files;
        private FileWorker _currentFileWorker;
        private bool _copyStarted = false;

        private const string kJobFilename = "InitialJobs.xml";

        public Configure(ListBox.ObjectCollection files)
        {
            InitializeComponent();

            backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;

            _files = files;

            // Get defaults
            int percentSpeed = Properties.Settings.Default.SectionInitialSpeed;
            int silenceSeconds = Properties.Settings.Default.SilenceInitialDuration;

            numChunkSpeed.Value = percentSpeed;
            numSilenceSeconds.Value = silenceSeconds;

            int behave = Properties.Settings.Default.FirstSectionBehaviour;

            radHandleAsNormal.Checked = (behave == 0);
            radPlayOnceFullSpeed.Checked = (behave == 1);
            radRemove.Checked = (behave == 2);

            // Populate tasks (not saved)
            if (File.Exists(kJobFilename))
                LoadInitialJobList();
            else
            {
                // No saved lists...create a trivial default set
                AddJob(new Job(Job.Type.AddSection, 100));
                AddJob(new Job(Job.Type.AddSilence, 2));
            }
        }

        private void LoadInitialJobList()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(kJobFilename);
            XmlElement rootEl = doc.DocumentElement;

            foreach (XmlNode child in rootEl.ChildNodes)
            {
                if (child is XmlElement)
                {
                    XmlElement element = child as XmlElement;
                    Job job = new Job();
                    job.FromXMLElement(element);
                    AddJob(job);
                }
            }
        }

        private void SaveInitialJobList()
        {
            XmlDocument doc = new XmlDocument();
            // Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-16", null);
            doc.AppendChild(dec);// Create the root element

            // Write the top level DB stuff
            XmlElement root = doc.CreateElement("JobList");
            
            // Write the list of jobs
            XmlElement jobList = doc.CreateElement("Jobs");
            foreach (Job job in lstActions.Items)
            {
                XmlElement current = job.ToXMLElement(doc);
                root.AppendChild(current);
            }

            doc.AppendChild(root);
            doc.Save(kJobFilename);
        }

        private void AddJob(Job job)
        {
            lstActions.Items.Add(job);
            btnClearAll.Enabled = true;
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            lstActions.Items.Clear();

            btnUp.Enabled = false;
            btnClearAll.Enabled = false;
            btnDown.Enabled = false;
            btnDeleteSelected.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (ActionsSanityCheck())
            {
                // Output folder
                FolderBrowserDialog dialog = new FolderBrowserDialog();

                if (!String.IsNullOrEmpty(Properties.Settings.Default.OutputFolder))
                    dialog.SelectedPath = Properties.Settings.Default.OutputFolder;

                dialog.ShowNewFolderButton = true;
                dialog.Description = "Output folder for created files";
                DialogResult dr = dialog.ShowDialog();

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    string path = dialog.SelectedPath;

                    // Check that the folder is not the same as any of the files'
                    foreach (MusicFile file in _files)
                    {
                        if (SameDirectory(System.IO.Path.GetDirectoryName(file.Filename), path))
                        {
                            MessageBox.Show(@"The output folder is the same as one or more input files' folder.
This is a beta version of this software, and as a safety precaution, the output files cannot go into the same folder as original files.
This is to prevent possible loss or corruption of data.", "Aborted", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                       /* if (!file.CanProcessFile())
                        {
                            MessageBox.Show(String.Format("{0}{1}Cannot be parsed as a valid MP3 file", file.Filename, Environment.NewLine));
                            return;
                        }*/
                    }

                    FileWorker.FirstSectionBehaviour firstSection = FileWorker.FirstSectionBehaviour.kHandleAsNormal;

                    if (radPlayOnceFullSpeed.Checked)
                        firstSection = FileWorker.FirstSectionBehaviour.kOnceAtFullSpeed;
                    else if (radRemove.Checked)
                        firstSection = FileWorker.FirstSectionBehaviour.kDelete;

                    int fileIndex = 1; // Counting from 1 (for user display)
                    foreach (MusicFile file in _files)
                    {
                        file.ProcessIntoSections();

                        List<Job> actions = new List<Job>();
                        foreach (Job job in lstActions.Items)
                            actions.Add(job);

                        _currentFileWorker = new FileWorker(file.Sections, actions, file, firstSection, path);

                        InvokeIfRequired(Copying.Instance, delegate
                        {
                            Copying.Instance.InfoText = String.Format("Processing file: {0}/{1}", fileIndex++, _files.Count);
                        });

                        if (backgroundWorker.IsBusy != true)
                        {
                            backgroundWorker.RunWorkerAsync();
                        }

                        dr = Copying.Instance.ShowDialog();

                        if (dr == DialogResult.Cancel)
                        {
                            backgroundWorker.CancelAsync();
                        }
                    }

                    MessageBox.Show("All files done");
                }

                // Save settings
                Properties.Settings.Default.SilenceInitialDuration = (int)numSilenceSeconds.Value;
                Properties.Settings.Default.SectionInitialSpeed = (int)numChunkSpeed.Value;
                Properties.Settings.Default.OutputFolder = dialog.SelectedPath;

                if (radHandleAsNormal.Checked)
                    Properties.Settings.Default.FirstSectionBehaviour = 0;
                else if (radPlayOnceFullSpeed.Checked)
                    Properties.Settings.Default.FirstSectionBehaviour = 1;
                else // assume remove
                    Properties.Settings.Default.FirstSectionBehaviour = 2;

                SaveInitialJobList();
            }
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

        private bool ActionsSanityCheck()
        {
            // Check current list of actions. If OK, return true

            // 1. Empty list
            if (lstActions.Items.Count == 0)
            {
                MessageBox.Show("No actions defined");
                return false;
            }

            // 2. Two sections of audio with no silence inbetween
            bool lastWasAudio = ((lstActions.Items[lstActions.Items.Count - 1] as Job).JobType == Job.Type.AddSection);
            bool foundGapless = false;
            foreach (Job job in lstActions.Items)
            {
                bool currentIsAudio = (job.JobType == Job.Type.AddSection);

                if (lastWasAudio && currentIsAudio)
                    foundGapless = true;

                lastWasAudio = currentIsAudio;
            }

            if (foundGapless)
            {
                DialogResult dr = MessageBox.Show(@"It is recommended that all audio sections be separated by a few seconds of silence.
Otherwise sections will directly run after each other, which will sound ugly.
Are you sure you want to proceed?", "Proceed?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (dr != System.Windows.Forms.DialogResult.Yes)
                    return false;
            }

            return true;
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection selectedItems = new ListBox.SelectedObjectCollection(lstActions);
            selectedItems = lstActions.SelectedItems;

            if (lstActions.SelectedIndex != -1)
            {
                for (int i = selectedItems.Count - 1; i >= 0; i--)
                    lstActions.Items.Remove(selectedItems[i]);
            }

            btnDeleteSelected.Enabled = false;

            if (lstActions.Items.Count == 0)
                btnClearAll.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddJob(new Job(Job.Type.AddSection, (int)numChunkSpeed.Value));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddJob(new Job(Job.Type.AddSilence, (int)numSilenceSeconds.Value));
        }

        private void lstActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool itemSelected = (lstActions.SelectedIndex >= 0);

            btnUp.Enabled = itemSelected;
            btnDown.Enabled = itemSelected;
            btnDeleteSelected.Enabled = itemSelected;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (lstActions.SelectedIndex > 0)
            {
                int index = lstActions.SelectedIndex;
                object temp = lstActions.Items[index - 1];
                lstActions.Items[index - 1] = lstActions.Items[index];
                lstActions.Items[index] = temp;

                lstActions.SelectedIndex--;
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (lstActions.SelectedIndex >= 0 && lstActions.SelectedIndex < lstActions.Items.Count - 1)
            {
                int index = lstActions.SelectedIndex;
                object temp = lstActions.Items[index + 1];
                lstActions.Items[index + 1] = lstActions.Items[index];
                lstActions.Items[index] = temp;

                lstActions.SelectedIndex++;
            }
        }

        private void BackgroundWorker_DoWork(object sender, EventArgs e)
        {
            _copyStarted = true;

           /* InvokeIfRequired(Copying.Instance, delegate
            {
                Copying.Instance.InfoText = "INITIAL TEXT";// Processing file 1/1 ...";
            });

            InvokeIfRequired(Copying.Instance, delegate
            {
                Copying.Instance.ProgressBarValue = 0;
            });*/

            _currentFileWorker.DoWork();
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, EventArgs e)
        {
            if (_copyStarted) 
            {
                _copyStarted = false;
                Copying.Instance.Close();
                //string message = String.Format("Done");
                //MessageBox.Show(message, "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static void InvokeIfRequired(Control control, MethodInvoker action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }
    }
}
