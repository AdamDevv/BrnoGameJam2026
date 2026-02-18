using System.Linq;
using APX.Extra.OdinExtensions;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Sirenix.Utilities;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
#endif

namespace APX.Extra.Misc
{
    public enum RandomDistribution
    {
        Uniform,
        Normal60,
        Normal80,
        Normal90,
        Normal99,
        LinearLeft50,
        LinearLeft100,
        LinearRight50,
        LinearRight100,
        ExpoLeft2,
        ExpoLeft5,
        ExpoRight2,
        ExpoRight5,
    }

    public static class RandomDistributionExtensions
    {
        public static float Evaluate(this RandomDistribution randomDistribution, float min, float max) => randomDistribution switch
        {
            RandomDistribution.Uniform => Random.Range(min, max),
            RandomDistribution.Normal60 => RandomFromDistribution.RandomRangeNormalDistribution(min, max, RandomFromDistribution.ConfidenceLevel_e._60),
            RandomDistribution.Normal80 => RandomFromDistribution.RandomRangeNormalDistribution(min, max, RandomFromDistribution.ConfidenceLevel_e._80),
            RandomDistribution.Normal90 => RandomFromDistribution.RandomRangeNormalDistribution(min, max, RandomFromDistribution.ConfidenceLevel_e._95),
            RandomDistribution.Normal99 => RandomFromDistribution.RandomRangeNormalDistribution(min, max, RandomFromDistribution.ConfidenceLevel_e._99),
            RandomDistribution.LinearLeft50 => RandomFromDistribution.RandomRangeLinear(min, max, -0.5f),
            RandomDistribution.LinearLeft100 => RandomFromDistribution.RandomRangeLinear(min, max, -1),
            RandomDistribution.LinearRight50 => RandomFromDistribution.RandomRangeLinear(min, max, 0.5f),
            RandomDistribution.LinearRight100 => RandomFromDistribution.RandomRangeLinear(min, max, 1),
            RandomDistribution.ExpoLeft2 => RandomFromDistribution.RandomRangeExponential(min, max, 2, RandomFromDistribution.Direction_e.Left),
            RandomDistribution.ExpoLeft5 => RandomFromDistribution.RandomRangeExponential(min, max, 5, RandomFromDistribution.Direction_e.Left),
            RandomDistribution.ExpoRight2 => RandomFromDistribution.RandomRangeExponential(min, max, 2, RandomFromDistribution.Direction_e.Right),
            RandomDistribution.ExpoRight5 => RandomFromDistribution.RandomRangeExponential(min, max, 5, RandomFromDistribution.Direction_e.Right),
            _ => throw new System.ArgumentOutOfRangeException(nameof(randomDistribution), randomDistribution, null)
        };
    }

#if UNITY_EDITOR
    [UsedImplicitly]
    public class RandomDistributionDrawer : OdinValueDrawer<RandomDistribution>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var propertyRect = EditorGUILayout.BeginHorizontal();
            CallNextDrawer(label);
            var rect = GUILayoutUtility.GetRect(18, 18, SirenixGUIStyles.Button,  GUILayoutOptions.ExpandWidth(false).Width(18));
            if (SirenixEditorGUI.IconButton(rect, FontAwesomeEditorIcons.ChartSimpleSolid, "Show Preview"))
            {
                var preview = new RandomDistributionPreview(ValueEntry.SmartValue);
                preview.ShowInPopup(propertyRect);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    public class RandomDistributionPreview
    {
        public RandomDistribution RandomDistribution { get; set; }

        private const float WINDOW_WIDTH = 250;

        private const int REPETITIONS = 10000;
        public const int COUNT = 50;

        private int[] _values;

        public RandomDistributionPreview(RandomDistribution randomDistribution)
        {
            RandomDistribution = randomDistribution;
            RefreshValues();
        }

        public OdinEditorWindow ShowInPopup()
        {
            return OdinEditorWindow.InspectObjectInDropDown(this, WINDOW_WIDTH);
        }

        public OdinEditorWindow ShowInPopup(Rect buttonRect)
        {
            return OdinEditorWindow.InspectObjectInDropDown(this, buttonRect, WINDOW_WIDTH);
        }

        public OdinEditorWindow ShowInPopup(Vector2 windowPosition)
        {
            return OdinEditorWindow.InspectObjectInDropDown(this, windowPosition, WINDOW_WIDTH);
        }

        [OnInspectorGUI]
        private void Draw()
        {
            var rect = GUILayoutUtility.GetRect(WINDOW_WIDTH, 200);
            var maxValue = _values.Max() * 1.1f;
            for (var i = 0; i < COUNT; i++)
            {
                var height = Mathf.Clamp01(_values[i] / maxValue);
                var subRect = rect.Split(i, COUNT);
                var finalSubRect = subRect.AlignBottom(Mathf.RoundToInt(subRect.height * height));
                SirenixEditorGUI.DrawSolidRect(finalSubRect, Color.green, false);
            }
        }

        [Button]
        private void RefreshValues()
        {
            _values ??= new int[COUNT];
            for (var i = 0; i < _values.Length; ++i)
            {
                _values[i] = 0;
            }

            for (var i = 0; i < REPETITIONS; ++i)
            {
                var value = RandomDistribution.Evaluate(0, COUNT - 1);
                _values[Mathf.FloorToInt(value)]++;
            }
        }
    }
#endif
}
