﻿namespace Tools_Injector_Mod_Menu.Patch_Manager
{
    public static class Enums
    {
        public enum TypeAbi
        {
            Arm,
            Arm64
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