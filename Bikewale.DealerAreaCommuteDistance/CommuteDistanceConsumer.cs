﻿
using Consumer;
using RabbitMQ.Client;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Bikewale.DealerAreaCommuteDistance
{
    /// <summary>
    /// Created by  :   Sumit Kate on 19 Aug 2016
    /// Description :   Rabbit MQ Consumer for Dealer-Area Commute Distance
    /// </summary>
    public class CommuteDistanceConsumer
    {
        internal void RabbitMQExecution(string queueName, string hostName)
        {
            string operationType = string.Empty;
            int cityID;
            UInt32 areaId, dealerId;
            double lattitude, longitude;
            try
            {
                // Create the connection based on Host Name and Queue Name
                CreateConnection.Connect(queueName, hostName);
                IConnection Connection = CreateConnection.Connection;
                // Application Name sent in the E-Mail
                SendMail.APPLICATION = ConfigurationManager.AppSettings["ConsumerName"].ToString();
                queueName = ConfigurationManager.AppSettings["QueueName"].ToUpper();

                if (Connection != null)
                {
                    CreateConnection.CreateChannel();
                    IModel Model = CreateConnection.Model;
                    QueueingBasicConsumer consumer = CreateConnection.CreateConsumer();

                    // Creating a delegate method in case of consumer that is cancelled
                    consumer.ConsumerCancelled += consumer_ConsumerCancelled;
                    CommuteDistanceBL bl = new CommuteDistanceBL();
                    while (true) // This keeps the consumer on in a seeking mode
                    {
                        // Your Business Logic comes here
                        Logs.WriteInfoLog("RabbitMQ Execution: Waiting for job");
                        RabbitMQ.Client.Events.BasicDeliverEventArgs e = (RabbitMQ.Client.Events.BasicDeliverEventArgs)consumer.Queue.Dequeue();
                        NameValueCollection nvc = null;
                        try
                        {
                            // Convert the Byte Array to NVC object
                            nvc = ByteArrayToObject(e.Body);
                            if (nvc != null && nvc.HasKeys() && !String.IsNullOrEmpty(nvc["operationType"]))
                            {
                                if (nvc["iteration"] == "5")
                                {
                                    Model.BasicReject(e.DeliveryTag, false);
                                    Logs.WriteInfoLog("Message Rejected because iteration count is" + nvc["iteration"]);
                                    continue;
                                }
                                operationType = nvc["operationType"].ToUpper();
                                var watch = System.Diagnostics.Stopwatch.StartNew();
                                Logs.WriteInfoLog("Started at : " + DateTime.Now);
                                switch (operationType)
                                {
                                    case "INSERTAREA":
                                        cityID = Convert.ToInt32(nvc["par_cityid"]);
                                        areaId = Convert.ToUInt32(nvc["par_id"]);
                                        lattitude = Convert.ToDouble(nvc["par_lattitude"]);
                                        longitude = Convert.ToDouble(nvc["par_longitude"]);
                                        if (bl.IsAreaExists(areaId))
                                        {
                                            bl.UpdateCommuteDistanceForAreaAdd(cityID, areaId, lattitude, longitude);
                                        }
                                        Model.BasicAck(e.DeliveryTag, false);
                                        break;
                                    case "UPDATEAREA":
                                        areaId = Convert.ToUInt32(nvc["par_id"]);
                                        lattitude = Convert.ToDouble(nvc["par_lattitude"]);
                                        longitude = Convert.ToDouble(nvc["par_longitude"]);
                                        if (bl.IsAreaGeoLocationChanged(areaId, lattitude, longitude))
                                            bl.UpdateCommuteDistanceForArea(areaId, lattitude, longitude);
                                        Model.BasicAck(e.DeliveryTag, false);
                                        break;
                                    case "UPDATEBIKEDEALER":
                                        dealerId = Convert.ToUInt32(nvc["par_dealerid"]);
                                        lattitude = Convert.ToDouble(nvc["par_lattitude"]);
                                        longitude = Convert.ToDouble(nvc["par_longitude"]);
                                        if (bl.IsDealerGeoLocationChanged(dealerId, lattitude, longitude))
                                            bl.UpdateCommuteDistanceForDealerUpdate(dealerId, lattitude, longitude);
                                        Model.BasicAck(e.DeliveryTag, false);
                                        break;
                                    default:
                                        Model.BasicReject(e.DeliveryTag, false);
                                        break;
                                }

                                watch.Stop();
                                var elapsedMs = String.Format("Processing Time taken in ms : {0}", watch.Elapsed.TotalSeconds);

                                Logs.WriteInfoLog(elapsedMs);
                            }
                            else
                            {
                                Model.BasicReject(e.DeliveryTag, false);
                                Logs.WriteInfoLog("Invalid operation type : " + Newtonsoft.Json.JsonConvert.SerializeObject(nvc));
                            }

                        }
                        catch (Exception ex)
                        {
                            Logs.WriteErrorLog(String.Format("Exception occured : {0}, Data : {1}", ex.Message, Newtonsoft.Json.JsonConvert.SerializeObject(nvc)));
                            Model.BasicReject(e.DeliveryTag, false);
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("RabbitMQExecution: " + ex.Message);
                SendMail.HandleException(ex, "Bikewale.CommuteDistanceConsumer/RabbitMQExecution:-Consumer Closed");
            }
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

        private void consumer_ConsumerCancelled(object sender, RabbitMQ.Client.Events.ConsumerEventArgs args)
        {
            Logs.WriteInfoLog("Consumer Cancelled event called");
            CreateConnection.CloseConnections();
            CreateConnection.RefreshNodes();
            CreateConnection.GetNextNode();
            RabbitMQExecution(CreateConnection.QueueName, CreateConnection.serverIp);
        }
    }
}
