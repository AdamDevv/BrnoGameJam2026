using APGame.Managers;
using DG.Tweening;
using UnityEngine;

namespace APGame.LevelFeatures.RotateClock
{
    public class LockedClockHand : MonoBehaviour
    {
        private Vector3 _startRot;

        private void Start()
        {
            _startRot = transform.rotation.eulerAngles;
        }

        private void OnMouseDown()
        {
            if (!GameManager.Instance.IsInputEnabled) return;

            transform.DOKill();
            transform.rotation = Quaternion.Euler(_startRot);
            transform.DOShakeRotation(0.4f, new Vector3(0, 0, 2), vibrato: 15, randomness: 1, randomnessMode: ShakeRandomnessMode.Harmonic);
        }
    }
}
