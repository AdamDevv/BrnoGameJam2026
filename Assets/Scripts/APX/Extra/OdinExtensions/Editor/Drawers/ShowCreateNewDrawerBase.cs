using System;
using APX.Extra.Misc;
using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ActionResolvers;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using NamedValue = Sirenix.OdinInspector.Editor.ActionResolvers.NamedValue;

namespace APX.Extra.OdinExtensions.Editor.Drawers
{
    [DrawerPriority(DrawerPriorityLevel.WrapperPriority)]
    public abstract class ShowCreateNewDrawerBase<T> : OdinAttributeDrawer<ShowCreateNewAttribute, T>
    {
        protected ValueResolver<Type> _overrideTypeResolver;
        protected ValueResolver<bool> _showIfResolver;
        protected ValueResolver<string> _overrideDefaultNameResolver;
        protected ValueResolver<string> _defaultPathResolver;
        protected ActionResolver _onCreatedNewResolver;

        protected override void Initialize()
        {
            _overrideTypeResolver = ValueResolver.Get(Property, Attribute.GetOverrideTypeFunc, Attribute.OverrideType);

            if (!string.IsNullOrEmpty(Attribute.ShowIf))
                _showIfResolver = ValueResolver.Get<bool>(Property, Attribute.ShowIf);

            if (!string.IsNullOrEmpty(Attribute.OverrideDefaultName))
                _overrideDefaultNameResolver = ValueResolver.GetForString(Property, Attribute.OverrideDefaultName);

            if (!string.IsNullOrEmpty(Attribute.DefaultPath))
                _defaultPathResolver = ValueResolver.GetForString(Property, Attribute.DefaultPath);

            if (!string.IsNullOrEmpty(Attribute.OnCreatedNew))
                _onCreatedNewResolver = ActionResolver.Get(Property, Attribute.OnCreatedNew, new NamedValue[] {new("value", typeof(T))});
        }
        
        protected override void DrawPropertyLayout(GUIContent label)
        {
            ValueResolver.DrawErrors(_overrideTypeResolver, _showIfResolver, _overrideDefaultNameResolver, _defaultPathResolver);
            ActionResolver.DrawErrors(_onCreatedNewResolver);
            
            EditorGUILayout.BeginHorizontal();
            CallNextDrawer(label);
            if (_showIfResolver == null || _showIfResolver.HasError || _showIfResolver.GetValue())
            {
                var rect = GUILayoutUtility.GetRect(18, 18, SirenixGUIStyles.Button, GUILayoutOptions.ExpandWidth(false).Width(18));
                if (SirenixEditorGUI.IconButton(rect, FontAwesomeEditorIcons.FileMedicalSolid, "Create new"))
                {
                    var baseType = GetBaseType();
                    OdinExtensionUtils.DrawSubtypeDropDownOrCall(baseType, CreateInstance);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        protected string GetDefaultName(Type type)
        {
            if (_overrideDefaultNameResolver != null && !_overrideDefaultNameResolver.HasError)
                return _overrideDefaultNameResolver.GetValue();
            if (type != null)
                return $"New {type.Name}";
            
            return $"New {typeof(T).Name}";
        }

        protected virtual Type GetBaseType() => _overrideTypeResolver != null && !_overrideTypeResolver.HasError ? _overrideTypeResolver.GetValue() : null;
        protected string GetDefaultPath() => _defaultPathResolver != null && !_defaultPathResolver.HasError ? PathUtility.GetAbsolutePath(_defaultPathResolver.GetValue()) : Application.dataPath;
        private void CreateInstance(Type type)
        {
            if (TryCreateInstance(type, GetDefaultPath(), GetDefaultName(type), out var instance))
            {
                ValueEntry.SmartValue = instance;
                ValueEntry.ApplyChanges();

                if (_onCreatedNewResolver != null && !_onCreatedNewResolver.HasError)
                {
                    _onCreatedNewResolver.Context.NamedValues.Set("value", instance);
                    _onCreatedNewResolver.DoAction();
                }

            }
        }

        protected abstract bool TryCreateInstance(Type type, string defaultPath, string defaultName, out T result);
    }
}
