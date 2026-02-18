using System;
using System.Collections.Generic;
using APX.Extra.Tweens.Segments;
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace APX.Extra.Tweens
{
    [Serializable]
    public class TweenAnimationField : ATweenAnimationField
    {
        [PropertySpace]
        [PropertyOrder(10)]
        [SerializeField]
        private TweenObjectReferenceValueSet _References;

        [FormerlySerializedAs("_Sequence")]
        [PropertyOrder(10)]
        [InlineProperty]
        [SerializeReference]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(ShowFoldout = false, OnTitleBarGUI = "OnSequenceTitleBarGUI")]
        [ValueDropdown("GetAllSequenceParts", DrawDropdownForListElements = false)]
        protected List<ITweenSegment> _Segments = new List<ITweenSegment>();

        protected override void PopulateSequence(Sequence sequence)
        {
            _References.AssignTempReferences();
            foreach (var animPart in _Segments)
            {
                animPart.AddToSequence(sequence);
            }
            _References.ClearTempReferences();
        }

        public override IDictionary<TweenObjectReference, Object> GetReferencesDictionary()
        {
            return _References?.GetReferencesDictionary();
        }

#if UNITY_EDITOR
        public override void PopulateReferences(HashSet<TweenObjectReference> references)
        {
            foreach (var segment in _Segments) segment?.PopulateReferences(references);
        }

        public override void PopulateTargets(TweenTargetCollection targets)
        {
            foreach (var segment in _Segments) segment?.PopulateTargets(targets);
        }

        [UsedImplicitly]
        private void OnSequenceTitleBarGUI(InspectorProperty property)
        {
            TweenEditorUtils.DrawSavePresetToolbarButton(_Segments, property);
        }

        [UsedImplicitly]
        private IEnumerable<ValueDropdownItem<ITweenSegment>> GetAllSequenceParts(InspectorProperty property)
        {
            return TweenEditorUtils.GetAllSequenceSegments(property);
        }
#endif
    }
}
