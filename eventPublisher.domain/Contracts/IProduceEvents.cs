using eventPublisher.domain.models;

namespace eventPublisher.domain.contracts
{
    public interface IProduceEvents
    {
        void SendEvent(ApplicationEvent applicationEvent, string data);
    }
}