using Carwale.Entity.CarData;
using System;
using System.Collections.Generic;

namespace Carwale.Entity.Dealers
{
    [Serializable]
    public class DealerListMakeEntity
    {
        public List<DealerStateEntity> DealersByMake;

        public CarMakeEntityBase CarDetails;

        public int MakeId { get; set; }
        public List<BreadcrumbEntity> BreadcrumbEntitylist { get; set; }
    }
}
