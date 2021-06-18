using System.Collections.Generic;
using System.Xml.Serialization;

namespace Tools_Injector_Mod_Menu.Patch_Manager
{
    [XmlRoot(ElementName = "OffsetInfo")]
    public class OffsetInfo
    {
        [XmlElement(ElementName = "OffsetId")] public int OffsetId { get; set; }

        [XmlElement(ElementName = "Offset")] public string Offset { get; set; }

        [XmlElement(ElementName = "Hex")] public string Hex { get; set; }

        [XmlElement(ElementName = "HookInfo")] public HookInfo HookInfo { get; set; }

        [XmlElement(ElementName = "Name")] public string Name { get; set; }

        [XmlElement(ElementName = "Method")] public (string, string) Method { get; set; }
    }

    public class HookInfo
    {
        [XmlElement(ElementName = "Type")] public Enums.Type Type { get; set; }
        
        [XmlElement(ElementName = "Value")] public string Value { get; set; }

        [XmlElement(ElementName = "Links")] public string Links { get; set; }

        [XmlElement(ElementName = "FieldInfo")] public FieldInfo FieldInfo { get; set; }
    }

    public class FieldInfo
    {
        [XmlElement(ElementName = "Field")] public bool Field { get; set; }

        [XmlElement(ElementName = "Type")] public Enums.Type Type { get; set; }

        [XmlElement(ElementName = "Offset")] public string Offset { get; set; }
    }

    public class TFiveMenu
    {
        [XmlElement(ElementName = "GameName")] public string GameName { get; set; }

        [XmlElement(ElementName = "TypeAbi")] public Enums.TypeAbi TypeAbi { get; set; }

        [XmlElement(ElementName = "Target")] public string Target { get; set; }

        [XmlElement] public List<FunctionList> FunctionList { get; set; }
    }

    [XmlRoot(ElementName = "FunctionList")]
    public class FunctionList
    {
        [XmlElement(ElementName = "OffsetList")] public List<OffsetInfo> OffsetList { get; set; }

        [XmlElement(ElementName = "CheatName")] public string CheatName { get; set; }

        [XmlElement(ElementName = "FunctionType")] public Enums.FunctionType FunctionType { get; set; }
        
        [XmlElement(ElementName = "MultipleValue")] public bool MultipleValue { get; set; }
    }
}