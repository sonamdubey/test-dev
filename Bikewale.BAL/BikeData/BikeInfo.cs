using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
namespace Bikewale.BAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 27 Jan 2017
    /// Summary : Class have methods to get the models for the bike info
    /// </summary>
    public class BikeInfo : IBikeInfo
    {
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;
        /// <summary>
        /// Constructor to initialize all the dependencies
        /// </summary>
        /// <param name="_cache"></param>
        /// <param name="_genericBike"></param>
        public BikeInfo(IBikeModelsCacheRepository<int> modelCache)
        {
            _modelCache = modelCache;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 25 jan 2017
        /// Summary : Function will return the bike info model to bind with view.
        /// Modified By :- subodh Jain 10 Feb 2017
        /// Summary :- BikeInfo Slug details
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public Bikewale.Models.Shared.BikeInfo GetBikeInfo(uint modelId)
        {
            Bikewale.Models.Shared.BikeInfo objBikeInfo = null;

            try
            {
                GenericBikeInfo objBikes = _modelCache.GetBikeInfo(modelId);

                if (objBikes != null)
                {
                    objBikeInfo = new Bikewale.Models.Shared.BikeInfo();

                    objBikeInfo.Info = objBikes;
                    objBikeInfo.ModelId = modelId;

                    if (objBikes.Make != null)
                        objBikeInfo.Url = string.Format("{0}/m/{1}-bikes/{2}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objBikes.Make.MaskingName, objBikes.Model.MaskingName);

                    if (objBikes.Model != null)
                        objBikeInfo.Bike = string.Format("{0} {1}", objBikes.Make.MakeName, objBikes.Model.ModelName);

                    objBikeInfo.PQSource = (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_GenricBikeInfo_Widget;

                };
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.BAL.BikeData.BikeInfo.GetBikeInfo_{0}", modelId));
            }
            return objBikeInfo;
        }   // End of GetBikeInfo
        /// <summary>
        /// Created By :- subodh Jain 10 Feb 2017
        /// Summary :- BikeInfo Slug details
        /// </summary>
        public GenericBikeInfo GetBikeInfo(uint modelId, uint cityId)
        {
            GenericBikeInfo genericBike = null;
            if (modelId > 0)
            {
                if (cityId > 0)
                {
                    genericBike = _modelCache.GetBikeInfo(modelId, cityId);
                }
                else
                {
                    genericBike = _modelCache.GetBikeInfo(modelId);
                }
            }
            return genericBike;
        }
    }   // class
}   // namespace
