using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.DAL.Location;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.BAL.Location
{
    public class States : IState
    {
        private readonly IStateCacheRepository _objStates = null, _objStatesCity = null;

        public States()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IStateCacheRepository, StateCacheRepository>()
                    .RegisterType<IState, StateRepository>()
                    .RegisterType<ICacheManager, MemcacheManager>();
                _objStates = container.Resolve<IStateCacheRepository>();
                _objStatesCity = container.Resolve<IStateCacheRepository>();
            }
        }

        public List<StateEntityBase> GetStates()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Created By : vivek gupta 
        /// Date : 24 june 2016
        /// desc : get dealer states
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<DealerStateEntity> GetDealerStates(uint makeId)
        {
            return _objStates.GetDealerStates(makeId);
        }
        /// <summary>
        /// Created By:- Subodh Jain 29 may 2016
        /// Description :- Fetch Dealers for make in all states with cities
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public DealerLocatorList GetDealerStatesCities(uint makeId)
        {
            return _objStatesCity.GetDealerStatesCities(makeId);
        }
        public Hashtable GetMaskingNames()
        {
            throw new NotImplementedException();
        }
    }
}
