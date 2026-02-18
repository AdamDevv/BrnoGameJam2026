using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting;

namespace APX.Extra.StateControls.StateChange
{
    [Serializable, Preserve]
    public abstract class AStateValuePreset
    {
        public abstract void ApplyTo(object targetObject);

#if UNITY_EDITOR
        public abstract string FieldName { get; }
#endif

        public abstract Type TargetType { get; }
        public abstract bool IsValidFor(object targetObject);
        public abstract bool IsValidForType(Type targetType);

        public AStateValuePreset GetCopy()
        {
            return JsonUtility.FromJson(JsonUtility.ToJson(this), GetType()) as AStateValuePreset;
        }
    }

    [Serializable, Preserve]
    public abstract class AStateValuePreset<T> : AStateValuePreset
    {
        public abstract void ApplyTo(T targetObject);

        public override Type TargetType => typeof(T);
        public override bool IsValidFor(object targetObject) { return targetObject is T; }
        public override bool IsValidForType(Type targetType) { return typeof(T).IsAssignableFrom(targetType); }
    }

    [Serializable, Preserve]
    public abstract class AStateValuePreset<T, U> : AStateValuePreset<T>
    {
        [SerializeField, LabelText("$FieldName")]
        [InfoBox("$InfoText", VisibleIf = "@this.InfoText != null")]
        private U _presetState;
        
        public U PresetState
        {
            get => _presetState; 
            set => _presetState = value;
        }

#if UNITY_EDITOR
        public virtual string InfoText { get; }
#endif

        protected AStateValuePreset() { }

        protected AStateValuePreset(U presetState) { _presetState = presetState; }
    }
}
