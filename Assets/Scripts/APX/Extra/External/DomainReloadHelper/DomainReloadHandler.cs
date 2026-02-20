#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using APX.Extra.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace APX.Extra.External.DomainReloadHelper
{
    public static class DomainReloadHandler
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnRuntimeLoad()
        {
            EditorApplication.playModeStateChanged -= HandlePlayModeStateChanged;
            EditorApplication.playModeStateChanged += HandlePlayModeStateChanged;

            if (EditorSettings.enterPlayModeOptionsEnabled && (EditorSettings.enterPlayModeOptions & EnterPlayModeOptions.DisableDomainReload) > 0)
            {
                FastOrderedReload();
                // FastFieldsReload();
                // FastMethodsReload();
                // ReloadDomain();
            }
            else
            {
                Debug.Log($"[DomainReloadHandler] Turned off when Unity domain reload is On");
            }
        }

        private static void HandlePlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode && CustomDomainReloadSettings.ReloadOnPlayModeExit)
            {
                Debug.Log("[DomainReloadHandler] ReloadOnPlayModeExit is turned on in CustomDomainReloadSettings..");
                FastOrderedReload();
            }
        }

        public static void ReloadDomain()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            var valuesReset = 0;
            var executedMethods = 0;

            foreach (var member in GetMembers<ClearOnReloadAttribute>(true))
            {
                //Fields
                var field = member as FieldInfo;

                if (field != null && !field.FieldType.IsGenericParameter && field.IsStatic)
                {
                    try
                    {
                        field.SetValue(null, null);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e);
                        Debug.LogError(
                            $"[DomainReloadHandler] Unable to SetValue of field of type {field.FieldType.Name} name {field.Name} {(field.DeclaringType == null ? string.Empty : $"in {field.DeclaringType.FullName} ")} see exception above");
                    }

                    valuesReset++;
                }

                //Properties
                var property = member as PropertyInfo;

                if (property != null && !property.PropertyType.IsGenericParameter && property.GetAccessors(true).Any(x => x.IsStatic))
                {
                    try
                    {
                        property.SetValue(null, null);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e);
                        Debug.LogError($"[DomainReloadHandler] Unable to SetValue of field of type {property.PropertyType.Name} name {property.Name} see exception above");
                    }

                    valuesReset++;
                }
            }

            //Execute on reload
            foreach (var member in GetMethodMembers<ExecuteOnReloadAttribute>(true))
            {
                var method = member as MethodInfo;

                if (method != null && !method.IsGenericMethod && method.IsStatic)
                {
                    method.Invoke(null, new object[] { });
                    executedMethods++;
                }
            }

            Debug.Log($"[DomainReloadHandler] Reset {valuesReset} members, executed {executedMethods} methods in {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Stop();
        }

        private static void FastOrderedReload()
        {
            Profiler.BeginSample("DomainReloadHandler");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            var executedMethods = 0;
            var clearedValues = 0;

            var orderedMembers = TypeCache.GetFieldsWithAttribute<ClearOnReloadAttribute>()
                .Filter(FilterFields)
                .Cast<MemberInfo>()
                .Concat(TypeCache.GetMethodsWithAttribute<ExecuteOnReloadAttribute>())
                .OrderBy(info => info.GetCustomAttribute<DomainReloadHelperAttribute>().Order);

            foreach (var member in orderedMembers)
            {
                if (member is MethodInfo method)
                {
                    if (method.IsGenericMethod || !method.IsStatic) continue;

                    method.Invoke(null, new object[] { });
                    executedMethods++;
                }
                else if (member is FieldInfo field)
                {
                    var fieldType = field.FieldType;

                    // Extract attribute and access its parameters
                    var reloadAttribute = field.GetCustomAttribute<ClearOnReloadAttribute>();
                    if (reloadAttribute == null)
                        continue;
                    var valueToAssign = reloadAttribute.ValueToAssign;
                    var assignNewTypeInstance = reloadAttribute.AssignNewTypeInstance;

                    // Use valueToAssign only if it's convertible to the field value type
                    object value = null;
                    if (valueToAssign != null)
                    {
                        value = System.Convert.ChangeType(valueToAssign, fieldType);
                        if (value == null)
                            Debug.LogWarning($"[{nameof(DomainReloadHandler)}] Unable to assign value of type {valueToAssign.GetType()} to field {field.Name} of type {fieldType}.");
                    }

                    // If assignNewTypeInstance is set, create a new instance of this type and assign it to the field
                    if (assignNewTypeInstance)
                        value = System.Activator.CreateInstance(fieldType);

                    try
                    {
                        field.SetValue(null, value);
                        clearedValues++;
                    }
                    catch
                    {
                        if (valueToAssign == null)
                            Debug.LogWarning($"[{nameof(DomainReloadHandler)}] Unable to clear field {field.Name}.");
                        else
                            Debug.LogWarning($"[{nameof(DomainReloadHandler)}] Unable to assign field {field.Name}.");
                    }
                }
            }

            stopwatch.Stop();


            Profiler.EndSample();
        }

        private static bool FilterFields(FieldInfo field)
        {
            var filterValue = field != null && !field.FieldType.IsGenericParameter && field.IsStatic;
            if (field != null && !filterValue)
                Debug.LogWarning($"[{nameof(DomainReloadHandler)}] Inapplicable field {field.Name} to clear; must be static and non-generic.");
            return filterValue;
        }

        private static int FastMethodsReload()
        {
            Profiler.BeginSample("DomainReloadHandler");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            var executedMethods = 0;

            foreach (var method in TypeCache.GetMethodsWithAttribute<ExecuteOnReloadAttribute>())
            {
                if (method == null || method.IsGenericMethod || !method.IsStatic) continue;

                method.Invoke(null, new object[] { });
                executedMethods++;
            }

            stopwatch.Stop();
            Debug.Log($"[{nameof(DomainReloadHandler)}] Executed {executedMethods} methods in {stopwatch.ElapsedMilliseconds} ms");

            Profiler.EndSample();

            return executedMethods;
        }

        private static int FastFieldsReload()
        {
            Profiler.BeginSample("DomainReloadHandler");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            var clearedValues = 0;

            foreach (var field in TypeCache.GetFieldsWithAttribute<ClearOnReloadAttribute>())
            {
                if (field != null && !field.FieldType.IsGenericParameter && field.IsStatic)
                {
                    var fieldType = field.FieldType;

                    // Extract attribute and access its parameters
                    var reloadAttribute = field.GetCustomAttribute<ClearOnReloadAttribute>();
                    if (reloadAttribute == null)
                        continue;
                    var valueToAssign = reloadAttribute.ValueToAssign;
                    var assignNewTypeInstance = reloadAttribute.AssignNewTypeInstance;

                    // Use valueToAssign only if it's convertible to the field value type
                    object value = null;
                    if (valueToAssign != null)
                    {
                        value = System.Convert.ChangeType(valueToAssign, fieldType);
                        if (value == null)
                            Debug.LogWarning($"[{nameof(DomainReloadHandler)}] Unable to assign value of type {valueToAssign.GetType()} to field {field.Name} of type {fieldType}.");
                    }

                    // If assignNewTypeInstance is set, create a new instance of this type and assign it to the field
                    if (assignNewTypeInstance)
                        value = System.Activator.CreateInstance(fieldType);

                    try
                    {
                        field.SetValue(null, value);
                        clearedValues++;
                    }
                    catch
                    {
                        if (valueToAssign == null)
                            Debug.LogWarning($"[{nameof(DomainReloadHandler)}] Unable to clear field {field.Name}.");
                        else
                            Debug.LogWarning($"[{nameof(DomainReloadHandler)}] Unable to assign field {field.Name}.");
                    }
                }
                else
                {
                    Debug.LogWarning($"[{nameof(DomainReloadHandler)}] Inapplicable field {field.Name} to clear; must be static and non-generic.");
                }
            }

            stopwatch.Stop();
            Debug.Log($"[{nameof(DomainReloadHandler)}] Cleared {clearedValues} fields in {stopwatch.ElapsedMilliseconds} ms");

            Profiler.EndSample();

            return clearedValues;
        }

        public static void ClearPrivateStaticField(System.Type type, string fieldName)
        {
            if (type == null)
            {
                Debug.LogError($"[{nameof(DomainReloadHandler)}] 'type' was null!");
                return;
            }

            var field = type.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            if (field != null)
            {
                field.SetValue(null, null);
            }
        }

        public static void ClearPrivateStaticFields(System.Type type, string[] fieldNames)
        {
            if (type == null)
            {
                Debug.LogError($"[{nameof(DomainReloadHandler)}] 'type' was null!");
                return;
            }

            foreach (var fieldName in fieldNames)
            {
                ClearPrivateStaticField(type, fieldName);
            }
        }

        private static IEnumerable<MemberInfo> GetMethodMembers<TAttribute>(bool inherit) where TAttribute : System.Attribute
        {
            var members = new List<MemberInfo>();

            const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

            foreach (var a in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a.IsNonUserAssembly())
                    continue;

                try
                {
                    //Methods
                    members.AddRange(a.GetTypes()
                        .Where(t => t.IsClass)
                        .Where(t => !t.IsGenericParameter)
                        .SelectMany(t => t.GetMethods(flags), (t, m) => new {t, m})
                        .Where(t => !t.m.ContainsGenericParameters)
                        .Where(t => t.m.IsDefined(typeof(TAttribute), inherit))
                        .Select(t => t.m));
                }
                catch (ReflectionTypeLoadException)
                {
                    continue;
                }
            }

            return members;
        }

        private static IEnumerable<MemberInfo> GetMembers<TAttribute>(bool inherit) where TAttribute : System.Attribute
        {
            var members = new List<MemberInfo>();

            const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy;

            foreach (var a in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (var t in a.GetTypes())
                    {
                        if (!t.IsClass)
                            continue;

                        //Fields
                        foreach (var member in t.GetFields(flags))
                        {
                            if (member.IsDefined(typeof(TAttribute), inherit))
                                members.Add(member);
                        }

                        //Properties
                        foreach (var member in t.GetProperties(flags))
                        {
                            if (member.IsDefined(typeof(TAttribute), inherit))
                                members.Add(member);
                        }

                        //Events
                        foreach (var eventInfo in t.GetEvents(flags))
                        {
                            if (eventInfo.IsDefined(typeof(TAttribute), inherit))
                                members.Add(GetEventField(t, eventInfo.Name));
                        }
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                    continue;
                }
            }

            return members;
        }

        private static FieldInfo GetEventField(System.Type type, string eventName)
        {
            FieldInfo field = null;
            while (type != null)
            {
                /* Find events defined as field */
                field = type.GetField(eventName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null && (field.FieldType == typeof(System.MulticastDelegate) || field.FieldType.IsSubclassOf(typeof(System.MulticastDelegate))))
                    break;

                /* Find events defined as property { add; remove; } */
                field = type.GetField("EVENT_" + eventName.ToUpper(), BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null)
                    break;
                type = type.BaseType;
            }

            return field;
        }
    }
}
#endif
