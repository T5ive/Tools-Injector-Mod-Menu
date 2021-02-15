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

        [XmlElement(ElementName = "FunctionValue")] public string FunctionValue { get; set; }

        [XmlElement(ElementName = "MultipleValue")] public bool MultipleValue { get; set; }
    }
}