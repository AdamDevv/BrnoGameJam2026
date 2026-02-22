using System.Linq;
using APX.Extra.Misc;
using UnityEngine;

namespace APGame.LevelFeatures.StarCreateNumbers
{
    public class StarNoPurpose : MonoBehaviour
    {
        private void OnMouseEnter()
        {
            FindObjectsByType<StarSystem>(FindObjectsSortMode.None)
                .Where(ss => ss.IsAnyStarActive && !ss.IsFinished)
                .ForEach(ss => {
                    ss.ResetProgress();
                });
        }
    }
}
