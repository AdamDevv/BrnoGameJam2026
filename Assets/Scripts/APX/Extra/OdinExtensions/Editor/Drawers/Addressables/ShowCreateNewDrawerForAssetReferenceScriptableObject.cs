using System;
using APX.Extra.EditorUtils;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace APX.Extra.OdinExtensions.Editor.Drawers.Addressables
{
    [DrawerPriority(0, 200, 0)]
    public class ShowCreateNewDrawerForAssetReferenceScriptableObject: ShowCreateNewDrawerBase<AssetReferenceT<ScriptableObject>>
    {
        protected override bool TryCreateInstance(Type type, string defaultPath, string defaultName, out AssetReferenceT<ScriptableObject> result)
        {
            var path = defaultPath;
            if (string.IsNullOrEmpty(path)) 
                path = SelectionUtils.GetSelectedPathOrFallback();

            var instance = OdinExtensionUtils.CreateNewInstanceOfType(type, path, defaultName);
            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(instance, out var guid, out long _))
            {
                result = new AssetReferenceT<ScriptableObject>(guid);
                return true;
            }
            
            result = default;
            return false;
        }
        
        protected override Type GetBaseType() => base.GetBaseType() ?? ValueEntry.BaseValueType;
    }
}
