#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using APX.Extra.EditorUtils;
using APX.Extra.OdinExtensions.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Extra.GameManagement
{
    [Serializable]
    public abstract class ACollectionSelectorManagementPanel<TElement> : ACollectionManagementPanel<TElement>, ISelectorManagementPanel where TElement : ScriptableObject
    {
        [HorizontalGroup("Collection", 280)]
        [EnhancedBoxGroup("Collection/List", Order = -50)]
        [ShowInInspector]
        [LabelText("Search")]
        [LabelWidth(60)]
        [PropertyOrder(100)]
        [OnValueChanged(nameof(RefreshFilteredElements))]
        protected override string SearchFilter
        {
            get => _searchFilter; 
            set => _searchFilter = value;
        }

        [EnhancedBoxGroup("Collection/List")]
        [PropertyOrder(100)]
        [ShowInInspector]
        [LabelText(" ")]
        [ListItemSelector(nameof(SetSelected))]
        [CustomValueDrawer(nameof(ListElementDrawer))]
        [ListDrawerSettings(ShowFoldout = false, IsReadOnly = true, DraggableItems = false, OnTitleBarGUI = nameof(OnElementsTitlebarGUI), ShowItemCount = false, NumberOfItemsPerPage = 25)]
        public override List<TElement> Elements
        {
            get => _elements; 
            set => _elements = value;
        }
        
        [HorizontalGroup("Collection")]
        [LabelWidth(180)]
        [EnhancedBoxGroup("Collection/Preview")]
        [ShowInInspector]
        [PropertyOrder(130)]
        [InfoBox("No element selected!", VisibleIf = "@this.SelectedElement == null")]
        [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
        public TElement SelectedElement { get; set; }
        
        private void SetSelected(int index)
        {
            SelectedElement = index >= 0 && index < Elements.Count ? Elements[index] : null;
        }

        public void Select(TElement element)
        {
            WaitInEditor.ForNextFrame(() =>
            {
                SelectedElement = element;
            });
        }

        public void Select(ScriptableObject element)
        {
            if (element is TElement tElement)
            {
                Select(tElement);
            }
        }

        public bool CanSelect(ScriptableObject element)
        {
            return element is TElement tElement && RawElementFilterPredicate(tElement);
        }
        
        protected override void OnNewElement(TElement newElement)
        {
            Select(newElement);
        }

        protected override void OnElementsRefreshed()
        {
            base.OnElementsRefreshed();
            if (SelectedElement == null) SelectedElement = Elements.FirstOrDefault();
        }
    }
}

#endif