using System;
using System.IO;
using System.Windows.Forms;

namespace LanguageAudioTool
{
    public partial class ConfigureSections : Form
    {
        //private const string kAudioDefSepBySilence = "SeparatedBySilence";
        //private const string kAudioDefByDuration = "ByDuration";
        //private const string kAudioDefWholeFile = "WholeFile";
        private FileWorker.FirstSectionBehaviour _firstBehaviour;
        private MusicFile.SectionDefinition _sectionDefinition;


        public FileWorker.FirstSectionBehaviour FirstSectionBehaviour
        {
            get { return _firstBehaviour; }
        }

        public MusicFile.SectionDefinition SectionDefinitionProp
        {
            get { return _sectionDefinition; }
        }

        public int SectionByDurationDuration
        {
            get { return (int)numAudioSectionLength.Value; }
        }

        public ConfigureSections()
        {
            InitializeComponent();

            _sectionDefinition = (MusicFile.SectionDefinition)Properties.Settings.Default.AudioSectionDefinition;

            radAudioDef1.Checked = _sectionDefinition == MusicFile.SectionDefinition.kSeparatedBySilence;
            radAudioDef2.Checked = _sectionDefinition == MusicFile.SectionDefinition.kByDuration;
            radAudioDef3.Checked = _sectionDefinition == MusicFile.SectionDefinition.kWholeFile;

            numAudioSectionLength.Value = Properties.Settings.Default.AudioSectionLengthSecs;

            _firstBehaviour = (FileWorker.FirstSectionBehaviour)Properties.Settings.Default.FirstSectionBehaviour;

            radHandleAsNormal.Checked = (_firstBehaviour == FileWorker.FirstSectionBehaviour.kHandleAsNormal);
            radPlayOnceFullSpeed.Checked = (_firstBehaviour == FileWorker.FirstSectionBehaviour.kOnceAtFullSpeed);
            radRemove.Checked = (_firstBehaviour == FileWorker.FirstSectionBehaviour.kDelete);

            CalcFirstSectionBehaviour();

            UpdateRadButtonConflicts();
        }

        private void CalcFirstSectionBehaviour()
        {
            _firstBehaviour = FileWorker.FirstSectionBehaviour.kHandleAsNormal;

            if (radPlayOnceFullSpeed.Checked)
                _firstBehaviour = FileWorker.FirstSectionBehaviour.kOnceAtFullSpeed;
            else if (radRemove.Checked)
                _firstBehaviour = FileWorker.FirstSectionBehaviour.kDelete;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            // Convert UI into internal types
            CalcFirstSectionBehaviour();

            if (radAudioDef1.Checked)
                _sectionDefinition = MusicFile.SectionDefinition.kSeparatedBySilence;
            else if (radAudioDef2.Checked)
                _sectionDefinition = MusicFile.SectionDefinition.kByDuration;
            else
                _sectionDefinition = MusicFile.SectionDefinition.kWholeFile;


            // Save settings
            Properties.Settings.Default.AudioSectionLengthSecs = (int)numAudioSectionLength.Value;
            Properties.Settings.Default.AudioSectionDefinition = (int)_sectionDefinition;
            Properties.Settings.Default.FirstSectionBehaviour = (int)_firstBehaviour;

            Close();
        }

        private void radAudioDef3_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRadButtonConflicts();
        }

        private void UpdateRadButtonConflicts()
        {
            if (radAudioDef3.Checked)
            {
                // Whole file is one section, so only valid processing option is "handle as normal"
                radHandleAsNormal.Checked = true;
                radPlayOnceFullSpeed.Enabled = false;
                radRemove.Enabled = false;
            }
            else
            {
                radPlayOnceFullSpeed.Enabled = true;
                radRemove.Enabled = true;
            }
            
            if (radPlayOnceFullSpeed.Checked || radRemove.Checked)
            {
                // These options not compatible with all audio file being one section
                radAudioDef3.Enabled = false;

                if (radAudioDef3.Checked)
                    radAudioDef1.Checked = true;
            }
            else
                radAudioDef3.Enabled = true;
        }

        private void radAudioDef1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRadButtonConflicts();
        }

        private void radAudioDef2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRadButtonConflicts();
        }

        private void radHandleAsNormal_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRadButtonConflicts();
        }

        private void radPlayOnceFullSpeed_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRadButtonConflicts();
        }

        private void radRemove_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRadButtonConflicts();
        }
    }
}
