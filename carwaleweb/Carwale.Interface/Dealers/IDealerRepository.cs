using Carwale.Entity.CarData;
using Carwale.Entity.Classified.Chat;
using Carwale.Entity.Dealers;
using System.Collections.Generic;

namespace Carwale.Interfaces.Dealers
{
    public interface IDealerRepository
    {
        DealerDetails GetDealerDetailsOnDealerId(int dealerId);
        List<CarModelSummary> GetDealerModelsOnMake(int makeId, int dealerId);
        DealerChatInfo GetDealerMobileFromChatToken(string chatToken);
    }
}
