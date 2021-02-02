using System;
using System.Windows.Forms;

namespace Tools_Injector_Mod_Menu
{
    public partial class FrmSeekBar : Form
    {
        public FrmSeekBar()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void NumValueChanged(object sender, EventArgs e)
        {
            if (numMax.Value <= numMin.Value)
            {
                numMax.Value = numMin.Value + 1;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FrmMain.SeekBar = $"{numMin.Value}_{numMax.Value}";
            Dispose();
        }
    }
}