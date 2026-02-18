using System;
using System.Collections.Generic;
using APX.Extra.DataStructures;
using APX.Extra.Misc;
using APX.Extra.OdinExtensions.Attributes;
using APX.GlobalLocating.Abstractions;
using APX.ObjectLocating.Util;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
#endif

namespace APX.GlobalLocating.Models
{
    [Serializable]
    internal class GlobalLocatorRecord
    {
        private static readonly Dictionary<Type, ObjectRecordType> _objectRecordsTypeDictionary = new();

        [SerializeField]
        [CustomValueDrawer("TypeCustomValueDrawer")]
        private UType _Type;

        [SerializeField]
        [ShowCreateNew(GetOverrideTypeFunc = "@this.Type", DefaultPath = "Assets/Resources/GlobalLocatorObjects",
            OverrideDefaultName = "@this.Type.Type.Name")]
        private AGlobalLocatorObject _Object;

        public UType Type => _Type;

        public AGlobalLocatorObject Object
        {
            get => _Object;
            set => _Object = value;
        }

        public GlobalLocatorRecord(UType type, AGlobalLocatorObject obj)
        {
            _Type = type;
            _Object = obj;
        }

        #if UNITY_EDITOR

        public ObjectRecordType ObjectRecordTypeEditorOnly => GetObjectType(_Type);

        [UsedImplicitly]
        private UType TypeCustomValueDrawer(UType value, GUIContent label, Func<GUIContent, bool> callNextDrawer,
            InspectorProperty property)
        {
            var objectType = GetObjectType(value);

            string objectTypeLabel = objectType switch
            {
                ObjectRecordType.Provider => GlobalLocatorConstants.OBJECT_TYPE_LABEL_PROVIDER,
                ObjectRecordType.Settings => GlobalLocatorConstants.OBJECT_TYPE_LABEL_SETTINGS,
                _ => GlobalLocatorConstants.OBJECT_TYPE_LABEL_UNKNOWN
            };


            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(objectTypeLabel, GlobalLocatorConstants.OBJECT_TYPE_LABEL_GUI_STYLE, GUILayout.Width(20));
            EditorGUILayout.LabelField(value?.Type?.Name.Nicify() ?? "Unknown type");

            EditorGUILayout.EndHorizontal();

            return value;
        }

        private static ObjectRecordType GetObjectType(UType value)
        {
            ObjectRecordType objectRecordType = ObjectRecordType.Unknown;

            if (value?.Type is not null && !_objectRecordsTypeDictionary.TryGetValue(value.Type, out objectRecordType))
            {
                for (var type = value.Type; type is not null; type = type.BaseType)
                {
                    if (!type.IsAbstract)
                    {
                        continue;
                    }

                    Type genericTypeDefinition = type.IsGenericType ? type.GetGenericTypeDefinition() : null;

                    if (genericTypeDefinition == typeof(AGlobalLocatorDefinitionsProvider<,>))
                    {
                        objectRecordType = ObjectRecordType.Provider;
                        break;
                    }

                    if (genericTypeDefinition == typeof(AGlobalLocatorObject<>))
                    {
                        objectRecordType = ObjectRecordType.Settings;
                        break;
                    }
                }
            }

            return objectRecordType;
        }

        #endif
    }
}
