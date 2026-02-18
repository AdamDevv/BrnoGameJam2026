using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace APX.Extra.Misc
{
    [Serializable]
    [InlineProperty]
    [JsonObject(MemberSerialization.OptIn)]
    public class ToggleableField<T>
    {
        [FormerlySerializedAs("isToggled")]
        [SerializeField]
        [HorizontalGroup(Width = 20)]
        [HideLabel]
        [JsonProperty("HasValue")]
        private bool _IsToggled;

        [FormerlySerializedAs("field")]
        [SerializeField]
        [HorizontalGroup]
        [HideLabel]
        [JsonProperty("Value")]
        private T _Field;
        
        public bool IsToggled
        {
            get => _IsToggled; 
            set => _IsToggled = value;
        }
        
        public T Field
        {
            get => _Field; 
            set => _Field = value;
        }

        public static implicit operator T(ToggleableField<T> value) => value._Field;
        public static implicit operator ToggleableField<T>(T value) => new(value);

        public ToggleableField() { }

        public ToggleableField(T field, bool isToggled = false)
        {
            _Field = field;
            _IsToggled = isToggled;
        }
    }
}
