using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tools_Injector_Mod_Menu.Patch_Manager;

namespace Tools_Injector_Mod_Menu
{
    //TODO Check offset, hex before save
    //Confirm Save/Close
    public partial class FrmFunction : Form
    {
        private readonly int _index;
        private Enums.FunctionType _type;

        public FrmFunction(int index)
        {
            InitializeComponent();
            _index = index;
            AddListValues();
        }

        private void AddListValues()
        {
            txtNameCheat.Text = OffsetPatch.FunctionList[_index].CheatName;
            txtValues.Text = OffsetPatch.FunctionList[_index].FunctionValue;
            _type = OffsetPatch.FunctionList[_index].FunctionType;
            foreach (var t in OffsetPatch.FunctionList[_index].OffsetList)
            {
                dataList.Rows.Add(t.Offset, t.Hex);
            }

            if (_type == Enums.FunctionType.ToggleSeekBar ||
                _type == Enums.FunctionType.ButtonOnOffSeekBar ||
                _type == Enums.FunctionType.ToggleInputValue ||
                _type == Enums.FunctionType.ButtonOnOffInputValue)
            {
                chkMultiple.Enabled = true;
                txtValues.Enabled = true;
            }

            if (_type == Enums.FunctionType.Category)
            {
                txtValues.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to save?\n\n" +
                                       "Click \"OK\" to confirm.\n\n" +
                                       "Click \"Cancel\" to cancel.")) return;
            try
            {
                var offsetList = new List<OffsetInfo>();
                for (var i = 0; i < dataList.Rows.Count; i++)
                {
                    var offset = new OffsetInfo
                    {
                        OffsetId = i,
                        Offset = dataList.Rows[i].Cells[0].Value.ToString(),
                        Hex = dataList.Rows[i].Cells[1].Value.ToString()
                    };
                    offsetList.Add(offset);
                }

                OffsetPatch.FunctionList[_index] = new FunctionList()
                {
                    CheatName = txtNameCheat.Text,
                    FunctionValue = txtValues.Text,
                    FunctionType = _type,
                    OffsetList = offsetList,
                    MultipleValue = chkMultiple.Checked
                };

                Dispose();
            }
            catch (Exception exception)
            {
                MyMessage.MsgShowError("Error" + exception.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to close?\n\n" +
                                       "Click \"OK\" to confirm.\n\n" +
                                       "Click \"Cancel\" to cancel.")) return;
            Dispose();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveRows();
            }
        }

        private void RemoveRows()
        {
            try
            {
                if (dataList.RowCount <= 1 || dataList.CurrentCell.RowIndex == dataList.RowCount - 1) return;
                dataList.Rows.RemoveAt(dataList.SelectedCells[0].RowIndex);
                dataList.ClearSelection();
            }
            catch
            {
                //
            }
        }

        private void dataList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                try
                {
                    var hti = dataList.HitTest(e.X, e.Y);
                    dataList.ClearSelection();
                    dataList.Rows[hti.RowIndex].Selected = true;
                }
                catch
                {
                    //
                }
            }

            if (dataList.SelectedRows.Count > 0)
            {
                addToolStripMenuItem.Enabled = false;
                removeToolStripMenuItem.Enabled = true;
            }
            else
            {
                addToolStripMenuItem.Enabled = true;
                removeToolStripMenuItem.Enabled = false;
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var rowToDelete = dataList.Rows.GetFirstRow(DataGridViewElementStates.Selected);
                dataList.Rows.RemoveAt(rowToDelete);
                dataList.ClearSelection();
            }
            catch
            {
                //
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataList.Rows.Add(null, null);
        }
    }
}