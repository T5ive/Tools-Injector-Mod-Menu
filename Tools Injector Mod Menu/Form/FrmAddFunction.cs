using System;
using System.Windows.Forms;
using Tools_Injector_Mod_Menu.Patch_Manager;

namespace Tools_Injector_Mod_Menu
{
    public partial class FrmAddFunction : Form
    {
        private readonly int _index;

        public FrmAddFunction(Enums.FunctionType type = Enums.FunctionType.Empty, int index = 1150)
        {
            InitializeComponent();
            _index = index;
            LoadType(type);
            if (type != Enums.FunctionType.Empty)
            {
                comboFunction.Enabled = false;
            }
        }

        private void LoadType(Enums.FunctionType type)
        {
            switch (type)
            {
                case Enums.FunctionType.Empty:
                case Enums.FunctionType.Category:
                    LoadUserControl(new CtrlCategory(this, _index));
                    break;

                case Enums.FunctionType.HookButton:
                case Enums.FunctionType.HookInputButton:
                    comboFunction.SelectedIndex = 1;
                    break;

                case Enums.FunctionType.HookButtonOnOf:
                case Enums.FunctionType.HookToggle:
                    comboFunction.SelectedIndex = 2;
                    break;

                case Enums.FunctionType.HookInputOnOff:
                case Enums.FunctionType.HookInputValue:
                case Enums.FunctionType.HookSeekBar:
                case Enums.FunctionType.HookSeekBarToggle:
                    comboFunction.SelectedIndex = 3;
                    break;

                case Enums.FunctionType.PatchButtonOnOff:
                case Enums.FunctionType.PatchLabel:
                case Enums.FunctionType.PatchToggle:
                    comboFunction.SelectedIndex = 4;
                    break;

                default:
                    comboFunction.SelectedIndex = 0;
                    break;
            }
        }

        private void comboFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboFunction.SelectedIndex)
            {
                case 0:
                    {
                        LoadUserControl(new CtrlCategory(this, _index));
                        break;
                    }
                case 1:
                    {
                        LoadUserControl(new CtrlHookButton(this, _index));
                        break;
                    }
                case 2:
                    {
                        LoadUserControl(new CtrlHook(this, _index));
                        break;
                    }
                case 3:
                    {
                        LoadUserControl(new CtrlHookInput(this, _index));
                        break;
                    }
                case 4:
                    {
                        LoadUserControl(new CtrlPatch(this, _index));
                        break;
                    }
            }
        }

        private void LoadUserControl(Control ctrl)
        {
            panelFunction.Controls.Clear();
            panelFunction.Controls.Add(ctrl);
        }
    }
}