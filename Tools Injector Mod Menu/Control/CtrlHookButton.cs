using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            var num = OffsetPatch.FunctionList[_index].FunctionExtra;
            _type = OffsetPatch.FunctionList[_index].FunctionType;
            switch (_type)
            {
                case Enums.FunctionType.HookButton:
                    radButton.Checked = true;
                    break;

                case Enums.FunctionType.HookInputButton:
                    numMax.Value = Convert.ToDecimal(num);
                    radInput.Checked = true;
                    break;
            }

            txtNameCheat.Text = OffsetPatch.FunctionList[_index].CheatName;

            foreach (var offset in OffsetPatch.FunctionList[_index].OffsetList)
            {
                var type = "";
                var values = "";
                for (var i = 0; i < offset.Method.Count; i++)
                {
                    type += $"{offset.Method[i].Item1}, ";
                    values += $"{offset.Method[i].Item2}, ";
                }
                type = type.Remove(type.Length - 2);
                values = values.Remove(values.Length - 2);
                dataList.Rows.Add(offset.Name, offset.Offset, offset.HookInfo.FieldInfo.Offset, type, values);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var offsetList = new List<OffsetInfo>();

                if (dataList.IsEmpty()) return;
                if (txtNameCheat.IsEmpty("Name Cheat")) return;

                for (var i = 0; i < dataList.Rows.Count; i++)
                {
                    var name = dataList.Rows[i].Cells[0].Value.IsEmpty() ? "" : dataList.Rows[i].Cells[0].Value.ToString();
                    var updateOffset = dataList.Rows[i].Cells[1].Value.IsEmpty() ? "" : dataList.Rows[i].Cells[1].Value.ToString();
                    var offset = dataList.Rows[i].Cells[2].Value.IsEmpty() ? "" : dataList.Rows[i].Cells[2].Value.ToString();
                    var type = dataList.Rows[i].Cells[3].Value.IsEmpty() ? "" : dataList.Rows[i].Cells[3].Value.ToString();
                    var values = dataList.Rows[i].Cells[4].Value.IsEmpty() ? "" : dataList.Rows[i].Cells[4].Value.ToString();

                    if (!offset.StartsWith("0x") || !updateOffset.StartsWith("0x"))
                    {
                        MyMessage.MsgShowWarning(@$"Offset At {i + 1}, does not start with ""0x"" Please check it again!!!");
                        return;
                    }

                    if (!type.IsEmpty(i + 1, "Type", false) && values.IsEmpty(i + 1, "Values"))
                    {
                        return;
                    }

                    if (radInput.Checked && !values.Contains("value"))
                    {
                        MyMessage.MsgShowWarning(@$"Values At {i + 1}, does not contains ""value"" for input value. Please check it again!!!");
                        return;
                    }

                    var typeList = type.RemoveMiniSpecialCharacters().Split(',');
                    var valueList = values.RemoveMiniSpecialCharacters().Split(',');
                    if (typeList.Length != valueList.Length)
                    {
                        MyMessage.MsgShowWarning($"Type At {i + 1}, does not equal Values At {i + 1}. Please check it again!!!");
                        return;
                    }

                    var method = typeList.Select((t, j) => (t, valueList[j])).ToList();

                    var fieldInfo = new FieldInfo
                    {
                        Type = Enums.Type.Empty,
                        Offset = offset
                    };
                    var hookInfo = new HookInfo
                    {
                        Type = Enums.Type.Empty,
                        Value = null,
                        Links = null,
                        FieldInfo = fieldInfo
                    };

                    var offsetInfo = new OffsetInfo
                    {
                        OffsetId = i,
                        Offset = updateOffset,
                        Hex = null,
                        HookInfo = hookInfo,
                        Name = name,
                        Method = method
                    };
                    offsetList.Add(offsetInfo);
                }

                var functionType = GetFunctionType();
                var functionExtra = numMax.Value.ToString(CultureInfo.InvariantCulture);
                if (_index == 1150)
                {
                    if (txtNameCheat.Text.IsDuplicateName(OffsetPatch.FunctionList))
                    {
                        return;
                    }
                    OffsetPatch.OffsetList = offsetList;
                    OffsetPatch.AddFunction(txtNameCheat.Text, functionType, functionExtra);
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
                        FunctionExtra = functionExtra,
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
            numMax.Enabled = radInput.Checked;
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