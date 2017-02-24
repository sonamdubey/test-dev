
using Bikewale.RabbitMq.LeadProcessingConsumer.AutoBizServiceRef;
using Consumer;
using RabbitMQ.Client;
using RabbitMqPublishing;
using System;
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

        private IConnection _connetionRabbitMq;
        private IModel _model;
        private QueueingBasicConsumer consumer;
        private NameValueCollection nvc = null;
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
                        uint pqId, dealerId, campaignId;
                        if (nvc != null
                            && nvc.HasKeys()
                            && (!String.IsNullOrEmpty(nvc["inquiryJson"])
                            && UInt32.TryParse(nvc["pqId"], out pqId)
                            && pqId > 0
                            && UInt32.TryParse(nvc["dealerId"], out dealerId)
                            && dealerId > 0
                            && UInt32.TryParse(nvc["campaignId"], out campaignId)
                            && campaignId > 0))
                        {
                            if (nvc["iteration"] == _retryCount)
                            {
                                _model.BasicReject(arg.DeliveryTag, false);
                                Logs.WriteInfoLog(String.Format("{0} Message Rejected because iteration count is {1}", Newtonsoft.Json.JsonConvert.SerializeObject(nvc), nvc["iteration"]));
                                continue;
                            }
                            UInt16 iteration = (UInt16)(nvc["iteration"] == null ? 1 : (UInt16.Parse(nvc["iteration"]) + 1));
                            if (_leadProcessor.PushLeadToAutoBiz(pqId, dealerId, campaignId, nvc["inquiryJson"], iteration))
                            {
                                _model.BasicAck(arg.DeliveryTag, false);
                            }
                            else
                            {
                                DeadLetterPublish(nvc, ConfigurationManager.AppSettings["QueueName"].ToUpper());
                                _model.BasicReject(arg.DeliveryTag, false);
                            }
                        }
                        else
                        {
                            _model.BasicReject(arg.DeliveryTag, false);
                            Logs.WriteInfoLog("Invalid NVC : " + Newtonsoft.Json.JsonConvert.SerializeObject(nvc));
                        }
                    }
                    catch (Exception ex)
                    {
                        _model.BasicReject(arg.DeliveryTag, false);
                        Logs.WriteInfoLog(string.Format("Message {1}. Some Error Occured while Dequeuing : {0}", Newtonsoft.Json.JsonConvert.SerializeObject(nvc), ex.Message));
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("ProcessMessages: " + ex.Message);
                SendMail.HandleException(ex, String.Format("Error in ProcessMessages - {0} - Closed", _applicationName));
            }
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
        /// <summary>
        /// Created by  :   Sumit Kate on 24 Feb 2017
        /// Description :   Type Initializer
        /// </summary>
        public LeadProcessor()
        {
            _repository = new LeadProcessingRepository();
            _inquiryAPI = new TCApi_Inquiry();
        }

        public bool PushLeadToAutoBiz(uint pqId, uint dealerId, uint campaignId, string inquiryJson, UInt16 retryAttempt)
        {
            bool isSuccess = false;
            string abInquiryId = string.Empty;
            uint abInqId = 0;
            try
            {
                abInquiryId = _inquiryAPI.AddNewCarInquiry(dealerId.ToString(), inquiryJson);
                if (UInt32.TryParse(abInquiryId, out abInqId) && abInqId > 0)
                {
                    _repository.UpdateDealerDailyLeadCount(campaignId, abInqId);
                    isSuccess = _repository.PushedToAB(pqId, abInqId, retryAttempt);
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(string.Format("Ex Message : {1}. PushToAb failed Data : {0}, pqId : {2},dealerId : {3}, campaignId : {4}", inquiryJson, ex.Message, pqId, dealerId, campaignId));
            }
            return isSuccess;
        }
    }
}
