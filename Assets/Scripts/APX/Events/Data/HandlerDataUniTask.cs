using System;
using APX.Events.Data.Abstractions;
using Cysharp.Threading.Tasks;

namespace APX.Events.Data
{
    internal class HandlerDataUniTask<T> : AHandlerData<T>
    {
        public Func<T, UniTaskVoid> Handler { get; }
            
        public HandlerDataUniTask(Func<T, UniTaskVoid> handler, bool triggerOnce)
            : base(handler, triggerOnce)
        {
            Handler = handler;
        }

        public override void Trigger(object @event) => Handler((T)@event);
    }
}