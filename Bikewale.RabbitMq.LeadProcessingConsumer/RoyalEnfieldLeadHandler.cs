using Consumer;
using System;
using System.Configuration;

namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Jul 2017
    /// Description :   Royal Enfield Lead Handler
    /// </summary>
    internal class RoyalEnfieldLeadHandler : ManufacturerLeadHandler
    {
        private readonly string _token;

        /// <summary>
        /// Type Initializer
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="urlAPI"></param>
        /// <param name="isAPIEnabled"></param>
        /// <param name="submitDuplicateLead"></param>
        public RoyalEnfieldLeadHandler(uint manufacturerId, string urlAPI, bool isAPIEnabled, bool submitDuplicateLead) : base(manufacturerId, urlAPI, isAPIEnabled, submitDuplicateLead)
        {
            _token = ConfigurationManager.AppSettings["RoyalEnfieldToken"];
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jul 2017
        /// Description :   Process Royal Enfield Manufacturer Lead
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public override bool Process(ManufacturerLeadEntityBase leadEntity)
        {
            return base.Process(leadEntity);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jul 2017
        /// Description :   Override the duplicatelead logic for Royal Enfield lead
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        protected override bool IsDuplicateLead(ManufacturerLeadEntityBase leadEntity)
        {
            // Check if lead is duplicate (already pushed), don't hit the RE API
            if (!base.LeadRepostiory.IsLeadExists(leadEntity.DealerId, leadEntity.CustomerMobile))
            {
                return true;
            }
            else
            {
                //Save response as duplicate lead
                base.LeadRepostiory.UpdateManufacturerLead(leadEntity.PQId, "BW Response: Duplicate lead not pushed in API", leadEntity.LeadId);
                return false;
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jul 2017
        /// Description :   Override PushLeadToManufacturer to submit lead to Royal Enfield Web Service
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        protected override string PushLeadToManufacturer(ManufacturerLeadEntityBase leadEntity)
        {
            string response = string.Empty;
            try
            {
                BikeQuotationEntity quotation = base.LeadRepostiory.GetPriceQuoteById(leadEntity.PQId);
                RoyalEnfieldDealer dealer = base.LeadRepostiory.GetRoyalEnfieldDealerById(leadEntity.ManufacturerDealerId);
                Logs.WriteInfoLog(String.Format("Royal Enfield Request : {0}", Newtonsoft.Json.JsonConvert.SerializeObject(leadEntity)));

                using (RoyalEnfieldWebAPI.Service service = new RoyalEnfieldWebAPI.Service())
                {
                    response = service.Organic(leadEntity.CustomerName, leadEntity.CustomerMobile, "India", dealer.DealerState,
                                dealer.DealerCity, leadEntity.CustomerEmail, quotation.ModelName, dealer.DealerName, "",
                                "https://www.bikewale.com", _token, "bikewale", dealer.DealerCode);
                }

                if (string.IsNullOrEmpty(response))
                {
                    response = "Null response recieved from Royal Enfield API.";
                }
                Logs.WriteInfoLog(String.Format("Royal Enfield Response : {0}", response));
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("PushLeadToRoyalEnfield : LeadId :{0}, ErrorMessage: {1}", leadEntity.LeadId, ex.Message));
            }
            return response;
        }

    }
}
