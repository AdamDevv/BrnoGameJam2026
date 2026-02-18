using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace APX.Extra.Tweens.Segments
{
    [Serializable]
    public class SequenceTweenSegment : ABaseTweenSegment, IDirectTweenSegment, IPresetTweenSegment, IParentTweenSegment
    {
        [InlineProperty]
        [PropertyOrder(20)]
        [SerializeReference]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(ShowFoldout = false, OnTitleBarGUI = "OnSequenceTitleBarGUI")]
        [ValueDropdown("GetAllSequenceParts", DrawDropdownForListElements = false)]
        private List<ITweenSegment> _Sequence = new List<ITweenSegment>();
        
        public override void AddToSequence(Sequence sequence)
        {
            var subSequence = DOTween.Sequence();
            foreach (var animPart in _Sequence)
            {
                animPart.AddToSequence(subSequence);
            }
            AddToSequence(sequence, subSequence);
        }

        public SequenceTweenSegment() { }
        public SequenceTweenSegment(List<ITweenSegment> sequence)
        {
            _Sequence = sequence;
        }

        public SequenceTweenSegment(List<ITweenSegment> sequence, TweenAdditionType operation, float insertionPosition = 0) : base(operation, insertionPosition)
        {
            _Sequence = sequence;
        }

        public IEnumerable<ITweenSegment> GetChildSegments()
        {
            return _Sequence ?? Enumerable.Empty<ITweenSegment>();
        }

#if UNITY_EDITOR

        public bool TryConvertToDirectSegment(out IDirectTweenSegment result, IDictionary<TweenObjectReference, Object> targets)
        {
            var convertedSequence = new List<ITweenSegment>();
            foreach (var segment in _Sequence)
            {
                if (segment is not IPresetTweenSegment presetSegment || !presetSegment.TryConvertToDirectSegment(out var convertedSegment, targets)) continue;
                convertedSequence.Add(convertedSegment);
            }
            result = new SequenceTweenSegment(convertedSequence);
            return true;
        }

        public bool TryConvertToPresetSegment(out IPresetTweenSegment result, IDictionary<Object, TweenObjectReference> references)
        {
            var convertedSequence = new List<ITweenSegment>();
            foreach (var segment in _Sequence)
            {
                if (segment is not IDirectTweenSegment directSegment || !directSegment.TryConvertToPresetSegment(out var convertedSegment, references)) continue;
                convertedSequence.Add(convertedSegment);
            }
            result = new SequenceTweenSegment(convertedSequence);
            return true;
        }

        public override string GetSummary()
        {
            return $"{GetSummaryPrefix()} Sequence";
        }

        public override string LabelName => "Sequence";

        [UsedImplicitly]
        private void OnSequenceTitleBarGUI(InspectorProperty property)
        {
            TweenEditorUtils.DrawSavePresetToolbarButton(_Sequence, property);
        }

        [UsedImplicitly]
        private IEnumerable<ValueDropdownItem<ITweenSegment>> GetAllSequenceParts(InspectorProperty property)
        {
            return TweenEditorUtils.GetAllSequenceSegments(property);
        }

        public override void PopulateReferences(HashSet<TweenObjectReference> references)
        {
            base.PopulateReferences(references);
            foreach (var segment in _Sequence) segment?.PopulateReferences(references);
        }

        public override void PopulateTargets(TweenTargetCollection targets)
        {
            base.PopulateTargets(targets);
            foreach (var segment in _Sequence) segment?.PopulateTargets(targets);
        }
#endif
    }
}
