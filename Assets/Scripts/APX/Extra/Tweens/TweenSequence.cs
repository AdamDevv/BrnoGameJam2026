using System;
using System.Collections.Generic;
using APX.Extra.Tweens.Segments;
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace APX.Extra.Tweens
{
    [Serializable]
    [InlineProperty]
    [HideLabel]
    public class TweenSequence : ITweenTargetProvider
    {
        [FormerlySerializedAs("_Sequence")]
        [InlineProperty]
        [SerializeReference]
        [HideReferenceObjectPicker]
        [LabelText("@$property.Parent.NiceName")]
        [ListDrawerSettings(OnTitleBarGUI = "OnSequenceTitleBarGUI")]
        [ValueDropdown("GetAllSequenceParts", DrawDropdownForListElements = false)]
        protected List<ITweenSegment> _Segments = new List<ITweenSegment>();

        public IEnumerable<ITweenSegment> Segments => _Segments;

        public virtual Sequence CreateSequence()
        {
            var sequence = DOTween.Sequence();
            foreach (var animPart in _Segments)
            {
                animPart?.AddToSequence(sequence);
            }
            return sequence;
        }

        public TweenSequence() { }
        public TweenSequence(List<ITweenSegment> segments)
        {
            _Segments = segments;
        }

#if UNITY_EDITOR
        public void PopulateReferences(HashSet<TweenObjectReference> references)
        {
            foreach (var segment in _Segments)
            {
                segment?.PopulateReferences(references);
            }
        }

        public void PopulateTargets(TweenTargetCollection targets)
        {
            foreach (var segment in _Segments)
            {
                segment?.PopulateTargets(targets);
            }
        }

        [UsedImplicitly]
        protected void OnSequenceTitleBarGUI(InspectorProperty property)
        {
            TweenEditorUtils.DrawSavePresetToolbarButton(_Segments, property);
        }

        [UsedImplicitly]
        protected IEnumerable<ValueDropdownItem<ITweenSegment>> GetAllSequenceParts(InspectorProperty property)
        {
            return TweenEditorUtils.GetAllSequenceSegments(property);
        }
#endif
    }
}
