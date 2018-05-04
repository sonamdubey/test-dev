using Bikewale.Cache.Core;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Consumer;
using Microsoft.Practices.Unity;
using System;
using System.Collections;

namespace Bikewale.RabbitMq.LeadProcessingConsumer.Cache
{
    /// <summary>
    /// Created by  :  Pratibha Verma on 4 April 2018
    /// Description :  cache layer to get honda model mapping
    /// </summary>
    public class HondaModelCacheRepository : IHondaModelCache
    {
        private readonly ICacheManager _cache;
        private readonly LeadProcessingRepository _leadRepository = null;
        public HondaModelCacheRepository()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICacheManager,MemcacheManager>();
                _cache = container.Resolve<ICacheManager>();
            }
            _leadRepository = new LeadProcessingRepository();
        }
        public Hashtable GetHondaModelMapping()
        {
            Hashtable hondaModel = null;
            try
            {
               hondaModel = _cache.GetFromCache<Hashtable>("BW_hondamodelmapping_v1", new TimeSpan(7, 0, 0, 0), () => _leadRepository.GetHondaModelApiMapping());              
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Bikewale.RabbitMq.LeadProcessingConsumer.Cache.GetHondaModelMapping()", ex);
            }
            return hondaModel;
        }
    }
}
