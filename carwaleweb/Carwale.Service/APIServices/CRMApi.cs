using System;
using Carwale.Interfaces;
using Carwale.Entity;
using Carwale.BL.CRM;
using Carwale.Notifications;
using Carwale.Entity.PriceQuote;
using Microsoft.Practices.Unity;
using Carwale.DAL;

namespace Carwale.Service
{
    /// <summary>
    /// Business Logic of interacting with Carwale CRM WebService
    /// </summary>
    public class CRMApi<T, TResponse> : IAPIService<T, TResponse> where T : PriceQuoteEntity
                                                                    where TResponse : APIResponseEntity, new()                                                             
    {
        private readonly IPriceQuoteRepository<PriceQuoteEntity> _pqRepo;

        public CRMApi(IPriceQuoteRepository<PriceQuoteEntity> pqRepo)
        {
            _pqRepo = pqRepo;
        }

        public TResponse Request(T t)
        {
            string response = string.Empty;

            try
            {                
                // Generate xml
                var xmlFormatter = new FormatCRMXml();
                string xmlString = xmlFormatter.GetXMLFormat(t);               

                // CRM API call
                var client = new PushCRMSoapClient();
                response = client.PushToCRM(xmlString, "1");                
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, string.Empty);
                objErr.LogException(); 
            }

            var responseEntity = new TResponse()
            {
                ResponseId = string.IsNullOrWhiteSpace(response) ? 0 : (Convert.ToInt64(response) > 0 ? Convert.ToUInt64(response) : 0)
            };

            t.CRMLeadId = responseEntity.ResponseId;

            return responseEntity;
        }

        public void UpdateResponse(T t, TResponse t2)
        {
            try
            {
                _pqRepo.UpdateCRMLeadId(t);
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, string.Empty);
                objErr.LogException();
            } 
        }
    }
}