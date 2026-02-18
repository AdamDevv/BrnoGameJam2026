using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.Misc
{
    public static class ReflectionHelper
    {
        public static List<Type> GetAssignableSubtypes(Type targetType, bool onlyExportedTypes = true, string[] excludedAssemblies = null)
        {
            static bool Contains(IReadOnlyList<AssemblyName> array, string item)
            {
                for (var index = 0; index < array.Count; ++index)
                {
                    if (array[index].ToString() == item)
                        return true;
                }

                return false;
            }

            var result = new List<Type>();

            Assembly parentAssembly = null;
            string parentAssemblyName = null;
            var assemblies = new Assembly[0];
            try
            {
                parentAssembly = targetType.Assembly;
                parentAssemblyName = parentAssembly.GetName().ToString();
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            foreach (var assembly in assemblies)
            {
                if (assembly.IsDynamic) continue;
                if (excludedAssemblies != null)
                {
                    var isExcluded = false;
                    foreach (var excludedAssembly in excludedAssemblies)
                    {
                        if (assembly.FullName.StartsWith(excludedAssembly)) isExcluded = true;
                    }
                    if (isExcluded) continue;
                }
                
                if (assembly != parentAssembly && !Contains(assembly.GetReferencedAssemblies(), parentAssemblyName)) continue;
                try
                {
                    var types = onlyExportedTypes ? assembly.GetExportedTypes() : assembly.GetTypes();

                    foreach (var type in types)
                    {
                        if (!type.IsAbstract && !type.IsGenericType && targetType.IsAssignableFrom(type))
                        {
                            result.Add(type);
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    Debug.LogException(ex);
                    foreach (var inner in ex.LoaderExceptions)
                    {
                        Debug.LogException(inner);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            return result;
        }

        public static Type GetCommonBaseType(this IEnumerable<Type> types)
        {
            if (types == null)
                return null;

            Type commonBase = null;
            foreach (var type in types)
            {
                if (type == null)
                    return null;

                commonBase ??= type;
                while (!commonBase.IsAssignableFrom(type))
                {
                    commonBase = commonBase.BaseType;
                    if (commonBase == null)
                        return null;
                }
            }
            return commonBase;
        }

        public static Type GetMostSpecificType(this IEnumerable<Type> types)
        {
            if (types == null)
                return null;

            Type mostSpecific = null;
            foreach (var type in types)
            {
                if (type == null)
                    return null;

                mostSpecific ??= type;

                var assignableFrom = mostSpecific.IsAssignableFrom(type);
                var assignableTo = type.IsAssignableFrom(mostSpecific);

                if (assignableFrom || assignableTo)
                {
                    if (assignableFrom)
                        mostSpecific = type;
                }
                else
                {
                    return null;
                }
            }
            return mostSpecific;
        }

        public static List<T> GetDerivedClassesOfType<T>(params object[] constructorArgs) where T : class
        {
            List<T> objects = new List<T>();
            foreach (Type type in Assembly.GetAssembly(typeof(T)).GetTypes()
                         .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }

            return objects;
        }

        public static List<Type> GetDerivedTypes<T>() where T : class
        {
            List<Type> types = new List<Type>();
            foreach (Type type in Assembly.GetAssembly(typeof(T)).GetTypes()
                         .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                types.Add(type);
            }

            return types;
        }

#if UNITY_EDITOR
        public static List<T> GetTypeCacheDerivedClassesOfType<T>(params object[] constructorArgs) where T : class
        {
            List<T> objects = new List<T>();
            foreach (Type type in TypeCache.GetTypesDerivedFrom<T>())
            {
                if (!type.IsAbstract && type.GetConstructor(Type.EmptyTypes) != null) objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }

            return objects;
        }

        public static IEnumerable<Type> GetTypeCacheDerivedTypes<T>() where T : class
        {
            return TypeCache.GetTypesDerivedFrom<T>();
        }

        public static IEnumerable<Type> GetTypeCacheDerivedGenericTypes<TParent,T>()
        {
            var parentTypes = TypeCache.GetTypesDerivedFrom<TParent>();
            var genericTypes = TypeCache.GetTypesDerivedFrom<T>();
            foreach (var genericType in genericTypes)
            {
                if (genericType.IsAbstract)
                    continue;

                foreach (var parentType in parentTypes)
                {
                    if (parentType.IsAbstract || !parentType.IsGenericType)
                        continue;

                    var arguments = parentType.GetGenericArguments();
                    if (arguments.Length != 1 || !arguments[0].GetGenericParameterConstraints().All(c => c.IsAssignableFrom(genericType)))
                        continue;

                    yield return parentType.MakeGenericType(genericType);
                }
            }
        }

        public static IEnumerable<TParent> GetTypeCacheDerivedClassesOfGenericTypes<TParent, T>(params object[] constructorArgs)
        {
            foreach (var type in GetTypeCacheDerivedGenericTypes<TParent, T>())
            {
                yield return (TParent) Activator.CreateInstance(type, constructorArgs);
            }
        }
#endif

        public static IEnumerable<T> AttributesOfType<T>(bool includeNonPublic = false) where T : Attribute
        {
            return new AttributesEnumerable<T>() { IncludeNonPublic = includeNonPublic };
        }

        public class AttributesEnumerable<T> : IEnumerable<T> where T : Attribute
        {
            public bool IncludeNonPublic;

            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

            public IEnumerator<T> GetEnumerator()
            {
                Assembly attributeAssembly = null;
                string attributeAssemblyName = null;
                Assembly[] assemblies = null;
                try
                {
                    attributeAssembly = typeof(T).Assembly;
                    attributeAssemblyName = attributeAssembly.GetName().ToString();
                    assemblies = AppDomain.CurrentDomain.GetAssemblies();
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("ERROR Source:{0}, Msg:{1}, HelpLink: {2}", e.Source, e.Message, e.HelpLink);
                }

                for (int i = 0; i < assemblies.Length; i++)
                {
                    Assembly assembly = assemblies[i];

                    if (assembly.IsDynamic) continue;
                    if (IsNonUserAssembly(assembly)) continue;
                    if (assembly != attributeAssembly && !Contains(assembly.GetReferencedAssemblies(), attributeAssemblyName)) continue;

                    var types = IncludeNonPublic ? assembly.GetTypes() : assembly.GetExportedTypes();

                    foreach (var type in types)
                    {
                        T requiredAttribute = type.GetCustomAttribute<T>(false);
                        if (requiredAttribute != null)
                        {
                            yield return requiredAttribute;
                        }
                    }
                }
            }
        }

        public static IEnumerable<Type> TypesWithAttribute<T>(bool inherit = false, bool includeNonPublic = false) where T : Attribute
        {
            return new TypesWithAttributeEnumerable<T>(inherit) { IncludeNonPublic = includeNonPublic };
        }

        public class TypesWithAttributeEnumerable<T> : IEnumerable<Type> where T : Attribute
        {
            private bool _inherit;
            public bool IncludeNonPublic;

            public TypesWithAttributeEnumerable(bool inherit = false) { _inherit = inherit; }

            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

            public IEnumerator<Type> GetEnumerator()
            {
                Assembly attributeAssembly = null;
                string attributeAssemblyName = null;
                Assembly[] assemblies = null;
                try
                {
                    attributeAssembly = typeof(T).Assembly;
                    attributeAssemblyName = attributeAssembly.GetName().ToString();
                    assemblies = AppDomain.CurrentDomain.GetAssemblies();
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("ERROR Source:{0}, Msg:{1}, HelpLink: {2}", e.Source, e.Message, e.HelpLink);
                }

                for (int i = 0; i < assemblies.Length; i++)
                {
                    Assembly assembly = assemblies[i];

                    if (assembly.IsDynamic) continue;
                    if (IsNonUserAssembly(assembly)) continue;
                    if (!_inherit && assembly != attributeAssembly && !Contains(assembly.GetReferencedAssemblies(), attributeAssemblyName)) continue;

                    var types = IncludeNonPublic ? assembly.GetTypes() : assembly.GetExportedTypes();

                    foreach (var type in types)
                    {
                        T requiredAttribute = type.GetCustomAttribute<T>(_inherit);
                        if (requiredAttribute != null)
                        {
                            yield return type;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindingFlags">must include at least one of BindingFlags.Instance and BindingFlags.Static, along with at least one of BindingFlags.NonPublic and BindingFlags.Public</param>
        /// <param name="inherit"></param>
        /// <param name="includeNonPublic"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> MethodsWithAttribute<T>(BindingFlags bindingFlags, bool inherit = false, bool includeNonPublic = false) where T : Attribute
        {
            return new MethodsWithAttributeEnumerable<T>(bindingFlags, inherit) { IncludeNonPublic = includeNonPublic };
        }

        public class MethodsWithAttributeEnumerable<T> : IEnumerable<MethodInfo> where T : Attribute
        {
            private bool _inherit;
            public bool IncludeNonPublic;
            public BindingFlags BindingFlags;

            public MethodsWithAttributeEnumerable(BindingFlags bindingFlags, bool inherit = false)
            {
                BindingFlags = bindingFlags;
                _inherit = inherit;
            }

            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

            public IEnumerator<MethodInfo> GetEnumerator()
            {
                Assembly attributeAssembly = null;
                string attributeAssemblyName = null;
                Assembly[] assemblies = null;
                try
                {
                    attributeAssembly = typeof(T).Assembly;
                    attributeAssemblyName = attributeAssembly.GetName().ToString();
                    assemblies = AppDomain.CurrentDomain.GetAssemblies();
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("ERROR Source:{0}, Msg:{1}, HelpLink: {2}", e.Source, e.Message, e.HelpLink);
                }

                for (int i = 0; i < assemblies.Length; i++)
                {
                    Assembly assembly = assemblies[i];

                    if (assembly.IsDynamic) continue;
                    if (assembly.IsNonUserAssembly()) continue;
                    if (!_inherit && assembly != attributeAssembly && !Contains(assembly.GetReferencedAssemblies(), attributeAssemblyName)) continue;

                    var types = IncludeNonPublic ? assembly.GetTypes() : assembly.GetExportedTypes();

                    foreach (var type in types)
                    {
                        foreach (var method in type.GetMethods(BindingFlags))
                        {
                            T requiredAttribute = method.GetCustomAttribute<T>(_inherit);
                            if (requiredAttribute != null)
                            {
                                yield return method;
                            }
                        }
                    }
                }
            }
        }

        public static bool IsNonUserAssembly(this Assembly assembly)
        {
            var fullname = assembly.FullName.ToLower();
            var cannotStartWith = new string[] {"unity", "system.", "mscorlib", "mono.","microsoft" ,"google.", "firebase.", "sirenix.", "dotween", "tapjoy"};
            return cannotStartWith.Any(c => fullname.StartsWith(c));
        }

        public static IEnumerable<Type> CustomTypes() { return new CustomTypeEnumerable(); }

        public class CustomTypeEnumerable : IEnumerable<Type>
        {
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

            public IEnumerator<Type> GetEnumerator()
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

                for (int i = 0; i < assemblies.Length; i++)
                {
                    Assembly assembly = assemblies[i];

                    if (assembly.IsDynamic) continue;
                    if (IsNonUserAssembly(assembly)) continue;
                    var types = assembly.GetExportedTypes();

                    foreach (var type in types)
                    {
                        yield return type;
                    }
                }
            }
        }

        public static IEnumerable<Type> CustomTypes<T>() { return new AssignableTypeEnumerable<T>(); }
        public static IEnumerable<Type> CustomTypes(Type targetType) { return new AssignableTypeEnumerable(targetType); }

        public class AssignableTypeEnumerable<T> : IEnumerable<Type>
        {
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

            public IEnumerator<Type> GetEnumerator()
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                Type targetType = typeof(T);
                Assembly parentAssembly = targetType.Assembly;
                string parentAssemblyName = parentAssembly.GetName().ToString();

                for (int i = 0; i < assemblies.Length; i++)
                {
                    Assembly assembly = assemblies[i];

                    if (assembly.IsDynamic) continue;
                    string fullname = assembly.FullName;
                    if (IsNonUserAssembly(assembly)) continue;
                    if (assembly != parentAssembly && !Contains(assembly.GetReferencedAssemblies(), parentAssemblyName)) continue;

                    var types = assembly.GetExportedTypes();

                    foreach (var type in types)
                    {
                        yield return type;
                    }
                }
            }
        }

        public class AssignableTypeEnumerable : IEnumerable<Type>
        {
            protected Type _type;

            public AssignableTypeEnumerable(Type type) { _type = type; }

            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

            public IEnumerator<Type> GetEnumerator()
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                Assembly parentAssembly = _type.Assembly;
                string parentAssemblyName = parentAssembly.GetName().ToString();

                for (int i = 0; i < assemblies.Length; i++)
                {
                    Assembly assembly = assemblies[i];

                    if (assembly.IsDynamic) continue;
                    string fullname = assembly.FullName;
                    if (IsNonUserAssembly(assembly)) continue;
                    if (assembly != parentAssembly && !Contains(assembly.GetReferencedAssemblies(), parentAssemblyName)) continue;

                    var types = assembly.GetExportedTypes();

                    foreach (var type in types)
                    {
                        yield return type;
                    }
                }
            }
        }

        public static List<Type> GetAssignableTypes<T>()
        {
            return GetAssignableTypes(typeof(T));
        }

        public static List<Type> GetAssignableTypes(Type type)
        {
            List<Type> types = new List<Type>();
            foreach (Type customType in CustomTypes(type))
            {
                types.Add(customType);
            }

            return types;
        }

        private static bool Contains(AssemblyName[] array, string item)
        {
            for (int index = 0; index < array.Length; ++index)
            {
                if (array[index].ToString() == item)
                    return true;
            }

            return false;
        }
    }
}
