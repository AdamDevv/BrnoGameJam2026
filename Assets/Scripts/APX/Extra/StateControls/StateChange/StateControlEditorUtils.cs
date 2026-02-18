#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APX.Extra.Misc;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace APX.Extra.StateControls.StateChange
{
    public static class StateControlEditorUtils
    {
        public static List<ValueDropdownItem<AStateValuePreset>> GetValuePresetDropdown(Object target, List<AStateValuePreset> valuePresets)
        {
            var availableStateValuePresets = ReflectionHelper.GetTypeCacheDerivedClassesOfType<AStateValuePreset>();
            var availableValues = new List<ValueDropdownItem<AStateValuePreset>>();

            foreach (var stateValuePreset in availableStateValuePresets)
            {
                if (!stateValuePreset.IsValidFor(target) || ContainsSamePreset(valuePresets, stateValuePreset))
                    continue;

                availableValues.Add(new ValueDropdownItem<AStateValuePreset>(stateValuePreset.FieldName, stateValuePreset));
            }

            return availableValues;
        }

        public static List<ValueDropdownItem<AStateValuePreset>> GetValuePresetDropdown(Type targetType, List<AStateValuePreset> valuePresets)
        {
            var availableStateValuePresets = ReflectionHelper.GetTypeCacheDerivedClassesOfType<AStateValuePreset>();
            var availableValues = new List<ValueDropdownItem<AStateValuePreset>>();

            foreach (var stateValuePreset in availableStateValuePresets)
            {
                if (!stateValuePreset.IsValidForType(targetType) || ContainsSamePreset(valuePresets, stateValuePreset))
                    continue;

                availableValues.Add(new ValueDropdownItem<AStateValuePreset>(stateValuePreset.FieldName, stateValuePreset));
            }

            return availableValues;
        }

        public static void CustomAddFunction(Object target, List<AStateValuePreset> valuePresets)
        {
            var availableStateValuePresets = ReflectionHelper.GetTypeCacheDerivedClassesOfType<AStateValuePreset>();
            List<ValueDropdownItem<AStateValuePreset>> availableValues = null;

            for (var i = 0; i < availableStateValuePresets.Count; i++)
            {
                var stateValuePreset = availableStateValuePresets[i];
                if (stateValuePreset.IsValidFor(target))
                {
                    if (!ContainsSamePreset(valuePresets, stateValuePreset))
                    {
                        if (availableValues == null) availableValues = new List<ValueDropdownItem<AStateValuePreset>>();
                        availableValues.Add(new ValueDropdownItem<AStateValuePreset>(stateValuePreset.FieldName, stateValuePreset));
                    }
                }
            }

            var selector = new GenericSelector<ValueDropdownItem<AStateValuePreset>>("Value Presets", false, x => x.Text, availableValues);
            selector.SetSelection(null);
            selector.SelectionTree.Config.DrawSearchToolbar = false;
            // selector.SelectionTree.DefaultMenuStyle.Height = 22;
            selector.EnableSingleClickToSelect();
            selector.SelectionConfirmed += selection => valuePresets.Add(selection.FirstOrDefault().Value);
            selector.ShowInPopup();
        }


        public static void PickTarget(Action<Object> setter, Object target)
        {
            if (target is GameObject gameObjectTarget)
            {
                IList<ValueDropdownItem<Object>> availableValues = new List<ValueDropdownItem<Object>>();

                availableValues.Add(new ValueDropdownItem<Object>($"As GameObject ({gameObjectTarget.name})", target));

                var components = gameObjectTarget.GetComponents<Component>();

                for (var i = 0; i < components.Length; i++)
                {
                    var component = components[i];
                    availableValues.Add(new ValueDropdownItem<Object>(component.GetType().Name, component));
                }

                var selector =
                    new GenericSelector<ValueDropdownItem<Object>>("Choose target GameObject or Component", false, x => x.Text, availableValues);
                selector.SetSelection(null);
                selector.SelectionTree.Config.DrawSearchToolbar = false;
                // selector.SelectionTree.DefaultMenuStyle.Height = 22;
                selector.EnableSingleClickToSelect();
                selector.SelectionConfirmed += selection => { setter.Invoke(selection.FirstOrDefault().Value); };
                selector.ShowInPopup();
            }
        }

        public static bool ContainsSamePreset(List<AStateValuePreset> valuePresets, AStateValuePreset valuePreset)
        {
            if (valuePresets != null)
            {
                for (var i = 0; i < valuePresets.Count; i++)
                {
                    var aStateValuePreset = valuePresets[i];
                    if (aStateValuePreset.GetType().Equals(valuePreset.GetType()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool CheckForValuePresetsWarnings(Object target, List<AStateValuePreset> valuePresets, out string message)
        {
            message = null;
            StringBuilder stringBuilder = null;

            if (target == null)
            {
                stringBuilder = new StringBuilder("Set target object");
            }

            if (valuePresets != null && valuePresets.Count > 0)
            {
                for (var i = 0; i < valuePresets.Count; i++)
                {
                    var stateValuePreset = valuePresets[i];
                    if (stateValuePreset != null && !stateValuePreset.IsValidFor(target))
                    {
                        if (stringBuilder == null)
                        {
                            stringBuilder = new StringBuilder();
                            stringBuilder.Append("Target '");
                            stringBuilder.Append(target != null ? target.GetType().Name : "[null]");
                            stringBuilder.Append("' cannot use value preset:\n");
                        }
                        else
                        {
                            stringBuilder.Append("\n");
                        }

                        stringBuilder.Append(stateValuePreset.FieldName);
                    }
                }
            }
            else
            {
                if (stringBuilder == null)
                {
                    stringBuilder = new StringBuilder();
                }
                else
                {
                    stringBuilder.Append("\n");
                }

                stringBuilder.Append("Add at least one state preset");
            }

            if (stringBuilder != null) message = stringBuilder.ToString();

            return stringBuilder != null;
        }

        public static bool CheckForValuePresetsWarnings(Type targetType, List<AStateValuePreset> valuePresets, out string message)
        {
            message = null;
            StringBuilder stringBuilder = null;

            if (targetType == null)
            {
                stringBuilder = new StringBuilder("Set target object");
            }

            if (valuePresets != null && valuePresets.Count > 0)
            {
                for (var i = 0; i < valuePresets.Count; i++)
                {
                    var stateValuePreset = valuePresets[i];
                    if (stateValuePreset != null && !stateValuePreset.IsValidForType(targetType))
                    {
                        if (stringBuilder == null)
                        {
                            stringBuilder = new StringBuilder();
                            stringBuilder.Append("Target '");
                            stringBuilder.Append(targetType.Name);
                            stringBuilder.Append("' cannot use value preset:\n");
                        }
                        else
                        {
                            stringBuilder.Append("\n");
                        }

                        stringBuilder.Append(stateValuePreset.FieldName);
                    }
                }
            }
            else
            {
                if (stringBuilder == null)
                {
                    stringBuilder = new StringBuilder();
                }
                else
                {
                    stringBuilder.Append("\n");
                }

                stringBuilder.Append("Add at least one state preset");
            }

            if (stringBuilder != null) message = stringBuilder.ToString();

            return stringBuilder != null;
        }

        public static bool CheckUnusablePresetsWarnings(Object target, List<AStateValuePreset> valuePresets, out string message)
        {
            message = null;
            StringBuilder stringBuilder = null;

            if (valuePresets != null && valuePresets.Count > 0)
            {
                for (var i = 0; i < valuePresets.Count; i++)
                {
                    var stateValuePreset = valuePresets[i];
                    if (stateValuePreset == null)
                    {
                        stringBuilder = new StringBuilder();
                        stringBuilder.Append("Preset at index '");
                        stringBuilder.Append(i);
                        stringBuilder.Append("' is null!");
                    }
                    else if (!stateValuePreset.IsValidFor(target))
                    {
                        stringBuilder = new StringBuilder();
                        stringBuilder.Append("Target '");
                        stringBuilder.Append(target != null ? target.GetType().Name : "[null]");
                        stringBuilder.Append("' cannot use value preset:\n");
                        stringBuilder.Append(stateValuePreset.FieldName);
                    }
                }
            }

            if (stringBuilder != null) message = stringBuilder.ToString();

            return stringBuilder != null;
        }

        public static bool CheckForObjectPresetsWarnings(List<ObjectStatePreset> objectPresets, out string message)
        {
            message = null;
            StringBuilder stringBuilder = null;
            if (objectPresets != null && objectPresets.Count > 1)
            {
                for (var i = 0; i < objectPresets.Count - 1; i++)
                {
                    var objectStatePreset1 = objectPresets[i];
                    for (var j = i + 1; j < objectPresets.Count; j++)
                    {
                        var objectStatePreset2 = objectPresets[j];
                        if (ReferenceEquals(objectStatePreset1.Target, objectStatePreset2.Target) && objectStatePreset1.Target != null)
                        {
                            if (stringBuilder == null)
                            {
                                stringBuilder = new StringBuilder("Duplicate target objects found:\n");
                            }
                            else
                            {
                                stringBuilder.Append('\n');
                            }

                            stringBuilder.Append(objectStatePreset1.Target.GetType().Name);
                        }
                    }
                }

                for (var i = 0; i < objectPresets.Count; i++)
                {
                    var objectStatePreset1 = objectPresets[i];
                    if (objectStatePreset1.Target == null)
                    {
                        if (stringBuilder == null)
                        {
                            stringBuilder = new StringBuilder("Contains empty target object!");
                        }
                        else
                        {
                            stringBuilder.Append("\nContains empty target object!");
                        }

                        break;
                    }
                }

                for (var i = 0; i < objectPresets.Count; i++)
                {
                    var objectStatePreset1 = objectPresets[i];
                    if (objectStatePreset1.ValuePresets == null || objectStatePreset1.ValuePresets.Count == 0)
                    {
                        if (stringBuilder == null)
                        {
                            stringBuilder = new StringBuilder();
                        }
                        else
                        {
                            stringBuilder.Append("\n");
                        }

                        stringBuilder.Append("Target '");
                        stringBuilder.Append(objectStatePreset1.Target != null ? objectStatePreset1.Target.GetType().Name : "[null]");
                        stringBuilder.Append("' has no states defined!");
                    }
                }

                if (stringBuilder != null) message = stringBuilder.ToString();
            }

            return stringBuilder != null;
        }
    }
}
#endif
