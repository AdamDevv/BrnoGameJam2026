#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using APX.Extra.EditorUtils;
using APX.Extra.OdinExtensions;
using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.GameManagement
{
    [Serializable]
    public abstract class ACollectionManagementPanel<TElement> : AManagementPanel where TElement : ScriptableObject
    {
        [HorizontalGroup("Collection")]
        [EnhancedBoxGroup("Collection/List", Order = -50)]
        [ShowInInspector]
        [LabelText("Search")]
        [LabelWidth(60)]
        [PropertyOrder(100)]
        [OnValueChanged(nameof(RefreshFilteredElements))]
        protected virtual string SearchFilter
        {
            get => _searchFilter; 
            set => _searchFilter = value;
        }

        [EnhancedBoxGroup("Collection/List")]
        [PropertyOrder(100)]
        [ShowInInspector]
        [CustomValueDrawer(nameof(ListElementDrawer))]
        [ListDrawerSettings(ShowFoldout = false, IsReadOnly = true, DraggableItems = false, OnTitleBarGUI = nameof(OnElementsTitlebarGUI), ShowItemCount = false, NumberOfItemsPerPage = 25)]
        public virtual List<TElement> Elements
        {
            get => _elements; 
            set => _elements = value;
        }

        protected List<TElement> _elements = new List<TElement>();

        protected virtual bool ShowDuplicateButton => true;
        protected virtual bool ShowIconBeforeElement => false;

        private AsyncAssetsFinder<TElement> _assetsFinder;
        protected string _searchFilter;
        private AsyncAssetsFinder<TElement> AssetsFinder => _assetsFinder ??= new AsyncAssetsFinder<TElement>(RefreshFilteredElements, RawElementFilterPredicate);

        
        protected virtual bool AssetFilter(TElement element)
        {
            return string.IsNullOrWhiteSpace(SearchFilter) || FuzzySearch.Contains(SearchFilter, element.name);
        }

        protected void RefreshFilteredElements()
        {
            Elements = AssetsFinder.CurrentElements.Where(AssetFilter).ToList();
            OnElementsRefreshed();
        }

        protected virtual void OnElementsRefreshed()
        {

        }

        protected void Rescan()
        {
            AssetsFinder.SearchAssetsAsync();
        }

        protected virtual TElement ListElementDrawer(TElement value, GUIContent label, Func<GUIContent, bool> callNextDrawer)
        {
            EditorGUILayout.BeginHorizontal();

            if (ShowIconBeforeElement)
            {
                (Texture texture, Color? color) iconData = GetIconDataForElement(value);

                if (iconData.texture is not null)
                {
                    var iconRect = GUILayoutUtility.GetRect(19, 19, GUILayout.Width(19));

                    var prevColor = GUI.color;
                    GUI.color = iconData.color ?? GUI.color;

                    GUI.DrawTexture(iconRect, iconData.texture);
                    GUI.color = prevColor;
                }
            }

            ListElementValueDrawer(value, label, callNextDrawer);

            GUILayout.Space(4);

            if (ShowDuplicateButton)
            {
                var copyRect = GUILayoutUtility.GetRect(18, 18, SirenixGUIStyles.Button,  GUILayoutOptions.ExpandWidth(false).Width(18));
                if (SirenixEditorGUI.IconButton(copyRect, FontAwesomeEditorIcons.CopySolid, "Create Copy"))
                {
                    EditorApplication.delayCall += () =>
                    {
                        var duplicate = OdinExtensionUtils.DuplicateAsset(value);
                        AssetsFinder.CurrentElements.Add(duplicate);
                        OnNewElement(duplicate);
                        RefreshFilteredElements();
                    };
                }
                GUILayout.Space(4);
            }
            
            var deleteRect = GUILayoutUtility.GetRect(18, 18, SirenixGUIStyles.Button,  GUILayoutOptions.ExpandWidth(false).Width(18));
            if (SirenixEditorGUI.IconButton(deleteRect, EditorIcons.X, "Delete Record"))
            {
                if (EditorUtility.DisplayDialog("Delete asset", $"Do you really want to remove asset {value.name}?", "Yes", "No"))
                {
                    EditorApplication.delayCall += () =>
                    {
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(value));
                        AssetsFinder.CurrentElements.Remove(value);
                        OnDeletedElement(value);
                        RefreshFilteredElements();
                    };
                }
            }
            EditorGUILayout.EndHorizontal();
            return value;
        }

        protected virtual void ListElementValueDrawer(TElement value, GUIContent label, Func<GUIContent, bool> callNextDrawer)
        {
            callNextDrawer(label);
        }
        
        [OnInspectorInit]
        protected virtual void OnInspectorInit()
        {
            Rescan();
        }

        protected void OnElementsTitlebarGUI()
        {
            if (!AssetsFinder.IsSearching)
            {
                GUILayout.Label($"Count: {Elements.Count}", SirenixGUIStyles.CenteredGreyMiniLabel);
            }
            else
            {
                GUILayout.Label($"Scanning {AssetsFinder.CurrentSearchingIndex}/{AssetsFinder.NumberOfResultsToSearch}", SirenixGUIStyles.RightAlignedGreyMiniLabel);
            }

            GUIHelper.PushGUIEnabled(!AssetsFinder.IsSearching);

            OnElementsTitlebarCustomGUI();
            
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh) && !AssetsFinder.IsSearching)
            {
                Rescan();
            }

            if (AssetUtilities.CanCreateNewAsset<TElement>())
            {
                if (SirenixEditorGUI.ToolbarButton(EditorIcons.Plus) && !AssetsFinder.IsSearching)
                {
                    var element = Elements.LastOrDefault() ?? AssetsFinder.CurrentElements.LastOrDefault();
                    var path = element != null ? AssetUtilities.GetAssetLocation(element) : null;
                    OdinExtensionUtils.DrawSubtypeDropDownOrCall(typeof(TElement), type =>
                    {
                        EditorApplication.delayCall += () =>
                        {
                            var newInstance = OdinExtensionUtils.CreateNewInstanceOfType<TElement>(type, path);
                            EditorGUIUtility.PingObject(newInstance);
                            OnNewElement(newInstance);
                            Rescan();
                        };
                    });
                }
            }

            GUIHelper.PopGUIEnabled();
        }

        protected virtual void OnElementsTitlebarCustomGUI()
        {
            
        }

        protected virtual (Texture texture, Color? color) GetIconDataForElement(TElement element)
        {
            return (FontAwesomeEditorIcons.CircleRegular.Inactive, null);
        }

        protected virtual void OnNewElement(TElement newElement)
        {
            
        }

        protected virtual void OnDeletedElement(TElement deletedElement)
        {
            
        }

        /// <summary>
        /// Additional filter to decide whether this element should be included in raw elements
        /// </summary>
        protected virtual bool RawElementFilterPredicate(TElement element)
        {
            return true;
        }
    }
}

#endif