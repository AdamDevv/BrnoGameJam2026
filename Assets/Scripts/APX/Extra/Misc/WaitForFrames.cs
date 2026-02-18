using System;
using UnityEngine;

namespace APX.Extra.Misc
{
    public class WaitForFrames : CustomYieldInstruction
    {
        private int _remainingFrames;

        public override bool keepWaiting
        {
            get
            {
                if (_remainingFrames == 0) return false;
                _remainingFrames--;
                return true;
            }
        }

        public WaitForFrames(int frameCount)
        {
            if (frameCount <= 0)
            {
                throw new ArgumentOutOfRangeException("frameCount", "Cannot wait for less that 1 frame");
            }
            _remainingFrames = frameCount;
        }
    }
}
