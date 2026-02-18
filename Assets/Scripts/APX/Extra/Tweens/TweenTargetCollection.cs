using System;
using System.Collections.Generic;

namespace APX.Extra.Tweens
{
    public class TweenTargetCollection
    {
        public Dictionary<UnityEngine.Object, Type> Targets { get; } = new();

        public void Add(UnityEngine.Object target, Type targetType)
        {
            if (Targets.TryGetValue(target, out var currentType))
            {

            }
            Targets[target] = targetType;
        }
    }
}
