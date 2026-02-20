using System;
using APX.Events.Data.Abstractions;

namespace APX.Events.Data
{
    internal class HandlerData<T> : AHandlerData<T>
    {
        public Action<T> Handler { get; }

        public HandlerData(Action<T> handler, bool triggerOnce)
            : base(handler, triggerOnce)
        {
            Handler = handler;
        }

        public override void Trigger(object @event) => Handler((T)@event);
    }
}