using System.Collections.Generic;

namespace APX.Extra.Tweens
{
    public interface ITweenTargetProvider
    {
#if UNITY_EDITOR
        void PopulateReferences(HashSet<TweenObjectReference> references);
        void PopulateTargets(TweenTargetCollection targets);
#endif
    }
}
