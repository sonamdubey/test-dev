using Bikewale.Cache.Location;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.BAL.Location
{
    public class States : IState
    {
        private readonly IStateCacheRepository _objStates = null;

        public States()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IStateCacheRepository, StateCacheRepository>();
                _objStates = container.Resolve<IStateCacheRepository>();
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
    }
}
