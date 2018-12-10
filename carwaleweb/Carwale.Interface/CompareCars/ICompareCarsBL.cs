using Carwale.Entity;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Geolocation;
using System.Collections.Generic;

namespace Carwale.Interfaces.CompareCars
{
    public interface ICompareCarsBL
    {
        CCarData Get(List<int> versionIds, bool getFeaturedVersion, Location custLocation = null, int platform = -1,bool isOrp=false);
        MetaTagsEntity GetCanonical(List<CarWithImageEntity> carData, int featuredVersionId);
        List<HotCarComparison> GetHotComaprisons(Pagination page, int cityId = -1,bool isOrp = false);
        CompareCarsDetails GetCompareCarList(Pagination page);        
    }
}
