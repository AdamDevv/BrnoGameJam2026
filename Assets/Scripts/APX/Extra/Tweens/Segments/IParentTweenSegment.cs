using System.Collections.Generic;

namespace APX.Extra.Tweens.Segments
{
    public interface IParentTweenSegment
    {
        IEnumerable<ITweenSegment> GetChildSegments();
    }
}
