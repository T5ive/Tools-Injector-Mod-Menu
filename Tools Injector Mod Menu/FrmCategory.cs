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
            if (!Utility.IsEmpty(txtValues))
            {
                FrmMain.Category = txtValues.Text;
            }
            else
            {
                MyMessage.MsgShow("TextBox is Empty!!", MessageBoxIcon.Error);
                return;
            }
            Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}