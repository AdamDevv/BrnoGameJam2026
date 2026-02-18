// // Author: Jan Krejsa
// // Created: 03.05.2024
// // Copyright (c) Noxgames
// // http://www.noxgames.com/
//
// using System;
// using System.Collections.Generic;
// using System.Text;
// using Sirenix.OdinInspector.Editor;
// using UnityEditor;
// using UnityEditor.Localization;
// using UnityEditor.SceneManagement;
// using UnityEngine;
// using UnityEngine.Localization.Tables;
// using UnityEngine.SceneManagement;
// using Object = UnityEngine.Object;
//
// namespace Sirenix.OdinInspector.Modules.Localization.Editor
// {
//     public class LocalizationUsages
//     {
//         public class LocalizationEntryUsagesData
//         {
//             [ShowInInspector]
//             [LabelWidth(40)]
//             [HorizontalGroup]
//             public string Table { get; }
//
//             [ShowInInspector]
//             [HorizontalGroup]
//             [LabelWidth(30)]
//             public string Key { get; }
//
//             [ShowInInspector]
//             [HorizontalGroup]
//             [LabelWidth(75)]
//             public string Translation { get; }
//
//             [PropertySpace(10)]
//             [ShowInInspector]
//             [ListDrawerSettings(DefaultExpandedState = true)]
//             public int UsageCount => Usages.Count;
//
//             public long TableEntryId { get; }
//
//             [PropertySpace(10)]
//             [ShowInInspector]
//             public List<UsageEntry> Usages { get; } = new();
//
//             public LocalizationEntryUsagesData(string table, long tableEntryId, string key, string translationValue)
//             {
//                 Table = table;
//                 TableEntryId = tableEntryId;
//                 Key = key;
//                 Translation = translationValue;
//             }
//
//             public void RefreshLiveReferences()
//             {
//                 foreach (var usage in Usages)
//                 {
//                     usage.RefreshLiveReference();
//                 }
//             }
//         }
//
//         public class UsageEntry
//         {
//             [ShowInInspector]
//             public Object Asset { get; }
//
//             [ShowInInspector]
//             [ShowIf(nameof(ShowGameObject))]
//             public Object GameObject { get; }
//
//             [ShowInInspector]
//             //[ShowIf(nameof(ShowComponent))]
//             public Object Component { get; }
//
//             [ShowInInspector]
//             [ShowIf(nameof(ShowLiveReference))]
//             //[LabelText("Component")]
//             public Object LiveReference { get; private set; }
//
//             [ShowInInspector]
//             [ShowIf(nameof(ShowComponentPath))]
//             public string ComponentPath { get; }
//
//             [ShowInInspector]
//             public string PropertyPath { get; }
//             [ShowInInspector]
//             public string PropertyName { get; }
//
//             private bool ShowGameObject => GameObject != null && GameObject != Asset && GameObject != Component;
//             private bool ShowComponent => Component != null && Component != Asset && !ShowLiveReference;
//             private bool ShowLiveReference => LiveReference != null;
//             private bool ShowComponentPath => !string.IsNullOrWhiteSpace(ComponentPath);
//
//             public UsageEntry(Object asset, Object unityComponent, string componentPath, string propertyPath, string propertyName)
//             {
//                 Asset = asset;
//                 Component = unityComponent;
//                 GameObject = unityComponent is Component component ? component.gameObject : unityComponent;
//                 ComponentPath = componentPath;
//                 PropertyPath = propertyPath;
//                 PropertyName = propertyName;
//             }
//
//             public void RefreshLiveReference()
//             {
//                 LiveReference = null;
//                 if (Component == null || Component is not Component component)
//                     return;
//
//                 var componentIndex = component.GetComponentIndex();
//                 var stack = new Stack<int>();
//
//                 var root = (Asset is GameObject asset) ? asset.transform : null;
//                 var t = component.transform;
//                 while (t != null && t != root)
//                 {
//                     stack.Push(t.GetSiblingIndex());
//                     t = t.parent;
//                 }
//
//                 Transform GetSceneRoot()
//                 {
//                     var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
//                     if (prefabStage != null)
//                     {
//                         return prefabStage.prefabContentsRoot.transform;
//                     }
//
//                     var activeScene = SceneManager.GetActiveScene();
//                     return activeScene.GetRootGameObjects()[stack.Pop()].transform;
//                 }
//
//                 t = GetSceneRoot();
//                 if (t == null)
//                     return;
//                 while (stack.Count > 0)
//                 {
//                     var index = stack.Pop();
//                     if (index >= t.childCount)
//                         return;
//                     t = t.GetChild(index);
//                 }
//
//                 var components = t.GetComponents<Component>();
//                 if (componentIndex >= components.Length)
//                     return;
//
//                 var liveComponent = components[componentIndex];
//                 if (liveComponent.name != component.name || liveComponent.GetType() != component.GetType())
//                     return;
//                 LiveReference = liveComponent;
//             }
//         }
//
//         private const string PROGRESS_BAR_TITLE = "Localization Usage Finder";
//
//         private Dictionary<(string collectionName, long entryId), LocalizationEntryUsagesData> _cache = new();
//         public bool IsInitialized { get; private set; } = false;
//
//         public void Refresh()
//         {
//             InitializeResults();
//
//             EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, "Collecting Scene assets...", 0f);
//             var sceneGUIDs = AssetDatabase.FindAssets("t:Scene", new[] { "Assets" });
//
//             EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, "Collecting Prefab assets...", 0f);
//             var prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
//
//             EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, "Collecting ScriptableObject assets...", 0f);
//             var scriptableObjectGUIDs = AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets" });
//
//             var totalCount = sceneGUIDs.Length + prefabGUIDs.Length + scriptableObjectGUIDs.Length;
//
//             for (var sceneIndex = 0; sceneIndex < sceneGUIDs.Length; sceneIndex++)
//             {
//                 var totalIndex = sceneIndex;
//                 var sceneGUID = sceneGUIDs[sceneIndex];
//                 try
//                 {
//                     var path = AssetDatabase.GUIDToAssetPath(sceneGUID);
//                     var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
//                     var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
//
//                     EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, $"({totalIndex + 1} / {totalCount}) Processing scene {sceneIndex + 1} / {sceneGUIDs.Length}: {scene.name}", (float) totalIndex / totalCount);
//
//                     foreach (var t in Object.FindObjectsOfType<Transform>(true))
//                     {
//                         foreach (var c in t.GetComponents<Component>())
//                         {
//                             if (c == null)
//                                 continue;
//                             ExtractTableReferences(c, asset, GetFullPath(c));
//                         }
//                     }
//                 }
//                 catch (Exception ex)
//                 {
//                     Debug.LogException(ex);
//                 }
//             }
//
//             for (var prefabIndex = 0; prefabIndex < prefabGUIDs.Length; prefabIndex++)
//             {
//                 var totalIndex = sceneGUIDs.Length + prefabIndex;
//                 var prefabGUID = prefabGUIDs[prefabIndex];
//                 try
//                 {
//                     var path = AssetDatabase.GUIDToAssetPath(prefabGUID);
//                     var asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
//
//                     EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, $"({totalIndex + 1} / {totalCount}) Processing prefab {prefabIndex + 1} / {prefabGUIDs.Length}: {asset.name}", (float) totalIndex / totalCount);
//
//                     foreach (var t in asset.GetComponentsInChildren<Transform>(true))
//                     {
//                         foreach (var c in t.GetComponents<Component>())
//                         {
//                             if (c == null)
//                                 continue;
//
//                             ExtractTableReferences(c, asset, GetFullPath(c));
//                         }
//                     }
//                 }
//                 catch (Exception ex)
//                 {
//                     Debug.LogException(ex);
//                 }
//             }
//
//             for (var scriptableObjectIndex = 0; scriptableObjectIndex < scriptableObjectGUIDs.Length; scriptableObjectIndex++)
//             {
//                 var totalIndex = sceneGUIDs.Length + prefabGUIDs.Length + scriptableObjectIndex;
//                 var scriptableObjectGUID = scriptableObjectGUIDs[scriptableObjectIndex];
//                 try
//                 {
//                     var path = AssetDatabase.GUIDToAssetPath(scriptableObjectGUID);
//                     var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
//
//                     if (asset == null)
//                         continue;
//
//                     EditorUtility.DisplayProgressBar(PROGRESS_BAR_TITLE, $"({totalIndex + 1} / {totalCount}) Processing scriptable object {scriptableObjectIndex + 1} / {scriptableObjectGUIDs.Length}: {asset.name}", (float) totalIndex / totalCount);
//                     ExtractTableReferences(asset, asset, null);
//                 }
//                 catch (Exception ex)
//                 {
//                     Debug.LogException(ex);
//                 }
//             }
//
//             IsInitialized = true;
//             EditorUtility.ClearProgressBar();
//         }
//
//         public bool TryGetEntryUsages((string collectionName, long entryId) key, out LocalizationEntryUsagesData data)
//         {
//             data = default;
//             return IsInitialized && _cache != null && _cache.TryGetValue(key, out data);
//         }
//
//         private void InitializeResults()
//         {
//             if (_cache == null)
//                 _cache = new();
//             else
//                 _cache.Clear();
//
//             var stringTableCollections = LocalizationEditorSettings.GetStringTableCollections();
//             foreach (var stringTableCollection in stringTableCollections)
//             {
//                 var tableName = stringTableCollection.TableCollectionName;
//                 var defaultTable = stringTableCollection.StringTables[0];
//
//                 foreach (var sharedTableEntry in stringTableCollection.SharedData.Entries)
//                 {
//                     var entryId = sharedTableEntry.Id;
//
//                     var id = (tableName, entryId);
//                     if (_cache.ContainsKey(id))
//                     {
//                         Debug.LogError($"Duplicate localization entry found: {tableName}/{sharedTableEntry.Key} (ID {entryId})");
//                         continue;
//                     }
//
//                     string GetLocalizedValue()
//                     {
//                         if (defaultTable.TryGetValue(entryId, out var defaultValue))
//                             return defaultValue.Value;
//                         return "<not found>";
//                     }
//
//                     _cache.Add(id, new LocalizationEntryUsagesData(tableName, entryId, sharedTableEntry.Key, GetLocalizedValue()));
//                 }
//             }
//         }
//
//         private void ExtractTableReferences(Object unityObject, Object asset, string transformPath)
//         {
//             if (PrefabUtility.IsPartOfPrefabInstance(unityObject))
//                 return; // Ignore in nested prefabs and prefabs in scenes
//
//             try
//             {
//                 var so = new SerializedObject(unityObject);
//                 var itr = so.GetIterator();
//                 while (itr.Next(true))
//                 {
//                     if (itr.type == "LocalizedString" || itr.type == "UnityLocalizedString")
//                     {
//                         var collectionNameProperty = itr.FindPropertyRelative("m_TableReference.m_TableCollectionName");
//                         var collectionName = collectionNameProperty.stringValue;
//
//                         TableReference tableReference;
//                         if (collectionName.StartsWith("GUID:"))
//                             tableReference = Guid.Parse(collectionName.Substring("GUID:".Length, collectionName.Length - "GUID:".Length));
//                         else
//                             tableReference = collectionName;
//
//                         var key = itr.FindPropertyRelative("m_TableEntryReference.m_Key");
//                         var keyId = itr.FindPropertyRelative("m_TableEntryReference.m_KeyId");
//                         TableEntryReference tableEntryReference;
//                         if (keyId.longValue == 0)
//                             tableEntryReference = key.stringValue;
//                         else
//                             tableEntryReference = keyId.longValue;
//
//                         var collection = LocalizationEditorSettings.GetStringTableCollection(tableReference);
//                         var stringTableEntry = collection?.SharedData.GetEntryFromReference(tableEntryReference);
//                         if (stringTableEntry != null)
//                         {
//                             RegisterOccurrence((collection.TableCollectionName, stringTableEntry.Id), asset, unityObject, transformPath, itr.propertyPath, itr.displayName);
//                         }
//                     }
//                 }
//             }
//             catch (Exception ex)
//             {
//                 Debug.LogException(ex);
//             }
//         }
//
//         private void RegisterOccurrence((string TableCollectionName, long Id) key, Object asset, Object component, string componentPath, string propertyPath, string propertyName)
//         {
//             if (!_cache.TryGetValue(key, out var entry))
//             {
//                 entry = new LocalizationEntryUsagesData(key.TableCollectionName, key.Id, "MISSING", "MISSING");
//                 _cache.Add(key, entry);
//             }
//
//             entry.Usages.Add(new UsageEntry(asset, component, componentPath, propertyPath, propertyName));
//         }
//
//         private static string GetFullPath(Component component)
//         {
//             var transform = component.transform;
//             var pathBuilder = new StringBuilder();
//             while(transform != null)
//             {
//                 pathBuilder.Insert(0, transform.name);
//                 pathBuilder.Insert(0, "/");
//                 transform = transform.parent;
//             }
//
//             pathBuilder.Append(" C:" + component.GetType().Name);
//             return pathBuilder.ToString();
//         }
//     }
// }
