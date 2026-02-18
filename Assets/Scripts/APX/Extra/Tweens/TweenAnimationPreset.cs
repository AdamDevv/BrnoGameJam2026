using System.Collections.Generic;
using System.Linq;
using APX.Extra.Tweens.Segments;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Extra.Tweens
{
    public class TweenAnimationPreset : ATweenPreset
    {
        [SerializeField]
        [OnValueChanged("RefreshReferences", true)]
        private TweenSequence _Sequence;

        public Sequence CreateSequence()
        {
            return _Sequence?.CreateSequence();
        }

        public IEnumerable<ITweenSegment> GetSegments()
        {
            return _Sequence?.Segments ?? Enumerable.Empty<ITweenSegment>();
        }

#if UNITY_EDITOR
        public void Initialize(List<ITweenSegment> segments)
        {
            _Sequence = new TweenSequence(segments);
            RefreshReferences();
        }

        public override void PopulateReferences(HashSet<TweenObjectReference> references)
        {
            _Sequence?.PopulateReferences(references);
        }

        public override void PopulateTargets(TweenTargetCollection targets)
        {
            _Sequence?.PopulateTargets(targets);
        }
#endif
    }
}
