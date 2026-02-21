using System.Collections.Generic;
using System.Linq;
using APGame.Abstractions;
using APGame.LevelFeatures.Common;
using APX.Util.OdinAttributes;
using UnityEngine;

namespace APGame.LevelFeatures.DragDropNumbers
{
    public class DragDropNumbersClock : MonoBehaviour, IClock
    {
        [SerializeField] private LockedClockHand _HourHand;
        [SerializeField] private LockedClockHand _MinuteHand;
        [SerializeField, RequiredAsset] private DragNumber _DragNumberPrefab;

        public readonly List<DragNumber> DragNumbers = new();

        public void SetTime(int hour, int minute)
        {
            _HourHand.SetRotation(hour * 360f / 12/* + minute * 360f / 60 / 12*/);
            _MinuteHand.SetRotation(minute * 360f / 60);

            int[] randomizedNumberOrder = { 10, 8, 3, 12, 11, 6, 4, 9, 7, 2, 5, 1 };
            // var randomizedPositions = Enumerable.Range(0, 12).OrderBy(_ => Random.value).ToArray();
            for (int i = 0; i < 12; i++)
            {
                var dragNumber = Instantiate(_DragNumberPrefab, transform);
                dragNumber.Initialize(randomizedNumberOrder[i], i + 1, this);
                DragNumbers.Add(dragNumber);
            }
        }

        public int GetNumberOnNumberPosition(int numberPosition) => DragNumbers.First(n => n.PositionNumber == numberPosition).Number;

        public Vector2 GetTargetPositionForNumberPosition(int number)
        {
            int normalizedNumber = number % 12;

            float angleDegrees = 90f - (normalizedNumber * 30f);
            float angleRadians = angleDegrees * Mathf.Deg2Rad;

            float x = Mathf.Cos(angleRadians);
            float y = Mathf.Sin(angleRadians);

            Vector3 offset = new Vector3(x, y, 0) * 2.278f;

            return transform.position + offset;
        }
    }
}
