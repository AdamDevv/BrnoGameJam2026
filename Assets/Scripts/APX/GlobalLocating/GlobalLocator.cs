using System;
using System.Collections.Generic;
using System.Linq;
using APX.Extra.OdinExtensions.Attributes;
using APX.GlobalLocating.Abstractions;
using APX.GlobalLocating.Models;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using APX.Extra.Misc;
using APX.Util;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using APX.Extra.EditorUtils;
using Sirenix.OdinInspector.Editor.Validation;
#endif

namespace APX.ObjectLocating
{
    [GlobalConfig(SettingsConstants.SETTINGS_ASSET_PATH)]
    public class GlobalLocator : GlobalConfig<GlobalLocator>
    {
        [SerializeField]
        [EnhancedTableList(AlwaysExpanded = true, IsReadOnly = true, OnTitleBarGUI = "OnRegisteredObjectsTileBarGUI")]
        [HideLabel]
        [EnhancedValidate("ValidateRecords")]
        private List<GlobalLocatorRecord> _Records = new();

        private readonly Dictionary<Type, AGlobalLocatorObject> _recordsTypeDictionary = new();

        public static T Get<T>() where T : AGlobalLocatorObject => Instance.GetInternal<T>();

        private T GetInternal<T>() where T : AGlobalLocatorObject
        {
            if (!_recordsTypeDictionary.TryGetValue(typeof(T), out AGlobalLocatorObject instance))
            {
                instance = _Records.FirstOrDefault(r => r.Type == typeof(T))?.Object;
                _recordsTypeDictionary.Add(typeof(T), instance);
            }

            if (instance is null)
            {
                throw new Exception($"No type of {typeof(T).Name} found in GlobalLocator");
            }

            return instance as T;
        }


        #if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            WaitInEditor.ForNextFrame(RefreshRegisteredObjectsEditorOnly);
        }

        internal static void RefreshRegisteredObjectsEditorOnly()
        {
            // Remove unknown type records
            Instance._Records = Instance._Records.Where(t => t.Type?.Type is not null).ToList();

            // Get missing types
            GlobalLocatorRecord[] newRecords = GetGlobalLocatorObjectTypes()
                .Where(t => Instance._Records.All(r => r.Type != t))
                .Select(t => new GlobalLocatorRecord(t, null))
                .ToArray();

            // Populate missing types
            var recordsWithMissingTypes = newRecords.Append(Instance._Records.Where(r => r.Object is null)).ToArray();

            foreach (var record in recordsWithMissingTypes)
            {
                record.Object = EditorAssetUtils.FindAssetOfType(record.Type.Type) as AGlobalLocatorObject;
            }

            Instance._Records.AddRange(newRecords);

            // Order by record type
            Instance._Records = Instance._Records.OrderBy(r => r.ObjectRecordTypeEditorOnly).ThenBy(r => r.Type?.Type?.Name).ToList();

            EditorUtility.SetDirty(Instance);
        }

        private static IEnumerable<Type> GetGlobalLocatorObjectTypes() =>
            TypeUtil.GetTypesAssignableFrom(typeof(AGlobalLocatorObject)).Where(t => !t.IsAbstract);

        [UsedImplicitly]
        private void ValidateRecords(List<GlobalLocatorRecord> value, ValidationResult result, InspectorProperty property)
        {
            if (value.Any(r => r.Type?.Type is null))
            {
                result.AddError("Contains records without assigned types.")
                    .WithFix(RefreshRegisteredObjectsEditorOnly);
                return;
            }

            var emptyObjectRecords = value.Where(r => r.Object is null).ToArray();
            if (emptyObjectRecords.Any())
            {
                result.AddError("Contains records without assigned objects: " + string.Join(", ", emptyObjectRecords.Select(r => r.Type.Type.Name)));
                return;
            }

            var missingTypes = GetGlobalLocatorObjectTypes()
                .Where(t => value.All(r => r.Type?.Type != t))
                .ToArray();

            if (missingTypes.Length != 0)
            {
                result.AddError("Missing types in the records: " + string.Join(", ", missingTypes.Select(t => t.Name)))
                    .WithFix(RefreshRegisteredObjectsEditorOnly);
                return;
            }

            var invalidTypes = value.Where(r => r.Object.GetType() != r.Type.Type).ToArray();
            if (invalidTypes.Length != 0)
            {
                result.AddError("Invalid types in the records: " + string.Join(", ", invalidTypes.Select(r => r.Type.Type.Name)))
                    .WithFix(() =>
                    {
                        foreach (GlobalLocatorRecord record in invalidTypes)
                        {
                            record.Object = null;
                        }

                        RefreshRegisteredObjectsEditorOnly();
                    });
                return;
            }
        }

        [UsedImplicitly]
        private void OnRegisteredObjectsTileBarGUI(InspectorProperty property)
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {
                RefreshRegisteredObjectsEditorOnly();
            }
        }

        private const string SETTINGS_PATH = "Project/APX/Settings/Global Object Locator";
        private static readonly IEnumerable<string> KEYWORDS = new[] { "ObjectLocator" };

        [MenuItem("APX/Settings/Global Object Locator")]
        public static void OpenLoaderSettings()
        {
            SettingsService.OpenProjectSettings(SETTINGS_PATH);
        }

        [SettingsProvider]
        protected static SettingsProvider RegisterSettingsProvider()
        {
            return Instance == null ? null : AssetSettingsProvider.CreateProviderFromObject(SETTINGS_PATH, Instance, KEYWORDS);
        }
        #endif
    }
}
