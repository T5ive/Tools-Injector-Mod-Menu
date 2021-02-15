namespace Tools_Injector_Mod_Menu.Patch_Manager
{
    public static class Enums
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
            ButtonOnOffSeekBar,
            ButtonOnOffInputValue,
            Patch,
            Category,
            Empty
        }
    }
}