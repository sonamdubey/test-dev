﻿using Consumer;
using RabbitMQ.Client;
using RabbitMqPublishing;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bikewale.RabbitMq.CapitalFirstLeadConsumer
{
    internal class LeadConsumer
    {
        private readonly string _applicationName, _retryCount, _RabbitMsgTTL;
        private IConnection _connetionRabbitMq;
        private IModel _model;
        private QueueingBasicConsumer consumer;
        private string _queueName, _hostName;

        private NameValueCollection nvc = new NameValueCollection();
        public LeadConsumer(string queueName, string consumerName, string retryCount, string rabbitMsgTTL)
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
                while (true)
                {
                    Logs.WriteInfoLog("RabbitMQ Execution: Waiting for job");
                    RabbitMQ.Client.Events.BasicDeliverEventArgs arg = (RabbitMQ.Client.Events.BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    try
                    {
                        nvc = ByteArrayToObject(arg.Body);

                        if (nvc != null
                            && nvc.HasKeys()
                            && !String.IsNullOrEmpty(nvc["ctLeadId"])
                            && !String.IsNullOrEmpty(nvc["voucherCode"])
                            && !String.IsNullOrEmpty(nvc["expiryDate"])
                            && !String.IsNullOrEmpty(nvc["agentName"])
                            && !String.IsNullOrEmpty(nvc["agentContactNumber"]))
                        {
                            CarTradeVoucher voucher = new CarTradeVoucher();
                            voucher.LeadId = nvc["ctLeadId"];
                            voucher.VoucherCode = nvc["voucherCode"];
                            voucher.ExpiryDate = Convert.ToDateTime(nvc["expiryDate"]);
                            voucher.AgentName = nvc["agentName"];
                            voucher.AgentContactNumber = nvc["agentContactNumber"];

                            string jsonVoucher = Newtonsoft.Json.JsonConvert.SerializeObject(voucher);

                            Logs.WriteInfoLog(String.Format("CarTrade Lead #{0} ", voucher.LeadId));

                            if (!String.IsNullOrEmpty(jsonVoucher))
                            {
                                if (nvc["iteration"] == _retryCount)
                                {
                                    _model.BasicReject(arg.DeliveryTag, false);
                                    Logs.WriteInfoLog(String.Format("{0} Message Rejected because iteration count is {1}", jsonVoucher, nvc["iteration"]));
                                    continue;
                                }

                                bool success = false;

                                using (LeadConsumerBL _businesslayer = new LeadConsumerBL())
                                {
                                    success = _businesslayer.ProcessLead(voucher);
                                }
                                if (success)
                                {
                                    //Logic

                                    Logs.WriteInfoLog(String.Format("{0} processed successfully.", jsonVoucher));
                                    _model.BasicAck(arg.DeliveryTag, false);
                                }
                                else
                                {
                                    Logs.WriteInfoLog(String.Format("{0} Message processed into dead letter queue", jsonVoucher));
                                    DeadLetterPublish(nvc, ConfigurationManager.AppSettings["QueueName"].ToUpper());
                                    _model.BasicReject(arg.DeliveryTag, false);
                                }
                            }
                            else
                            {
                                _model.BasicReject(arg.DeliveryTag, false);
                                Logs.WriteInfoLog(String.Format("jsonVoucher is null : {0}", jsonVoucher));
                            }
                        }
                        else
                        {
                            _model.BasicReject(arg.DeliveryTag, false);
                            Logs.WriteInfoLog("jsonVoucher Message is invalid");
                        }
                    }
                    catch (Exception ex)
                    {
                        _model.BasicReject(arg.DeliveryTag, false);
                        Logs.WriteInfoLog(String.Format("Error occured while processing message: CT Lead : {0}, Message: {1}", nvc["ctLeadId"], ex.Message));
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
    }
}
