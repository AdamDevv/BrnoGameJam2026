using System;
using APGame.Abstractions;
using APGame.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APGame.InGame
{
    public class Clock : MonoBehaviour, IClock
    {
        [SerializeField] [Required] private ClockBehaviourType _ClockBehaviourType;
        [SerializeField] [Required] private ClockHand _MinuteHand;
        [SerializeField] [Required] private ClockHand _HourHand;

        public ClockBehaviourType ClockBehaviourType => _ClockBehaviourType;

        public ClockHand MinuteHand => _MinuteHand;
        public ClockHand HourHand => _HourHand;

        public void SetTimeUsingMinuteHand(int minutes)
        {
            // Snap minutes
            // _MinuteHand.SetValue(minutes);

            switch (_ClockBehaviourType)
            {
                case ClockBehaviourType.FreeMove:
                    break;
                case ClockBehaviourType.HourDependent:
                case ClockBehaviourType.Codependent:
                    float hourValue = _HourHand.Value + (minutes / 60f);
                    const int threshold = 25;
                    const int thresholdInv = 60 - threshold;
                    if (_MinuteHand.PreviousValue > thresholdInv && _MinuteHand.Value < threshold)
                    {
                        hourValue += 1;
                    }
                    else if (_MinuteHand.PreviousValue < threshold && _MinuteHand.Value > thresholdInv)
                    {
                        hourValue -= 1;
                    }

                    hourValue = (hourValue + 12) % 12;

                    _HourHand.SetValueSmooth(hourValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetTimeUsingHourHand(int hours, float hourHandAngle)
        {
            switch (_ClockBehaviourType)
            {
                case ClockBehaviourType.FreeMove:
                    break;
                case ClockBehaviourType.HourDependent:
                    float hourValue = hours + (_MinuteHand.Value / 60f);
                    _HourHand.SetValueSmooth(hourValue);
                    break;
                case ClockBehaviourType.Codependent:
                    float minuteValue = (hourHandAngle % 30) / 30;
                    _MinuteHand.SetValueSmooth(minuteValue * 60);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetTime(int hours, int minutes)
        {
            _MinuteHand.SetValue(minutes);
            _HourHand.SetValueSmooth(hours + (minutes / 60f));
        }
    }
}
