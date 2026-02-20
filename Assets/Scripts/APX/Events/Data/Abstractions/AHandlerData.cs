namespace APX.Events.Data.Abstractions
{
    internal abstract class AHandlerData<T> : IHandlerData
    {
        public object HandlerAsObject { get; }
        public bool TriggerOnce { get; }

        protected AHandlerData(object handlerAsObject, bool triggerOnce)
        {
            HandlerAsObject = handlerAsObject;
            TriggerOnce = triggerOnce;
        }

        public abstract void Trigger(object @event);
    }
}