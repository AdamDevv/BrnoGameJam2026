using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace APX.Extra.Misc
{
    public static class TypeExtensions
    {
        public static string GetFriendlyName(this Type type)
        {
            var friendlyName = type.Name;
            if (type.IsArray)
            {
                return GetFriendlyName(type.GetElementType()) + "[]";
            }

            if (type.IsGenericType)
            {
                var stringBuilder = new StringBuilder(friendlyName, 60);
                var iBacktick = friendlyName.IndexOf('`');
                if (iBacktick > 0)
                {
                    stringBuilder.Length = iBacktick;
                }

                stringBuilder.Append('<');
                var typeParameters = type.GetGenericArguments();
                for (var i = 0; i < typeParameters.Length; ++i)
                {
                    var typeParamName = GetFriendlyName(typeParameters[i]);
                    if (i > 0) stringBuilder.Append(',');
                    stringBuilder.Append(typeParamName);
                }

                stringBuilder.Append('>');
                return stringBuilder.ToString();
            }

            return friendlyName;
        }

        public static bool ExplicitlyDefinesAttribute(this Type type, Type attributeType)
        {
            if (type == null || attributeType == null) return false;
            return type.CustomAttributes.Any(a => a.AttributeType == attributeType);
        }
        
        public static FieldInfo[] GetFieldInfosIncludingBaseClasses(this Type type, BindingFlags bindingFlags)
        {
            var fieldInfos = type.GetFields(bindingFlags);

            // If this class doesn't have a base, don't waste any time
            if (type.BaseType == typeof(object))
            {
                return fieldInfos;
            }

            // Otherwise, collect all types up to the furthest base class
            var currentType = type;
            var fieldComparer = new MemberInfoComparer();
            var fieldInfoList = new HashSet<FieldInfo>(fieldInfos, fieldComparer);
            while (currentType != typeof(object))
            {
                fieldInfos = currentType.GetFields(bindingFlags);
                fieldInfoList.UnionWith(fieldInfos);
                currentType = currentType.BaseType;
            }
            return fieldInfoList.ToArray();
        }
        
        public static PropertyInfo[] GetPropertyInfosIncludingBaseClasses(this Type type, BindingFlags bindingFlags)
        {
            var propertyInfos = type.GetProperties(bindingFlags);

            // If this class doesn't have a base, don't waste any time
            if (type.BaseType == typeof(object))
            {
                return propertyInfos;
            }

            // Otherwise, collect all types up to the furthest base class
            var currentType = type;
            var memberComparer = new MemberInfoComparer();
            var infoList = new HashSet<PropertyInfo>(propertyInfos, memberComparer);
            while (currentType != typeof(object))
            {
                propertyInfos = currentType.GetProperties(bindingFlags);
                infoList.UnionWith(propertyInfos);
                currentType = currentType.BaseType;
            }
            return infoList.ToArray();
        }
        
        private class MemberInfoComparer : IEqualityComparer<MemberInfo>
        {
            public bool Equals(MemberInfo x, MemberInfo y)
            {
                return x.DeclaringType == y.DeclaringType && x.Name == y.Name;
            }

            public int GetHashCode(MemberInfo obj)
            {
                return obj.Name.GetHashCode() ^ obj.DeclaringType.GetHashCode();
            }
        }

    }
}
