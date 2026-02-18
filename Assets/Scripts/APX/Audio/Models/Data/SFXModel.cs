using APX.Audio.Models.Data.Variations;
using APX.Audio.Utils;
using APX.Extra.OdinExtensions;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using APX.Extra.Misc;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif

namespace APX.Audio.Models.Data
{
    [Serializable]
    public class SFXModel
    {
        [FoldoutGroup("Additional Settings", expanded: false)]
        [SerializeField]
        private bool _HasRange;

        [HideIf(nameof(_HasRange), Animate = false)]
        [SerializeField]
        [HideLabel]
        private SFXAudioClipVariation _soloSFXAudioClipData;

        [ShowIf(nameof(_HasRange), Animate = false)]
        [SerializeField]
        [CustomValueDrawer("CustomRangeAudioClipDataListDrawer")]
        private SFXAudioClipVariationInRange[] _RangeAudioClipDataList;

        public SFXInstance GetSFXInstance()
        {
            SFXAudioClipVariation audioClipData;

            if (_HasRange)
            {
                #if UNITY_EDITOR

                if (_RangeAudioClipDataList.Length == 0)
                {
                    Debug.LogWarning("No audio clip data found in range.");
                    audioClipData = null;
                }
                else if (_RangeAudioClipDataList.TryGetFirst(e => e.DebugSolo, out var soloAudioClipData))
                {
                    audioClipData = soloAudioClipData;
                }
                else
                {
                    audioClipData = _RangeAudioClipDataList[Random.Range(0, _RangeAudioClipDataList.Length)];
                }

                #else

                audioClipData = _RangeAudioClipDataList[Random.Range(0, _RangeAudioClipDataList.Length)];

                #endif
            }
            else
            {
                audioClipData = _soloSFXAudioClipData;
            }


            return audioClipData?.GetSFXInstance();
        }

        #if UNITY_EDITOR
        [OnInspectorGUI]
        private void DrawControls()
        {
            var buttonStyle = new GUIStyle(SirenixGUIStyles.Button)
            {
                padding = new RectOffset(4, 4, 4, 4),
            };

            GUILayout.Space(5f);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            SirenixEditorGUI.BeginBox();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(FontAwesomeEditorIcons.PlaySolid.Highlighted, buttonStyle, GUILayout.Width(22), GUILayout.Height(22)))
            {
                var instance = GetSFXInstance();
                EditorAudioUtils.PlayPreview(instance.AudioClip, instance.Volume, instance.Pitch, instance.MixerGroup);
            }

            if (GUILayout.Button(FontAwesomeEditorIcons.StopSolid.Highlighted, buttonStyle, GUILayout.Width(22), GUILayout.Height(22)))
            {
                EditorAudioUtils.StopPreview();
            }

            GUILayout.EndHorizontal();
            SirenixEditorGUI.EndBox();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(2f);
        }

        private SFXAudioClipVariationInRange CustomRangeAudioClipDataListDrawer(SFXAudioClipVariationInRange value, GUIContent label,
            Func<GUIContent, bool> callNextDrawer, InspectorProperty property)
        {
            var soloElement = _RangeAudioClipDataList.FirstOrDefault(e => e.DebugSolo);
            var isDisabledBySolo = soloElement is not null && soloElement != value;

            EditorGUILayout.BeginHorizontal();

            Rect rect = GUILayoutUtility.GetRect(200, 20, GUILayout.Width(20));
            var debugSolo = GUI.Toggle(rect, value.DebugSolo, new GUIContent("", "Solo (debug)"));

            if (debugSolo)
            {
                _RangeAudioClipDataList.ForEach(e => e.DebugSolo = false);
            }

            value.DebugSolo = debugSolo;

            GUI.enabled = !isDisabledBySolo;

            EditorGUILayout.BeginVertical();
            callNextDrawer(label);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            GUI.enabled = true;

            return value;
        }
        #endif
    }
}
