using UnityEngine;

namespace APX.Extra.Misc
{
    public enum AnchorPresets
    {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottonCenter,
        BottomRight,
        BottomStretch,

        VertStretchLeft,
        VertStretchRight,
        VertStretchCenter,

        HorStretchTop,
        HorStretchMiddle,
        HorStretchBottom,

        StretchAll
    }

    public enum PivotPresets
    {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottomCenter,
        BottomRight,
    }

    public static class RectTransformExtensions
    {
        /// <summary>
        /// https://answers.unity.com/questions/1013011/convert-recttransform-rect-to-screen-space.html?page=1&pageSize=5&sort=votes
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Rect ToScreenSpaceRect(this RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            var position = transform.position;
            Rect rect = new Rect(position.x, Screen.height - position.y, size.x, size.y);
            var pivot = transform.pivot;
            rect.x -= (pivot.x * size.x);
            rect.y -= ((1.0f - pivot.y) * size.y);
            return rect;
        }

        public static Vector2 PointLocalToScreenSpace(this RectTransform transform, Vector2 point)
        {
            Vector2 lossyScale = transform.lossyScale;
            Vector3 position = transform.position;
            Vector2 size = Vector2.Scale(transform.rect.size, lossyScale);

            point = Vector2.Scale(point, lossyScale);
            point.Set(point.x + position.x, point.y + Screen.height - position.y);

            var pivot = transform.pivot;
            point.x -= (pivot.x * size.x);
            point.y -= ((1.0f - pivot.y) * size.y);
            return point;
        }


        public static Rect RectTransformToScreenSpace(this RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
            rect.x -= (transform.pivot.x * size.x);
            rect.y -= ((1.0f - transform.pivot.y) * size.y);
            return rect;
        }

        public static Vector3 PointRelativeLocalToWorldSpace(this RectTransform transform, Vector3 point)
        {
            Vector2 size = transform.rect.size;
            var pivot = transform.pivot;
            float x = point.x * size.x - (pivot.x * size.x);
            float y = point.y * size.y - ((1.0f - pivot.y) * size.y);
            point.Set(x, y, point.z);
            point = transform.localToWorldMatrix.MultiplyPoint(point);

            return point;
        }

        public static Vector3 PointLocalToWorldSpace(this RectTransform transform, Vector3 point)
        {
            Vector2 size = transform.rect.size;
            var pivot = transform.pivot;
            float x = point.x - (pivot.x * size.x);
            float y = point.y - ((1.0f - pivot.y) * size.y);
            point.Set(x, y, point.z);
            point = transform.localToWorldMatrix.MultiplyPoint(point);

            return point;
        }

        public static void ResetOffset(this RectTransform source)
        {
            source.offsetMin = Vector2.zero;
            source.offsetMax = Vector2.zero;
        }

        public static void SetAnchor(this RectTransform source, AnchorPresets allign, int offsetX = 0, int offsetY = 0)
        {
            source.anchoredPosition = new Vector3(offsetX, offsetY, 0);

            switch (allign)
            {
                case (AnchorPresets.TopLeft):
                {
                    source.anchorMin = new Vector2(0, 1);
                    source.anchorMax = new Vector2(0, 1);
                    break;
                }
                case (AnchorPresets.TopCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 1);
                    source.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
                case (AnchorPresets.TopRight):
                {
                    source.anchorMin = new Vector2(1, 1);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }

                case (AnchorPresets.MiddleLeft):
                {
                    source.anchorMin = new Vector2(0, 0.5f);
                    source.anchorMax = new Vector2(0, 0.5f);
                    break;
                }
                case (AnchorPresets.MiddleCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0.5f);
                    source.anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                }
                case (AnchorPresets.MiddleRight):
                {
                    source.anchorMin = new Vector2(1, 0.5f);
                    source.anchorMax = new Vector2(1, 0.5f);
                    break;
                }

                case (AnchorPresets.BottomLeft):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(0, 0);
                    break;
                }
                case (AnchorPresets.BottonCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0);
                    source.anchorMax = new Vector2(0.5f, 0);
                    break;
                }
                case (AnchorPresets.BottomRight):
                {
                    source.anchorMin = new Vector2(1, 0);
                    source.anchorMax = new Vector2(1, 0);
                    break;
                }

                case (AnchorPresets.HorStretchTop):
                {
                    source.anchorMin = new Vector2(0, 1);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }
                case (AnchorPresets.HorStretchMiddle):
                {
                    source.anchorMin = new Vector2(0, 0.5f);
                    source.anchorMax = new Vector2(1, 0.5f);
                    break;
                }
                case (AnchorPresets.HorStretchBottom):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(1, 0);
                    break;
                }

                case (AnchorPresets.VertStretchLeft):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(0, 1);
                    break;
                }
                case (AnchorPresets.VertStretchCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0);
                    source.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
                case (AnchorPresets.VertStretchRight):
                {
                    source.anchorMin = new Vector2(1, 0);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }

                case (AnchorPresets.StretchAll):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }
            }
        }

        public static void SetPivot(this RectTransform source, PivotPresets preset)
        {
            switch (preset)
            {
                case (PivotPresets.TopLeft):
                {
                    source.pivot = new Vector2(0, 1);
                    break;
                }
                case (PivotPresets.TopCenter):
                {
                    source.pivot = new Vector2(0.5f, 1);
                    break;
                }
                case (PivotPresets.TopRight):
                {
                    source.pivot = new Vector2(1, 1);
                    break;
                }

                case (PivotPresets.MiddleLeft):
                {
                    source.pivot = new Vector2(0, 0.5f);
                    break;
                }
                case (PivotPresets.MiddleCenter):
                {
                    source.pivot = new Vector2(0.5f, 0.5f);
                    break;
                }
                case (PivotPresets.MiddleRight):
                {
                    source.pivot = new Vector2(1, 0.5f);
                    break;
                }

                case (PivotPresets.BottomLeft):
                {
                    source.pivot = new Vector2(0, 0);
                    break;
                }
                case (PivotPresets.BottomCenter):
                {
                    source.pivot = new Vector2(0.5f, 0);
                    break;
                }
                case (PivotPresets.BottomRight):
                {
                    source.pivot = new Vector2(1, 0);
                    break;
                }
            }
        }

        public static void SetToFillParent(this RectTransform rectTransform)
        {
            if (rectTransform == null) return;

            rectTransform.localPosition = Vector3.zero;
            rectTransform.offsetMin = rectTransform.offsetMax = rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.localScale = Vector3.one;
        }

        public static Rect GetScreenRect(this RectTransform rectTransform)
        {
            var canvas = rectTransform.GetComponentInParent<Canvas>();
            var cam = canvas.worldCamera;
            if (cam == null)
            {
                cam = Camera.main;
            }

            if (cam == null)
            {
                cam = Object.FindAnyObjectByType<Camera>();
            }

            var localRect = rectTransform.rect;
            var worldMin = rectTransform.TransformPoint(localRect.min);
            var worldMax = rectTransform.TransformPoint(localRect.max);

            var screenMin = cam.WorldToScreenPoint(worldMin);
            var screenMax = cam.WorldToScreenPoint(worldMax);

            return Rect.MinMaxRect(screenMin.x, screenMin.y, screenMax.x, screenMax.y);
        }
        
        public static Vector3 GetPivotInWorldSpace(this RectTransform source)
        {
            // Rewrite Rect.NormalizedToPoint without any clamping.
            Vector2 pivot = new Vector2(
                source.rect.xMin + source.pivot.x * source.rect.width,
                source.rect.yMin + source.pivot.y * source.rect.height);
            // Apply scaling and rotations.
            return source.TransformPoint(new Vector3(pivot.x, pivot.y, 0f));
        }
        
        public static void SetPivotInWorldSpace(this RectTransform source, Vector3 pivot)
        {
            // Strip scaling and rotations.
            pivot = source.InverseTransformPoint(pivot);
            Vector2 pivot2 = new Vector2(
                (pivot.x - source.rect.xMin) / source.rect.width,
                (pivot.y - source.rect.yMin) / source.rect.height);
 
            // Now move the pivot, keeping and restoring the position which is based on it.
            Vector2 offset = pivot2 - source.pivot;
            offset.Scale(source.rect.size);
            Vector3 worldPos = source.position + source.TransformVector(offset);
            source.pivot = pivot2;
            source.position = worldPos;
        }

    }
}
