using UnityEngine;

namespace APX.Extra.Tweens.Segments
{
    public static class TweenSegmentExtensions
    {
        public static TSegment GetCopy<TSegment>(this TSegment segment) where TSegment : ITweenSegment
        {
            return (TSegment) JsonUtility.FromJson(JsonUtility.ToJson(segment), segment.GetType());
        }

        public static ITweenSegment GetCopy(this ITweenSegment segment)
        {
            return (ITweenSegment) JsonUtility.FromJson(JsonUtility.ToJson(segment), segment.GetType());
        }
    }
}
