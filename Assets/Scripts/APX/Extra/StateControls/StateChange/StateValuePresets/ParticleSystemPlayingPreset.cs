using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace APX.Extra.StateControls.StateChange.StateValuePresets
{
    [Preserve]
    [Serializable]
    public class ParticleSystemPlayingPreset : AStateValuePreset<ParticleSystem, bool>
    {
#if UNITY_EDITOR
        public override string FieldName => "Playing";
#endif

        [SerializeField]
        [Tooltip("Play all child ParticleSystems as well")]
        private bool _WithChildren = true;
        
        public override void ApplyTo(object targetObject)
        {
            ApplyTo(targetObject as ParticleSystem);
        }
        
        public override void ApplyTo(ParticleSystem targetObject)
        {
            if (targetObject != null)
            {
                if (PresetState)
                    targetObject.Play(_WithChildren);
                else
                    targetObject.Stop(_WithChildren);
            }
        }
    }
}
