#if UNITY_EDITOR

using UnityEngine;

namespace APX.Extra.GameManagement
{
    public interface ISelectorManagementPanel
    {
        bool CanSelect(ScriptableObject element);
        void Select(ScriptableObject element);
    }
}

#endif