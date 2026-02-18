using UnityEngine;

namespace APX.ObjectLocating.Util
{
    internal class GlobalLocatorConstants
    {
        public static readonly GUIStyle OBJECT_TYPE_LABEL_GUI_STYLE = new()
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };

        public const string OBJECT_TYPE_LABEL_UNKNOWN = "<color=white>?</color>";
        public const string OBJECT_TYPE_LABEL_PROVIDER = "<color=green>P</color>";
        public const string OBJECT_TYPE_LABEL_SETTINGS = "<color=yellow>S</color>";
    }
}
