using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.GenericBikes;
using Bikewale.Notifications;

namespace Bikewale.BAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 27 Jan 2017
    /// Summary : Class have methods to get the models for the bike info
    /// </summary>
    public class BikeInfo : IBikeInfo
    {
        private readonly ICacheManager cache = null;
        private readonly IGenericBikeRepository genericBike = null;

        /// <summary>
        /// Constructor to initialize all the dependencies
        /// </summary>
        /// <param name="_cache"></param>
        /// <param name="_genericBike"></param>
        public BikeInfo(ICacheManager _cache, IGenericBikeRepository _genericBike)
        {
            cache = _cache;
            genericBike = _genericBike;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 25 jan 2017
        /// Summary : Function will return the bike info model to bind with view.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public Bikewale.Models.Shared.BikeInfo GetBikeInfo(uint modelId)
        {
            Bikewale.Models.Shared.BikeInfo objBikeInfo = null;

            try
            {
                GenericBikeInfo objBikes = genericBike.GetGenericBikeInfo(modelId);

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


    }   // class
}   // namespace
