
using System;
using System.Windows.Forms;

namespace LanguageAudioTool
{
    public partial class Copying : Form
    {
        public string InfoText
        {
            get { return lblInfoText.Text; }
            set { lblInfoText.Text = value; }
        }

        public int ProgressBarValue
        {
            set 
            { 
                progressBar1.Value = value;
                //progressBar1.Refresh(); 
            }
        }

        #region singleton overhead

        static Copying _instance = null;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Copying() { } // Do not use

        Copying()
        {
            InitializeComponent();
        }

        public static Copying Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Copying();
                return _instance;
            }
        }
        #endregion
    }
}
