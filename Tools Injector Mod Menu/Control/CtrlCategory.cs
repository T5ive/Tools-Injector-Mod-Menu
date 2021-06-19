using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tools_Injector_Mod_Menu.Patch_Manager;

namespace Tools_Injector_Mod_Menu
{
    public partial class CtrlCategory : UserControl
    {
        private readonly int _index;
        private readonly FrmAddFunction _frmAddFunction;

        public CtrlCategory(FrmAddFunction frmAdd, int index)
        {
            InitializeComponent();
            _index = index;
            _frmAddFunction = frmAdd;
            _frmAddFunction.Text = "Category";
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
            txtCagtegory.Text = OffsetPatch.FunctionList[_index].CheatName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            const Enums.FunctionType functionType = Enums.FunctionType.Category;
            try
            {
                if (Utility.IsEmpty(txtNameCheat)) return;
                if (Utility.IsEmpty(txtCagtegory)) return;

                if (_index == 1150)
                {
                    OffsetPatch.AddFunction(txtNameCheat.Text, functionType, txtCagtegory.Text);
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
                        OffsetList = new List<OffsetInfo>(),
                        FunctionExtra = txtCagtegory.Text,
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
    }
}