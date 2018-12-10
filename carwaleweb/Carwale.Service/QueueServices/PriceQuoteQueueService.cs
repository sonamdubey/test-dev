using System;
using System.Collections.Generic;
using Carwale.Interfaces;
using Carwale.Entity;
using Carwale.Notifications;
using Carwale.Entity.PriceQuote;
using RabbitMqPublishing;

namespace Carwale.Service
{
    public class PriceQuoteQueueService<T> : IRequestManager<T> where T : PriceQuoteEntity
    {
        private readonly ICustomerBL<Customer, CustomerOnRegister> _customerRepo;
        private readonly IPriceQuoteRepository<PriceQuoteEntity> _pqRepo;

        public PriceQuoteQueueService(
            ICustomerBL<Customer, CustomerOnRegister> customerRepo, 
            IPriceQuoteRepository<PriceQuoteEntity> pqRepo)
        {
            _customerRepo = customerRepo;
            _pqRepo = pqRepo;
        }

        public U ProcessRequest<U>(T t)
        {            
            try
            {
                ulong customerId = RegisterCustomer(t);

                if(customerId > 0)
                { 
                    t.CustomerId = customerId; // Assign registered customer id property of price quote entity object 

                    _pqRepo.UpdateCustomerId(t); // Update CustomerId in NewCarPurchaseInquires Table

                    CallApis(t);

                    //_leadScore.CalculateScore(t); // Lead Scoring
                }
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "Carwale.Service.PriceQuoteQueueService");
                objErr.LogException();

                //send to failsed queue
                RabbitMqPublish publish = new RabbitMqPublish();
                publish.PublishAnyObjectToQueue<T>("PRICE_QUOTE_QUEUE_FAILED", t);
            }

            return default(U);
        }

        private void CallApis(PriceQuoteEntity t)
        {
            List<string> clientList = GetAPIClients(t);

            foreach (var client in clientList)
            {
                //string classPath = "Carwale.Service.CRMApi<T, TResponse>";//ConfigurationManager.AppSettings["CRMApi"];

                //IAPIService<PriceQuoteEntity, APIResponseEntity> _clientApiService = (IAPIService<PriceQuoteEntity, APIResponseEntity>)Activator.CreateInstance(Type.GetType(classPath));

                // Call the CRM API
                IAPIService<PriceQuoteEntity, APIResponseEntity> _clientApiService = new CRMApi<PriceQuoteEntity, APIResponseEntity>(_pqRepo);
                APIResponseEntity apiResponse = _clientApiService.Request(t);

                // Update API response
                _clientApiService.UpdateResponse(t, apiResponse);
            }
        }

        private ulong RegisterCustomer(PriceQuoteEntity t)
        {
            var customer = new Customer()
            {
                Name = t.Name,
                Email = t.Email,
                Mobile = t.Mobile
            };

            // Register customer to the CarWale's customers database, get an id in return
            ulong customerId = (ulong)Convert.ChangeType(_customerRepo.CreateCustomer(customer).CustomerId, typeof(ulong));

            return customerId;
        }                

        private List<string> GetAPIClients(PriceQuoteEntity t)
        {
            var clientList = new List<string>();
            
            if (_pqRepo.CanBePushToCRM(t))
            {
                clientList.Add("CRMApi");
            }
                        
            return clientList;
        }

        public ulong ProcessDealerInquiry(T t)
        {
            throw new NotImplementedException();
        }
    }
}
