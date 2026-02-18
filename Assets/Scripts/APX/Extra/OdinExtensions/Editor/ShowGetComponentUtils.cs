using System;
using System.Linq;
using APX.Extra.OdinExtensions.Attributes;
using UnityEngine;

namespace APX.Extra.OdinExtensions.Editor
{
    public static class ShowGetComponentUtils
    {
        public static bool TrySearchComponent<TComponent>(Component parent, GetComponentTargetPlace targetPlace, out TComponent result) where TComponent : Component
        {
            foreach (var targetPlaceFlag in Enum.GetValues(typeof(GetComponentTargetPlace)).Cast<GetComponentTargetPlace>())
            {
                if (!targetPlace.HasFlag(targetPlaceFlag)) 
                    continue;
                
                var newValue = targetPlaceFlag switch
                {
                    GetComponentTargetPlace.Local => parent.GetComponent<TComponent>(),
                    GetComponentTargetPlace.Parent => parent.GetComponentInParent<TComponent>(),
                    GetComponentTargetPlace.Children => parent.GetComponentInChildren<TComponent>(),
                    GetComponentTargetPlace.Anywhere => UnityEngine.Object.FindAnyObjectByType<TComponent>(),
                    _ => throw new ArgumentOutOfRangeException()
                };
                if (newValue == null) 
                    continue;
                
                result = newValue;
                return newValue != null;
            }

            result = null;
            return false;
        }
    }
}
