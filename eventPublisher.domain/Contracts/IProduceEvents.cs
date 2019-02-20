namespace eventPublisher.domain.contracts
{
    public interface IProduceEvents
    {
        void SendEvent(string topic, string data);
    }
}