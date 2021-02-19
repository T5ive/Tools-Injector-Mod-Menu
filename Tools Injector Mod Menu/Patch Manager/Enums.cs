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
            Double,
            Empty
        }
        public enum FunctionType
        {
            Toggle, //Patch
            ToggleHook, // Hook Method
            ToggleSeekBar, //Hook Method
            ToggleInputValue, //Hook Method
            ButtonOnOff, //Patch
            ButtonOnOffHook, // Hook Method
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