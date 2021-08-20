namespace Tools_Injector_Mod_Menu.Patch_Manager
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
            Bool = 0,
            Double = 100,
            Float = 101,
            Int = 102,
            Long = 103,
            Void = 300,
            Links = 301,
            Empty = 1000
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
            Empty = 100
        }

        public enum LogsType
        {
            CompileMenu,
            Success,
            Warning,
            Error,
            Logs,
            Dump,
            Decompile,
            CompileApk,
        }

        public enum ProcessType
        {
            CompileApk,
            MenuFull,
            ApkFull1Decompile,
            ApkFull1,
            ApkFull2Decompile,
            ApkFull2,
            None = 100
        }
    }
}