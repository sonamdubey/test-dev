using Bikewale.BAL.Dealer;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Common;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Added By : Sajal Gupta on 19-12-2016;
    /// Desc : class to bind Dealers Count In Near By Cities 
    /// </summary>
    /// <param name="rptAlternativeBikes"></param>
    public class BindDealersCountInNearByCitiesControl
    {
        public int FetchedRecordsCount { get; set; }
        public uint MakeId { get; set; }
        public uint CityId { get; set; }
        public int TopCount { get; set; }

        /// <summary>
        /// Added By : Sajal Gupta on 19-12-2016;
        /// Desc : Bind Dealers Count In Near By Cities 
        /// </summary>
        /// <param name="rptAlternativeBikes"></param>
        public IEnumerable<NearByCityDealerCountEntity> BindDealersCountInNearByCities()
        {
            IEnumerable<NearByCityDealerCountEntity> DealerCountCityList = null;
            FetchedRecordsCount = 0;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealer, Dealer>()
                        .RegisterType<IDealerCacheRepository, DealerCacheRepository>()
                        .RegisterType<ICacheManager, MemcacheManager>();

                    IDealerCacheRepository objDealer = container.Resolve<IDealerCacheRepository>();

                    DealerCountCityList = objDealer.FetchNearByCityDealersCount(MakeId, CityId);

                    if (DealerCountCityList != null && DealerCountCityList.Count() > 0)
                    {
                        DealerCountCityList = DealerCountCityList.Take(TopCount);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BindDealersCountInNearByCities");
                objErr.SendMail();
            }
            return DealerCountCityList;
        }
    }
}