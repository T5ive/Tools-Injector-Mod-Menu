using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tools_Injector_Mod_Menu
{
    public partial class FrmToggleHook : Form
    {
        public FrmToggleHook()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to save?\n\n" +
                                       "Click \"OK\" to confirm.\n\n" +
                                       "Click \"Cancel\" to cancel.")) return;
            if (chkField.Checked)
            {
                if (Utility.IsEmpty(txtOffset)) return;
                Values.Field = chkField.Checked;
                Values.Offset = txtOffset.Text;
            }
            Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to close?\n\n" +
                                       "Click \"OK\" to confirm.\n\n" +
                                       "Click \"Cancel\" to cancel.")) return;
            Dispose();
        }

        private void chkField_CheckedChanged(object sender, EventArgs e)
        {
            txtOffset.Enabled = chkField.Checked;
        }
    }
}
