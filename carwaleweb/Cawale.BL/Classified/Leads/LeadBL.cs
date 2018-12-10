using AutoMapper;
using Carwale.BL.Tracking;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Blocking;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Interfaces.Classified.Leads;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Newtonsoft.Json;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Carwale.BL.Classified.Leads
{
    public class LeadBL : ILeadBL
    {
        private static readonly string _leadQueue = ConfigurationManager.AppSettings["UsedCarLeadQueue"];
        private static readonly byte[] _chatEncryptionKey = Convert.FromBase64String(ConfigurationManager.AppSettings["UsedCarChatEncryptionKey"]);
        private static readonly byte[] IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // This IV is used for all the users.
        private static readonly string _buyerUserIdPrefix = "CW_";
        private const string _leadWrapperKey = "UsedLeadWrapper";
        public const string TrackingCategory = "UsedStockLead";
        private readonly IEnumerable<int> _nonVerificationIosVersions = Enumerable.Range(23, 4);
        private readonly ILeadRepository _leadRepo;
        private readonly ILeadNotifications _leadNotifications;
        private readonly ISellerRepository _sellerRepo;
        private readonly IBlockIPRepository _blockIpRepo;
        private readonly IBlockMobileRepository _blockMobileRepo;
        private readonly BhriguTracker _bhrigutrack;
        private readonly IAggregationsRepository _aggregationsRepository;

        public LeadBL(
            ILeadRepository leadRepo, 
            ILeadNotifications leadNotifications,
            ISellerRepository sellerRepo, 
            IBlockIPRepository blockIpRepo, 
            IBlockMobileRepository blockMobileRepo, 
            BhriguTracker bhrigutrack,
            IAggregationsRepository aggregationsRepository
            )
        {
            _leadRepo = leadRepo;
            _leadNotifications = leadNotifications;
            _sellerRepo = sellerRepo;
            _blockIpRepo = blockIpRepo;
            _blockMobileRepo = blockMobileRepo;
            _bhrigutrack = bhrigutrack;
            _aggregationsRepository = aggregationsRepository;
        }

        public LeadReport ProcessLead(LeadDetail lead)
        {
            LeadReport report = new LeadReport();
            if (_blockMobileRepo.IsNumberBlocked(lead.Buyer.Mobile))
            {
                report.Status = LeadStatus.MobileBlocked;
            }
            else if (!string.IsNullOrEmpty(lead.IPAddress) && _blockIpRepo.IsIpBlocked(lead.IPAddress))
            {
                report.Status = LeadStatus.IpBlocked;
            }
            else
            {
                int leadId;
                report.Status = _leadRepo.CheckLeadStatus(lead.Buyer.Mobile, lead.IPAddress, lead.Stock.InquiryId, lead.Stock.IsDealer, out leadId);
                report.LeadId = leadId;
                if(report.Status != LeadStatus.Duplicate && lead.IsLeadFromChatSms)
                {
                    report.Status = LeadStatus.InvalidChatSmsLead;
                }
                else if (report.Status == LeadStatus.Valid || report.Status == LeadStatus.Duplicate)
                {
                    // OTP code was removed from some versions of iOS so skipping verification for these
                    if (!(lead.LeadSource == Platform.CarwaleiOS && _nonVerificationIosVersions.Contains(lead.AppVersion)) && !lead.IsVerified)
                    {
                        report.Status = LeadStatus.Unverified;
                        _leadRepo.InsertUnverifiedLead(lead);
                    }
                    else
                    {
                        ProcessValidLead(lead, report);
                    }
                }
            }
            TrackLead(lead, report.Status.ToString(), report.LeadId);
            return report;
        }

        private void TrackLead(LeadDetail lead, string action, int leadId)
        {
            try
            {
                Entity.Classified.SellerType sellerType = null;
                if (lead.LeadTrackingParams.QueryString != "-1")
                {
                    string qs = (!string.IsNullOrEmpty(lead.LeadTrackingParams.QueryString))
                                            ? $"{ "{\"" }{ lead.LeadTrackingParams.QueryString.Replace("=", "\":\"").Replace("&", "\",\"").Replace("+", " ") }{ "\"}" }"
                                            : "{}";                                 //converting qs to json string
                    FilterInputs filterInputs = JsonConvert.DeserializeObject<FilterInputs>(qs);
                    sellerType = _aggregationsRepository.GetSellerTypeCount(filterInputs);
                }
                var label = new Dictionary<string, string>()
                        {
                            {"LeadId" , leadId.ToString()},
                            {"ProfileId" , lead.Stock.ProfileId},
                            {"BuyerName", lead.Buyer.Name},
                            {"BuyerMobile", lead.Buyer.Mobile},
                            {"BuyerEmail", lead.Buyer.Email},
                            {"Source", lead.LeadSource.ToString()},
                            {"Origin", lead.LeadTrackingParams?.OriginId.ToString()},
                            {"AppVersion", lead.AppVersion.ToString()},
                            {"RatingText", lead.Stock.RatingText},
                            {"Rank", lead.LeadTrackingParams?.Rank},
                            {"DeliveryCity", lead.LeadTrackingParams?.DeliveryCity.ToString() },
                            {"LeadType", lead.LeadTrackingParams?.LeadType.ToString()},
                            {"DealerCount", sellerType?.Dealer.ToString()},
                            {"IndividualCount", sellerType?.Individual.ToString()},
                            {"CtePackageId", lead.Stock.CtePackageId.ToString()},
                            {"SlotId", lead.LeadTrackingParams?.SlotId.ToString()}
                        };
                if (lead?.LeadTrackingParams != null && RegExValidations.IsValidLatLong(lead.LeadTrackingParams.Latitude, lead.LeadTrackingParams.Longitude))
                {
                    label.Add("latitude", lead.LeadTrackingParams.Latitude.ToString(CultureInfo.InvariantCulture));
                    label.Add("longitude", lead.LeadTrackingParams.Longitude.ToString(CultureInfo.InvariantCulture));
                }
                _bhrigutrack.Track(TrackingCategory, action, label);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void ProcessValidLead(LeadDetail lead, LeadReport report)
        {
            bool isDuplicate = (report.Status == LeadStatus.Duplicate); 
            int buyerCity = (lead.LeadTrackingParams != null && lead.LeadTrackingParams.DeliveryCity > 0) ? lead.LeadTrackingParams.DeliveryCity : lead.Stock.CityId;
            report.BuyerInfo = GetBuyerInfo(lead.Buyer.Mobile);
            int leadId = _leadRepo.InsertLead(lead, buyerCity, report.BuyerInfo, isDuplicate);
            report.LeadId = isDuplicate ? report.LeadId : leadId;
            if (report.LeadId > 0)
            {
                try
                {
                    if (!isDuplicate)
                    {
                        LeadWrapper wrapper = Mapper.Map<LeadWrapper>(lead);
                        wrapper.LeadId = report.LeadId;
                        PublishLeadToQueue(wrapper);
                    }
                    else
                    {
                        report.Status = LeadStatus.Duplicate;
                    }

                    if (!isDuplicate || 
                        (lead.LeadTrackingParams.LeadType != LeadType.ChatLead &&_leadRepo.ShouldResendNotification(report.LeadId, lead.Stock.IsDealer)))
                    {
                        report.Seller = GetSeller(lead.Stock.InquiryId, lead.Stock.IsDealer);
                        SendLeadNotifications(report.LeadId, lead.Stock, lead.Buyer, report.Seller, lead.LeadSource);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
            else
            {
                Logger.LogError(string.Format("Got leadId 0 for ProfileId : {0} BuyerMobile : {1}", lead.Stock.ProfileId, lead.Buyer.Mobile));
            }
        }

        public static void PublishLeadToQueue(LeadWrapper leadWrapper)
        {
            try
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add(_leadWrapperKey, JsonConvert.SerializeObject(leadWrapper));

                RabbitMqPublish publisher = new RabbitMqPublish();
                publisher.PublishToQueue(_leadQueue, nvc);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public void SendLeadNotifications(int leadId, LeadStockSummary stock, Buyer buyer, Seller seller, Platform leadSource)
        {
            LeadNotificationData notifData = new LeadNotificationData()
            {
                Stock = stock,
                Buyer = buyer,
                Seller = seller,
                LeadSource = leadSource
            };

            _leadNotifications.SendSMSToBuyer(notifData);
            _leadNotifications.SendSMSToSeller(notifData);

            if (!string.IsNullOrEmpty(notifData.Buyer.Email))
            {
                _leadNotifications.SendEmailToBuyer(notifData);
            }

            if (!string.IsNullOrEmpty(notifData.Seller.Email))
            {
                _leadNotifications.SendEmailToSeller(notifData);
            }

            _leadRepo.InsertLeadNotifications(leadId, stock.IsDealer);
        }

        public Seller GetSeller(int inquiryId, bool isDealer)
        {
            var seller = isDealer ? _sellerRepo.GetDealerSeller(inquiryId) : _sellerRepo.GetIndividualSeller(inquiryId);
            if (seller != null && string.IsNullOrEmpty(seller.DisplayNumber))
            {
                seller.DisplayNumber = seller.Mobile;
            }
            seller.DisplayNumber = seller.DisplayNumber.Replace(",", ", ");
            return seller;
        }

        public static BuyerInfo GetBuyerInfo(string mobile)
        {
            string userId = GetBuyerUserId(mobile);
            return new BuyerInfo
            {
                Mobile = mobile,
                UserId = userId,
                AccessToken = GetBuyerAccesToken(userId)
            };
        }

        private static string GetBuyerAccesToken(string userId)
        {
            return AESEncryptionUtility.EncryptUrlSafe(Encoding.ASCII.GetBytes(userId), _chatEncryptionKey, IV, CipherMode.CBC, PaddingMode.PKCS7);
        }

        private static string GetBuyerUserId(string mobile)
        {
            return _buyerUserIdPrefix + AESEncryptionUtility.EncryptSelectorSafe(Encoding.ASCII.GetBytes(mobile), _chatEncryptionKey, IV, CipherMode.CBC, PaddingMode.PKCS7);
        }
    }
}
