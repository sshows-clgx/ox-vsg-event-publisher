namespace eventPublisher.domain.contracts
{
    public interface IProduceEvents
    {
        void SendEvent(string topic, int eventId, string data);
    }
}