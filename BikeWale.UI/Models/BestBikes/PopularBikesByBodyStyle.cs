using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System.Linq;
using Bikewale.Utility;

namespace Bikewale.Models.BestBikes
{ 
    /// <summary>
    /// Created by : Aditi Srivastava on 25 Mar 2017
    /// Summary    : Model to fetch data for popular bikes by body style
    /// </summary>
    public class PopularBikesByBodyStyle
    {
        private readonly IBikeModelsCacheRepository<int> _models = null;
        
        #region Constructor
        public PopularBikesByBodyStyle(IBikeModelsCacheRepository<int> models)
        {
            _models = models;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivastava on 25 Mar 2017
        /// Summary    : Get list of popular bikes by body style
        /// </summary>
        public PopularBodyStyleVM GetData(uint modelId, int topCount, uint cityId)
        {
            PopularBodyStyleVM objPopular = new PopularBodyStyleVM();
            try
            {
                objPopular.PopularBikes = _models.GetPopularBikesByBodyStyle((int)modelId, topCount, cityId);
                if(objPopular.PopularBikes!=null && objPopular.PopularBikes.Count()>0){
                    objPopular.BodyStyle = objPopular.PopularBikes.FirstOrDefault().BodyStyle;
                    objPopular.BodyStyleText = BodyStyleLinks.BodyStyleHeadingText(objPopular.BodyStyle);
                    objPopular.BodyStyleLinkTitle = BodyStyleLinks.BodyStyleFooterLink(objPopular.BodyStyle);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.BestBikes.PopularBikesByBodyStyle.GetData: ModelId {0}, TopCount {1}, cityId(3)", modelId,topCount,cityId));
            }
            return objPopular;
        }
        #endregion
    }
}