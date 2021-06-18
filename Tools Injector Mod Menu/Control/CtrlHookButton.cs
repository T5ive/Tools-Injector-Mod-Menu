using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tools_Injector_Mod_Menu.Patch_Manager;

namespace Tools_Injector_Mod_Menu
{
    public partial class CtrlHookButton : UserControl
    {
        private readonly int _index;
        private Enums.FunctionType _type;
        private readonly FrmAddFunction _frmAddFunction;

        public CtrlHookButton(FrmAddFunction frmAdd, int index = 1150)
        {
            InitializeComponent();
            _index = index;
            _frmAddFunction = frmAdd;
            _frmAddFunction.Text = HookName();
            if (_index != 1150)
            {
                AddListValues();
            }
            else
            {
                btnSave.Text = "Add Function";
            }
        }

        private void AddListValues()
        {
            _type = OffsetPatch.FunctionList[_index].FunctionType;
            switch (_type)
            {
                case Enums.FunctionType.HookButtonOnOf:
                    radButton.Checked = true;
                    break;

                case Enums.FunctionType.HookToggle:
                    radInput.Checked = true;
                    break;
            }

            txtNameCheat.Text = OffsetPatch.FunctionList[_index].CheatName;

            foreach (var offset in OffsetPatch.FunctionList[_index].OffsetList)
            {
                dataList.Rows.Add(offset.Name, offset.Offset, offset.Method.Item1, offset.Method.Item2);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var offsetList = new List<OffsetInfo>();

                if (Utility.IsEmpty(dataList)) return;
                if (Utility.IsEmpty(txtNameCheat)) return;

                for (var i = 0; i < dataList.Rows.Count; i++)
                {
                    var name = Utility.IsEmpty(dataList.Rows[i].Cells[0].Value, 0) ? "" : dataList.Rows[i].Cells[0].Value.ToString();
                    var offset = Utility.IsEmpty(dataList.Rows[i].Cells[1].Value, 1) ? "" : dataList.Rows[i].Cells[1].Value.ToString();
                    var type = Utility.IsEmpty(dataList.Rows[i].Cells[2].Value, 2) ? "" : dataList.Rows[i].Cells[2].Value.ToString();
                    var values = Utility.IsEmpty(dataList.Rows[i].Cells[3].Value, 3) ? "" : dataList.Rows[i].Cells[3].Value.ToString();

                    if (!offset.StartsWith("0x"))
                    {
                        MyMessage.MsgShowWarning(@$"Offset At {i + 1}, does not start with ""0x"" Please check it again!!!");
                        return;
                    }

                    if (Utility.IsEmpty(type, i + 1, "Type"))
                    {
                        return;
                    }

                    if (Utility.IsEmpty(values, i + 1, "Values"))
                    {
                        return;
                    }

                    if (radInput.Checked && !values.Contains("TFive"))
                    {
                        MyMessage.MsgShowWarning(@$"Values At {i + 1}, does not contains ""TFive"" for input value. Please check it again!!!");
                        return;
                    }

                    var offsetInfo = new OffsetInfo
                    {
                        OffsetId = i,
                        Offset = offset,
                        Hex = null,
                        HookInfo = OffsetPatch.HookValue(),
                        Name = name,
                        Method = (type, values)
                    };
                    offsetList.Add(offsetInfo);
                }

                var functionType = GetFunctionType();

                if (_index == 1150)
                {
                    OffsetPatch.OffsetList = offsetList;
                    OffsetPatch.AddFunction(txtNameCheat.Text, functionType);
                    OffsetPatch.OffsetList.Clear();
                }
                else
                {
                    var result = MyMessage.MsgYesNoCancel("Do you want to save?\n\n" +
                                                          "Click \"OK\" to confirm.\n\n" +
                                                          "Click \"Cancel\" to cancel.");
                    switch (result)
                    {
                        case DialogResult.Cancel:
                            return;
                        case DialogResult.No:
                            Values.Save = false;
                            Dispose();
                            _frmAddFunction.Dispose();
                            break;
                    }

                    OffsetPatch.FunctionList[_index] = new FunctionList
                    {
                        CheatName = txtNameCheat.Text,
                        FunctionType = functionType,
                        OffsetList = offsetList,
                        MultipleValue = false
                    };
                }

                Values.Save = true;
                Dispose();
                _frmAddFunction.Dispose();
            }
            catch (Exception exception)
            {
                MyMessage.MsgShowError("Error" + exception.Message);
            }
        }

        #region Data List

        private void dataList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveRows();
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

        #endregion Data List

        #region ToolStripMenuItem

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
            dataList.Rows.Add(null, null, null, null);
        }

        #endregion ToolStripMenuItem

        private void rad_CheckedChanged(object sender, EventArgs e)
        {
            _frmAddFunction.Text = HookName();
        }

        private string HookName()
        {
            return radButton.Checked ? "Hook: Button" :
                radInput.Checked ? "Hook: Input Button" :
                null;
        }

        private Enums.FunctionType GetFunctionType()
        {
            return radButton.Checked ? Enums.FunctionType.HookButton :
                radInput.Checked ? Enums.FunctionType.HookInputButton :
                Enums.FunctionType.Empty;
        }
    }
}