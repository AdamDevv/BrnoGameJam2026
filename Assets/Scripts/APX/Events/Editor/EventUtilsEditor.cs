#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using APX.Util;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;

namespace APX.Events.Editor
{
    public static class EventUtilsEditor
    {
        public static IEnumerable<ValueDropdownItem> GetEventsValueDropdown()
        {
            const string assembly = "APGame";
            const string namespacePrefix = "APGame.Events";

            return TypeCache.GetTypesDerivedFrom<object>(assembly)
                .Where(t => t.Namespace?.StartsWith(namespacePrefix) == true)
                .Select(e => new ValueDropdownItem(e.GetNiceName(), e.CreateDefaultValue()));
        }
    }
}

#endif
