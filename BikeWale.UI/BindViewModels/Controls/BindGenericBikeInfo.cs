using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By : Sushil Kumar on 2nd Jan 2016
    /// Generic Bike Functions
    /// </summary>
    public class BindGenericBikeInfo
    {
        public uint ModelId { get; set; }
        public bool IsUpcoming { get; set; }
        public bool IsDiscontinued { get; set; }
        /// <summary>
        /// Created By : Sushil Kumar on 2nd Jan 2016
        /// Summary :  To get generic bike info by modelid
        /// Modified By : Aditi Srivastava on 23 Jan 2017
        /// Summary     : Added properties for checking upcoming and discontinued bikemodels
        ///Modified By :- Subodh Jain 30 jan 2017
        ///Summary:- Shifted generic to bikemodel repository
        /// </summary>
        public GenericBikeInfo GetGenericBikeInfo()
        {
            GenericBikeInfo genericBikeInfo = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {

                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();
                    ;
                    var _objGenericBike = container.Resolve<IBikeModelsCacheRepository<int>>();


                    genericBikeInfo = _objGenericBike.GetGenericBikeInfo(ModelId);
                    if (genericBikeInfo != null)
                    {
                        if (genericBikeInfo.IsFuturistic)
                        {
                            IsUpcoming = true;
                        }
                        else if (genericBikeInfo.IsUsed && !genericBikeInfo.IsNew)
                        {
                            IsDiscontinued = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BindGenericBikeInfo.GetGenericBikeInfo");
            }
            return genericBikeInfo;
        }
    }
}