#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using APX.Extra.Misc;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace APX.Extra.Reboot
{
    public class GameRebootBuildProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 100;

        public void OnPreprocessBuild(BuildReport report)
        {
            GenerateLinkXml();
        }

        public static void GenerateLinkXml()
        {
            var linkXml = new XElement("linker");

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.IsDynamic) continue;
                if (assembly.IsNonUserAssembly()) continue;

                XElement assemblyElement = null;
                foreach (var type in assembly.GetTypes())
                {
                    XElement typeElement = null;
                    var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                    foreach (var method in methods)
                    {
                        var attr = method.GetCustomAttribute<ExecuteOnRebootAttribute>(true);
                        if (attr == null)
                            continue;

                        assemblyElement ??= new XElement("assembly", new XAttribute("fullname", assembly.FullName));
                        typeElement ??= new XElement("type", new XAttribute("fullname", type.FullName));

                        typeElement.Add(new XElement("method", new XAttribute("name", method.Name)));
                    }

                    if (typeElement != null)
                        assemblyElement.Add(typeElement);
                }

                if (assemblyElement != null)
                    linkXml.Add(assemblyElement);
            }

            var doc = new XDocument(linkXml);
            var path = $"{Application.dataPath}/Resources/GameRebootLinker";
            Directory.CreateDirectory(path);
            doc.Save($"{path}/link.xml");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
#endif
