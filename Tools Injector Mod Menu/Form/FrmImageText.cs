using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Tools_Injector_Mod_Menu
{
    public partial class FrmImageText : Form
    {
        public FrmImageText()
        {
            InitializeComponent();
        }

        private void FrmImageText_Load(object sender, EventArgs e)
        {
            txtImg.Text = FrmMain.ImageCode;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to save?\n\n" +
                                       "Click \"OK\" to confirm.\n\n" +
                                       "Click \"Cancel\" to cancel.")) return;
            FrmMain.ImageCode = $"{txtImg.Text}";
            Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to close?\n\n" +
                                       "Click \"OK\" to confirm.\n\n" +
                                       "Click \"Cancel\" to cancel.")) return;
            Dispose();
        }

        private void lbImgCompress_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://compresspng.com/");
        }

        private void lbImageEncoder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://codebeautify.org/image-to-base64-converter");
        }
    }
}