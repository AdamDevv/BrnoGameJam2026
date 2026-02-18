#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using APX.Extra.EditorUtils;
using APX.Extra.OdinExtensions;
using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace APX.Extra.GameManagement
{
    [Serializable]
    public abstract class ATableCollectionManagementPanel<TElement, TElementWrapper> : AManagementPanel
        where TElement : ScriptableObject
        where TElementWrapper : TableCollectionManagementElement<TElement>
    {
        [HorizontalGroup("Collection")]
        [EnhancedBoxGroup("Collection/List", Order = -50)]
        [ShowInInspector]
        [LabelText("Search")]
        [LabelWidth(60)]
        [PropertyOrder(100)]
        [OnValueChanged(nameof(RefreshFilteredElements))]
        protected virtual string SearchFilter { get => _searchFilter; set => _searchFilter = value; }

        [EnhancedBoxGroup("Collection/List")]
        [PropertyOrder(100)]
        [ShowInInspector]
        [EnhancedTableList(AlwaysExpanded = true, IsReadOnly = true, OnTitleBarGUI = nameof(OnElementsTitlebarGUI), NumberOfItemsPerPage = 25)]
        public virtual List<TElementWrapper> Elements
        {
            get => _elements;
            set => _elements = value;
        }

        protected List<TElementWrapper> _elements = new List<TElementWrapper>();

        protected virtual bool ShowDuplicateButton => true;

        private AsyncAssetsFinder<TElement> _assetsFinder;
        protected string _searchFilter;
        private AsyncAssetsFinder<TElement> AssetsFinder => _assetsFinder ??= new AsyncAssetsFinder<TElement>(RefreshFilteredElements, RawElementFilterPredicate);

        protected virtual bool AssetFilter(TElement element)
        {
            return string.IsNullOrWhiteSpace(SearchFilter) || FuzzySearch.Contains(SearchFilter, element.name);
        }

        protected abstract TElementWrapper CreateWrapper(TElement element);

        protected void RefreshFilteredElements()
        {
            Elements = AssetsFinder.CurrentElements.Where(AssetFilter).Select(CreateWrapper).ToList();
            OnElementsRefreshed();
        }

        protected virtual void OnElementsRefreshed()
        {

        }

        protected void Rescan()
        {
            AssetsFinder.SearchAssetsAsync();
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

    [Serializable]
    public abstract class TableCollectionManagementElement<TElement> where TElement : ScriptableObject
    {
        [ShowIf(nameof(ShowElement))]
        [SerializeField]
        private TElement _Element;
        public TElement Element => _Element;

        protected virtual bool ShowElement => true;

        protected TableCollectionManagementElement(TElement element)
        {
            _Element = element;
        }

        public static implicit operator TElement(TableCollectionManagementElement<TElement> wrapper) => wrapper.Element;
    }
}

#endif