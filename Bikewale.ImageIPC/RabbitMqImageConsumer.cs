using Consumer;
using MySql.CoreDAL;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqPublishing;
using s3fileupload;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bikewale.ImageIPC
{
    internal class RabbitMqImageConsumer
    {
        internal void RabbitMQExecution(string queueName, string hostName)
        {
            CreateConnection.Connect(queueName, hostName);
            IConnection iconnection = (IConnection)CreateConnection.Connection;
            SendMail.APPLICATION = "ImagesConsumerIPC";
            queueName = ConfigurationManager.AppSettings["QueueName"].ToUpper();
            if (iconnection == null)
                return;
            CreateConnection.CreateChannel();
            IModel imodel = (IModel)CreateConnection.Model;
            QueueingBasicConsumer consumer = CreateConnection.CreateConsumer();
            consumer.ConsumerCancelled += consumer_ConsumerCancelled;
            while (true)
            {
                Logs.WriteInfoLog("RabbitMQ Execution: Waiting for job");
                try
                {
                    BasicDeliverEventArgs deliverEventArgs = consumer.Queue.Dequeue();
                    NameValueCollection nvc = this.ByteArrayToObject(deliverEventArgs.Body);
                    try
                    {
                        Logs.WriteInfoLog("RabbitMQExecution :Received job : " + nvc["id"] + " : " + nvc["category"] + " : " + nvc["location"] + " : IsWaterMarked : " + nvc["iswatermark"] + ":IsCrop:" + nvc["iscrop"] + ":CustomSizeWidth:" + nvc["customsizewidth"] + ":CustomSizeHeight:" + nvc["customsizeheight"] + ":SaveOriginal:" + nvc["saveoriginal"]);
                        MemoryStream ms = this.ImageMemoryStreamFromByte(nvc["location"]);
                        if (nvc["id"] == null || ms == null)
                        {
                            Logs.WriteInfoLog("Message Rejected id is null or location is wrong");
                            imodel.BasicReject(deliverEventArgs.DeliveryTag, false);
                        }
                        else if (nvc["iteration"] == "9")
                        {
                            imodel.BasicReject(deliverEventArgs.DeliveryTag, false);
                            Logs.WriteInfoLog("Message Rejected because iteration count is" + nvc["iteration"]);
                        }
                        else
                        {
                            Uri uri = new Uri(nvc["location"]);
                            string str1 = string.Empty;
                            string environment = string.Empty;
                            string targetImagePath;
                            if (ConfigurationManager.AppSettings["Environment"].ToLower().Equals("production"))
                            {
                                targetImagePath = uri.LocalPath.Substring(1).ToLower();
                            }
                            else
                            {
                                targetImagePath = ConfigurationManager.AppSettings["Environment"] + uri.LocalPath.ToLower();
                                environment = "/" + ConfigurationManager.AppSettings["Environment"];
                            }
                            Logs.WriteInfoLog(targetImagePath);
                            if (new UploadToS3().CallS3Service(ms, targetImagePath))
                            {
                                Logs.WriteInfoLog("Image with target path; " + targetImagePath + " and id " + nvc["id"] + " has been uploaded ");
                                Console.WriteLine("Image with target path; " + targetImagePath + " and id " + nvc["id"] + " has been uploaded ");
                                ms.Dispose();
                                string str2 = string.Empty;
                                string qsforMasterImage = this.CreateQSforMasterImage(nvc);
                                if (this.CreateMasterImage(targetImagePath + qsforMasterImage))
                                {
                                    Logs.WriteInfoLog("Master created for image with target path; " + targetImagePath + " and id: " + nvc["id"]);
                                    Console.WriteLine("Master created for image with target path; " + targetImagePath + " and id: " + nvc["id"]);
                                    List<string> list = new List<string>((IEnumerable<string>)ConfigurationManager.AppSettings["CDNHostUrl"].Split(';'));
                                    Random random = new Random();
                                    string hostUrl = list[random.Next(list.Count)];
                                    if (this.UploadImageProcessComplete(nvc["id"], hostUrl, string.Empty, nvc["category"], environment))
                                    {
                                        Logs.WriteInfoLog("Database for image with target path; " + targetImagePath + " and id: " + nvc["id"] + " has been updated ");
                                        Console.WriteLine("Database for image with target path; " + targetImagePath + " and id: " + nvc["id"] + " has been updated ");
                                        Logs.WriteInfoLog("RabbitMQExecution:Process is successfull. Final acknowledgement is being done.");
                                        imodel.BasicAck(deliverEventArgs.DeliveryTag, false);
                                    }
                                    else
                                    {
                                        Logs.WriteErrorLog("Fail to update database for image with target path: " + targetImagePath + " and id: " + nvc["id"]);
                                        this.DeadLetterPublish(nvc, queueName);
                                        imodel.BasicReject(deliverEventArgs.DeliveryTag, false);
                                        Logs.WriteInfoLog("Message sent to deadletter queue with iteration " + nvc["iteration"]);
                                    }
                                }
                                else
                                {
                                    Logs.WriteErrorLog("Fail to create Master for image with target path; " + targetImagePath + " and id: " + nvc["id"]);
                                    this.DeadLetterPublish(nvc, queueName);
                                    imodel.BasicReject(deliverEventArgs.DeliveryTag, false);
                                    Logs.WriteInfoLog("Message sent to deadletter queue with iteration " + nvc["iteration"]);
                                }
                            }
                            else
                            {
                                Logs.WriteErrorLog("Image with target path: " + targetImagePath + " and id: " + nvc["id"] + " has NOT been uploaded ");
                                this.DeadLetterPublish(nvc, queueName);
                                imodel.BasicReject(deliverEventArgs.DeliveryTag, false);
                                Logs.WriteInfoLog("Message sent to deadletter queue with iteration " + nvc["iteration"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logs.WriteErrorLog("RabbitMQExecution :: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace);
                        imodel.BasicReject(deliverEventArgs.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {
                    Logs.WriteErrorLog("RabbitMQExecution: Consumer was Closed: " + ex.Message);
                    break;
                }
            }
        }

        private string CreateQSforMasterImage(NameValueCollection nvc)
        {
            string str1 = string.Empty;
            string str2 = string.Empty;
            return (string.IsNullOrEmpty(nvc["aspectratio"]) ? "?ar=1.77" : "?ar=" + nvc["aspectratio"]) + (string.IsNullOrEmpty(nvc["ismaster"]) ? "&ism=1" : "&ism=" + nvc["ismaster"]);
        }

        private bool CreateMasterImage(string imagePath)
        {
            bool flag = false;
            try
            {
                WebResponse response = WebRequest.Create(ConfigurationManager.AppSettings["IPCServerIP"] + imagePath).GetResponse();
                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                    flag = true;
                response.Dispose();
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("RabbitMqImgConsumer/Create Master Image: Error occured while creating master image from IPC is:" + ex.Message + "-Source is:" + ex.Source + "-Exception Stacktrace:" + ex.StackTrace);
            }
            return flag;
        }

        public MemoryStream ImageMemoryStreamFromByte(string location)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                byte[] buffer = new WebClient().DownloadData(location);
                memoryStream.Write(buffer, 0, buffer.Length);
                Logs.WriteInfoLog("ImageConsumerIPC/ImageMemoryStreamFromByte: Received image memory stream from location");
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("ImageConsumerIPC/ImageMemoryStreamFromByte :Error has been occured while downloading image memory stream from web error is:" + ex.Message + "-Source is:" + ex.Source);
                memoryStream = (MemoryStream)null;
            }
            return memoryStream;
        }

        private NameValueCollection ByteArrayToObject(byte[] byteArray)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream(byteArray);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
                memoryStream.Position = 0L;
                return (NameValueCollection)binaryFormatter.Deserialize((Stream)memoryStream);
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("ByteArrayToObject :Error while converting byte to image object " + ex.Message + "-Stack trace: " + ex.StackTrace);
            }
            return (NameValueCollection)null;
        }

        private void consumer_ConsumerCancelled(object sender, ConsumerEventArgs args)
        {
            Logs.WriteInfoLog("Consumer Cancelled event called");
            CreateConnection.CloseConnections();
            CreateConnection.RefreshNodes();
            CreateConnection.GetNextNode();
            this.RabbitMQExecution((string)CreateConnection.QueueName, (string)CreateConnection.serverIp);
        }

        private void DeadLetterPublish(NameValueCollection nvc, string queueName)
        {
            RabbitMqPublish rabbitMqPublish = new RabbitMqPublish();
            rabbitMqPublish.UseDeadLetterExchange = true;
            rabbitMqPublish.MessageTTL = (int.Parse(ConfigurationManager.AppSettings["RabbitMsgTTL"].ToString()));
            int num = nvc["iteration"] == null ? 1 : (int)short.Parse(nvc["iteration"]) + 1;
            nvc.Set("iteration", num.ToString());
            rabbitMqPublish.PublishToQueue(queueName, nvc);
        }

        internal bool UploadImageProcessComplete(string photoId, string hostUrl, string imageServers, string category, string environment)
        {
            bool flag = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(ConfigurationManager.AppSettings["SPName"].ToLower()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_photoid", DbType.Int64, Convert.ToInt32(photoId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_hosturl", DbType.String, 100, hostUrl));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_serverlist", DbType.String, 100, imageServers));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_category", DbType.String, 100, category));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_environment", DbType.String, 20, !string.IsNullOrEmpty(environment) ? environment : ""));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_maxservers", DbType.Int32, DBNull.Value));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    flag = true;
                    Logs.WriteInfoLog("UploadImageProcessCompleteCmn:Data Successfully Updated in final process");
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("UploadImageProcessCompleteCmn:ERROR OCCURED WHILE UPDATING DATA FROM DATABASE " + ex.Message, ex);
            }
            return flag;
        }
    }

    public class UploadToS3
    {
        internal bool CallS3Service(MemoryStream ms, string targetImagePath)
        {
            Logs.WriteInfoLog("CallS3Service:has been called with targetpath: " + targetImagePath);
            ConfigurationManager.AppSettings["s3Path"].ToString();
            UploadFile uploadFile = new UploadFile();
            bool flag;
            try
            {
                uploadFile.Bucket = ConfigurationManager.AppSettings["OutputBucket"].ToString();
                uploadFile.CreateBucket();
                uploadFile.UploadFileFromStream(targetImagePath, (Stream)ms);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                Logs.WriteErrorLog("ERROR in CallS3Service : " + ex.Message + " : " + ex.Source + " : " + ex.StackTrace);
            }
            return flag;
        }
    }
}