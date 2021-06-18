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
            Bool,
            Double,
            Float,
            Int,
            Long,
            Void,
            Links,
            Vector3,
            Empty
        }

        public enum FunctionType
        {
            Category,
            HookButton,
            HookButtonOnOf,
            HookInputButton,
            HookInputOnOff,
            HookInputValue,
            HookSeekBar,
            HookSeekBarToggle,
            HookToggle,
            PatchButtonOnOff,
            PatchLabel,
            PatchToggle,
            Empty
        }
    }
}