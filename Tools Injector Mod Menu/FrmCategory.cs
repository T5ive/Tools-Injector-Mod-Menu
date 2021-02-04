using System;
using System.Windows.Forms;

namespace Tools_Injector_Mod_Menu
{
    public partial class FrmCategory : Form
    {
        public FrmCategory()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to save?\n\n" +
                                      "Click \"OK\" to confirm.\n\n" +
                                      "Click \"Cancel\" to cancel.")) return;

            if (!Utility.IsEmpty(txtValues))
            {
                FrmMain.Category = txtValues.Text;
            }
            else
            {
                MyMessage.MsgShowError("TextBox is Empty!!");
                return;
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
    }
}