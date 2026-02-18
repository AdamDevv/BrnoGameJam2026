using UnityEngine;

namespace APX.Extra.Misc
{
    public static class RectExtensions
    {
        public static Vector2 Clamp(this Rect rect, Vector2 position)
        {
            return new Vector2(Mathf.Clamp(position.x, rect.xMin, rect.xMax), Mathf.Clamp(position.y, rect.yMin, rect.yMax));
        }
    }
}