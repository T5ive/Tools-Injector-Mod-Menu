using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tools_Injector_Mod_Menu.Patch_Manager;

namespace Tools_Injector_Mod_Menu
{
    public partial class CtrlHookInput : UserControl
    {
        private readonly int _index;
        private Enums.FunctionType _type;
        private readonly FrmAddFunction _frmAddFunction;

        public CtrlHookInput(FrmAddFunction frmAdd, int index = 1150)
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
            decimal min = 1, max;
            if (num.IsEmpty())
            {
                max = 0;
            }
            else
            {
                if (num.Contains("_"))
                {
                    var result = num.Split('_');
                    min = Convert.ToDecimal(result[0]);
                    max = Convert.ToDecimal(result[1]);
                }
                else
                {
                    max = Convert.ToDecimal(num);
                }
            }
            _type = OffsetPatch.FunctionList[_index].FunctionType;
            switch (_type)
            {
                case Enums.FunctionType.HookSeekBar:
                    radSeekBar.Checked = true;
                    numMin.Value = min;
                    break;

                case Enums.FunctionType.HookInputValue:
                    radInput.Checked = true;
                    break;

                case Enums.FunctionType.HookSeekBarToggle:
                    radSeekBarToggle.Checked = true;
                    numMin.Value = min;
                    break;

                case Enums.FunctionType.HookInputOnOff:
                    radInputOnOff.Checked = true;
                    break;
            }

            numMax.Value = max;
            txtNameCheat.Text = OffsetPatch.FunctionList[_index].CheatName;

            foreach (var offset in OffsetPatch.FunctionList[_index].OffsetList)
            {
                dataList.Rows.Add(offset.Name, offset.Offset, offset.HookInfo.Type.TypeToString(), offset.HookInfo.FieldInfo.Type.TypeToString(), offset.HookInfo.FieldInfo.Offset, offset.HookInfo.Links);
            }

            LoadDataList();
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
                    var offset = dataList.Rows[i].Cells[1].Value.IsEmpty() ? "" : dataList.Rows[i].Cells[1].Value.ToString();
                    var type = dataList.Rows[i].Cells[2].Value.IsEmpty() ? "" : dataList.Rows[i].Cells[2].Value.ToString();
                    var fieldType = dataList.Rows[i].Cells[3].Value.IsEmpty() ? "" : dataList.Rows[i].Cells[3].Value.ToString();
                    var fieldOffset = dataList.Rows[i].Cells[4].Value.IsEmpty() ? "" : dataList.Rows[i].Cells[4].Value.ToString();
                    var links = dataList.Rows[i].Cells[5].Value.IsEmpty() ? "" : dataList.Rows[i].Cells[5].Value.ToString();

                    FieldInfo fieldInfo;

                    if (!offset.StartsWith("0x"))
                    {
                        MyMessage.MsgShowWarning(@$"Offset At {i + 1}, does not start with ""0x"" Please check it again!!!");
                        return;
                    }

                    switch (type)
                    {
                        case "void" when fieldType.IsEmpty(i + 1, "Field Type"):
                        case "void" when fieldOffset.IsEmpty(i + 1, "Field Offset"):
                            return;

                        case "void" when !fieldOffset.StartsWith("0x"):
                            MyMessage.MsgShowWarning(@$"Field Offset At {i + 1}, does not start with ""0x"" Please check it again!!!");
                            return;

                        case "void":
                            fieldInfo = new FieldInfo
                            {
                                Type = fieldType.StringToType(),
                                Offset = fieldOffset
                            };
                            break;

                        case "links" when fieldType.IsEmpty(i + 1, "Field Type"):
                        case "links" when links.IsEmpty(i + 1, "Links"):
                            return;

                        case "links" when int.Parse(links) > dataList.RowCount || int.Parse(links) == i + 1:
                            MyMessage.MsgShowWarning($"At {i + 1}, Links is invalid. Please check it again!!!");
                            return;

                        case "links":
                            fieldInfo = new FieldInfo
                            {
                                Type = fieldType.StringToType(),
                                Offset = fieldOffset
                            };
                            break;

                        default:
                            fieldInfo = OffsetPatch.FieldValue();
                            break;
                    }

                    var hookInfo = new HookInfo
                    {
                        Type = type.StringToType(),
                        Value = null,
                        Links = links,
                        FieldInfo = fieldInfo
                    };

                    var offsetInfo = new OffsetInfo
                    {
                        OffsetId = i,
                        Offset = offset,
                        Hex = null,
                        HookInfo = hookInfo,
                        Name = name,
                        Method = new List<(string, string)> { (null, null) }
                    };
                    offsetList.Add(offsetInfo);
                }

                var functionType = GetFunctionType();

                var functionExtra = "";
                if (radSeekBarToggle.Checked || radSeekBar.Checked)
                {
                    functionExtra = $"{numMin.Value}_{numMax.Value}";
                }
                else if (radInputOnOff.Checked || radInput.Checked)
                {
                    if (numMax.Value != 0)
                    {
                        functionExtra = $"{numMax.Value}";
                    }
                }
                if (_index == 1150)
                {
                    if (txtNameCheat.Text.IsDuplicateName(OffsetPatch.FunctionList))
                    {
                        return;
                    }
                    OffsetPatch.OffsetList = offsetList;
                    OffsetPatch.AddFunction(txtNameCheat.Text, functionType, functionExtra, chkMultiple.Checked);
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
                        MultipleValue = chkMultiple.Checked
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

        private void LoadDataList()
        {
            for (var i = 0; i < dataList.RowCount; i++)
            {
                var typeValue = dataList.Rows[i].Cells[2].Value.IsEmpty() ? "" : dataList.Rows[i].Cells[2].Value.ToString();

                var fieldType = dataList.Rows[i].Cells[3];
                var fieldOffset = dataList.Rows[i].Cells[4];
                var links = dataList.Rows[i].Cells[5];

                if (typeValue == "void")
                {
                    fieldType.ReadOnly = false;
                    fieldOffset.ReadOnly = false;
                    fieldOffset.Style.BackColor = Color.White;
                }
                else
                {
                    fieldType.ReadOnly = true;
                    fieldOffset.Style.BackColor = Color.Silver;
                    fieldOffset.ReadOnly = true;

                    if (typeValue == "links")
                    {
                        fieldType.ReadOnly = false;
                        links.Style.BackColor = Color.White;
                        links.ReadOnly = false;
                    }
                    else
                    {
                        fieldType.ReadOnly = true;
                        links.Style.BackColor = Color.Silver;
                        links.ReadOnly = true;
                    }
                }
            }
        }

        private void dataList_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox cb)
            {
                cb.SelectedIndexChanged += SelectionChange;
            }
        }

        private void SelectionChange(object sender, EventArgs e)
        {
            try
            {
                var cb = (ComboBox)sender;
                if (dataList.CurrentRow == null) return;

                var type = dataList.CurrentRow.Cells[2];
                var fieldType = dataList.CurrentRow.Cells[3];
                var fieldOffset = dataList.CurrentRow.Cells[4];
                var links = dataList.CurrentRow.Cells[5];

                if (dataList.CurrentCell == type)
                {
                    if (cb.Text == "void")
                    {
                        fieldType.ReadOnly = false;
                        fieldOffset.ReadOnly = false;
                        fieldOffset.Style.BackColor = Color.White;
                    }
                    else
                    {
                        //fieldType.Value = "";
                        fieldType.ReadOnly = true;
                        //fieldOffset.Value = "";
                        fieldOffset.Style.BackColor = Color.Silver;
                        fieldOffset.ReadOnly = true;

                        if (cb.Text == "links")
                        {
                            fieldType.ReadOnly = false;
                            links.Style.BackColor = Color.White;
                            links.ReadOnly = false;
                        }
                        else
                        {
                            //fieldType.Value = "";
                            fieldType.ReadOnly = true;
                            //links.Value = "";
                            links.Style.BackColor = Color.Silver;
                            links.ReadOnly = true;
                        }
                    }
                }
                if (dataList.CurrentCell == fieldType && type.Value.ToString() == "void" && cb.Text == "bool")
                {
                    MyMessage.MsgShowWarning("Not allow Field Type bool on Type void");
                    cb.SelectedIndex = 3;
                }
            }
            catch
            {
            }
        }

        private void dataList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveRows();
            }

            if (e.KeyCode == Keys.V && e.Control)
            {
                if (dataList.GetCellCount(DataGridViewElementStates.Selected) > 0 && !dataList.CurrentCell.ReadOnly)
                {
                    dataList.CurrentCell.Value = Clipboard.GetText();
                }
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
            dataList.Rows.Add(null, null, "int", null, null, null, null);
        }

        #endregion ToolStripMenuItem

        private string HookName()
        {
            return radSeekBar.Checked ? "Hook: SeekBar" :
                radInput.Checked ? "Hook: Input Value" :
                radSeekBarToggle.Checked ? "Hook: SeekBar Toggle" :
                radInputOnOff.Checked ? "Hook: Input Value On/Off" :
                null;
        }

        private Enums.FunctionType GetFunctionType()
        {
            return radSeekBar.Checked ? Enums.FunctionType.HookSeekBar :
                radInput.Checked ? Enums.FunctionType.HookInputValue :
                radSeekBarToggle.Checked ? Enums.FunctionType.HookSeekBarToggle :
                radInputOnOff.Checked ? Enums.FunctionType.HookInputOnOff :
                Enums.FunctionType.Empty;
        }

        private void NumValueChanged(object sender, EventArgs e)
        {
            if (radInputOnOff.Checked || radInput.Checked) return;
            if (numMax.Value <= numMin.Value)
            {
                numMax.Value = numMin.Value + 1;
            }
        }

        private void rad_CheckedChanged(object sender, EventArgs e)
        {
            _frmAddFunction.Text = HookName();
            if (radSeekBarToggle.Checked || radSeekBar.Checked)
            {
                numMin.Enabled = true;
                numMax.Minimum = 2;
                numMax.Value = 100;
            }
            if (radInputOnOff.Checked || radInput.Checked)
            {
                numMin.Enabled = false;
                numMax.Minimum = 0;
                numMax.Value = 0;
            }
        }
    }
}