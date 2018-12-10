using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Interfaces;
using Carwale.Entity.ThirdParty.Leads;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Dealers;
using Carwale.Notifications;
using Carwale.Entity.Dealers;
using AutoMapper;
using Carwale.Utility;
using Carwale.Interfaces.Campaigns;
using Carwale.Entity.Enum;
using Carwale.BL.Campaigns;
using Carwale.Interfaces.Leads;

namespace Carwale.BL.ThirdParty.Leads
{
    public class ThirdPartyInquiryDealer<T> : IRequestManager<T>
        where T : ThirdPartyInquiryDetails
    {
        private readonly ICarVersionCacheRepository _carVersion;
        private readonly IDealerSponsoredAdRespository _dealerSponsor;
        private readonly IDealerInquiry _pqInquiryObj;
        private readonly ICampaign _campaignBl;
        public ThirdPartyInquiryDealer(IDealerSponsoredAdRespository dealerSponsor, ICarVersionCacheRepository carVersion,
           IDealerInquiry pqInquiryObj, ICampaign campaignBl)
        {
            _dealerSponsor = dealerSponsor;
            _carVersion = carVersion;
            _pqInquiryObj = pqInquiryObj;
            _campaignBl = campaignBl;
        }
        public U ProcessRequest<U>(T t)
        {
            ulong pqPqDealerAdLeadId = 0;
            int thirdPartyLeadId = 0;
            try
            {
                thirdPartyLeadId = _dealerSponsor.LogThirdPartyInquiryDetails(t);
                if (t.StatusId < 0)
                {
                    _dealerSponsor.UpdateThirdPartyInquiryPushResponse(thirdPartyLeadId, t.StatusId);
                    return (U)Convert.ChangeType(thirdPartyLeadId, typeof(U));
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ThirdPartyInquiryDealer.ProcessRequest1()");
                objErr.LogException();
            }
            try
            {
                if (t.ModelId < 1)
                {
                    var versionDetails = _carVersion.GetVersionDetailsById(t.VersionId);
                    t.ModelId = versionDetails.ModelId;
                }
                {

                    int[] platFormArray = { 1, 43, 74, 83 };

                    foreach (int platformId in platFormArray)
                    {
                        var locationObj = new Entity.Geolocation.Location { CityId = t.CityId };
                        var sponsoredCampaignDetails = _campaignBl.GetCampaignByCarLocation(t.ModelId, locationObj, platformId, false);

                        if (CampaignValidation.IsCampaignValid(sponsoredCampaignDetails))
                        {
                            t.DealerId = sponsoredCampaignDetails.Id;
                            break;
                        }
                    }
                    try
                    {
                        if (t.DealerId < 1)
                        {
                            _dealerSponsor.UpdateThirdPartyInquiryPushResponse(thirdPartyLeadId, 0);
                            return (U)Convert.ChangeType(0, typeof(U));
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler objErr = new ExceptionHandler(ex, "ThirdPartyInquiryDealer.ProcessRequest2()");
                        objErr.LogException();
                    }
                }
                var dealerDetails = new DealerInquiryDetails();
                Mapper.CreateMap<ThirdPartyInquiryDetails, DealerInquiryDetails>();
                dealerDetails = Mapper.Map<DealerInquiryDetails>(t);
                dealerDetails.PQId = 0;
                dealerDetails.BuyTimeText = "1 week";
                dealerDetails.BuyTimeValue = 7;
                dealerDetails.LeadClickSource = 500;
                dealerDetails.InquirySourceId = "130";
                dealerDetails.IsAutoApproved = false;
                dealerDetails.AssignedDealerId = -1;
                pqPqDealerAdLeadId = _pqInquiryObj.ProcessRequest(dealerDetails);
                try
                {
                    _dealerSponsor.UpdateThirdPartyInquiryPushResponse(thirdPartyLeadId, Convert.ToInt32(pqPqDealerAdLeadId));
                }
                catch (Exception ex)
                {
                    ExceptionHandler objErr = new ExceptionHandler(ex, "ThirdPartyInquiryDealer.ProcessRequest3()");
                    objErr.LogException();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ThirdPartyInquiryDealer.ProcessRequest4()");
                objErr.LogException();
            }
            return (U)Convert.ChangeType(pqPqDealerAdLeadId, typeof(U));
        }
    }
}
