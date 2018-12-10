using System.Collections.Generic;

namespace Carwale.Interfaces.Blocking
{
    public interface IBlockIPRepository
    {
        void BlockIpAddresses(IEnumerable<string> ipAddresses, string reasonForBlocking, string addedBy);
        void UnblockIpAddresses(IEnumerable<string> ipAddresses);
        bool IsIpBlocked(string ip);
    }
}
