namespace APX.Events.Data
{
    internal interface IHandlerData
    {
        object HandlerAsObject { get; }
        bool TriggerOnce { get; }

        void Trigger(object @event);
    }
}