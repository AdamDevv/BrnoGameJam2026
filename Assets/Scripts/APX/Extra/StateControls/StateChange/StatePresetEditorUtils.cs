#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using APX.Extra.EditorUtils;
using APX.Extra.StateControls.StateChange.StateValuePresets;
using APX.Extra.StateControls.Toggle;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.StateControls.StateChange
{
    public static class StatePresetEditorUtils
    {
        [MenuItem("CONTEXT/MultiStateGameObjectBehaviour/Convert to Presets", false, 10000)]
        public static void ConvertToPresets(MenuCommand command)
        {
            if (command.context is not MultiStateGameObjectBehaviour behaviour) 
                return;

            var newStatePresets = new List<StatePreset>();
            var allTargets = behaviour._StatePresets.SelectMany(s => s.Targets).ToList();
            foreach (var statePreset in behaviour._StatePresets)
            {
                var newObjectPresets = new List<ObjectStatePreset>();
                foreach (var target in allTargets)
                {
                    var newPreset = new ObjectStatePreset(target);
                    var valuePreset = new GameObjectActivePreset
                    {
                        PresetState = statePreset.Targets.Contains(target)
                    };
                    newPreset._ValuePresets = new List<AStateValuePreset>{valuePreset};
                    newObjectPresets.Add(newPreset);
                }
                newStatePresets.Add(new StatePreset(){StateID = statePreset.StateID, ObjectPresets = newObjectPresets});
            }

            var newBehaviour = behaviour.ChangeScriptType<MultiStatePresetBehaviour>();
            newBehaviour._StatePresets = newStatePresets;
            EditorUtility.SetDirty(newBehaviour);
        }

        [MenuItem("CONTEXT/MultiStatePresetBehaviour/Convert to GameObjects", false, 10000)]
        public static void ConvertToGameObjects(MenuCommand command)
        {
            if (command.context is not MultiStatePresetBehaviour behaviour)
                return;

            var newPresets = new List<GameObjectItem>();
            foreach (var statePreset in behaviour._StatePresets)
            {
                var enabledObjects = new List<GameObject>();
                foreach (var objectPreset in statePreset.ObjectPresets)
                {
                    if (objectPreset.Target is GameObject go && objectPreset.ValuePresets.Count == 1 && objectPreset.ValuePresets[0] is GameObjectActivePreset goPreset)
                    {
                        if (goPreset.PresetState)
                        {
                            enabledObjects.Add(objectPreset.Target as GameObject);
                        }
                    }
                    else
                    {
                        Debug.LogError($"Preset {statePreset.StateID} contains value preset that is not for GameObject!");
                        return;
                    }
                }
                newPresets.Add(new GameObjectItem(statePreset.StateID, enabledObjects));
            }
            
            var newBehaviour = behaviour.ChangeScriptType<MultiStateGameObjectBehaviour>();
            newBehaviour._StatePresets = newPresets;
            EditorUtility.SetDirty(newBehaviour);
        }

        [MenuItem("CONTEXT/ToggleActiveObjects/Convert to Presets", false, 10000)]
        public static void ConvertToggleToPresets(MenuCommand command)
        {
            if (command.context is not ToggleActiveObjects behaviour)
                return;

            var newStatePresets = new List<ObjectToggleStatePreset>();
            foreach (var onTarget in behaviour._OnTargets)
            {
                var newPreset = new ObjectToggleStatePreset(onTarget)
                {
                    _OnStateValuePresets = new List<AStateValuePreset>{new GameObjectActivePreset { PresetState = true }},
                    _OffStateValuePresets = new List<AStateValuePreset>{new GameObjectActivePreset { PresetState = false }}
                };
                newStatePresets.Add(newPreset);
            }
            foreach (var offTarget in behaviour._OffTargets)
            {
                var newPreset = new ObjectToggleStatePreset(offTarget)
                {
                    _OnStateValuePresets = new List<AStateValuePreset>{new GameObjectActivePreset { PresetState = false }},
                    _OffStateValuePresets = new List<AStateValuePreset>{new GameObjectActivePreset { PresetState = true }}
                };
                newStatePresets.Add(newPreset);
            }

            var newBehaviour = behaviour.ChangeScriptType<ToggleState>();
            newBehaviour._ObjectToggleStatePreset = newStatePresets;
            EditorUtility.SetDirty(newBehaviour);
        }

        [MenuItem("CONTEXT/ToggleState/Convert to Active Objects", false, 10000)]
        public static void ConvertToggleToActiveObjects(MenuCommand command)
        {
            if (command.context is not ToggleState behaviour)
                return;

            var onObjects = new List<GameObject>();
            var offObjects = new List<GameObject>();

            foreach (var statePreset in behaviour._ObjectToggleStatePreset)
            {
                if (statePreset.Target is GameObject go &&
                    statePreset._OnStateValuePresets.Count == 1 && statePreset._OnStateValuePresets[0] is GameObjectActivePreset onPreset &&
                    statePreset._OffStateValuePresets.Count == 1 && statePreset._OffStateValuePresets[0] is GameObjectActivePreset offPreset)
                {
                    if (onPreset.PresetState)
                    {
                        onObjects.Add(go);
                    }
                    if (offPreset.PresetState)
                    {
                        offObjects.Add(go);
                    }
                }
                else
                {
                    Debug.LogError("Preset contains value preset that is not for GameObject!");
                    return;
                }
            }
            var newBehaviour = behaviour.ChangeScriptType<ToggleActiveObjects>();
            newBehaviour._OnTargets = onObjects;
            newBehaviour._OffTargets = offObjects;
            EditorUtility.SetDirty(newBehaviour);
        }
    }
}
#endif
