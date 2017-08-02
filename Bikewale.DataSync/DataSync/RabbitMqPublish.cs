using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BWDataSync
{
    public class RabbitMqPublish
    {
        #region Variable declaration
        protected IModel Model;
        protected IConnection Connection;
        protected string QueueName;
        protected string ExchangeName;
        private List<String> nodes = new List<string>();
        private Random arbit = new Random();

        private bool _useDeadLetterExchange = false;
        public bool UseDeadLetterExchange
        {
            get { return _useDeadLetterExchange; }
            set { _useDeadLetterExchange = value; }
        }

        private int _messageTTL;
        public int MessageTTL
        {
            get { return _messageTTL; }
            set { _messageTTL = value; }
        }
        private bool _connectionShutdown;
        public bool ConnectionShutDown
        {
            get { return _connectionShutdown; }
            set { _connectionShutdown = value; }
        }
        private bool _connectionBlocked;
        public bool ConnectionBlocked
        {
            get { return _connectionBlocked; }
            set { _connectionBlocked = value; }
        }
        /// <summary>
        ///  Count to set while putting in deadLetter Queue 
        /// </summary>
        private int _rePublishCount = 0;
        public int RepublishCount
        {
            get { return _rePublishCount; }
            set { _rePublishCount = value; }
        }

        #endregion
        /// <summary>
        /// This Function Create a new queue and passes name value parameters
        /// </summary>
        /// <returns>bool</returns>
        public bool PublishToQueue(string appName, NameValueCollection nvc)
        {
            bool isComplete = false;
            try
            {
                //read servers list and parse them
                nodes.AddRange(ConfigurationManager.ConnectionStrings["RabbitMQServerList"].ConnectionString.Split(';'));
                //add hosts randomly
                string hostAdd = nodes[arbit.Next(nodes.Count)];
                appName = appName.ToUpper();
                //calling the producer which declare the queue , exchange and bind them together with a key 
                if (_useDeadLetterExchange)
                {
                    SetProducerWithDeadLetterExchange(hostAdd, "RabbitMq-" + appName + "-Queue", "RabbitMQ-" + appName + "-Exchange");
                }
                else
                {
                    SetProducer(hostAdd, "RabbitMq-" + appName + "-Queue", "RabbitMQ-" + appName + "-Exchange");
                }
                //set the broker on confirmation mode
                Model.ConfirmSelect();
                //serialize the imgAttr object
                BinaryFormatter bf = new BinaryFormatter();
                bf.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, nvc);
                //send message to queue
                if (_useDeadLetterExchange)
                    SendMessage(ms.ToArray(), "RabbitMQ-" + appName + "-ExchangeDL", "RabbitMq-" + appName + "-QueueDLKey", nvc["id"], nvc["category"]);
                else
                    SendMessage(ms.ToArray(), "RabbitMQ-" + appName + "-Exchange", "RabbitMq-" + appName + "-QueueKey", nvc["id"], nvc["category"]);
                isComplete = true;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "RabbitMqPublishingActivity");
                objErr.SendMail();
            }

            finally
            {
                if (Model != null)
                {
                    if (Model.IsOpen)
                        Model.Close();
                }
                if (Connection != null)
                {
                    if (Connection.IsOpen)
                        Connection.Close();
                }
            }
            return isComplete;
        }
        /// <summary>
        /// For Publishing generic objects 
        /// </summary>
        /// <returns></returns>
        public bool PublishAnyObjectToQueue<T>(string queueName, T t)
        {
            bool isComplete = false;
            try
            {
                //read servers list and parse them
                nodes.AddRange(ConfigurationManager.ConnectionStrings["RabbitMQServerList"].ConnectionString.Split(';'));
                //add hosts randomly
                string hostAdd = nodes[arbit.Next(nodes.Count)];

                //calling the producer which declare the queue , exchange and bind them together with a key 
                if (_useDeadLetterExchange)
                {
                    SetProducerWithDeadLetterExchange(hostAdd, queueName, queueName + "-Exchange");
                }
                else
                {
                    SetProducer(hostAdd, queueName, queueName + "-Exchange");
                }
                //set the broker on confirmation mode
                Model.ConfirmSelect();
                //serialize the imgAttr object
                BinaryFormatter bf = new BinaryFormatter();
                bf.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, t);
                //send message to queue
                if (_useDeadLetterExchange)
                    SendMessage(ms.ToArray(), queueName + "-ExchangeDL", queueName + "DLKey", "", "");
                else
                    SendMessage(ms.ToArray(), queueName + "-Exchange", queueName + "Key", "", "");
                isComplete = true;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "RabbitMqPublishingActivity");
                objErr.SendMail();
            }

            finally
            {
                if (Model != null)
                {
                    if (Model.IsOpen)
                        Model.Close();
                }
                if (Connection != null)
                {
                    if (Connection.IsOpen)
                        Connection.Close();
                }
            }
            return isComplete;
        }

        /// <summary>
        /// producer for publishing images to image queue
        /// </summary>
        /// <param name="hostName">server ip address</param>
        /// <param name="queueName">queue name</param>
        /// <param name="exchangeName">exchange name</param>
        private void SetProducer(string hostName, string queueName, string exchangeName)
        {

            ExchangeName = exchangeName;
            QueueName = queueName;
            ConnectionFactory connectionFactory = new ConnectionFactory();

            connectionFactory.HostName = hostName;
            try
            {
                Connection = connectionFactory.CreateConnection();
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BrokerUnreachableException")
                {
                    nodes.Remove(hostName);
                    if (nodes.Count > 0)
                    {
                        hostName = nodes[arbit.Next(nodes.Count)];
                        connectionFactory.HostName = hostName;
                        Connection = connectionFactory.CreateConnection();
                    }
                    else
                    {
                        return;
                    }
                }
            }

            Connection.ConnectionBlocked += Connection_ConnectionBlocked;
            //create a model for a channel
            Model = Connection.CreateModel();
            //create a direct exchange
            Model.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            //create a mirrored queue
            Dictionary<String, Object> args = new Dictionary<String, Object>();
            args.Add("x-ha-policy", "all");
            Model.QueueDeclare(QueueName, true, false, false, args);
            //bind the queue with exchange with a key 
            Model.QueueBind(queueName, ExchangeName, queueName + "Key", null);
        }
        /// <summary>
        /// Set Producer with dead letter Exchange
        /// </summary>
        /// <param name="hostName">server ip address</param>
        /// <param name="queueName">queue name</param>
        /// <param name="exchangeName">exchange name</param>
        private void SetProducerWithDeadLetterExchange(string hostName, string queueName, string exchangeName)
        {
            ExchangeName = exchangeName;
            QueueName = queueName;
            ConnectionFactory connectionFactory = new ConnectionFactory();

            connectionFactory.HostName = hostName;
            try
            {
                Connection = connectionFactory.CreateConnection();
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BrokerUnreachableException")
                {
                    nodes.Remove(hostName);
                    if (nodes.Count > 0)
                    {
                        hostName = nodes[arbit.Next(nodes.Count)];
                        connectionFactory.HostName = hostName;
                        Connection = connectionFactory.CreateConnection();
                    }
                    else
                    {
                        return;
                    }
                }
            }

            Connection.ConnectionBlocked += Connection_ConnectionBlocked;
            //create a model for a channel
            Model = Connection.CreateModel();
            //create a direct exchange for message to be published initially
            Model.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            //create a dead letter exchange for publishing message to final queue
            Model.ExchangeDeclare(ExchangeName + "DL", ExchangeType.Direct);
            //create a mirrored queue 
            Dictionary<String, Object> args = new Dictionary<String, Object>();
            args.Add("x-ha-policy", "all");
            args.Add("x-message-ttl", _messageTTL);
            args.Add("x-dead-letter-exchange", ExchangeName);
            args.Add("x-dead-letter-routing-key", queueName + "Key");

            Model.QueueDeclare(QueueName + "DL", true, false, false, args);
            Model.QueueBind(QueueName + "DL", ExchangeName + "DL", queueName + "DLKey", null);

            Model.QueueDeclare(QueueName, true, false, false, null);
            Model.QueueBind(queueName, ExchangeName, queueName + "Key", null);
        }
        /// <summary>
        /// method used to publish images to rabbitmq
        /// </summary>
        /// <param name="message">message to be published </param>
        /// <param name="exchangeName">exchange name</param>
        /// <param name="keyName">key name to bind queue and exchange</param>
        /// <param name="imageId">image id</param>
        /// <param name="category">categories from where photo belonged</param>
        private void SendMessage(byte[] message, string exchangeName, string keyName, string imageId, string category)
        {
            ExchangeName = exchangeName;
            IBasicProperties basicProperties = Model.CreateBasicProperties();
            //deliverymode = 2 means message is persistant
            basicProperties.DeliveryMode = 2;
            basicProperties.MessageId = category + "-" + imageId;
            //basicProperties.Headers.Add("republishcount", RepublishCount);

            basicProperties.Headers = new Dictionary<string, object>();
            basicProperties.Headers.Add("republishcount", RepublishCount);

            //publishing the message in a queue
            //true means message sending is mendatory otherwise it wont return message on un routed condition
            Model.BasicPublish(ExchangeName, keyName, true, false, basicProperties, message);

        }

        private void Connection_ConnectionBlocked(IConnection sender, ConnectionBlockedEventArgs args)
        {
            //Console.Write(args.Reason);
            ConnectionBlocked = true;
        }
    }
}
