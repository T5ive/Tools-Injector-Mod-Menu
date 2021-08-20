using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tools_Injector_Mod_Menu.Patch_Manager;

namespace Tools_Injector_Mod_Menu
{
    public partial class CtrlHook : UserControl
    {
        private readonly int _index;
        private Enums.FunctionType _type;
        private readonly FrmAddFunction _frmAddFunction;

        public CtrlHook(FrmAddFunction frmAdd, int index = 1150)
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
                    radButtonOnOff.Checked = true;
                    break;

                case Enums.FunctionType.HookToggle:
                    radToggle.Checked = true;
                    break;
            }

            txtNameCheat.Text = OffsetPatch.FunctionList[_index].CheatName;

            foreach (var offset in OffsetPatch.FunctionList[_index].OffsetList)
            {
                dataList.Rows.Add(offset.Name, offset.Offset, offset.HookInfo.Type.TypeToString(), offset.HookInfo.FieldInfo.Type.TypeToString(), offset.HookInfo.FieldInfo.Offset, offset.HookInfo.Value, offset.HookInfo.Links);
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
                    var value = dataList.Rows[i].Cells[5].Value.IsEmpty() ? "" : dataList.Rows[i].Cells[5].Value.ToString();
                    var links = dataList.Rows[i].Cells[6].Value.IsEmpty() ? "" : dataList.Rows[i].Cells[6].Value.ToString();

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

                        case "void" when fieldType != "bool" && value.IsEmpty(i + 1, "Value"):
                            return;

                        case "void":
                            fieldInfo = new FieldInfo
                            {
                                Type = fieldType.StringToType(),
                                Offset = fieldOffset
                            };
                            break;

                        case "links" when fieldType.IsEmpty(i + 1, "Field Type"):
                        case "links" when fieldType != "bool" && value.IsEmpty(i + 1, "Value"):
                        case "links" when links.IsEmpty(i + 1, "Links"):
                            return;

                        case "links" when int.Parse(links) > dataList.RowCount || int.Parse(links) == i + 1:
                            MyMessage.MsgShowWarning($"Links At {i + 1}, is invalid. Please check it again!!!");
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

                    if (type != "bool" && type != "void" && type != "links" && value.IsEmpty(i + 1, "Value"))
                    {
                        return;
                    }

                    var hookInfo = new HookInfo
                    {
                        Type = type.StringToType(),
                        Value = value,
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

                if (_index == 1150)
                {
                    if (txtNameCheat.Text.IsDuplicateName(OffsetPatch.FunctionList))
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

        private void LoadDataList()
        {
            for (var i = 0; i < dataList.RowCount; i++)
            {
                var typeValue = dataList.Rows[i].Cells[2].Value.IsEmpty() ? "" : dataList.Rows[i].Cells[2].Value.ToString();
                var fieldTypeValue = dataList.Rows[i].Cells[3].Value.IsEmpty() ? "" : dataList.Rows[i].Cells[3].Value.ToString();

                var fieldType = dataList.Rows[i].Cells[3];
                var fieldOffset = dataList.Rows[i].Cells[4];
                var value = dataList.Rows[i].Cells[5];
                var links = dataList.Rows[i].Cells[6];

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

                if (typeValue == "bool" || fieldTypeValue == "bool")
                {
                    value.Value = "";
                    value.Style.BackColor = Color.Silver;
                    value.ReadOnly = true;
                }
                else
                {
                    value.Style.BackColor = Color.White;
                    value.ReadOnly = false;
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
                var value = dataList.CurrentRow.Cells[5];
                var links = dataList.CurrentRow.Cells[6];

                if (dataList.CurrentCell == type)
                {
                    if (cb.Text == "bool")
                    {
                        //value.Value = "";
                        value.Style.BackColor = Color.Silver;
                        value.ReadOnly = true;
                    }
                    else
                    {
                        value.Style.BackColor = Color.White;
                        value.ReadOnly = false;

                        if (cb.Text == "void")
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

                            if (cb.Text == "links")
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

                if (dataList.CurrentCell == fieldType && type.Value.ToString() == "links" ||
                    type.Value.ToString() == "void")
                {
                    if (cb.Text == "bool")
                    {
                        value.Value = "";
                        value.Style.BackColor = Color.Silver;
                        value.ReadOnly = true;
                    }
                    else
                    {
                        value.Style.BackColor = Color.White;
                        value.ReadOnly = false;
                    }
                }
            }
            catch
            {
                //
            }
        }

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
            dataList.Rows.Add(null, null, "bool", null, null, null, null);
        }

        #endregion ToolStripMenuItem

        private void rad_CheckedChanged(object sender, EventArgs e)
        {
            _frmAddFunction.Text = HookName();
        }

        private string HookName()
        {
            return radButtonOnOff.Checked ? "Hook: Button On/Off" :
                radToggle.Checked ? "Hook: Toggle" :
                null;
        }

        private Enums.FunctionType GetFunctionType()
        {
            return radButtonOnOff.Checked ? Enums.FunctionType.HookButtonOnOf :
                radToggle.Checked ? Enums.FunctionType.HookToggle :
                Enums.FunctionType.Empty;
        }
    }
}