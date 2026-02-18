using System;
using APX.Extra.EditorUtils;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace APX.Extra.OdinExtensions.Editor.Drawers
{
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    public class ShowCreateNewDrawerForScriptableObject<T> : ShowCreateNewDrawerBase<T> where T : ScriptableObject
    {
        protected override bool TryCreateInstance(Type type, string defaultPath, string defaultName, out T result)
        {
            var path = defaultPath;
            if (string.IsNullOrEmpty(path)) 
                path = SelectionUtils.GetSelectedPathOrFallback();

            result = OdinExtensionUtils.CreateNewInstanceOfType<T>(type, path, defaultName);
            return result != null;
        }

        protected override Type GetBaseType() => base.GetBaseType() ?? ValueEntry.BaseValueType;
    }
}
