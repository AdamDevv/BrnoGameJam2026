using System;
using UnityEngine;

namespace APX.Extra.Misc
{
    public static class GenericTypeUtils
    {
        public static  bool TryGetGenericMethodBundle<TBundle>(out TBundle resultBundle) where TBundle : class
        {
            resultBundle = default;
            var subtypes = ReflectionHelper.GetAssignableSubtypes(typeof(TBundle), false, new []{"Unity, System"});
            if (subtypes.Count == 0)
            {
                Debug.LogError($"No implementation of {typeof(TBundle).Name} found!");
                return false;
            }
            if (subtypes.Count > 1)
            {
                Debug.LogError($"There are multiple implementations of {typeof(TBundle).Name}, this is not allowed!");
                return false;
            }
            resultBundle = Activator.CreateInstance(subtypes[0]) as TBundle;
            return true;
        }
    }
}
