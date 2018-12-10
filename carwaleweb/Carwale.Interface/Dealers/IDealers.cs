using Carwale.DTOs;
using Carwale.Entity.Dealers;
using Carwale.Entity.UsedCarsDealer;
using System.Collections.Generic;

namespace Carwale.Interfaces.Dealers
{
    public interface IDealers
    {
        DealerDetails GetDealerDetailsOnDealerId(int dealerId);
        List<DealerDetails> DealerDetailsOnMakeState(int makeId, int stateId, int count = -1);
        string GetDealerWorkingTime(string startTime,string endTime);
        List<AboutUsImageEntity> GetDealerImages(int dealerId);
    }
}
