using System.Collections;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APX.Extra.Misc
{
    public static class LayerExtensions
    {
        public static bool ContainsLayer(this GlobalLayerMask layerMask, int layer)
        {
            return ContainsLayer((LayerMask) layerMask, layer);
        }

        public static bool ContainsLayer(this LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }
        
        public static bool IsInLayerMask(this GameObject gameObject, GlobalLayerMask layerMask)
        {
            return layerMask.ContainsLayer(gameObject.layer);
        }
        
        public static bool IsInLayerMask(this Component component, GlobalLayerMask layerMask)
        {
            return IsInLayerMask(component.gameObject, layerMask);
        }
        
        public static bool IsInLayerMask(this GameObject gameObject, LayerMask layerMask)
        {
            return layerMask.ContainsLayer(gameObject.layer);
        }
        
        public static bool IsInLayerMask(this Component component, LayerMask layerMask)
        {
            return IsInLayerMask(component.gameObject, layerMask);
        }

        public static IEnumerable GetLayersForDisplay()
        {
            var layers = new ValueDropdownList<int>();
            for(var i=0; i<=31; i++)
            {
                var layerN= LayerMask.LayerToName(i);
                if (layerN.Length > 0) layers.Add($"{i}: {layerN}", i);
            }
            return layers;
        }
        
        public static string GetCommaSeparatedLayers(this GlobalLayerMask layerMask)
        {
            return GetCommaSeparatedLayers((LayerMask) layerMask);
        }
        
        public static string GetCommaSeparatedLayers(this LayerMask layerMask)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < 32; i++)
            {
                if (layerMask == (layerMask | (1 << i)))
                {
                    if (stringBuilder.Length != 0) stringBuilder.Append(",");

                    stringBuilder.Append(LayerMask.LayerToName(i));
                }
            }
            return stringBuilder.ToString();
        }
    }
}