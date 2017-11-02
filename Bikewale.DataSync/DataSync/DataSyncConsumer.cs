using Consumer;
using Dapper;
using MySql.Data.MySqlClient;
using RabbitMQ.Client;
using RabbitMqPublishing;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DataSync
{
    /// <summary>
    /// Created By : Sushil Kumar on 26th July 2017
    /// Description : Consumer to sync bikewale data to carwale and vice versa
    /// </summary>
    internal class DataSyncConsumer
    {
        private string _queueName, _hostName;
        private readonly string _applicationName, _retryCount, _RabbitMsgTTL, _BWConnectionString, _CWConnectionString;
        private IConnection _connetionRabbitMq;
        private IModel _model;
        private QueueingBasicConsumer consumer;

        /// <summary>
        /// Created By : Sushil Kumar on 26th July 2017
        /// Description : To set values for queueName and connection string properties
        /// </summary>
        public DataSyncConsumer()
        {
            _queueName = String.Format("RabbitMq-{0}-Queue", ConfigurationManager.AppSettings["QueueName"].ToUpper());
            _hostName = CreateConnection.nodes[(new Random()).Next(CreateConnection.nodes.Count)];
            _BWConnectionString = ConfigurationManager.AppSettings["BWconnectionString"];
            _CWConnectionString = ConfigurationManager.AppSettings["CWMYSQLconnectionString"];
            SendMail.APPLICATION = _applicationName = Convert.ToString(ConfigurationManager.AppSettings["ConsumerName"]);
            _retryCount = ConfigurationManager.AppSettings["RetryCount"];
            _RabbitMsgTTL = ConfigurationManager.AppSettings["RabbitMsgTTL"];
            InitConsumer();
        }

        /// <summary>
        /// Created By : Sushil Kumar on 26th July 2017
        /// Description : To initialize consumer
        /// </summary>
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

        /// <summary>
        /// Created By : Sushil Kumar on 26th July 2017
        /// Description : To process data sync queries
        /// </summary>
        internal void ProcessQueries()
        {

            while (true)
            {
                Logs.WriteInfoLog("RabbitMQ Execution: Waiting for job");
                try
                {
                    RabbitMQ.Client.Events.BasicDeliverEventArgs deliverEventArgs = (RabbitMQ.Client.Events.BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    IBasicProperties props = deliverEventArgs.BasicProperties;

                    NameValueCollection nvc = this.ByteArrayToObject(deliverEventArgs.Body);
                    string dBName = nvc["DBNAME"], spName = nvc["SPNAME"], iteration = nvc["iteration"];

                    if (!string.IsNullOrEmpty(dBName) && !string.IsNullOrEmpty(spName))
                    {
                        if (iteration == _retryCount)
                        {
                            _model.BasicReject(deliverEventArgs.DeliveryTag, false);
                            Logs.WriteInfoLog("Message Rejected because iteration count is" + iteration);
                        }
                        else if (nvc.Count >= Convert.ToInt32(_retryCount))
                        {
                            StringBuilder stringBuilder = new StringBuilder("Received job:");
                            DynamicParameters dynamicParameters1 = new DynamicParameters();

                            foreach (string index in nvc.Keys)
                            {
                                stringBuilder.Append(index + " -> " + nvc[index] + "; ");
                                dynamicParameters1.Add(index, (object)nvc[index], new DbType?(), new ParameterDirection?(), new int?(), new byte?(), new byte?());
                            }

                            Logs.WriteInfoLog(stringBuilder.ToString());

                            using (IDbConnection dbConnection1 = (IDbConnection)this.GetDBConnection(dBName))
                            {
                                try
                                {
                                    SqlMapper.Execute(dbConnection1, spName, (object)dynamicParameters1, (IDbTransaction)null, new int?(), new CommandType?(CommandType.StoredProcedure));

                                    _model.BasicAck(deliverEventArgs.DeliveryTag, false);
                                    Logs.WriteInfoLog(string.Format("RabbitMQExecution : The job with Database -> {0} :  Stored Procedure {1} executed successfully", dBName, spName));
                                }
                                catch (Exception ex)
                                {
                                    Logs.WriteInfoLog(string.Format("RabbitMQExecution : The job with Database -> {0} ;  Stored Procedure -> {1} failed to execute", dBName, spName));
                                    this.DeadLetterPublish(nvc, _queueName);

                                    _model.BasicReject(deliverEventArgs.DeliveryTag, false);

                                    Logs.WriteErrorLog(string.Format("RabbitMQExecution: Database Error: {0} - SP : {1} - DB - {2}", ex.Message, spName, dBName));
                                    SendMail.HandleException(ex, "BWDataSyncConsumer/Database Error: " + ex.Message);
                                }
                            }
                        }
                        else
                        {
                            _model.BasicReject(deliverEventArgs.DeliveryTag, false);
                        }
                    }
                    else
                    {
                        _model.BasicReject(deliverEventArgs.DeliveryTag, false);
                        Logs.WriteErrorLog("Incorrect input paramters");
                        SendMail.HandleException(new Exception("Incorrect Input paramters provided"), String.Format("Message rejected fro paramters - DbName : {0}, Storedprocedure : {1}", dBName, spName));
                    }
                }
                catch (Exception ex)
                {
                    Logs.WriteErrorLog("RabbitMQExecution: Consumer was Closed: " + ex.Message);
                    SendMail.HandleException(ex, "BWDataSyncConsumer/RabbitMQExecution:- Consumer Closed");
                }
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 26th July 2017
        /// Description : To get db connection for bikewale and carwale
        /// </summary>
        /// <param name="Database"></param>
        /// <returns></returns>
        private DbConnection GetDBConnection(string Database)
        {
            switch (Database)
            {
                case "BW":
                    if (!string.IsNullOrEmpty(_BWConnectionString))
                        return (DbConnection)new MySqlConnection(_BWConnectionString);
                    break;
                case "CW":
                case "CWMYSQL":
                    if (!string.IsNullOrEmpty(_CWConnectionString))
                        return (DbConnection)new MySqlConnection(_CWConnectionString);
                    break;
            }

            return (DbConnection)new SqlConnection();
        }

        private NameValueCollection ByteArrayToObject(byte[] byteArray)
        {
            MemoryStream memoryStream = new MemoryStream(byteArray);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
            memoryStream.Position = 0L;
            return (NameValueCollection)binaryFormatter.Deserialize((Stream)memoryStream);
        }

        private void DeadLetterPublish(NameValueCollection nvc, string queueName)
        {
            RabbitMqPublish rabbitMqPublish = new RabbitMqPublish();
            rabbitMqPublish.UseDeadLetterExchange = true;
            rabbitMqPublish.MessageTTL = int.Parse(_RabbitMsgTTL);
            int num = nvc["iteration"] == null ? 1 : (int)short.Parse(nvc["iteration"]) + 1;
            nvc.Set("iteration", num.ToString());
            rabbitMqPublish.PublishToQueue(queueName, nvc);
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
            ProcessQueries();
        }
    }
}