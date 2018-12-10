using Carwale.BL.PriceQuote;
using Carwale.Entity.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.Utility;
using Newtonsoft.Json;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Carwale.BL.NewCars
{
    public class Lead : ILead
    {
        private readonly IDealerSponsoredAdRespository _dealerSponsor;
        public Lead(IDealerSponsoredAdRespository dealerSponsor)
        {
            _dealerSponsor = dealerSponsor;
        }
        public bool RepushLead(List<string> LeadIds)
        {
            try
            {
                if (LeadIds != null && LeadIds.Count > 0)
                {
                    foreach (string leadId in LeadIds)
                    {
                        ulong pqLeadId = Convert.ToUInt32(CarwaleSecurity.Decrypt(HttpUtility.UrlDecode(leadId)));
                        var leadDetails = _dealerSponsor.GetLeadDetailsByLeadId(CustomParser.parseIntObject(pqLeadId)).FirstOrDefault();
                        if (leadDetails != null && leadDetails.DealerId > 0)
                        {
                            leadDetails.IsPushToThirdParty = leadDetails.LeadBussinessType == 1 ? true : false;
                            leadDetails.LeadPushSource = 1;
                            RabbitMqPublish publish = new RabbitMqPublish();
                            NameValueCollection inquiry = new NameValueCollection();
                            inquiry["content"] = JsonConvert.SerializeObject(leadDetails);
                            publish.PublishToQueue(PostPQProcess.GetPriceQuoteQueueName(leadDetails.DealerId), inquiry);
                        }
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Leads.ProcessRequest()");
                objErr.LogException();
                return false;
            }
        }
    }
}
