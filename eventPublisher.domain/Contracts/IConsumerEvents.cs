using System.Collections.Generic;
using System.Threading.Tasks;

namespace eventPublisher.domain.contracts
{
    public interface IConsumeEvents
    {
        void ReceiveEvents();
    }
}