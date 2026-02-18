using System.Collections.Generic;
using UnityEngine;

namespace APX.Extra.Tweens
{
    public interface ITweenReferencesHolder
    {
        public IDictionary<TweenObjectReference, Object> GetReferencesDictionary();
    }
}
