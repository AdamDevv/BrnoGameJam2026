using System.Collections.Generic;
using System.Linq;
using APX.Extra.EditorUtils;
using APX.Extra.OdinExtensions;
using APX.Extra.OdinExtensions.Attributes;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Validation;
#endif

namespace APX.GlobalLocating.Abstractions
{
    public abstract class AGlobalLocatorDefinitionsProvider<TDefinition, TProvider> : AGlobalLocatorObject<TProvider>
        where TDefinition : ScriptableObject
        where TProvider : AGlobalLocatorDefinitionsProvider<TDefinition, TProvider>
    {
        [SerializeField]
        [EnhancedValidate("ValidateDefinitionsEditorOnly", IncludeChildren = true)]
        [ListDrawerSettings(ShowFoldout = false)]
        private List<TDefinition> _Definitions = new();

        [SerializeField]
        private bool _RequireAllDefinitions = true;

        public static IReadOnlyList<TDefinition> Definitions => Instance._Definitions;

        #if UNITY_EDITOR

        [UsedImplicitly]
        protected virtual void ValidateDefinitionsEditorOnly(List<TDefinition> definitions, ValidationResult result, InspectorProperty property)
        {
            if (definitions is null)
                return;

            if (_RequireAllDefinitions)
            {
                var allAssets = EditorAssetUtils.FindAllAssetsOfType<TDefinition>();
                var missingAssets = allAssets.Except(definitions).ToArray();
                if (missingAssets.Length > 0)
                {
                    result.AddError($"There are {missingAssets.Length} missing definitions!")
                        .WithFix(() =>
                        {
                            _Definitions.Clear();
                            _Definitions.AddRange(allAssets);

                            property.ForceMarkDirty();
                        });
                }
            }
        }

        #endif
    }
}
