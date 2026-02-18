using System;
using System.Collections.Generic;
using APX.Audio.Models.Data.Variations;
using APX.Audio.Models.Definitions;
using APX.Audio.Services.DefinitionProviders;
using APX.Events;
using APX.Managers.GameObjects;
using APX.Util.OdinAttributes;
using UnityEngine;

namespace APX.Audio.GameObjects
{
    public class SFXManager : ASingletonPersistent<SFXManager>
    {
        private record SubscribedSFXData(Type EventType, Action<object> Handler);

        [SerializeField]
        [RequiredChild]
        private SFXPlayer _SFXPlayer;

        private readonly List<SubscribedSFXData> _subscribedSFXDataList = new();
        private IReadOnlyList<EventSFXDefinition> _eventSFXList;

        protected override void Initialize()
        {
            base.Initialize();

            _eventSFXList = EventSFXDefinitionsProvider.Definitions;
            InitializeSFXEvents();
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (SubscribedSFXData subscribedSFXData in _subscribedSFXDataList)
            {
                EventBus.Unsubscribe(subscribedSFXData.EventType, subscribedSFXData.Handler);
            }
        }

        public void Play(SFXInstance sfxInstance)
        {
            _SFXPlayer.Play(sfxInstance);
        }

        private void InitializeSFXEvents()
        {
            foreach (EventSFXDefinition definition in _eventSFXList)
            {
                Action<object> eventAction = _ =>
                {
                    if (definition.Disabled)
                    {
                        return;
                    }

                    Play(definition.SFXModel.GetSFXInstance());
                };

                foreach (object definitionEvent in definition.Events)
                {
                    var subscribedSFXData = new SubscribedSFXData(definitionEvent.GetType(), eventAction);

                    EventBus.Subscribe(subscribedSFXData.EventType, subscribedSFXData.Handler);

                    _subscribedSFXDataList.Add(subscribedSFXData);
                }
            }
        }
    }
}
