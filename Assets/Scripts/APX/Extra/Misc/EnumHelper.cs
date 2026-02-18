using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.Misc
{
    public static class EnumHelper
    {
#if UNITY_EDITOR
        internal static string EnumNameFromEnumField(FieldInfo field)
        {
            return field.IsDefined(typeof(ObsoleteAttribute), false) ? $"{(object) ObjectNames.NicifyVariableName(field.Name)} (Obsolete)" : ObjectNames.NicifyVariableName(field.Name);
        }

        internal static string EnumTooltipFromEnumField(FieldInfo field)
        {
            object[] customAttributes = field.GetCustomAttributes(typeof(TooltipAttribute), false);
            return customAttributes.Length > 0 ? ((TooltipAttribute) customAttributes.First()).tooltip : string.Empty;
        }


        public struct EnumData
        {
            public Enum[] Values;
            public int[] FlagValues;
            public string[] DisplayNames;
            public string[] Tooltip;
            public Type UnderlyingType;
            public bool Unsigned;
        }

        public static EnumData GetCachedEnumData(Type enumType)
        {
            EnumData enumData2 = new EnumData {UnderlyingType = Enum.GetUnderlyingType(enumType)};
            enumData2.Unsigned = enumData2.UnderlyingType == typeof(byte)
                                 || enumData2.UnderlyingType == typeof(ushort)
                                 || enumData2.UnderlyingType == typeof(uint)
                                 || enumData2.UnderlyingType == typeof(ulong);
            List<FieldInfo> list = enumType.GetFields(BindingFlags.Static | BindingFlags.Public)
                .OrderBy(f => f.MetadataToken).ToList();
            enumData2.DisplayNames = list.Select(f => EnumNameFromEnumField(f)).ToArray();
            enumData2.Tooltip = list.Select(f => EnumTooltipFromEnumField(f)).ToArray();
            enumData2.Values = list.Select(f => (Enum) Enum.Parse(enumType, f.Name)).ToArray();
            enumData2.FlagValues = !enumData2.Unsigned
                ? enumData2.Values.Select(v => (int) Convert.ToInt64(v)).ToArray()
                : enumData2.Values.Select(v => (int) Convert.ToUInt64(v)).ToArray();
            if (enumData2.UnderlyingType == typeof(ushort))
            {
                int index = 0;
                for (int length = enumData2.FlagValues.Length; index < length; ++index)
                {
                    if (enumData2.FlagValues[index] == ushort.MaxValue)
                        enumData2.FlagValues[index] = -1;
                }
            }
            else if (enumData2.UnderlyingType == typeof(byte))
            {
                int index = 0;
                for (int length = enumData2.FlagValues.Length; index < length; ++index)
                {
                    if (enumData2.FlagValues[index] == byte.MaxValue)
                        enumData2.FlagValues[index] = -1;
                }
            }

            return enumData2;
        }
        
        public static string GetEnumValueLabelText<T>(T value)
            where T: Enum
        {
            var enumType = typeof(T);
            var valueAsString = value.ToString();
            MemberInfo[] members = enumType.GetMember(valueAsString);
            if (!members.TryGetFirst(m => m.DeclaringType == enumType, out var member))
            {
                return valueAsString;
            }
            
            var attributes = member.GetCustomAttributes(typeof(LabelTextAttribute), false);
            if (attributes.Length < 1)
                return valueAsString;

            var attribute = (LabelTextAttribute) attributes[0];
            return attribute.Text;
        }
#endif

        public static bool IsOneOf<T>(this T value, params T[] comparisons)
            where T: Enum
        {
            foreach (var comp in comparisons)
            {
                if (Equals(value, comp))
                    return true;
            }

            return false;
        }
        
        public static bool IsOneOf<T>(this T value, T v1)
            where T: Enum
        {
            return Equals(value, v1);
        }
        
        public static bool IsOneOf<T>(this T value, T v1, T v2)
            where T: Enum
        {
            return Equals(value, v1) || Equals(value, v2);
        }
        
        public static bool IsOneOf<T>(this T value, T v1, T v2, T v3)
            where T: Enum
        {
            return Equals(value, v1) || Equals(value, v2) || Equals(value, v3);
        }
        
        public static bool IsOneOf<T>(this T value, T v1, T v2, T v3, T v4)
            where T: Enum
        {
            return Equals(value, v1) || Equals(value, v2) || Equals(value, v3) || Equals(value, v4);
        }

        public static T Next<T>(this T src) where T : Enum
        {
            var values = (T[])Enum.GetValues(src.GetType());
            var index = (Array.IndexOf(values, src) + 1).PositiveModulo(values.Length);
            return values[index];
        }
        
        public static T Prev<T>(this T src) where T : Enum
        {
            var values = (T[])Enum.GetValues(src.GetType());
            var index = (Array.IndexOf(values, src) - 1).PositiveModulo(values.Length);
            return values[index];
        }

    }
}

