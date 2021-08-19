using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tools_Injector_Mod_Menu.Patch_Manager;

namespace Tools_Injector_Mod_Menu
{
    public partial class CtrlPatch : UserControl
    {
        private readonly int _index;
        private Enums.FunctionType _type;
        private readonly FrmAddFunction _frmAddFunction;

        public CtrlPatch(FrmAddFunction frmAdd, int index = 1150)
        {
            InitializeComponent();
            _index = index;
            _frmAddFunction = frmAdd;
            _frmAddFunction.Text = PatchName();
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
                case Enums.FunctionType.PatchButtonOnOff:
                    radButtonOnOff.Checked = true;
                    break;

                case Enums.FunctionType.PatchLabel:
                    radLabel.Checked = true;
                    break;

                case Enums.FunctionType.PatchToggle:
                    radToggle.Checked = true;
                    break;
            }

            txtNameCheat.Text = OffsetPatch.FunctionList[_index].CheatName;

            foreach (var offset in OffsetPatch.FunctionList[_index].OffsetList)
            {
                dataList.Rows.Add(offset.Name, offset.Offset, offset.Hex);
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
                    var name = Utility.IsEmpty(dataList.Rows[i].Cells[0].Value) ? "" : dataList.Rows[i].Cells[0].Value.ToString();
                    var offset = Utility.IsEmpty(dataList.Rows[i].Cells[1].Value) ? "" : dataList.Rows[i].Cells[1].Value.ToString();
                    var hex = Utility.IsEmpty(dataList.Rows[i].Cells[2].Value) ? "" : dataList.Rows[i].Cells[2].Value.ToString();

                    if (Utility.IsEmpty(offset, i + 1, "Offset"))
                    {
                        return;
                    }

                    if (Utility.IsEmpty(hex, i + 1, "Hex"))
                    {
                        return;
                    }

                    if (!offset.StartsWith("0x"))
                    {
                        MyMessage.MsgShowWarning(@$"Offset At {i + 1}, does not start with ""0x"" Please check it again!!!");
                        return;
                    }

                    hex = Utility.InsertSpaces(dataList.Rows[i].Cells[2].Value.ToString());

                    var offsetInfo = new OffsetInfo
                    {
                        OffsetId = i,
                        Offset = offset,
                        Hex = hex,
                        HookInfo = OffsetPatch.HookValue(),
                        Name = name,
                        Method = new List<(string, string)> { (null, null) }
                    };
                    offsetList.Add(offsetInfo);
                }
                var functionType = GetFunctionType();
                if (_index == 1150)
                {
                    if (Utility.IsDuplicateName(txtNameCheat.Text, OffsetPatch.FunctionList))
                    {
                        return;
                    }
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
                        FunctionExtra = null,
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
            dataList.Rows.Add(null, null, null);
        }

        #endregion ToolStripMenuItem

        private void rad_CheckedChanged(object sender, EventArgs e)
        {
            _frmAddFunction.Text = PatchName();
        }

        private string PatchName()
        {
            return radButtonOnOff.Checked ? "Patch: Button On/Off" :
                radLabel.Checked ? "Patch: Label" :
                radToggle.Checked ? "Patch: Toggle" :
                null;
        }

        private Enums.FunctionType GetFunctionType()
        {
            return radButtonOnOff.Checked ? Enums.FunctionType.PatchButtonOnOff :
                radLabel.Checked ? Enums.FunctionType.PatchLabel :
                radToggle.Checked ? Enums.FunctionType.PatchToggle :
                Enums.FunctionType.Empty;
        }
    }
}