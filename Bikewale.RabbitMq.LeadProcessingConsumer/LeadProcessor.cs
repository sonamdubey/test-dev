﻿
using Bikewale.RabbitMq.LeadProcessingConsumer.AutoBizServiceRef;
using Consumer;
using RabbitMQ.Client;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
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
        private readonly LeadProcessingRepository _repository = null;

        private IConnection _connetionRabbitMq;
        private IModel _model;
        private QueueingBasicConsumer consumer;
        private NameValueCollection nvc = new NameValueCollection();
        private string _queueName, _hostName;
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
                InitConsumer();
                _leadProcessor = new LeadProcessor();
                _repository = new LeadProcessingRepository();
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
                        uint pqId, dealerId, pincodeId, leadSourceId, versionId, cityId, manufacturerDealerId, manufacturerLeadId;
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
                            UInt32.TryParse(nvc["manufacturerDealerId"], out manufacturerDealerId);
                            UInt32.TryParse(nvc["manufacturerLeadId"], out manufacturerLeadId);
                            var priceQuote = _leadProcessor.GetPriceQuoteDetails(pqId);


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
                                        success = PushManufacturerLead(priceQuote, pqId, pincodeId, leadSourceId, iteration, manufacturerDealerId, manufacturerLeadId);
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
                                    DeadLetterPublish(nvc, ConfigurationManager.AppSettings["QueueName"].ToUpper());
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

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jul 2017
        /// Description :   Process Manufacturer Lead
        /// </summary>
        /// <param name="priceQuote"></param>
        /// <param name="pqId"></param>
        /// <param name="pincodeId"></param>
        /// <param name="leadSourceId"></param>
        /// <param name="iteration"></param>
        /// <param name="manufacturerDealerId"></param>
        /// <param name="manufacturerLeadId"></param>
        /// <returns></returns>
        private bool PushManufacturerLead(PriceQuoteParametersEntity priceQuote, uint pqId, uint pincodeId, uint leadSourceId, ushort iteration, uint manufacturerDealerId, uint manufacturerLeadId)
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
                        LeadSourceId = leadSourceId,
                        ManufacturerDealerId = manufacturerDealerId,
                        LeadId = manufacturerLeadId

                    };

                    if (_leadProcessor.SaveManufacturerLead(leadEntity))
                    {
                        ManufacturerLeadEntityBase lead = new ManufacturerLeadEntityBase()
                        {
                            CampaignId = (uint)priceQuote.CampaignId,
                            CityId = priceQuote.CityId,
                            CustomerEmail = priceQuote.CustomerEmail,
                            CustomerMobile = priceQuote.CustomerMobile,
                            CustomerName = priceQuote.CustomerName,
                            DealerId = priceQuote.DealerId,
                            InquiryJSON = jsonInquiryDetails,
                            LeadId = manufacturerLeadId,
                            PinCodeId = pincodeId,
                            PQId = pqId,
                            RetryAttempt = iteration,
                            VersionId = priceQuote.VersionId,
                            ManufacturerDealerId = manufacturerDealerId
                        };

                        isSuccess = _leadProcessor.ProcessManufacturerLead(lead);
                        if (isSuccess)
                            Logs.WriteInfoLog(String.Format("Lead ID : {0} - submitted successfully", manufacturerLeadId));
                        else
                            Logs.WriteInfoLog(String.Format("Lead ID : {0} - process manufacturer failed", manufacturerLeadId));
                    }
                    else
                    {
                        Logs.WriteInfoLog(String.Format("Lead ID : {0} - save manufacturer failed", manufacturerLeadId));
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteInfoLog(String.Format("PushManufacturerLead(Lead Id = {0}) : {1}", manufacturerLeadId, ex.Message));
            }
            return isSuccess;
        }

        private bool PushDealerLead(PriceQuoteParametersEntity priceQuote, uint pqId, ushort iteration)
        {
            try
            {
                if (priceQuote != null)
                {
                    string jsonInquiryDetails = String.Format("{{ \"CustomerName\": \"{0}\", \"CustomerMobile\":\"{1}\", \"CustomerEmail\":\"{2}\", \"VersionId\":\"{3}\", \"CityId\":\"{4}\", \"CampaignId\":\"{5}\", \"InquirySourceId\":\"39\", \"Eagerness\":\"1\",\"ApplicationId\":\"2\"}}", priceQuote.CustomerName, priceQuote.CustomerMobile, priceQuote.CustomerEmail, priceQuote.VersionId, priceQuote.CityId, priceQuote.CampaignId);
                    Logs.WriteInfoLog(String.Format("Dealer Lead : CampaignId = {0}", priceQuote.CampaignId));

                    return (_leadProcessor.PushLeadToAutoBiz(pqId, priceQuote.DealerId, (uint)priceQuote.CampaignId, jsonInquiryDetails, iteration, LeadTypes.Dealer, 0));

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
        private readonly string _hondaGaddiAPIUrl, _bajajFinanceAPIUrl, _tataCapitalAPIUrl;
        private readonly uint _hondaGaddiId, _bajajFinanceId, _RoyalEnfieldId, _TataCapitalId;
        private readonly bool _isTataCapitalAPIStarted = false;
        private readonly IDictionary<uint, IManufacturerLeadHandler> handlers;

        /// <summary>
        /// Created by  :   Sumit Kate on 24 Feb 2017
        /// Description :   Type Initializer
        /// </summary>
        public LeadProcessor()
        {
            _repository = new LeadProcessingRepository();
            _hondaGaddiAPIUrl = ConfigurationManager.AppSettings["HondaGaddiAPIUrl"];
            _bajajFinanceAPIUrl = ConfigurationManager.AppSettings["BajajFinanceAPIUrl"];
            _tataCapitalAPIUrl = ConfigurationManager.AppSettings["TataCapitalAPIUrl"];

            UInt32.TryParse(ConfigurationManager.AppSettings["HondaGaddiId"], out _hondaGaddiId);
            UInt32.TryParse(ConfigurationManager.AppSettings["BajajFinanceId"], out _bajajFinanceId);
            UInt32.TryParse(ConfigurationManager.AppSettings["RoyalEnfieldId"], out _RoyalEnfieldId);
            UInt32.TryParse(ConfigurationManager.AppSettings["TataCapitalId"], out _TataCapitalId);
            Boolean.TryParse(ConfigurationManager.AppSettings["IsTataCapitalAPIStarted"], out _isTataCapitalAPIStarted);
            #region Manufacturer Lead Handlers. Please do not change this 
            handlers = new Dictionary<uint, IManufacturerLeadHandler>();
            //This handler processes all manufacturer lead if no specific handler is defined
            //Please do not remove this handler
            handlers.Add(0, new DefaultManufacturerLeadHandler(0, "", false));
            //Please add new manufacturer lead handler in this section
            handlers.Add(_hondaGaddiId, new HondaManufacturerLeadHandler(_hondaGaddiId, _hondaGaddiAPIUrl, true));
            handlers.Add(_bajajFinanceId, new BajajFinanceLeadHandler(_bajajFinanceId, _bajajFinanceAPIUrl, true));
            handlers.Add(_RoyalEnfieldId, new RoyalEnfieldLeadHandler(_RoyalEnfieldId, "", true, false));
            handlers.Add(_TataCapitalId, new TataCapitalLeadHandler(_TataCapitalId, _tataCapitalAPIUrl, _isTataCapitalAPIStarted));
            #endregion
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 27 Jun 2017
        /// Description :   Added LeadTypes parameter
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="dealerId"></param>
        /// <param name="campaignId"></param>
        /// <param name="inquiryJson"></param>
        /// <param name="retryAttempt"></param>
        /// <param name="leadType"></param>
        /// <returns></returns>
        public bool PushLeadToAutoBiz(uint pqId, uint dealerId, uint campaignId, string inquiryJson, UInt16 retryAttempt, LeadTypes leadType, uint leadId)
        {
            bool isSuccess = false;
            string abInquiryId = string.Empty;
            uint abInqId = 0;
            try
            {
                Logs.WriteInfoLog(String.Format("Push To AB Iteration {0}", retryAttempt));

                using (TCApi_Inquiry _inquiryAPI = new TCApi_Inquiry())
                {
                    abInquiryId = _inquiryAPI.AddNewCarInquiry(dealerId.ToString(), inquiryJson);
                }

                Logs.WriteInfoLog(String.Format("Response ab inquiryid : {0}", abInquiryId));
                if (UInt32.TryParse(abInquiryId, out abInqId) && abInqId > 0)
                {
                    Logs.WriteInfoLog("Update Lead Limit.");
                    if (campaignId > 0)
                    {
                        isSuccess = _repository.IsDealerDailyLeadLimitExceeds(campaignId);
                        isSuccess = _repository.UpdateDealerDailyLeadCount(campaignId, abInqId);
                        isSuccess = _repository.PushedToAB(pqId, abInqId, retryAttempt);
                    }
                    Logs.WriteInfoLog("Saved AB InquiryId");
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(string.Format("Ex Message : {1}. PushToAb failed Data : {0}, pqId : {2},dealerId : {3}, campaignId : {4}", inquiryJson, ex.Message, pqId, dealerId, campaignId));
            }
            return isSuccess;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jul 2017
        /// Description :   Process Manufacturer Lead
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        internal bool ProcessManufacturerLead(ManufacturerLeadEntityBase leadEntity)
        {
            bool leadProcessed = false;
            try
            {
                if (handlers != null && handlers.Count > 0)
                {
                    IManufacturerLeadHandler handler = null;
                    //Find the specific lead handler
                    if (!handlers.TryGetValue(leadEntity.DealerId, out handler))
                    {
                        //If no handler is present submit lead to default handler
                        if (handler == null)
                        {
                            handler = handlers[0];
                        }
                    }
                    //Make sure handler is present
                    if (handler != null)
                    {
                        //Process the lead
                        leadProcessed = handler.Process(leadEntity);

                        if (!leadProcessed)
                        {
                            Logs.WriteInfoLog(String.Format("Lead not processed : {0}", Newtonsoft.Json.JsonConvert.SerializeObject(leadEntity)));
                        }
                        else
                        {
                            Logs.WriteInfoLog(String.Format("Lead processed successfully : Lead Id - {0}", leadEntity.LeadId));
                        }
                    }
                    else
                    {
                        Logs.WriteInfoLog(String.Format("No Lead Handler is present : {0}", Newtonsoft.Json.JsonConvert.SerializeObject(leadEntity)));
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("Exception : ProcessManufacturerLead : {0}, Msg : {1}", Newtonsoft.Json.JsonConvert.SerializeObject(leadEntity), ex.Message));
            }
            return leadProcessed;
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
