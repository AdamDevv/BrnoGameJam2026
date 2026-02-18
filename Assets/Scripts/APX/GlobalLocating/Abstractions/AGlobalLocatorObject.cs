using APX.ObjectLocating;
using UnityEngine;

namespace APX.GlobalLocating.Abstractions
{
    public abstract class AGlobalLocatorObject : ScriptableObject { }

    public abstract class AGlobalLocatorObject<T> : AGlobalLocatorObject where T : AGlobalLocatorObject<T>
    {
        public static T Instance => GlobalLocator.Get<T>();
    }
}
