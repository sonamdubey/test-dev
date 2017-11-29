using Bikewale.BAL.Dealer;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Common;
using Bikewale.DAL.Dealer;
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
    /// Desc : Class to bind make Dealers Count In Near By Cities 
    /// </summary>
    public class BindDealersCountInNearByCitiesControl
    {
        public uint MakeId { get; set; }
        public uint CityId { get; set; }
        public uint TopCount { get; set; }

        protected IDealerCacheRepository objDealer;

        public BindDealersCountInNearByCitiesControl()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealer, Dealer>()
                        .RegisterType<IDealerCacheRepository, DealerCacheRepository>()
                        .RegisterType<IDealerRepository, DealersRepository>()
                        .RegisterType<ICacheManager, MemcacheManager>();

                    objDealer = container.Resolve<IDealerCacheRepository>();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BindDealersCountInNearByCitiesControl()");
                
            }
        }

        /// <summary>
        /// Added By : Sajal Gupta on 19-12-2016;
        /// Desc : Bind Dealers Count In Near By Cities 
        /// </summary>        
        public IEnumerable<NearByCityDealerCountEntity> BindDealersCountInNearByCities()
        {
            IEnumerable<NearByCityDealerCountEntity> DealerCountCityList = null;
            try
            {
                DealerCountCityList = objDealer.FetchNearByCityDealersCount(MakeId, CityId);

                if (DealerCountCityList != null && DealerCountCityList.Any())
                {
                    DealerCountCityList = DealerCountCityList.Take((int)TopCount);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BindDealersCountInNearByCities");
                
            }
            return DealerCountCityList;
        }
    }
}