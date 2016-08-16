
using Consumer;
using RabbitMQ.Client;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Bikewale.DealerAreaCommuteDistance
{
    public class CommuteDistanceConsumer
    {
        internal void RabbitMQExecution(string queueName, string hostName)
        {
            string operationType = string.Empty;
            int cityID = default(int);
            ushort areaId = default(ushort), dealerId = default(ushort);
            double lattitude = default(double), longitude = default(double);
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
                        try
                        {
                            RabbitMQ.Client.Events.BasicDeliverEventArgs e = (RabbitMQ.Client.Events.BasicDeliverEventArgs)consumer.Queue.Dequeue();
                            // Convert the Byte Array to NVC object
                            NameValueCollection nvc = ByteArrayToObject(e.Body);
                            if (nvc != null && nvc.HasKeys())
                            {
                                operationType = nvc["operationType"].ToUpper();
                                var watch = System.Diagnostics.Stopwatch.StartNew();
                                Logs.WriteInfoLog("Started at : " + DateTime.Now);
                                switch (operationType)
                                {
                                    case "INSERTAREA":
                                        cityID = Convert.ToInt32(nvc["par_cityid"]);
                                        areaId = Convert.ToUInt16(nvc["par_id"]);
                                        lattitude = Convert.ToDouble(nvc["par_lattitude"]);
                                        longitude = Convert.ToDouble(nvc["par_longitude"]);
                                        if (bl.IsAreaExists(areaId))
                                        {
                                            bl.UpdateCommuteDistanceForAreaAdd(cityID, areaId, lattitude, longitude);
                                        }
                                        Model.BasicAck(e.DeliveryTag, false);
                                        break;
                                    case "UPDATEAREA":
                                        areaId = Convert.ToUInt16(nvc["par_id"]);
                                        lattitude = Convert.ToDouble(nvc["par_lattitude"]);
                                        longitude = Convert.ToDouble(nvc["par_longitude"]);
                                        if (bl.IsAreaGeoLocationChanged(areaId, lattitude, longitude))
                                            bl.UpdateCommuteDistanceForArea(areaId, lattitude, longitude);
                                        Model.BasicAck(e.DeliveryTag, false);
                                        break;
                                    case "UPDATEBIKEDEALER":
                                        dealerId = Convert.ToUInt16(nvc["par_dealerid"]);
                                        lattitude = Convert.ToDouble(nvc["par_lattitude"]);
                                        longitude = Convert.ToDouble(nvc["par_longitude"]);
                                        if (bl.IsDealerGeoLocationChanged(dealerId, lattitude, longitude))
                                            bl.UpdateCommuteDistanceForDealerUpdate(dealerId, lattitude, longitude);
                                        Model.BasicAck(e.DeliveryTag, false);
                                        break;
                                    default:
                                        break;
                                }

                                watch.Stop();
                                var elapsedMs = String.Format("Processing Time taken in ms : {0}", watch.Elapsed.TotalSeconds);

                                Logs.WriteInfoLog(elapsedMs);
                            }

                        }
                        catch (Exception ex)
                        {
                            Logs.WriteErrorLog("RabbitMQExecution: Consumer was Closed: " + ex.Message);
                            SendMail.HandleException(ex, "Bikewale.CommuteDistanceConsumer/RabbitMQExecution:-Consumer Closed");
                            break;
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
