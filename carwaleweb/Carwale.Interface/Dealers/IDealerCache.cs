using Carwale.Entity.CarData;
using Carwale.Entity.Classified.Chat;
using Carwale.Entity.Dealers;
using System.Collections.Generic;

namespace Carwale.Interfaces.Dealers
{
    public interface IDealerCache
    {
        DealerDetails GetDealerDetailsOnDealerId(int dealerId);
        List<CarModelSummary> GetDealerModelsOnMake(int makeId, int dealerId);
        Dictionary<int, DealerDetails> MultiGetDealerDetails(List<int> dealerIds);
        DealerChatInfo GetDealerMobileFromChatToken(string chatToken);
    }
}
