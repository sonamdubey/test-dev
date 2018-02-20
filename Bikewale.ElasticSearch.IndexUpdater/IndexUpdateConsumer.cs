using Bikewale.DAL.CoreDAL;
using Consumer;
using Nest;
using RabbitMQ.Client;
using RabbitMqPublishing;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bikewale.ElasticSearch.IndexUpdaterConsumer
{
    internal class IndexUpdateConsumer
    {
        private readonly string _applicationName, _retryCount, _RabbitMsgTTL;
        private IConnection _connetionRabbitMq;
        private IModel _model;
        private QueueingBasicConsumer consumer;
        private string _queueName, _hostName;
        private ElasticClient client = ElasticSearchInstance.GetInstance();

        private NameValueCollection nvc = new NameValueCollection();
        public IndexUpdateConsumer(string queueName, string consumerName, string retryCount, string rabbitMsgTTL)
        {
            try
            {
                _queueName = String.Format("RabbitMq-{0}-Queue", queueName);
                _hostName = CreateConnection.nodes[(new Random()).Next(CreateConnection.nodes.Count)];
                // Application Name sent in the E-Mail
                SendMail.APPLICATION = _applicationName = Convert.ToString(consumerName);
                _retryCount = retryCount;
                _RabbitMsgTTL = rabbitMsgTTL;
                InitConsumer();
            }
            catch (Exception ex)
            {
                SendMail.HandleException(ex, String.Format("{0} - Closed on IndexUpdateConsumer constructor", _applicationName));
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
                while (true)
                {
                    Logs.WriteInfoLog("RabbitMQ Execution: Waiting for job");
                    RabbitMQ.Client.Events.BasicDeliverEventArgs arg = (RabbitMQ.Client.Events.BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    try
                    {
                        nvc = ByteArrayToObject(arg.Body);
                        if(nvc != null 
                            && nvc.HasKeys() 
                            && !String.IsNullOrEmpty(nvc["indexName"])
                            && !String.IsNullOrEmpty(nvc["documentType"])
                            && !String.IsNullOrEmpty(nvc["document"])
                            && !String.IsNullOrEmpty(nvc["documentId"]))
                        {
                            string indexName = nvc["indexName"];
                            string documentType = nvc["documentType"];
                            string documentJson = nvc["documentJson"];
                            int documentId = int.Parse(nvc["documentId"]);

                            if (client != null && client.IndexExists(indexName).Exists)
                            {
                                Logs.WriteInfoLog(String.Format("Index named {0} exists", indexName));
                                bool isOperationSuccessful = InsertOrUpdateDocumentInIndex(indexName, documentId, documentType, documentJson);
                                if (isOperationSuccessful)
                                {
                                    Logs.WriteInfoLog(String.Format("Document ID {0} successfully inserted/updated into the Index named {1}", indexName, documentId));
                                }
                                else
                                {
                                    Logs.WriteErrorLog(String.Format("An Error Occured while updating/inserting Document ID {0} into the Index named {1}", indexName, documentId));
                                }
                            }
                            else
                            {
                                Logs.WriteErrorLog(String.Format("Either client is null OR index named {0} does not exist.", indexName));
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        _model.BasicReject(arg.DeliveryTag, false);
                        if (nvc != null)
                        {
                            Logs.WriteInfoLog(String.Format("Error occured while processing message: indexName => {0}, documentId => {1}, documentType => {2}, document => {3}, ErrorMessage => {4}", nvc["indexName"], nvc["documentType"], nvc["document"], ex.Message));
                        }
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

            bf.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;

            // set memory stream position to starting point
            memstr.Position = 0;

            // Deserializes a stream into an object graph and return as a object.
            NameValueCollection obj = (NameValueCollection)bf.Deserialize(memstr);

            return obj;
        }
        
        /// <summary>
        /// This method is used to Insert/Update a document in an Elastic Search Index
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        private bool InsertOrUpdateDocumentInIndex(string indexName, int documentId, string documentType, string documentJson)
        {
            bool isSuccessful = false;
            if (client.DocumentExists(new DocumentExistsRequest(indexName, documentType, documentId)).Exists)
            {
                Logs.WriteInfoLog(String.Format("Document with ID = {0} exists => UPDATE IT", documentId));
            }
            else
            {
                Logs.WriteInfoLog(String.Format("Document with ID = {0} does not exist => INSERT IT", documentId));
            }

            isSuccessful = client.Index(documentJson, i => i
                                  .Index(indexName)
                                  .Type(documentType)
                                  .Id(documentId)
                                  )
                                  .IsValid;

            return isSuccessful;
        }
    }
}
