using System.Collections.Generic;
using System.Linq;

namespace APX.Extra.Misc
{
    public static class ToggleableFieldExtensions
    {
        public static IEnumerable<T> ToValueList<T>(this IEnumerable<ToggleableField<T>> list)
        {
            return list.Select(value => value.Field);
        }
        
        public static IEnumerable<T> ToValueListFiltered<T>(this IEnumerable<ToggleableField<T>> list, bool toggled = true)
        {
            return list.Where(value => value?.IsToggled == toggled).Select(value => value.Field);
        }
        
        public static IEnumerable<ToggleableField<T>> ToToggleableList<T>(this IEnumerable<T> list)
        {
            return list.Select(value => new ToggleableField<T>(value));
        }
        
        public static T? ToNullable<T>(this ToggleableField<T> field) where T : struct
        {
            return field.IsToggled ? field.Field : null;
        }

        public static ToggleableField<T> ToToggleableField<T>(this T? value) where T : struct
        {
            return new ToggleableField<T>(value ?? default, value.HasValue);
        }
        
        public static T EvaluateField<T>(this ToggleableField<T> field) where T: class
        {
            return field.IsToggled ? field.Field : null;
        }
    }
}
