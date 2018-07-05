
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using PriceQuoteConsumer.Entities;
using RabbitMqPublishing;

namespace PriceQuoteConsumer
{
	internal class PQConsumer
	{
		private string _queueName;
		private static PriceQuoteRepository _pqRepository;
		public PQConsumer()
		{
			_queueName = ConfigurationManager.AppSettings["QueueName"];
			_pqRepository = new PriceQuoteRepository();
		}

		public void ProcessMessages()
		{
			try
			{
				RabbitMqPublish _RabbitMQPublishing = new RabbitMqPublish();
				_RabbitMQPublishing.RunCousumer(RabbitMQExecution, _queueName);
			}
			catch (Exception ex)
			{
				Logger.LogError("PQConsumer.ProcessMessages : ", ex);
			}
		}

		static bool RabbitMQExecution(NameValueCollection nvc)
		{
			try
			{
				// For unvalidated messages dropping them from queue by returning true from the Execution method
                if (validateMessage(nvc))
                {
                    PriceQuoteParameters pqParams = new PriceQuoteParameters()
                    {
                        GUID = nvc["GUID"],
                        VersionId = Convert.ToUInt32(String.IsNullOrEmpty(nvc["versionId"]) ? "0" : nvc["versionId"]),
                        CityId = Convert.ToUInt32(String.IsNullOrEmpty(nvc["cityId"]) ? "0" : nvc["cityId"]),
                        AreaId = Convert.ToUInt32(String.IsNullOrEmpty(nvc["areaId"]) ? "0" : nvc["areaId"]),
                        BuyingPreference = Convert.ToUInt16(String.IsNullOrEmpty(nvc["buyingPreference"]) ? "0" : nvc["buyingPreference"]),
                        CustomerId = Convert.ToUInt64(String.IsNullOrEmpty(nvc["customerId"]) ? "0" : nvc["customerId"]),
                        CustomerName = nvc["customerName"],
                        CustomerEmail = nvc["customerEmail"],
                        CustomerMobile = nvc["customerMobile"],
                        ClientIP = nvc["clientIP"],
                        SourceId = Convert.ToUInt16(String.IsNullOrEmpty(nvc["sourceId"]) ? "0" : nvc["sourceId"]),
                        DealerId = Convert.ToUInt32(String.IsNullOrEmpty(nvc["dealerId"]) ? "0" : nvc["dealerId"]),
                        DeviceId = nvc["deviceId"],
                        UTMA = nvc["UTMA"],
                        UTMZ = nvc["UTMZ"],
                        PQSourceId = Convert.ToUInt16(String.IsNullOrEmpty(nvc["pqSourceId"]) ? "0" : nvc["pqSourceId"]),
                        RefGUID = nvc["refGUID"]
                    };
                    return _pqRepository.RegisterPriceQuote(pqParams);
                }
			}
			catch (Exception ex)
			{
				Logger.LogError(String.Format("PQConsumer.RabbitMQExecution: Error while performing operation for versionId = {0}, GUID = {1}, pqSourceId = {2}", nvc["versionId"], nvc["GUID"], nvc["pqSourceId"]), ex);
			}
			return true;
		}

		/// <summary>
		/// Method for validating queueMessage
		/// </summary>
		/// <param name="nvc"></param>
		/// <returns></returns>
		static bool validateMessage(NameValueCollection nvc)
		{
			try {
				if ( !String.IsNullOrEmpty(nvc["GUID"]) && !String.IsNullOrEmpty(nvc["versionId"]) && Convert.ToUInt32(nvc["versionId"]) > 0)
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(String.Format("PQConsumer.validateMessage : versionId = {0}, GUID = {1}", nvc["versionId"], nvc["GUID"]), ex);
			}
			return false;
		}
	}
}
