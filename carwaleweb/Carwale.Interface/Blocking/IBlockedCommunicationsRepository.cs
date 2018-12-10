using Carwale.Entity.Blocking;
using System.Collections.Generic;

namespace Carwale.Interfaces.Blocking
{
    public interface IBlockedCommunicationsRepository
    {
        bool IsCommunicationBlocked(BlockedCommunication communication);
        BlockedCommunicationRequest UnblockCommunication(BlockedCommunicationRequest communicationRequest);
        BlockedCommunicationRequest BlockCommunication(BlockedCommunicationRequest communicationRequest);
    }
}
