using System;
using System.Diagnostics;
using Sirenix.OdinInspector;

namespace APX.Extra.OdinExtensions.Attributes
{
    [DontApplyToListElements]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    [Conditional("UNITY_EDITOR")]
    public class EnhancedLabelTextAttribute : LabelTextAttribute
    {
        public string Texture;

        public EnhancedLabelTextAttribute(string text) : base(text)
        {
        }

        public EnhancedLabelTextAttribute(string text, bool nicifyText) : base(text, nicifyText)
        {
        }
        
        public EnhancedLabelTextAttribute(string text, string texture) : base(text)
        {
            Texture = texture;
        }
        public EnhancedLabelTextAttribute(string text, bool nicifyText, string texture) : base(text, nicifyText)
        {
            Texture = texture;
        }
    }
}
