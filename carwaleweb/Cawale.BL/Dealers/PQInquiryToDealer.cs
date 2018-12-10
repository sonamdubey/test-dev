using Carwale.BL.PriceQuote;
using Carwale.Entity.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.Dealers;
using System;
using Carwale.Notifications;
using Carwale.Interfaces.CarData;
using Newtonsoft.Json;
using Carwale.Utility;
using System.Web;
using Carwale.Entity.Enum;
using Carwale.Entity.Leads;
using AutoMapper;
using System.Collections.Generic;
using Carwale.Interfaces.Leads;
using Carwale.Notifications.Logs;
using RabbitMqPublishing;
using System.Collections.Specialized;

namespace Carwale.BL.Dealers
{
    public class PQInquiryToDealer : IDealerInquiry
    {
        private readonly IDealerSponsoredAdRespository _dealerSponsor;
        private readonly INewCarDealers _newCarDealers;

        public PQInquiryToDealer(
            IDealerSponsoredAdRespository dealerSponsor,
            ICarVersionCacheRepository carVersion,
            ICarModelCacheRepository carModelCacheRepo,
            INewCarDealers newCarDealers)
        {
            _dealerSponsor = dealerSponsor;
            _newCarDealers = newCarDealers;
        }

        /// <summary>
        /// This Function Do:
        /// 1. Update Customer Details to NEwPurchageCity Table
        /// 2. Gives Dealer Deatails Based On Dealer Id
        /// 3. Save Dealer Ad click Inquiry to PQDealerAdLeads
        /// 4. Base On DealerleadBusinessType Field Call Api's
        ///   I. If DealerleadBusinessType = 0 Then Call Autobiz Api  
        ///         I. Push Respose into PQDealerAdLeads
        ///         II. Send Sms To Dealer
        ///         III . Send Email To Dealer
        ///   II.  If DealerleadBusinessType = 1 Then Push PriceQuote object to Queue
        /// Written By : Ashish Verma on 2/7/2014
        /// Modified By : Rakesh Yadav on 07 Dec 2015, to delete campaign memcache objects
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<ulong> DealerInquiries(DealerInquiry dealerInquiry)
        {
            DealerInquiryDetails dealerInquiryDetails = Mapper.Map<DealerInquiry, DealerInquiryDetails>(dealerInquiry);
            if (dealerInquiry.LeadSource != null)
            {
                dealerInquiryDetails.OriginalLeadId = DecryptId(dealerInquiry.LeadSource.OriginalLeadId);
            }
            List<ulong> _pqDealerAdLeadIds = new List<ulong>();
            if (dealerInquiry.CarInquiry != null)
            {
                foreach (var car in dealerInquiry.CarInquiry)
                {
                    var lead = (DealerInquiryDetails)dealerInquiryDetails.Clone();
                    lead.ModelId = car.CarDetail.ModelId;
                    lead.VersionId = car.CarDetail.VersionId;
                    lead.AssignedDealerId = car.Seller.AssignedDealerId;
                    lead.DealerId = car.Seller.CampaignId;
                    _pqDealerAdLeadIds.Add(ProcessRequest(lead));
                }
            }
            else
            {
                _pqDealerAdLeadIds.Add(ProcessRequest(dealerInquiryDetails));
            }

            return _pqDealerAdLeadIds;
        }

        public ulong ProcessRequest(DealerInquiryDetails dealerInquiry)
        {
            CheckLeadCampaignId(dealerInquiry);
            CheckLeadCity(dealerInquiry);
            if (string.IsNullOrEmpty(dealerInquiry.EncryptedPQDealerAdLeadId))
            {
                dealerInquiry.predictionModelRequest = Carwale.BL.PresentationLogic.NewCar.GetPredictionModelRequest(HttpContext.Current.Request, dealerInquiry.ModelId, dealerInquiry.CityId,
                                            string.IsNullOrWhiteSpace(dealerInquiry.ZoneId) ? 0 : Convert.ToInt16(dealerInquiry.ZoneId), dealerInquiry.PlatformSourceId, dealerInquiry.DealerId);

                dealerInquiry.NewLead = true;
            }

            string leadSourceCategoryId;
            string leadSourceId;
            string leadSourceName;
            PostPQProcess.GetLeadSourceData(out leadSourceCategoryId, out leadSourceId, out leadSourceName);
            dealerInquiry.LeadSourceName = leadSourceName;
            dealerInquiry.LeadSourceCategoryId = leadSourceCategoryId;
            dealerInquiry.LeadSourceId = leadSourceId;


            SetRequestHeaderValues(dealerInquiry);
            SaveLeadDetails(dealerInquiry);
            PushLeadToQueue(dealerInquiry);
            return dealerInquiry.PQDealerAdLeadId;
        }

        private void CheckLeadCampaignId(DealerInquiryDetails dealerInquiry)
        {
            if (dealerInquiry.DealerId <= 0)
            {
                Logger.LogError(string.Format("Lead received without campaignId - {0}", JsonConvert.SerializeObject(dealerInquiry)));
            }
        }

        private void CheckLeadCity(DealerInquiryDetails dealerInquiry)
        {
            if (dealerInquiry.CityId <= 0)
            {
                Logger.LogError(string.Format("Lead received without cityId - {0}", JsonConvert.SerializeObject(dealerInquiry)));
                dealerInquiry.CityId = CustomerCookie.MasterCityId;
            }
        }

        private void SaveLeadDetails(DealerInquiryDetails dealerInquiry)
        {
            try
            {
                if(!string.IsNullOrWhiteSpace(dealerInquiry.EncryptedPQDealerAdLeadId))
                {
                    var encryptedLeadIds = dealerInquiry.EncryptedPQDealerAdLeadId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach(var leadId in encryptedLeadIds)
                    {
                        dealerInquiry.PQDealerAdLeadId = DecryptId(leadId);
                        if (dealerInquiry.Others!=null && dealerInquiry.Others.ContainsKey("EmailLead"))
                        {
                            _dealerSponsor.UpdateDealerSponserdInquiryEmail(dealerInquiry);
                        }else
                        {
                            _dealerSponsor.UpdateDealerSponserdInquiry(dealerInquiry);
                        }
                    }
                    dealerInquiry.PQDealerAdLeadId = DecryptId(encryptedLeadIds[0]);
                }
                else
                {
                    dealerInquiry.PQDealerAdLeadId = _dealerSponsor.SaveDealerSponserdInquiry(dealerInquiry);
                }


                if (!String.IsNullOrWhiteSpace(dealerInquiry.SponsoredBannerCookie))
                {
                    try
                    {
                        string[] sbArray = dealerInquiry.SponsoredBannerCookie.Split('|');
                        string tm = sbArray[0].Split('=')[1];
                        string tv = sbArray[1].Split('=')[1];
                        string fv = sbArray[2].Split('=')[1];

                        int targetModel = string.IsNullOrWhiteSpace(tm) ? -1 : Convert.ToInt32(tm);
                        int targetVersion = string.IsNullOrWhiteSpace(tv) ? -1 : Convert.ToInt32(tv);
                        int featuredVersion = string.IsNullOrWhiteSpace(fv) ? -1 : Convert.ToInt32(fv);
                        _dealerSponsor.SaveLeadSponsoredBanner(dealerInquiry.PQDealerAdLeadId, targetModel, targetVersion, featuredVersion);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler objErr = new ExceptionHandler(ex, "PQInquiryToDealer.SaveDealerSponserdInquiry: SponsoredBannerCookie:" + dealerInquiry.SponsoredBannerCookie);
                        objErr.LogException();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PQInquiryToDealer.SaveDealerSponserdInquiry: InputJson:" + JsonConvert.SerializeObject(dealerInquiry));
                objErr.LogException();
                _dealerSponsor.SaveFailedLeads(dealerInquiry, ex.Message);
            }
        }

        private void SetRequestHeaderValues(DealerInquiryDetails dealerInquiry)
        {
            try
            {

                string isWebViewValue = null;
                if (dealerInquiry.Others != null)
                {
                    dealerInquiry.Others.TryGetValue("IsWebView", out isWebViewValue);
                }
                    
                if (dealerInquiry.PlatformSourceId == (int)Platform.CarwaleAndroid )
                {
                    dealerInquiry.Others = dealerInquiry.Others != null ? dealerInquiry.Others : new Dictionary<string, string>();
                    if (!dealerInquiry.Others.ContainsKey("AppVersionId") && HttpContext.Current.Request.Headers["appVersion"] != null)
                    {
                        dealerInquiry.Others.Add("AppVersionId", HttpContext.Current.Request.Headers["appVersion"]);
                    }
                }

                bool isWebView = false;
                if (!string.IsNullOrEmpty(isWebViewValue))
                {
                    isWebView = CustomParser.parseBoolObject(isWebViewValue);
                }

                // Save dealer inquiry 
                var nonAppsPlatform = dealerInquiry.PlatformSourceId == (int)Platform.CarwaleDesktop || dealerInquiry.PlatformSourceId == (int)Platform.CarwaleMobile || isWebView;
                dealerInquiry.ABTest = nonAppsPlatform ? (ushort)CustomParser.parseShortObject(HttpContext.Current.Request.Cookies["_abtest"].Values) : dealerInquiry.ABTest;
                dealerInquiry.CwCookie = nonAppsPlatform ? UserTracker.GetSessionCookie() : HttpContext.Current.Request.Headers["IMEI"] != null ? HttpContext.Current.Request.Headers["IMEI"] : "";
                if (HttpContext.Current.Request.Cookies != null && nonAppsPlatform)
                {
                    dealerInquiry.UtmaCookie = (HttpContext.Current.Request.Cookies["__utma"] != null) ? HttpContext.Current.Request.Cookies["__utma"].Value : String.Empty;
                    dealerInquiry.UtmzCookie = (HttpContext.Current.Request.Cookies["_cwutmz"] != null) ? HttpContext.Current.Request.Cookies["_cwutmz"].Value : String.Empty;
                    dealerInquiry.ModelsHistory = (HttpContext.Current.Request.Cookies["_userModelHistory"] != null) ? HttpContext.Current.Request.Cookies["_userModelHistory"].Value : String.Empty;
                    dealerInquiry.Ltsrc = CommonLTS.CookieLTS != "-1" ? CommonLTS.CookieLTS.Split(':')[0] : "-1";
                }

            }
            catch(Exception ex)
            {
                Logger.LogError(String.Format("Error fetching cookies for Lead - {0} ",ex));
            }
        }

        private void PushLeadToQueue(DealerInquiryDetails dealerInquiry)
        {
            try
            {
                AddLeadTrackingInfo(dealerInquiry);
                // Initialize PPQ(Post Price Quote Process)
                RabbitMqPublish publish = new RabbitMqPublish();
                NameValueCollection inquiry = new NameValueCollection();
                inquiry["content"] = JsonConvert.SerializeObject(dealerInquiry);
                publish.PublishToQueue(PostPQProcess.GetPriceQuoteQueueName(dealerInquiry.DealerId), inquiry);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PQInquiryToDealer.queueService.Queue: InputJson:" + JsonConvert.SerializeObject(dealerInquiry));
                objErr.LogException();
                _dealerSponsor.SaveFailedLeads(dealerInquiry, ex.Message);
            }
        }
        private void AddLeadTrackingInfo(DealerInquiryDetails leadObject)
        {
            try
            {
                leadObject.ClientIP = Convert.ToString(UserTracker.GetUserIp());
                leadObject.UserAgent = Convert.ToString(HttpContext.Current.Request.UserAgent);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PQInquiryToDealer.AddLeadTrackingInfo()");
                objErr.LogException();
            }
        }
        private ulong DecryptId(string encryptedId)
        {
            UInt64 decryptedOriginalId = 0;
            try
            {
                if (!string.IsNullOrWhiteSpace(encryptedId))
                {
                    UInt64.TryParse(CarwaleSecurity.Decrypt(HttpUtility.UrlDecode(encryptedId)), out decryptedOriginalId);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Decrypt Original Lead Id" + ex);
            }
            return decryptedOriginalId;
        }
    }
}
