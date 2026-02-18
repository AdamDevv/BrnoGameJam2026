using System;
using System.Linq;

namespace APX.Util
{
    public static class MyReflectionHelper
    {
        public static object CreateDefaultValue(this Type type)
        {
            var constructor = type.GetConstructors().OrderBy(c => c.GetParameters().Length).FirstOrDefault();

            if (constructor is null)
            {
                throw new InvalidOperationException("No constructor found");
            }

            var parameters = constructor.GetParameters()
                .Select(p => p.HasDefaultValue ? p.DefaultValue :
                    p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null)
                .ToArray();

            return constructor.Invoke(parameters);
        }
    }
}
