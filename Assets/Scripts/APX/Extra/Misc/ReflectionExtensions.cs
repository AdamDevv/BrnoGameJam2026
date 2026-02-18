using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace APX.Extra.Misc
{
    public static class ReflectionExtensions
    {
        public static bool TryGetValue<T>(this FieldInfo fieldInfo, object target, out T result)
        {
            result = default;
            if (fieldInfo == null)
                return false;

            if (target == null)
                return false;

            var value = fieldInfo.GetValue(target);
            if (value is not T tValue)
                return false;

            result = tValue;
            return true;
        }

        public static bool TrySetValue<T>(this FieldInfo fieldInfo, object target, T value)
        {
            if (fieldInfo == null)
                return false;

            if (target == null)
                return false;

            fieldInfo.SetValue(target, value);
            return true;
        }

        public static T GetAttribute<T>(this ICustomAttributeProvider member, bool inherit) where T : System.Attribute
        {
            var array = member.GetAttributes<T>(inherit).ToArray();
            return array.Length != 0 ? array[0] : default;
        }

        /// <summary>
        /// Returns the first found non-inherited custom attribute of type T on this member
        /// Returns null if none was found
        /// </summary>
        public static T GetAttribute<T>(this ICustomAttributeProvider member) where T : System.Attribute => member.GetAttribute<T>(false);

        /// <summary>Gets all attributes of the specified generic type.</summary>
        /// <param name="member">The member.</param>
        /// <param name="inherit">If true, specifies to also search the ancestors of element for custom attributes.</param>
        public static IEnumerable<T> GetAttributes<T>(this ICustomAttributeProvider member, bool inherit) where T : System.Attribute
        {
            try
            {
                return member.GetCustomAttributes(typeof(T), inherit).Cast<T>();
            }
            catch
            {
                return System.Array.Empty<T>();
            }
        }
    }
}
