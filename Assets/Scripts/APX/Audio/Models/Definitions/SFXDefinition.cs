using APX.Audio.Models.Data;
using UnityEngine;

namespace APX.Audio.Models.Definitions
{
    public class SFXDefinition : ScriptableObject
    {
        [SerializeField]
        private SFXModel _Model;

        public SFXModel SFXModel => _Model;
    }
}
