using Bikewale.Interfaces.Finance;
using Bikewale.Interfaces.Finance.BajajAuto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Finance.BajajAuto;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;

namespace Bikewale.Cache.Finance.BajajAuto
{
    public class BajajAutoCache : IBajajAutoCache
    {
        private readonly ICacheManager _cacheManager;
        private readonly IBajajAutoRepository _bajajAutoRepository;
        public BajajAutoCache(IBajajAutoRepository bajajAutoRepository, ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
            _bajajAutoRepository = bajajAutoRepository;
        }

        public BajajBikeMappingEntity GetBajajFinanceBikeMappingInfo(uint versionId, uint pincodeId)
        {
            try
            {
                string key = string.Format("BW_BajaFinanceBikeMapping_version_{0}_pincode_{1}", versionId, pincodeId);
                return _cacheManager.GetFromCache(key, new TimeSpan(1, 0, 0, 0), ()=>_bajajAutoRepository.GetBajajFinanceBikeMappingInfo(versionId, pincodeId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Cache.Finance.BajajAuto.BajajAutoCache.GetBajajFinanceBikeMappingInfo_verisonId_{0}_pincodeId_{1}", versionId, pincodeId));
            }
            return null;
        }
    }
}
