using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace LanguageAudioTool
{
    public partial class ConfigureJobs : Form
    {
        private const string kJobFilename = "InitialJobs.xml";

        public ListBox.ObjectCollection Jobs
        {
            get { return lstActions.Items; }
        }

        public ConfigureJobs()
        {
            InitializeComponent();

            // Get defaults
            int percentSpeed = Properties.Settings.Default.SectionInitialSpeed;
            int silenceSeconds = Properties.Settings.Default.SilenceInitialDuration;
            int beepMS = Properties.Settings.Default.BeepInitialDurationMS;

            numChunkSpeed.Value = percentSpeed;
            numSilenceSeconds.Value = silenceSeconds;
            numBeep.Value = 0.001m * beepMS;

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

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ActionsSanityCheck())
            {
                SaveInitialJobList();
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
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

        private void btnBeep_Click(object sender, EventArgs e)
        {
            AddJob(new Job(Job.Type.AddBeep, (int)(numBeep.Value * 1000m)));
        }
    }
}
