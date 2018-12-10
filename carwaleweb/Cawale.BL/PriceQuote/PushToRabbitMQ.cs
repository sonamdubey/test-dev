using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces;
using System.Configuration;
using Carwale.Notifications;
using System.Collections.Specialized;


namespace Carwale.BL.PriceQuote
{
    public static class PostPQProcess
    {
        private static readonly string _leadQueue = ConfigurationManager.AppSettings["RabbitMQPriceQuoteQueue"].ToString() == "" ? "Price_Quote_Queue" : ConfigurationManager.AppSettings["RabbitMQPriceQuoteQueue"];
        private static readonly string _slowLeadQueue = ConfigurationManager.AppSettings["DealerLeadQueueSlow"].ToString() == "" ? "Price_Quote_Queue_Slow" : ConfigurationManager.AppSettings["DealerLeadQueueSlow"];
        private static readonly List<int> _slowLeadQueueCampaigns = ConfigurationManager.AppSettings["DealerLeadQueueSlowCampaigns"].ToString().Split(new string[]{","}, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList();
        public static string GetPriceQuoteQueueName(int campaignId)
        {
            string queue = _slowLeadQueueCampaigns.Contains(campaignId) ? _slowLeadQueue : _leadQueue;
            return queue;
        }

        /// <summary>
        /// Get LeadSourcecategoryId and LeadSourceId
        /// </summary>
        /// <param name="leadSourceCategoryId"></param>
        /// <param name="leadSourceId"></param>
        /// <param name="leadSourceName"></param>
        public static void GetLeadSourceData(out string leadSourceCategoryId, out string leadSourceId, out string leadSourceName)
        {
            leadSourceCategoryId = "1";
            leadSourceId = "-1";
            leadSourceName = "SEO and Direct";

            //this function will find out the source of the lead. First check whether this lead has come from some campaign. For that check the property of LTS
            //if it is not -1 then this lead's source is that of campaign, that is value 2, else it is of value 1
            if (CommonLTS.CampaignId != "-1")
            {
                //the lead source category would be that of campaign that is value 2. leadsource category id would be
                //2, leadsourceid would be campaignid and the leadsourcename would be campaigncode

                leadSourceCategoryId = "2";
                leadSourceId = CommonLTS.CampaignId;
                leadSourceName = CommonLTS.CampaignCode;
            }
            else	//this is direct in this case
            {
                leadSourceCategoryId = ConfigurationManager.AppSettings["LeadSourceCategoryId"];
                leadSourceId = ConfigurationManager.AppSettings["LeadSourceId"];
                leadSourceName = ConfigurationManager.AppSettings["LeadSourceName"];

            }
        }
    }
}
