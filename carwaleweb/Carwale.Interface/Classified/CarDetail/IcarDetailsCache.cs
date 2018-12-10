using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Classified.CarDetails;

namespace Carwale.Interfaces.Classified.CarDetail
{
    public interface ICarDetailsCache
    {
        CarDetailsEntity GetDealerListingDetails(uint inquiryId);
        CarDetailsEntity GetIndividualListingDetails(uint inquiryId);
        void RefreshIndividualStockKey(int inquiryId, bool isCriticalRead);
    }
}
