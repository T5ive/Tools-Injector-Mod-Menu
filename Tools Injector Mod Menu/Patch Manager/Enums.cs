namespace Tools_Injector_Mod_Menu.Patch_Manager
{
    public class Enums
    {
        public enum TypeAbi
        {
            Arm,
            Arm64,
            X86,
            All
        }

        public enum FunctionType
        {
            Toggle,
            ButtonOnOff,
            ToggleSeekBar,
            ToggleInputValue,
            ButtonSeekBar,
            ButtonInputValue,
            InstanceSeekBar,
            InstanceInputValue,
            Category,
            Empty
        }
    }
}