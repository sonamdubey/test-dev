using Bikewale.BAL.GrpcFiles.Specs_Features;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData.NewLaunched;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 Mar 2017
    /// Description :   NewLaunchesBikesModel
    /// </summary>
    public class NewLaunchesBikesModel
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly InputFilter _filter = null;
        private readonly PQSourceEnum _pqSource;
        public NewLaunchesBikesModel(INewBikeLaunchesBL newLaunches, InputFilter filter, PQSourceEnum pqSource)
        {
            _newLaunches = newLaunches;
            _filter = filter;
            _pqSource = pqSource;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Returns the new bikes view model
        /// </summary>
        /// <returns></returns>
        public NewLaunchesBikesVM GetData()
        {
            NewLaunchesBikesVM objVM = null;
            try
            {
                objVM = new NewLaunchesBikesVM();
                objVM.Bikes = _newLaunches.GetBikes(_filter);
                IEnumerable<NewLaunchedBikeEntityBase> newLaunchesList = objVM.Bikes.Bikes;
                if (newLaunchesList != null && newLaunchesList.Any())
                {
                    var versionMinSpecs = SpecsFeaturesServiceGateway.GetVersionsMinSpecs(newLaunchesList.Select(m => m.VersionId)).GetEnumerator();
                    foreach (var bike in newLaunchesList)
                    {
                        if (versionMinSpecs.MoveNext())
                        {
                            bike.MinSpecsList = versionMinSpecs.Current.MinSpecsList;
                            bike.MinSpecsCount = 4;
                        }
                    }
                }
                objVM.Makes = _newLaunches.GetMakeList();
                objVM.PqSource = _pqSource;
                objVM.Page_H2 = string.Format("Latest bikes in India - {0}", DateTime.Now.Year);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "NewLaunchesBikesModel.GetData()");
            }
            return objVM;
        }
    }
}