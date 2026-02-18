// // Author: Jan Krejsa
// // Created: 03.05.2024
// // Copyright (c) Noxgames
// // http://www.noxgames.com/
//
// using Sirenix.OdinInspector.Editor;
// using UnityEditor.SceneManagement;
// using UnityEngine.SceneManagement;
//
// namespace Sirenix.OdinInspector.Modules.Localization.Editor
// {
//     public class ViewLocalizationEntryUsagesWindow: OdinEditorWindow
//     {
//         [ShowInInspector]
//         [InlineProperty]
//         [HideLabel]
//         [HideReferenceObjectPicker]
//         [ShowIf("_data")]
//         private LocalizationUsages.LocalizationEntryUsagesData _data;
//
//         private static string GetLabelText(LocalizationUsages.LocalizationEntryUsagesData data)
//         {
//             return data != null ? $"Usages of {data.Table}/{data.Key}:" : "Missing...";
//         }
//
//         public static void OpenWindow(LocalizationUsages.LocalizationEntryUsagesData data)
//         {
//             var windowLabel = GetLabelText(data);
//             var window = GetWindow<ViewLocalizationEntryUsagesWindow>(windowLabel);
//             window.SetData(data);
//         }
//
//         private void SetData(LocalizationUsages.LocalizationEntryUsagesData data)
//         {
//             _data = data;
//             if (_data != null)
//                 _data.RefreshLiveReferences();
//         }
//
//         [OnInspectorInit]
//         private void OnInspectorInit()
//         {
//             EditorSceneManager.activeSceneChangedInEditMode -= OnSceneChanged;
//             EditorSceneManager.activeSceneChangedInEditMode += OnSceneChanged;
//
//             PrefabStage.prefabStageOpened -= OnPrefabStageChanged;
//             PrefabStage.prefabStageOpened += OnPrefabStageChanged;
//
//             PrefabStage.prefabStageClosing -= OnPrefabStageChanged;
//             PrefabStage.prefabStageClosing += OnPrefabStageChanged;
//
//
//             if (_data != null)
//                 _data.RefreshLiveReferences();
//         }
//
//         [OnInspectorDispose]
//         private void OnInspectorDispose()
//         {
//             EditorSceneManager.activeSceneChangedInEditMode -= OnSceneChanged;
//         }
//
//         private void OnSceneChanged(Scene arg0, Scene arg1)
//         {
//             if (_data != null)
//                 _data.RefreshLiveReferences();
//         }
//
//
//         private void OnPrefabStageChanged(PrefabStage obj)
//         {
//             if (_data != null)
//                 _data.RefreshLiveReferences();
//         }
//     }
// }
