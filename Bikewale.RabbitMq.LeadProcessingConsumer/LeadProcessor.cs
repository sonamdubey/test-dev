
using Bikewale.RabbitMq.LeadProcessingConsumer.AutoBizServiceRef;
using Consumer;
using RabbitMQ.Client;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 24 Feb 2017
    /// Description :   LeadProcessor handles the RabbitMq Related functions as follows
    /// - Open connetion to Rabbit Mq queue
    /// - Read queue message and forward to Business Layer for further processing
    /// </summary>
    internal class LeadConsumer
    {
        private readonly string _applicationName, _retryCount, _RabbitMsgTTL;
        private readonly LeadProcessor _leadProcessor;

        private IConnection _connetionRabbitMq;
        private IModel _model;
        private QueueingBasicConsumer consumer;
        private NameValueCollection nvc = new NameValueCollection();
        private string _queueName, _hostName;
        private uint _hondaGaddiId, _bajajFinanceId;
        public LeadConsumer()
        {
            try
            {
                _queueName = String.Format("RabbitMq-{0}-Queue", ConfigurationManager.AppSettings["QueueName"].ToUpper());
                _hostName = CreateConnection.nodes[(new Random()).Next(CreateConnection.nodes.Count)];
                // Application Name sent in the E-Mail
                SendMail.APPLICATION = _applicationName = Convert.ToString(ConfigurationManager.AppSettings["ConsumerName"]);
                _retryCount = ConfigurationManager.AppSettings["RetryCount"];
                _RabbitMsgTTL = ConfigurationManager.AppSettings["RabbitMsgTTL"];
                UInt32.TryParse(ConfigurationManager.AppSettings["HondaGaddiId"], out _hondaGaddiId);
                UInt32.TryParse(ConfigurationManager.AppSettings["BajajFinanceId"], out _bajajFinanceId);
                InitConsumer();
                _leadProcessor = new LeadProcessor();

            }
            catch (Exception ex)
            {
                SendMail.HandleException(ex, String.Format("{0} - Closed on Lead Consumer constructor", _applicationName));
            }
        }

        private void InitConsumer()
        {
            try
            {
                // Create the connection based on Host Name and Queue Name
                CreateConnection.Connect(_queueName, _hostName);
                _connetionRabbitMq = CreateConnection.Connection;
                if (_connetionRabbitMq != null)
                {
                    CreateConnection.CreateChannel();
                    _model = CreateConnection.Model;
                    if (_model == null)
                    {
                        SendMail.HandleException(new Exception("_model is null"), String.Format("{0} - Consumer Closed", _applicationName));
                    }
                    else
                    {
                        consumer = CreateConnection.CreateConsumer();
                        if (consumer == null)
                        {
                            SendMail.HandleException(new Exception("consumer is null. Failed to Create consumer"), String.Format("{0} - Consumer Closed", _applicationName));
                        }
                        else
                        {
                            consumer.ConsumerCancelled += consumer_ConsumerCancelled;
                        }
                    }
                }
                else
                {
                    SendMail.HandleException(new Exception("_connetionRabbitMq is null"), String.Format("{0} - Consumer Closed", _applicationName));
                }
            }
            catch (Exception ex)
            {
                SendMail.HandleException(ex, String.Format("Error in InitConsumer - {0} - Closed", _applicationName));
            }
        }

        public void ProcessMessages()
        {
            try
            {
                while (true && _leadProcessor != null)
                {
                    Logs.WriteInfoLog("RabbitMQ Execution: Waiting for job");
                    RabbitMQ.Client.Events.BasicDeliverEventArgs arg = (RabbitMQ.Client.Events.BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    try
                    {
                        nvc = ByteArrayToObject(arg.Body);
                        uint pqId, dealerId, pincodeId, leadSourceId, versionId, cityId;
                        LeadTypes leadType = default(LeadTypes);
                        if (nvc != null
                            && nvc.HasKeys()
                            && UInt32.TryParse(nvc["pqId"], out pqId) && pqId > 0
                            && UInt32.TryParse(nvc["dealerId"], out dealerId) && dealerId > 0
                            && UInt32.TryParse(nvc["versionId"], out versionId) && versionId > 0
                            && UInt32.TryParse(nvc["cityId"], out cityId) && cityId > 0
                            )
                        {
                            Logs.WriteInfoLog(String.Format("Pqid :{0}, dealerid : {1} - Message received for processing", pqId, dealerId));

                            Enum.TryParse(nvc["leadType"], out leadType);
                            UInt32.TryParse(nvc["pincodeId"], out pincodeId);
                            UInt32.TryParse(nvc["LeadSourceId"], out leadSourceId);

                            PriceQuoteParametersEntity priceQuote = new PriceQuoteParametersEntity()
                            {
                                CustomerName = nvc["customerName"],
                                CustomerEmail = nvc["customerEmail"],
                                CustomerMobile = nvc["customerMobile"],
                                VersionId = versionId,
                                DealerId = dealerId,
                                CityId = cityId,
                                CampaignId = 0
                            };

                            if (!leadType.Equals(LeadTypes.Manufacturer))
                            {
                                priceQuote = _leadProcessor.GetPriceQuoteDetails(pqId);
                            }


                            if (priceQuote != null)
                            {
                                if (nvc["iteration"] == _retryCount)
                                {
                                    _model.BasicReject(arg.DeliveryTag, false);
                                    Logs.WriteInfoLog(String.Format("{0} Message Rejected because iteration count is {1}", Newtonsoft.Json.JsonConvert.SerializeObject(nvc), nvc["iteration"]));
                                    continue;
                                }

                                UInt16 iteration = (UInt16)(nvc["iteration"] == null ? 1 : (UInt16.Parse(nvc["iteration"]) + 1));

                                bool success = false;
                                switch (leadType)
                                {
                                    case LeadTypes.Dealer:
                                        success = PushDealerLead(priceQuote, pqId, iteration);
                                        break;
                                    case LeadTypes.Manufacturer:
                                        success = PushManufacturerLead(priceQuote, pqId, pincodeId, leadSourceId, iteration);
                                        break;
                                    default:
                                        success = PushDealerLead(priceQuote, pqId, iteration);
                                        break;
                                }

                                if (success)
                                {
                                    Logs.WriteInfoLog(String.Format("Pqid :{0}, dealerid : {1} Message processed successfully", pqId, dealerId));
                                    _model.BasicAck(arg.DeliveryTag, false);
                                }
                                else
                                {
                                    Logs.WriteInfoLog(String.Format("Pqid :{0}, dealerid : {1}, Message processed into dead letter queue", pqId, dealerId));
                                    DeadLetterPublish(nvc, _queueName);
                                    _model.BasicReject(arg.DeliveryTag, false);
                                }
                            }
                            else
                            {
                                _model.BasicReject(arg.DeliveryTag, false);
                                Logs.WriteInfoLog(String.Format("Pqid :{0}, dealerid : {1} - PriceQuote data is null", pqId, dealerId));
                            }
                        }
                        else
                        {
                            _model.BasicReject(arg.DeliveryTag, false);
                            Logs.WriteInfoLog(String.Format("Pqid :{0}, dealerid : {1} Message is invalid", nvc["pqId"], nvc["dealerId"]));
                        }
                    }
                    catch (Exception ex)
                    {
                        _model.BasicReject(arg.DeliveryTag, false);
                        Logs.WriteInfoLog(String.Format("Pqid :{0}, dealerid : {1}. Error occured while processing message: Message : {2}", nvc["pqId"], nvc["dealerId"], ex.Message));
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("ProcessMessages: " + ex.Message);
                SendMail.HandleException(ex, String.Format("Error in ProcessMessages - {0} - Closed", _applicationName));
            }
        }

        private bool PushManufacturerLead(PriceQuoteParametersEntity priceQuote, uint pqId, uint pincodeId, uint leadSourceId, ushort iteration)
        {
            bool isSuccess = false;
            try
            {
                if (priceQuote != null)
                {
                    Logs.WriteInfoLog(String.Format("Manufacturer Lead started processing."));


                    string jsonInquiryDetails = String.Format("{{ \"CustomerName\": \"{0}\", \"CustomerMobile\":\"{1}\", \"CustomerEmail\":\"{2}\", \"VersionId\":\"{3}\", \"CityId\":\"{4}\", \"CampaignId\":\"{5}\", \"InquirySourceId\":\"39\", \"Eagerness\":\"1\",\"ApplicationId\":\"2\"}}", priceQuote.CustomerName, priceQuote.CustomerMobile, priceQuote.CustomerEmail, priceQuote.VersionId, priceQuote.CityId, priceQuote.CampaignId);

                    ManufacturerLeadEntity leadEntity = new ManufacturerLeadEntity()
                    {
                        PQId = pqId,
                        Mobile = priceQuote.CustomerMobile,
                        Email = priceQuote.CustomerEmail,
                        Name = priceQuote.CustomerName,
                        DealerId = priceQuote.DealerId,
                        PinCodeId = pincodeId,
                        LeadSourceId = leadSourceId

                    };


                    if (_leadProcessor.SaveManufacturerLead(leadEntity))
                    {
                        isSuccess = _leadProcessor.PushLeadToAutoBiz(pqId, priceQuote.DealerId, (uint)priceQuote.CampaignId, jsonInquiryDetails, iteration);

                        if (priceQuote.DealerId == _hondaGaddiId)
                        {
                            Logs.WriteInfoLog(String.Format("Honda gaadi.com  Lead started processing."));
                            isSuccess = _leadProcessor.PushLeadToGaadi(leadEntity);
                            Logs.WriteInfoLog(String.Format("Honda gaadi.com  Lead submitted."));
                        }
                        else if (priceQuote.DealerId == _bajajFinanceId)
                        {
                            Logs.WriteInfoLog(String.Format("Bajaj Finance Lead started processing."));
                            isSuccess = _leadProcessor.PushLeadToBajajFinance(priceQuote, pqId, pincodeId);
                            Logs.WriteInfoLog(String.Format("Bajaj Finance Lead submitted."));
                        }
                    }

                    Logs.WriteInfoLog(String.Format("Manufacturer Lead submitted."));

                }
            }
            catch (Exception ex)
            {
                Logs.WriteInfoLog(String.Format("PushManufacturerLead(pqId={0}) : {1}", pqId, ex.Message));
            }

            return false;
        }

        private bool PushDealerLead(PriceQuoteParametersEntity priceQuote, uint pqId, ushort iteration)
        {
            try
            {
                if (priceQuote != null)
                {
                    string jsonInquiryDetails = String.Format("{{ \"CustomerName\": \"{0}\", \"CustomerMobile\":\"{1}\", \"CustomerEmail\":\"{2}\", \"VersionId\":\"{3}\", \"CityId\":\"{4}\", \"CampaignId\":\"{5}\", \"InquirySourceId\":\"39\", \"Eagerness\":\"1\",\"ApplicationId\":\"2\"}}", priceQuote.CustomerName, priceQuote.CustomerMobile, priceQuote.CustomerEmail, priceQuote.VersionId, priceQuote.CityId, priceQuote.CampaignId);
                    Logs.WriteInfoLog(String.Format("Dealer Lead : CampaignId = {0}", priceQuote.CampaignId));

                    return (_leadProcessor.PushLeadToAutoBiz(pqId, priceQuote.DealerId, (uint)priceQuote.CampaignId, jsonInquiryDetails, iteration));

                }
            }
            catch (Exception ex)
            {
                Logs.WriteInfoLog(String.Format("PushDealerLead(pqId={0}) : {1}", pqId, ex.Message));
            }

            return false;
        }


        private void consumer_ConsumerCancelled(object sender, RabbitMQ.Client.Events.ConsumerEventArgs args)
        {
            Logs.WriteInfoLog("Consumer Cancelled event called");
            CreateConnection.CloseConnections();
            CreateConnection.RefreshNodes();
            CreateConnection.GetNextNode();
            _queueName = CreateConnection.QueueName;
            _hostName = CreateConnection.serverIp;
            InitConsumer();
            ProcessMessages();
        }

        /// <summary>
        /// Publish the message in Dead Letter Que in the event of failure
        /// </summary>
        /// <param name="nvc">Name Value Collection having parameters to be passed</param>
        /// <param name="queueName">Name of the queue for which Dead Letter Queue is Published</param>
        private void DeadLetterPublish(NameValueCollection nvc, string queueName)
        {
            // Create the instance of Publisher Object
            RabbitMqPublish publish = new RabbitMqPublish();

            // Enable dead letter exchange in the queue
            publish.UseDeadLetterExchange = true;
            publish.MessageTTL = int.Parse(_RabbitMsgTTL);
            // Increment the iteration value
            int iteration = nvc["iteration"] == null ? 1 : Int16.Parse(nvc["iteration"]) + 1;

            // Set the value of iteration in the Name Value Collection passed
            nvc.Set("iteration", iteration.ToString());

            // Finally publish to the queue
            publish.PublishToQueue(queueName, nvc);
        }

        /// <summary>
        /// The method converts the byte array to nvc object
        /// </summary>
        /// <param name="byteArray">Byte Array</param>
        /// <returns>Name Value Collection</returns>
        private NameValueCollection ByteArrayToObject(byte[] byteArray)
        {
            // convert byte array to memory stream
            MemoryStream memstr = new MemoryStream(byteArray);

            // create new BinaryFormatter
            BinaryFormatter bf = new BinaryFormatter();

            // bf.Binder = new Version1ToVersion2DeserializationBinder();
            bf.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;

            // set memory stream position to starting point
            memstr.Position = 0;

            // Deserializes a stream into an object graph and return as a object.
            NameValueCollection obj = (NameValueCollection)bf.Deserialize(memstr);

            return obj;
        }
    }

    internal class LeadProcessor
    {
        private readonly LeadProcessingRepository _repository = null;
        private readonly TCApi_Inquiry _inquiryAPI = null;
        private HttpClient _httpClient;
        private string _hondaGaddiAPIUrl, _bajajFinanceAPIUrl;


        /// <summary>
        /// Created by  :   Sumit Kate on 24 Feb 2017
        /// Description :   Type Initializer
        /// </summary>
        public LeadProcessor()
        {
            _repository = new LeadProcessingRepository();
            _inquiryAPI = new TCApi_Inquiry();
            _httpClient = new HttpClient();
            _hondaGaddiAPIUrl = ConfigurationManager.AppSettings["HondaGaddiAPIUrl"];
            _bajajFinanceAPIUrl = ConfigurationManager.AppSettings["BajajFinanceAPIUrl"];
        }

        public bool PushLeadToAutoBiz(uint pqId, uint dealerId, uint campaignId, string inquiryJson, UInt16 retryAttempt)
        {
            bool isSuccess = false;
            string abInquiryId = string.Empty;
            uint abInqId = 0;
            try
            {
                Logs.WriteInfoLog(String.Format("Push To AB Iteration {0}", retryAttempt));
                abInquiryId = _inquiryAPI.AddNewCarInquiry(dealerId.ToString(), inquiryJson);
                Logs.WriteInfoLog(String.Format("Response ab inquiryid : {0}", abInquiryId));
                if (UInt32.TryParse(abInquiryId, out abInqId) && abInqId > 0)
                {
                    Logs.WriteInfoLog("Update Lead Limit.");
                    if (campaignId > 0)
                    {
                        isSuccess = _repository.IsDealerDailyLeadLimitExceeds(campaignId);
                        isSuccess = _repository.UpdateDealerDailyLeadCount(campaignId, abInqId);
                    }

                    isSuccess = _repository.PushedToAB(pqId, abInqId, retryAttempt);
                    Logs.WriteInfoLog("Saved AB InquiryId");
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(string.Format("Ex Message : {1}. PushToAb failed Data : {0}, pqId : {2},dealerId : {3}, campaignId : {4}", inquiryJson, ex.Message, pqId, dealerId, campaignId));
            }
            return isSuccess;
        }

        public bool PushLeadToGaadi(ManufacturerLeadEntity leadEntity)
        {
            string leadURL = string.Empty;
            string response = string.Empty;
            bool isSuccess = false;
            try
            {

                BikeQuotationEntity quotation = _repository.GetPriceQuoteById(leadEntity.PQId);

                GaadiLeadEntity gaadiLead = new GaadiLeadEntity()
                {
                    City = quotation.City,
                    Email = leadEntity.Email,
                    Make = quotation.MakeName,
                    Mobile = leadEntity.Mobile,
                    Model = quotation.ModelName,
                    Name = leadEntity.Name,
                    Source = "bikewale",
                    State = quotation.State
                };

                if (_httpClient != null)
                {
                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(gaadiLead);
                    byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(jsonString);
                    leadURL = String.Format("{1}{0}", _hondaGaddiAPIUrl, System.Convert.ToBase64String(toEncodeAsBytes));

                    using (HttpResponseMessage _response = _httpClient.GetAsync(leadURL).Result)
                    {
                        if (_response.IsSuccessStatusCode)
                        {
                            if (_response.StatusCode == System.Net.HttpStatusCode.OK) //Check 200 OK Status        
                            {
                                response = _response.Content.ReadAsStringAsync().Result;
                                _repository.UpdateManufacturerLead(leadEntity.PQId, leadEntity.Email, leadEntity.Mobile, response);
                                _response.Content.Dispose();
                                _response.Content = null;
                                isSuccess = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteInfoLog(String.Format("PushLeadToGaadi : {0}", ex.Message));
            }
            return isSuccess;
        }


        internal bool PushLeadToBajajFinance(PriceQuoteParametersEntity priceQuote, uint pqId, uint pincodeId)
        {
            bool isSuccess = false;
            try
            {
                BajajFinanceLeadEntity bikeMappingInfo = _repository.GetBajajFinanceBikeMappingInfo(priceQuote.VersionId, pincodeId);
                if (bikeMappingInfo != null)
                {
                    if (!string.IsNullOrEmpty(priceQuote.CustomerName))
                    {
                        int spaceIndex = priceQuote.CustomerName.IndexOf(" ");
                        if (spaceIndex > 0)
                        {
                            bikeMappingInfo.FirstName = priceQuote.CustomerName.Substring(0, spaceIndex);
                            bikeMappingInfo.LastName = priceQuote.CustomerName.Substring(spaceIndex + 1);
                        }
                    }

                    bikeMappingInfo.MobileNo = priceQuote.CustomerMobile;
                    bikeMappingInfo.EmailID = priceQuote.CustomerEmail;
                }

                BajajFinanceLeadInput bajajLeadInput = new BajajFinanceLeadInput()
                {
                    leadData = new List<BajajFinanceLeadEntity>() { bikeMappingInfo }
                };

                if (_httpClient != null)
                {
                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(bajajLeadInput);
                    HttpContent httpContent = new StringContent(jsonString);
                    using (HttpResponseMessage _response = _httpClient.PostAsync(_bajajFinanceAPIUrl, httpContent).Result)
                    {
                        if (_response.IsSuccessStatusCode)
                        {
                            if (_response.StatusCode == System.Net.HttpStatusCode.OK) //Check 200 OK Status        
                            {
                                string response = _response.Content.ReadAsStringAsync().Result;
                                _repository.UpdateManufacturerLead(pqId, priceQuote.CustomerEmail, priceQuote.CustomerMobile, response);
                                _response.Content.Dispose();
                                _response.Content = null;
                                isSuccess = true;
                            }
                        }
                    }

                    Logs.WriteInfoLog(String.Format("Bajaj Finance Reuest Response : {0}", jsonString));
                }
            }
            catch (Exception ex)
            {
                Logs.WriteInfoLog(String.Format("PushLeadToBajajFinance : {0}", ex.Message));
            }
            return isSuccess;
        }

        internal PriceQuoteParametersEntity GetPriceQuoteDetails(uint pqId)
        {
            return _repository.FetchPriceQuoteDetailsById(pqId);
        }

        internal bool SaveManufacturerLead(ManufacturerLeadEntity leadEntity)
        {
            return _repository.SaveManufacturerLead(leadEntity);
        }

    }
}
