using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Tools_Injector_Mod_Menu.Patch_Manager
{
    public static class ModMenuPattern
    {
        private static readonly List<FunctionList> FUNCTION_LIST = OffsetPatch.FunctionList;

        public static string MemoryPatch()
        {
            const string result = "MemoryPatch ";
            var patchValues = "";
            foreach (var function in FUNCTION_LIST)
            {
                var cheatName = function.CheatName.RemoveSuperSpecialCharacters().ReplaceNumCharacters();
                foreach (var offsetInfo in function.OffsetList)
                {
                    switch (function.FunctionType)
                    {
                        case Enums.FunctionType.PatchButtonOnOff:
                        case Enums.FunctionType.PatchToggle:
                            patchValues += $"{cheatName}_{offsetInfo.OffsetId}, ";
                            break;
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(patchValues))
            {
                return result + patchValues.Remove(patchValues.Length - 2) + ";";
            }

            return result + ";";
        }

        public static string NewVariable()
        {
            var result = "";
            var newLine = Environment.NewLine;
            foreach (var function in FUNCTION_LIST)
            {
                var cheatName = function.CheatName.RemoveSuperSpecialCharacters().ReplaceNumCharacters();
                switch (function.FunctionType)
                {
                    case Enums.FunctionType.PatchButtonOnOff:
                    case Enums.FunctionType.PatchToggle:
                        result += $"bool _{cheatName};{newLine}";
                        break;

                    case Enums.FunctionType.HookToggle:
                    case Enums.FunctionType.HookButtonOnOf:
                        result += $"bool _{cheatName};{newLine}";
                        result = function.OffsetList.Where(offsetInfo => offsetInfo.HookInfo.Type == Enums.Type.Links).Aggregate(result, (current, offsetInfo) => current + $"{Utility.TypeToString(offsetInfo.HookInfo.FieldInfo.Type)} _{cheatName}{offsetInfo.OffsetId}{Utility.TypeToStringEnd(offsetInfo.HookInfo.FieldInfo.Type)}{newLine}");

                        break;

                    case Enums.FunctionType.HookSeekBar:
                    case Enums.FunctionType.HookInputValue:
                        result += $"int _{cheatName}Value = 1;{newLine}";
                        result = function.OffsetList.Where(offsetInfo => offsetInfo.HookInfo.Type == Enums.Type.Links).Aggregate(result, (current, info) => current + $"{Utility.TypeToString(info.HookInfo.FieldInfo.Type)} _{cheatName}{info.OffsetId}{Utility.TypeToStringEnd(info.HookInfo.FieldInfo.Type)}{newLine}");
                        break;

                    case Enums.FunctionType.HookSeekBarToggle:
                    case Enums.FunctionType.HookInputOnOff:
                        result += $"bool _{cheatName};{newLine}";
                        result += $"int _{cheatName}Value = 1;{newLine}";
                        result = function.OffsetList.Where(offsetInfo => offsetInfo.HookInfo.Type == Enums.Type.Links).Aggregate(result, (current, info) => current + $"{Utility.TypeToString(info.HookInfo.FieldInfo.Type)} _{cheatName}{info.OffsetId}{Utility.TypeToStringEnd(info.HookInfo.FieldInfo.Type)}{newLine}");
                        break;

                    case Enums.FunctionType.HookButton:
                        result += $"void *btn{cheatName};{newLine}";
                        break;

                    case Enums.FunctionType.HookInputButton:
                        result += $"void *btn{cheatName};{newLine}";
                        for (var i = 0; i < function.OffsetList.Count; i++)
                        {
                            var offsetInfo = function.OffsetList[i];
                            var buttonType = GetInputValues(offsetInfo.Method);
                            if (buttonType == 1)
                            {
                                result += $"int _{cheatName}Value{i} = 1;{newLine}";
                            }
                        }

                        break;
                }
            }

            return result.Length == 0 ? result : result.Remove(result.LastIndexOf(newLine, StringComparison.Ordinal));
        }

        public static string NewMethod()
        {
            var result = "";
            var btnResult = "";
            var newLine = Environment.NewLine;
            foreach (var function in FUNCTION_LIST)
            {
                var cheatName = function.CheatName.RemoveSuperSpecialCharacters().ReplaceNumCharacters();
                var links = 0;
                foreach (var info in function.OffsetList.Where(info => info.HookInfo.Type == Enums.Type.Links))
                {
                    links = int.Parse(info.HookInfo.Links);
                }

                foreach (var offsetInfo in function.OffsetList)
                {
                    var type = offsetInfo.HookInfo.Type;
                    var typeString = Utility.TypeToString(type);

                    var typeField = offsetInfo.HookInfo.FieldInfo.Type;
                    var typeFieldString = Utility.TypeToString(typeField);

                    var hookValue = offsetInfo.HookInfo.Value;

                    var resultField = "";
                    var fieldOffset = offsetInfo.HookInfo.FieldInfo.Offset;

                    var resultToggle = "";
                    var resultMultiple = "";

                    var linkResult = "";
                    var id = offsetInfo.OffsetId + 1;
                    if (id == links)
                    {
                        linkResult += $"&& _{cheatName}{id} ";
                    }

                    switch (function.FunctionType)
                    {
                        case Enums.FunctionType.HookToggle:
                        case Enums.FunctionType.HookButtonOnOf:
                            {
                                switch (type)
                                {
                                    case Enums.Type.Void:
                                        {
                                            if (offsetInfo.HookInfo.FieldInfo.Offset.Contains(","))
                                            {
                                                foreach (var offset in fieldOffset.Split(','))
                                                {
                                                    offset.RemoveSuperSpecialCharacters();
                                                    resultField += offsetInfo.HookInfo.FieldInfo.Type == Enums.Type.Bool
                                                        ? $"*({typeFieldString} *) ((uint64_t) instance + {offset}) = _{cheatName};{newLine}        "
                                                        : $"*({typeFieldString} *) ((uint64_t) instance + {offset}) = {hookValue};{newLine}        ";
                                                }
                                            }
                                            else
                                            {
                                                resultField += offsetInfo.HookInfo.FieldInfo.Type == Enums.Type.Bool
                                                    ? $"*({typeFieldString} *) ((uint64_t) instance + {fieldOffset}) = _{cheatName};"
                                                    : $"*({typeFieldString} *) ((uint64_t) instance + {fieldOffset}) = {hookValue};";
                                            }

                                            result += $@"void (*old_{cheatName}{id})(void *instance);
void Update{cheatName}{id}(void *instance) {{
    if (instance != NULL && _{cheatName} {linkResult}) {{
        {resultField}
    }}
    return old_{cheatName}{id}(instance);
}}{newLine}
";
                                            break;
                                        }

                                    case Enums.Type.Bool:
                                        result += $@"{typeString} (*old_{cheatName}{id})(void *instance);
{typeString} Update{cheatName}{id}(void *instance) {{
    if (instance != NULL && _{cheatName} {linkResult}) {{
        return _{cheatName};
    }}
    return old_{cheatName}{id}(instance);
}}{newLine}
";
                                        break;

                                    case Enums.Type.Double:
                                    case Enums.Type.Float:
                                    case Enums.Type.Int:
                                    case Enums.Type.Long:
                                        result += $@"{typeString} (*old_{cheatName}{id})(void *instance);
{typeString} Update{cheatName}{id}(void *instance) {{
    if (instance != NULL && _{cheatName} {linkResult}) {{
        return ({typeString}) {hookValue};
    }}
    return old_{cheatName}{id}(instance);
}}{newLine}
";
                                        break;
                                }

                                break;
                            }
                        case Enums.FunctionType.HookSeekBar:
                        case Enums.FunctionType.HookInputValue:
                        case Enums.FunctionType.HookSeekBarToggle:
                        case Enums.FunctionType.HookInputOnOff:
                            {
                                switch (function.FunctionType)
                                {
                                    case Enums.FunctionType.HookSeekBarToggle:
                                    case Enums.FunctionType.HookInputOnOff:
                                        resultToggle += $"&& _{cheatName} ";
                                        break;
                                }

                                switch (type)
                                {
                                    case Enums.Type.Void:
                                        {
                                            if (offsetInfo.HookInfo.FieldInfo.Offset.Contains(","))
                                            {
                                                foreach (var offset in fieldOffset.Split(','))
                                                {
                                                    offset.RemoveSuperSpecialCharacters();
                                                    resultMultiple += function.MultipleValue
                                                        ? $"*({typeFieldString} *) ((uint64_t) instance + {offset}) = _{cheatName}Value*old_{cheatName}(instance);{newLine}        "
                                                        : $"*({typeFieldString} *) ((uint64_t) instance + {offset}) = _{cheatName}Value;{newLine}        ";
                                                }
                                            }
                                            else
                                            {
                                                resultMultiple += function.MultipleValue
                                                ? $"*({typeFieldString} *) ((uint64_t) instance + {fieldOffset}) = _{cheatName}Value*old_{cheatName}(instance);"
                                                : $"*({typeFieldString} *) ((uint64_t) instance + {fieldOffset}) = _{cheatName}Value;";
                                            }
                                            result += $@"void (*old_{cheatName}{id})(void *instance);
void Update{cheatName}{id}(void *instance) {{
    if (instance != NULL && _{cheatName}Value > 1 {resultToggle}{linkResult}) {{
        {resultMultiple}
    }}
    return old_{cheatName}{id}(instance);
}}{newLine}
";
                                            break;
                                        }
                                    case Enums.Type.Double:
                                    case Enums.Type.Float:
                                    case Enums.Type.Int:
                                    case Enums.Type.Long:
                                        {
                                            resultMultiple += function.MultipleValue
                                                ? $"return ({typeString}) _{cheatName}Value*old_{cheatName}{id}(instance);"
                                                : $"return ({typeString}) _{cheatName}Value;";

                                            result += $@"{typeString} (*old_{cheatName}{id})(void *instance);
{typeString} Update{cheatName}{id}(void *instance) {{
    if (instance != NULL && _{cheatName}Value > 1 {resultToggle}{linkResult}) {{
        {resultMultiple}
    }}
    return old_{cheatName}{id}(instance);
}}{newLine}
";
                                            break;
                                        }
                                }
                                break;
                            }
                        case Enums.FunctionType.HookButton:
                        case Enums.FunctionType.HookInputButton:
                            {
                                var args = GetArgs(offsetInfo.Method);
                                result += $"void (*{cheatName}Method{id})(void *instance{args});{newLine}{newLine}";
                                btnResult += $"btn{cheatName} = instance;{newLine}    ";
                                break;
                            }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(btnResult))
            {
                result += $@"void (*old_Update)(void *instance);
void Update(void *instance) {{
    {btnResult.Remove(btnResult.LastIndexOf(newLine, StringComparison.Ordinal))}
    return old_Update(instance);
}}
";
            }

            return result.Length == 0 ? result : result.Remove(result.LastIndexOf(newLine, StringComparison.Ordinal));
        }

        public static string HackThread()
        {
            var result = "";
            result += Patch();
            result += Hook();
            return result;
        }

        private static string Patch()
        {
            var result = "";
            var newLine = Environment.NewLine;
            foreach (var function in FUNCTION_LIST)
            {
                var cheatName = function.CheatName.RemoveSuperSpecialCharacters().ReplaceNumCharacters();
                foreach (var offsetInfo in function.OffsetList)
                {
                    switch (function.FunctionType)
                    {
                        case Enums.FunctionType.PatchButtonOnOff:
                        case Enums.FunctionType.PatchToggle:
                            {
                                result += $@"hexPatches.{cheatName}_{offsetInfo.OffsetId} = MemoryPatch::createWithHex(targetLibName,
                                            string2Offset(OBFUSCATE(""{offsetInfo.Offset}"")),
                                            OBFUSCATE(""{offsetInfo.Hex}""));{newLine}    ";
                                break;
                            }
                        case Enums.FunctionType.PatchLabel:
                            {
                                result += $@"PATCHOFFSET(""{offsetInfo.Offset}"", ""{offsetInfo.Hex}"");{newLine}    ";
                                break;
                            }
                    }
                }
            }

            return result;
        }

        private static string Hook()
        {
            var result = "";
            var newLine = Environment.NewLine;
            var btnCount = 0;
            var hookBtn = "";
            foreach (var function in FUNCTION_LIST)
            {
                var cheatName = function.CheatName.RemoveSuperSpecialCharacters().ReplaceNumCharacters();
                foreach (var offsetInfo in function.OffsetList)
                {
                    var id = offsetInfo.OffsetId + 1;
                    switch (function.FunctionType)
                    {
                        case Enums.FunctionType.HookToggle:
                        case Enums.FunctionType.HookButtonOnOf:
                        case Enums.FunctionType.HookSeekBar:
                        case Enums.FunctionType.HookInputValue:
                        case Enums.FunctionType.HookSeekBarToggle:
                        case Enums.FunctionType.HookInputOnOff:
                            {
                                if (offsetInfo.HookInfo.Type == Enums.Type.Links) break;
                                result += $@"HOOK(""{offsetInfo.Offset}"", Update{cheatName}{id}, old_{cheatName}{id});{newLine}    ";
                                break;
                            }
                        case Enums.FunctionType.HookButton:
                        case Enums.FunctionType.HookInputButton:
                            {
                                var args = GetTypeArgs(offsetInfo.Method);
                                result += $"{cheatName}Method{id} = (void(*)(void *{args}))getAbsoluteAddress(targetLibName, {offsetInfo.HookInfo.FieldInfo.Offset});{newLine}    ";
                                if (!hookBtn.Contains(offsetInfo.Offset))
                                {
                                    hookBtn +=
                                        $@"HOOK(""{offsetInfo.Offset}"", Update, old_Update);{newLine}    ";
                                    btnCount++;
                                }
                                break;
                            }
                    }
                }
            }

            if (btnCount > 0)
            {
                result += hookBtn;
            }
            return result;
        }

        public static string ToastHere(ListBox listBox)
        {
            return listBox.Items.Cast<object>().Aggregate("", (current, value) => current + ($@"MakeToast(env, context, OBFUSCATE(""{value}""), Toast::LENGTH_LONG);" + Environment.NewLine + "    "));
        }

        public static string FeaturesList()
        {
            var result = "";
            var realCount = 0;
            foreach (var function in FUNCTION_LIST)
            {
                var num = function.FunctionType == Enums.FunctionType.Category ? "0" : (realCount + 1).ToString();
                var type = function.FunctionType;
                var cheatName = function.CheatName;
                var functionExtra = function.FunctionExtra;
                if (functionExtra == "0" || string.IsNullOrWhiteSpace(functionExtra))
                    functionExtra = "";
                else
                    functionExtra = $"_{functionExtra}";
                var featureType = Utility.FunctionTypeToStringFeatures(type);
                switch (type)
                {
                    case Enums.FunctionType.Category:
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_{featureType}{functionExtra}""),";
                        break;

                    case Enums.FunctionType.HookButton:
                    case Enums.FunctionType.HookButtonOnOf:
                    case Enums.FunctionType.HookToggle:
                    case Enums.FunctionType.PatchButtonOnOff:
                    case Enums.FunctionType.PatchLabel:
                    case Enums.FunctionType.PatchToggle:
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_{featureType}_{cheatName}""),";
                        break;

                    case Enums.FunctionType.HookSeekBar:
                    case Enums.FunctionType.HookSeekBarToggle:
                    case Enums.FunctionType.HookInputOnOff:
                    case Enums.FunctionType.HookInputValue:
                    case Enums.FunctionType.HookInputButton:
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_{featureType}_{cheatName}{functionExtra}""),";
                        break;
                }

                if (type != Enums.FunctionType.Category)
                {
                    realCount++;
                }
            }
            return result;
        }

        public static string NewFeatures()
        {
            var result = "";
            var realCount = 0;
            var newLine = Environment.NewLine;
            foreach (var function in FUNCTION_LIST)
            {
                var num = function.FunctionType == Enums.FunctionType.Category ? "0" : (realCount + 1).ToString();
                var type = function.FunctionType;
                var cheatName = function.CheatName.RemoveSuperSpecialCharacters().ReplaceNumCharacters();
                switch (type)
                {
                    case Enums.FunctionType.PatchButtonOnOff:
                    case Enums.FunctionType.PatchToggle:
                        {
                            var offsetListModify = function.OffsetList.Aggregate("", (current, info) => current + $@"hexPatches.{cheatName}_{info.OffsetId}.Modify();
                                ");

                            var offsetListRestore = function.OffsetList.Aggregate("", (current, info) => current + $@"hexPatches.{cheatName}_{info.OffsetId}.Restore();
                                ");

                            result += $@"
                        case {num}:
                            _{cheatName} = boolean;
                            if (_{cheatName}) {{
                                {offsetListModify.Remove(offsetListModify.LastIndexOf(newLine, StringComparison.Ordinal))}
                            }} else {{
                                {offsetListRestore.Remove(offsetListRestore.LastIndexOf(newLine, StringComparison.Ordinal))}
                            }}
                            break;{Environment.NewLine}";
                            break;
                        }
                    case Enums.FunctionType.HookButtonOnOf:
                    case Enums.FunctionType.HookToggle:
                        {
                            result += $@"
                        case {num}:
                            _{cheatName} = boolean;
                            break;{Environment.NewLine}";
                            break;
                        }
                    case Enums.FunctionType.HookSeekBarToggle:
                    case Enums.FunctionType.HookInputOnOff:
                        {
                            result += $@"
                        case {num}:
                            _{cheatName} = boolean;
                            if (value >= 1) {{
                                _{cheatName}Value = value;
                            }}
                            break;{Environment.NewLine}";
                            break;
                        }
                    case Enums.FunctionType.HookSeekBar:
                    case Enums.FunctionType.HookInputValue:
                        {
                            result += $@"
                        case {num}:
                            if (value >= 1) {{
                                _{cheatName}Value = value;
                            }}
                            break;{Environment.NewLine}";
                            break;
                        }
                    case Enums.FunctionType.HookButton:
                    case Enums.FunctionType.HookInputButton:
                        {
                            var args = function.OffsetList.Aggregate("", (current, info) => current + GetValuesArgs(info.Method));
                            var buttonValue = function.OffsetList.Aggregate("", (current, offsetInfo) => current + $"{cheatName}Method{offsetInfo.OffsetId + 1}(btn{cheatName}{args});{newLine}                               ");
                            var inputValue = "";
                            if (type == Enums.FunctionType.HookInputButton)
                            {
                                inputValue += "&& value >= 1";
                            }
                            result += $@"
                        case {num}:
                            if (btn{cheatName} != NULL {inputValue}) {{
                               {buttonValue.Remove(buttonValue.LastIndexOf(newLine, StringComparison.Ordinal))}
                            }}
                            break;{Environment.NewLine}";
                            break;
                        }
                }

                if (type != Enums.FunctionType.Category)
                {
                    realCount++;
                }
            }
            return result;
        }

        private static string GetArgs(IReadOnlyList<(string, string)> method)
        {
            var result = "";
            if (method[0].Item1?.Length == 0 && method[0].Item2?.Length == 0) return "";
            for (var i = 0; i < method.Count; i++)
            {
                result += $", {method[i].Item1} _{method[i].Item1}{i}";
            }
            return result;
        }

        private static string GetTypeArgs(IReadOnlyList<(string, string)> method)
        {
            var result = "";
            if (method[0].Item1?.Length == 0 && method[0].Item2?.Length == 0) return "";
            for (var i = 0; i < method.Count; i++)
            {
                result += $", {method[i].Item1}";
            }
            return result;
        }

        private static string GetValuesArgs(IReadOnlyList<(string, string)> method)
        {
            var result = "";
            if (method[0].Item1?.Length == 0 && method[0].Item2?.Length == 0) return "";
            for (var i = 0; i < method.Count; i++)
            {
                result += $", {method[i].Item2}";
            }
            return result;
        }

        private static int GetInputValues(IReadOnlyList<(string, string)> method)
        {
            for (var i = 0; i < method.Count; i++)
            {
                var value = method[i].Item2;
                if (value == "value")
                {
                    return 1;
                }
            }
            return 0;
        }
    }
}