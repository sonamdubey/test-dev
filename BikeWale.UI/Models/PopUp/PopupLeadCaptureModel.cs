﻿using Bikewale.Entities.PriceQuote;
using Bikewale.Notifications;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Bikewale.Models.PopUp
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 04-Sep-2017
    /// Summary: Model for independent lead capture popup
    /// </summary>
    public class PopupLeadCaptureModel
    {
        private PopupLeadCaptureVM viewModel;
        private readonly string _queryString;

        public PopupLeadCaptureModel(string queryString)
        {
            _queryString = queryString;
        }
        public PopupLeadCaptureVM GetData()
        {
            viewModel = new PopupLeadCaptureVM();
            ParseQueryString(_queryString);
            BindPageMetaTags(viewModel.PageMetaTags);
            return viewModel;
        }

        /// <summary>
        /// Parses the query string.
        /// </summary>
        /// <param name="qs">The query string.</param>
        public void ParseQueryString(string queryString)
        {
            string qs = Utils.Utils.DecryptTripleDES(queryString);
            try
            {
                uint _modelId, _cityId, _areaId;
                string _bikeName, _location, _city, _area;
                bool _isManufacturerCampaign;

                NameValueCollection queryCollection = HttpUtility.ParseQueryString(qs);
                var dict = HttpUtility.ParseQueryString(qs);
                viewModel.PopupJson = Utility.EncodingDecodingHelper.EncodeTo64(Newtonsoft.Json.JsonConvert.SerializeObject(
                    dict.AllKeys.ToDictionary(k => k, k => dict[k])
                ));

                #region Parse the Query collection

                uint.TryParse(queryCollection["modelid"], out _modelId);
                uint.TryParse(queryCollection["cityid"], out _cityId);
                uint.TryParse(queryCollection["areaid"], out _areaId);

                _bikeName = queryCollection["bikename"];
                _location = queryCollection["location"];
                _city = queryCollection["city"];
                _area = queryCollection["area"];

                bool.TryParse(queryCollection["isManufacturer"], out _isManufacturerCampaign);
                viewModel.Url = queryCollection["url"];
                viewModel.LeadCapture = new LeadCaptureEntity()
                {
                    ModelId = _modelId,
                    Area = _area,
                    AreaId = _areaId,
                    City = _city,
                    CityId = _cityId,
                    Location = _location,
                    BikeName = _bikeName,
                    IsManufacturerCampaign = _isManufacturerCampaign
                };

                #endregion
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "PopupLeadCaptureModel.ParseQueryString()");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 14-Sep-2017
        /// Description :  Bind page meta tags.
        /// </summary>
        /// <param name="pageMetaTags"></param>
        private void BindPageMetaTags(PageMetaTags pageMetaTags)
        {
            try
            {
                pageMetaTags.Title = "Please provide more details | BikeWale";
                pageMetaTags.Description = "Please provide more details to proceed further.";
            }
            catch (Exception ex)
            {
                Notifications.ErrorClass objErr = new Notifications.ErrorClass(ex, "PopupLeadCaptureModel.BindPageMetaTags()");
            }
        }

    }
}