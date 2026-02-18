using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using APX.Extra.Misc;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace APX.Extra.Reboot
{
    public static class GameRebootUtility
    {
        public static void Reboot()
        {
            RebootAsync().Forget();
        }

        public static async UniTask RebootAsync()
        {
            Debug.Log("[GameReboot] Reboot begin");
            await UniTask.Yield(); // Wait for end of frame to ensure all synchronous operations are done
            var tmpScene = SceneManager.CreateScene("TMP");
            SceneManager.SetActiveScene(tmpScene);
            var camera = new GameObject("Camera", typeof(Camera)).GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;
            camera.depth = 100;

            await UniTask.Yield();
            await InvokeRebootAttributes();

            Debug.Log("[GameReboot] Reloading boot scene");
            await SceneManager.LoadSceneAsync(0); // Load boot scene, will also unload TMP scene as we load in single mode

            Debug.Log("[GameReboot] Reboot completed");
        }

        private static async UniTask InvokeRebootAttributes()
        {
            var members = GetMembersWithAttribute<ARebootAttribute>().OrderBy(x => x.Item2.Order).ToList();
            foreach (var (member, attribute) in members)
            {
                Debug.Log($"[GameReboot] {GetAttributeActionName(attribute)} {member.ReflectedType.FullName}.{member.Name}");
                try
                {
                    switch (attribute)
                    {
                        case ClearOnRebootAttribute:
                            switch (member)
                            {
                                case FieldInfo field:
                                    field.SetValue(null, null);
                                    break;

                                case PropertyInfo property:
                                    property.SetValue(null, null);
                                    break;
                            }
                            break;
                        case ExecuteOnRebootAttribute:
                            switch (member)
                            {
                                case MethodInfo method:
                                    var result = method.Invoke(null, null);
                                    if (result is UniTask uniTask)
                                        await uniTask;
                                    break;
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        private static string GetAttributeActionName(ARebootAttribute attribute) => attribute switch
        {
            ClearOnRebootAttribute => "Clear",
            ExecuteOnRebootAttribute => "Execute",
            _ => "Unknown"
        };

        private static IEnumerable<(MemberInfo, TAttribute)> GetMembersWithAttribute<TAttribute>() where TAttribute : Attribute
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.IsDynamic) continue;
                if (assembly.IsNonUserAssembly()) continue;

                foreach (var type in assembly.GetTypes())
                {
                    var members = type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                    foreach (var member in members)
                    {
                        var attr = member.GetCustomAttribute<TAttribute>(true);
                        if (attr != null)
                            yield return (member, attr);
                    }
                }
            }
        }
    }
}
