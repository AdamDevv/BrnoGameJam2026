using System.Collections.Generic;
using System.Linq;
using APX.Audio.Models.Data;
using APX.Audio.Utils;
using APX.Extra.OdinExtensions;
using APX.Extra.OdinExtensions.Attributes;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using APX.Events.Editor;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Validation;
#endif

namespace APX.Audio.Models.Definitions
{
    public class EventSFXDefinition : ScriptableObject
    {
        private enum SFXType
        {
            Local,
            Referenced
        }

        [SerializeField]
        [EnumToggleButtons]
        [HideLabel]
        [BoxGroup("SFX Settings")]
        private SFXType _SFXType = SFXType.Local;

        [Required]
        [ShowIf(nameof(_SFXType), SFXType.Referenced, animate: false)]
        [AssetSelector]
        [SerializeField]
        [BoxGroup("SFX Settings")]
        [InlineIconButton(EditorIconsBundle.FontAwesome, "StopSolid", "StopPreview")]
        [InlineIconButton(EditorIconsBundle.FontAwesome, "PlaySolid", "PlayPreview")]
        private SFXDefinition _ReferencedSFX;

        [Required]
        [ShowIf(nameof(_SFXType), SFXType.Local, animate: false)]
        [HideLabel]
        [SerializeField]
        [BoxGroup("SFX Settings")]
        private SFXModel _LocalSFX;

        [SerializeReference]
        [PropertySpace]
        [EnhancedValueDropdown("GetEvents")]
        [Required]
        [EnhancedValidate("ValidateEvents")]
        public object[] _Events = { };

        [PropertyOrder(999)]
        [PropertySpace]
        [SerializeField]
        private bool _Disabled;

        public SFXModel SFXModel => _SFXType == SFXType.Local ? _LocalSFX : _ReferencedSFX.SFXModel;
        public object[] Events => _Events;
        public bool Disabled => _Disabled;

        #if UNITY_EDITOR

        [UsedImplicitly]
        private IEnumerable<ValueDropdownItem> GetEvents() => EventUtilsEditor.GetEventsValueDropdown();

        [UsedImplicitly]
        private void ValidateEvents(object[] value, ValidationResult result, InspectorProperty property)
        {
            if (value is null)
            {
                return;
            }

            var allTypes = _Events.Where(e => e is not null).ToArray();
            var uniqueTypes = allTypes.Select(e => e.GetType()).Distinct().ToArray();

            if (allTypes.Length != uniqueTypes.Length)
            {
                result.AddError("List contains duplicate elements")
                    .WithFix(() =>
                    {
                        _Events = uniqueTypes.Select(t => _Events.Where(e => e is not null).First(e => e.GetType() == t)).ToArray();
                    });
            }
        }

        [UsedImplicitly]
        private void PlayPreview()
        {
            EditorAudioUtils.PlayPreview(_ReferencedSFX?.SFXModel.GetSFXInstance());
        }

        [UsedImplicitly]
        private void StopPreview()
        {
            EditorAudioUtils.StopPreview();
        }

        #endif
    }
}
