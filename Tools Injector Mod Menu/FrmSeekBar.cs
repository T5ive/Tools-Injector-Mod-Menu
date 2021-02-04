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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to close?\n\n" +
                                       "Click \"OK\" to confirm.\n\n" +
                                       "Click \"Cancel\" to cancel.")) return;
            FrmMain.SeekBar = $"{numMin.Value}_{numMax.Value}";
            Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to close?\n\n" +
                                       "Click \"OK\" to confirm.\n\n" +
                                       "Click \"Cancel\" to cancel.")) return;
            Dispose();
        }

        private void NumValueChanged(object sender, EventArgs e)
        {
            if (numMax.Value <= numMin.Value)
            {
                numMax.Value = numMin.Value + 1;
            }
        }
    }
}