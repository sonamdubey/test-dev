using System;
using System.Linq;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using System.Collections.Generic;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.Entities.BikeData;

namespace Bikewale.Models.BestBikes
{
    /// <summary>
    /// Created by : Aditi Srivastava on 25 Mar 2017
    /// Summary    : Model to fetch data for popular bikes by body style
    /// </summary>
    public class PopularBikesByBodyStyle
    {
        #region Private variables
        private readonly IBikeModels<BikeModelEntity, int> _models = null;
        private readonly IApiGatewayCaller _apiGatewayCaller;
        private int TotalWidgetItems = 9;
        #endregion
        
        #region Public Properties
        public int TopCount { get; set; }
        public uint CityId { get; set; }
        public uint ModelId { get; set; }
        #endregion

        #region Constructor
        public PopularBikesByBodyStyle(IBikeModels<BikeModelEntity, int> models)
        {
            _models = models;
            _apiGatewayCaller = new ApiGatewayCaller();
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
                objPopular.PopularBikes = _models.GetMostPopularBikesByModelBodyStyle((int)ModelId, TotalWidgetItems, CityId, true);
                if (objPopular.PopularBikes != null && objPopular.PopularBikes.Any())
                {
                    objPopular.PopularBikes = objPopular.PopularBikes.Take(TopCount);
                    objPopular.BodyStyle = objPopular.PopularBikes.FirstOrDefault().BodyStyle;
                    objPopular.BodyStyleText = BodyStyleLinks.BodyStyleHeadingText(objPopular.BodyStyle);
                    objPopular.BodyStyleLinkTitle = BodyStyleLinks.BodyStyleFooterLink(objPopular.BodyStyle);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.BestBikes.PopularBikesByBodyStyle.GetData");
            }
            return objPopular;
        }
        #endregion
    }
}