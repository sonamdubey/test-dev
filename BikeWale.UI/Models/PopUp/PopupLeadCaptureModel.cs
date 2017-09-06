using Bikewale.Entities.PriceQuote;
using Bikewale.Notifications;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

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
            return viewModel;
        }

        /// <summary>
        /// Parses the query string.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        public void ParseQueryString(string queryString)
        {
            string encrypted = Utils.Utils.EncryptTripleDES("modelid=411&cityid=40&areaid=&bikename=Bajaj%20Rs%20200&location=&city=&area=&ismanufacturer=False&dealerid=24434&dealername=Bajaj+Finance&dealerarea=Agashi&versionid=526&leadsourceid=1111&pqsourceid=2222&isleadpopup=True&mfgcampid=3&pqid=65446544&pageurl=&clientip=124.10.101.55&dealerheading=Get Offers from Bajaj Finance Ltd.&dealermessage=Thank you for providing your details. Bajaj Finance will reach out to you soon.&dealerdescription=Get lowest EMI options on your bike purchase!&pincoderequired=True&emailrequired=True&dealersrequired=False");
            queryString = Utils.Utils.DecryptTripleDES(encrypted);
            try
            {
                //string decodedQueryString = Utils.Utils.DecryptTripleDES(encodedQueryString);
                uint _modelId, _cityId, _areaId;
                string _bikeName, _location, _city, _area;
                bool _isManufacturerCampaign;

                NameValueCollection queryCollection = HttpUtility.ParseQueryString(queryString);
                var dict = HttpUtility.ParseQueryString(queryString);
                viewModel.PopupJson = new JavaScriptSerializer().Serialize(
                    dict.AllKeys.ToDictionary(k => k, k => dict[k])
                );

                #region Parse the Query collection

                uint.TryParse(queryCollection["modelid"], out _modelId);
                uint.TryParse(queryCollection["cityid"], out _cityId);
                uint.TryParse(queryCollection["areaid"], out _areaId);

                _bikeName = queryCollection["bikename"];
                _location = queryCollection["location"];
                _city = queryCollection["city"];
                _area = queryCollection["area"];

                bool.TryParse(queryCollection["isManufacturer"], out _isManufacturerCampaign);

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

    }
}