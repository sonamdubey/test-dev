using System.Collections.Generic;

namespace Carwale.Interfaces.Blocking
{
    public interface IBlockMobileRepository
    {
        void BlockMobileNos(IEnumerable<string> mobiles, string reasonForBlocking, string addedBy);
        void UnblockMobileNos(IEnumerable<string> mobiles);
        bool IsNumberBlocked(string number);
    }
}
