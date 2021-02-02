using System.Windows.Forms;

namespace Tools_Injector_Mod_Menu
{
    public class MyMessage
    {
        private const string Caption = "Tools Injector";

        public static DialogResult MsgShow(string text)
        {
            return MessageBox.Show(text, Caption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        public static DialogResult MsgShow(string text, MessageBoxIcon icon)
        {
            return MessageBox.Show(text, Caption, MessageBoxButtons.OK, icon);
        }

        public static bool MsgOkCancel(string text)
        {
            return MessageBox.Show(text, Caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK;
        }
    }
}