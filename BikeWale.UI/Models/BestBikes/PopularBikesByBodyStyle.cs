using System;
using System.Linq;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;

namespace Bikewale.Models.BestBikes
{
    /// <summary>
    /// Created by : Aditi Srivastava on 25 Mar 2017
    /// Summary    : Model to fetch data for popular bikes by body style
    /// </summary>
    public class PopularBikesByBodyStyle
    {
        #region Private variables
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private int TotalWidgetItems = 9;
        #endregion
        
        #region Public Properties
        public int TopCount { get; set; }
        public uint CityId { get; set; }
        public uint ModelId { get; set; }
        #endregion

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
        public PopularBodyStyleVM GetData()
        {
            PopularBodyStyleVM objPopular = new PopularBodyStyleVM();
            try
            {
                objPopular.PopularBikes = _models.GetPopularBikesByBodyStyle((int)ModelId, TotalWidgetItems, CityId);
                if (objPopular.PopularBikes != null && objPopular.PopularBikes.Count() > 0)
                {
                    objPopular.PopularBikes = objPopular.PopularBikes.Take(TopCount);
                    objPopular.BodyStyle = objPopular.PopularBikes.FirstOrDefault().BodyStyle;
                    objPopular.BodyStyleText = BodyStyleLinks.BodyStyleHeadingText(objPopular.BodyStyle);
                    objPopular.BodyStyleLinkTitle = BodyStyleLinks.BodyStyleFooterLink(objPopular.BodyStyle);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BestBikes.PopularBikesByBodyStyle.GetData");
            }
            return objPopular;
        }
        #endregion
    }
}