using Bikewale.DAL.Location;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.BAL.Location
{
    public class States : IState
    {
        private readonly IState objStates = null;

        public States()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IState, StateRepository>();
                objStates = container.Resolve<IState>();
            }
        }

        public List<StateEntityBase> GetStates()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get list of makes along with total dealers count for each make
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DealerStateEntity> GetDealerStates(uint makeId)
        {
            IEnumerable<DealerStateEntity> objDealerStates = objStates.GetDealerStates(makeId);
            return objDealerStates;
        }
    }
}
