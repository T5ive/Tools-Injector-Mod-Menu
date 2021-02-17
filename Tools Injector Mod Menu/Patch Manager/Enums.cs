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

        public enum Type
        {
            Int,
            Long,
            Float,
            Double
        }
        public enum FunctionType
        {
            Toggle, //Patch
            ToggleSeekBar, //Hook Method
            ToggleInputValue, //Hook Method
            ButtonOnOff, //Patch
            ButtonOnOffSeekBar, //Hook Method
            ButtonOnOffInputValue, //Hook Method
            Button, //Hook Method
            ButtonSeekBar, //Hook Method
            ButtonInputValue, //Hook Method
            Patch, //Patch
            Category,
            Empty
        }
    }
}