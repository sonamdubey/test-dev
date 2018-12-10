using Carwale.DAL.Classified;
using Carwale.Entity.Enum;
using Carwale.Entity.Stock;
using Carwale.Interfaces.Classified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.Classified.Stock
{
    public class GetUsedCarDealerStatus : IGetUsedCarDealerStatus
    {
        private readonly IGetDealerStatus _getDealerStatus = null;

        public GetUsedCarDealerStatus(IGetDealerStatus getDealerStatus)
        {
            _getDealerStatus = getDealerStatus;
        }


        public String GetDealerStatus(int? SellerId)
        {
            
            var status = _getDealerStatus.GetUsedCarDealerStatus(SellerId);
            if (status.IsDealerMissing)
            {
                return "dealer is not mapped";
            }
            if (status.IsPackageMissing)
            {
                return "package has not been mapped";
            }
            if (status.PackageStartDate > DateTime.Now)
            {
                return "package has future date";
            }
            if (!status.IsMigrated)
            {
                return "dealer is not migrated";
            }
            if (status.PackageEndDate < DateTime.Now)
            {
                return "package has expired";
            }
            return "ok";

        }
    }
}
