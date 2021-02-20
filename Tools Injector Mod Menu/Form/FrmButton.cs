using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Tools_Injector_Mod_Menu
{
    public partial class FrmButton : Form
    {
        //seekbar_0_100
        //inputvalue
        public FrmButton()
        {
            InitializeComponent();
        }

        private void FrmButton_Load(object sender, EventArgs e)
        {
            try
            {
                txtOffset.Text = Values.Offset;
                if (Values.Method.Count > 0)
                {
                    for (int i = 0; i < Values.Method.Count; i++)
                    {
                        var items = new ListViewItem(Values.Method[i].Item1);
                        items.SubItems.Add(Values.Method[i].Item2);
                        listView1.Items.Add(items);
                    }
                }
            }
            catch
            {
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(Utility.IsEmpty(txtType) || Utility.IsEmpty(txtValue)) return;
            var items = new ListViewItem(txtType.Text);
            items.SubItems.Add(txtValue.Text);
            listView1.Items.Add(items);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var count = listView1.SelectedItems.Count;
            if (listView1.Items.Count != 0 && count != 0)
            {
                if (MyMessage.MsgOkCancel("You sure you want remove this function in the List?"))
                {
                    for (var i = 0; i < listView1.Items.Count; i++)
                    {
                        if (listView1.Items[i].Selected)
                        {
                            listView1.Items[i].Remove();
                        }
                    }
                }
            }
            else
            {
                MyMessage.MsgShowWarning("There is no Items Functions in the List!!");
            }
        }

        private List<(string, string)> ListToTuple()
        {
            var item = new List<(string, string)>();
            for (var i = 0; i < listView1.Items.Count; i++)
            {
                item.Add((listView1.Items[i].Text, listView1.Items[i].SubItems[1].Text));
            }
            return item.ToList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to save?\n\n" +
                                       "Click \"OK\" to confirm.\n\n" +
                                       "Click \"Cancel\" to cancel.")) return;
            Values.Method = ListToTuple().ToList();
            Values.Offset = txtOffset.Text;
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