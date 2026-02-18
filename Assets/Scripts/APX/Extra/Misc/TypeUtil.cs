using System;
using System.Collections.Generic;
using System.Linq;

namespace APX.Extra.Misc
{
    public static class TypeUtil
    {

        public static IEnumerable<Type> GetTypes(Func<Type, bool> predicate)
        {
            foreach (var assemb in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var tp in assemb.GetTypes())
                {
                    if (predicate == null || predicate(tp)) yield return tp;
                }
            }
        }

        public static IEnumerable<Type> GetTypesAssignableFrom(Type rootType)
        {
            foreach (var assemb in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var tp in assemb.GetTypes())
                {
                    if (rootType.IsAssignableFrom(tp)) yield return tp;
                }
            }
        }

        public static IEnumerable<Type> GetTypesAssignableFrom(System.Reflection.Assembly assemb, Type rootType)
        {
            foreach (var tp in assemb.GetTypes())
            {
                if (rootType.IsAssignableFrom(tp) && rootType != tp) yield return tp;
            }
        }

        public static bool IsType(Type tp, Type assignableType)
        {
            if (assignableType.IsGenericType)
            {
                while (tp != null && tp != typeof(object))
                {
                    var ctp = tp.IsGenericType ? tp.GetGenericTypeDefinition() : tp;
                    if (ctp == assignableType) return true;
                    tp = tp.BaseType;
                }
                return false;
            }
            else
            {
                return assignableType.IsAssignableFrom(tp);
            }
        }

        public static bool IsType(Type tp, params Type[] assignableTypes)
        {
            foreach (var otp in assignableTypes)
            {
                if (otp.IsAssignableFrom(tp)) return true;
            }

            return false;
        }

        public static object GetDefaultValue(this Type tp)
        {
            if (tp == null) throw new ArgumentNullException("tp");

            if (tp.IsValueType)
                return Activator.CreateInstance(tp);
            else
                return null;
        }


        public static Type ParseType(string assembName, string typeName)
        {
            var assemb = (from a in AppDomain.CurrentDomain.GetAssemblies()
                          where a.GetName().Name == assembName || a.FullName == assembName
                          select a).FirstOrDefault();
            if (assemb != null)
            {
                return (from t in assemb.GetTypes()
                        where t.FullName == typeName
                        select t).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public static Type FindType(string typeName, bool useFullName = false, bool ignoreCase = false)
        {
            if (string.IsNullOrEmpty(typeName)) return null;

            bool isArray = typeName.EndsWith("[]");
            if (isArray)
                typeName = typeName.Substring(0, typeName.Length - 2);

            StringComparison e = (ignoreCase) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            if (useFullName)
            {
                foreach (var assemb in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var t in assemb.GetTypes())
                    {
                        if (string.Equals(t.FullName, typeName, e))
                        {
                            if (isArray)
                                return t.MakeArrayType();
                            else
                                return t;
                        }
                    }
                }
            }
            else
            {
                foreach (var assemb in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var t in assemb.GetTypes())
                    {
                        if (string.Equals(t.Name, typeName, e) || string.Equals(t.FullName, typeName, e))
                        {
                            if (isArray)
                                return t.MakeArrayType();
                            else
                                return t;
                        }
                    }
                }
            }
            return null;
        }

        public static Type FindType(string typeName, Type baseType, bool useFullName = false, bool ignoreCase = false)
        {
            if (string.IsNullOrEmpty(typeName)) return null;
            if (baseType == null) throw new ArgumentNullException("baseType");

            bool isArray = typeName.EndsWith("[]");
            if (isArray)
                typeName = typeName.Substring(0, typeName.Length - 2);

            StringComparison e = (ignoreCase) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            if(useFullName)
            {
                foreach (var assemb in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var t in assemb.GetTypes())
                    {
                        if (baseType.IsAssignableFrom(t) && string.Equals(t.FullName, typeName, e))
                        {
                            if (isArray)
                                return t.MakeArrayType();
                            else
                                return t;
                        }
                    }
                }
            }
            else
            {
                foreach (var assemb in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var t in assemb.GetTypes())
                    {
                        if (baseType.IsAssignableFrom(t) && (string.Equals(t.Name, typeName, e) || string.Equals(t.FullName, typeName, e)))
                        {
                            if (isArray)
                                return t.MakeArrayType();
                            else
                                return t;
                        }
                    }
                }
            }

            return null;
        }

        public static bool IsListType(this Type tp)
        {
            if (tp == null) return false;

            if (tp.IsArray) return tp.GetArrayRank() == 1;

            var interfaces = tp.GetInterfaces();
            //if (interfaces.Contains(typeof(System.Collections.IList)) || interfaces.Contains(typeof(IList<>)))
            if (Array.IndexOf(interfaces, typeof(System.Collections.IList)) >= 0 || Array.IndexOf(interfaces, typeof(IList<>)) >= 0)
            {
                return true;
            }

            return false;
        }

        public static bool IsListType(this Type tp, bool ignoreAsInterface)
        {
            if (tp == null) return false;

            if (tp.IsArray) return tp.GetArrayRank() == 1;

            if (ignoreAsInterface)
            {
                //if (tp == typeof(System.Collections.ArrayList) || (tp.IsGenericType && tp.GetGenericTypeDefinition() == typeof(List<>))) return true;
                if (tp.IsGenericType && tp.GetGenericTypeDefinition() == typeof(List<>)) return true;
            }
            else
            {
                var interfaces = tp.GetInterfaces();
                //if (interfaces.Contains(typeof(System.Collections.IList)) || interfaces.Contains(typeof(IList<>)))
                if (Array.IndexOf(interfaces, typeof(System.Collections.IList)) >= 0 || Array.IndexOf(interfaces, typeof(IList<>)) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsListType(this Type tp, out Type innerType)
        {
            innerType = null;
            if (tp == null) return false;

            if (tp.IsArray)
            {
                if (tp.GetArrayRank() == 1)
                {
                    innerType = tp.GetElementType();
                    return true;
                }
                else
                    return false;
            }

            var interfaces = tp.GetInterfaces();
            if (Array.IndexOf(interfaces, typeof(System.Collections.IList)) >= 0 || Array.IndexOf(interfaces, typeof(IList<>)) >= 0)
            {
                if (tp.IsGenericType)
                {
                    innerType = tp.GetGenericArguments()[0];
                }
                else
                {
                    innerType = typeof(object);
                }
                return true;
            }

            return false;
        }

        public static bool IsListType(this Type tp, bool ignoreAsInterface, out Type innerType)
        {
            innerType = null;
            if (tp == null) return false;

            if (tp.IsArray)
            {
                if (tp.GetArrayRank() == 1)
                {
                    innerType = tp.GetElementType();
                    return true;
                }
                else
                    return false;
            }

            if (ignoreAsInterface)
            {
                if (tp.IsGenericType && tp.GetGenericTypeDefinition() == typeof(List<>))
                {
                    innerType = tp.GetGenericArguments()[0];
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var interfaces = tp.GetInterfaces();
                if (Array.IndexOf(interfaces, typeof(System.Collections.IList)) >= 0 || Array.IndexOf(interfaces, typeof(IList<>)) >= 0)
                {
                    if (tp.IsGenericType)
                    {
                        innerType = tp.GetGenericArguments()[0];
                    }
                    else
                    {
                        innerType = typeof(object);
                    }
                    return true;
                }
            }

            return false;
        }

        public static Type GetElementTypeOfListType(this Type tp)
        {
            if (tp == null) return null;

            if (tp.IsArray) return tp.GetElementType();

            var interfaces = tp.GetInterfaces();
            //if (interfaces.Contains(typeof(System.Collections.IList)) || interfaces.Contains(typeof(IList<>)))
            if (Array.IndexOf(interfaces, typeof(System.Collections.IList)) >= 0 || Array.IndexOf(interfaces, typeof(IList<>)) >= 0)
            {
                if (tp.IsGenericType) return tp.GetGenericArguments()[0];
                else return typeof(object);
            }

            return null;
        }



        private static Type _obsoleteAttribType = typeof(ObsoleteAttribute);
        public static bool IsObsolete(this System.Reflection.MemberInfo member)
        {
            return Attribute.IsDefined(member, _obsoleteAttribType);
        }

    }
}
