
using Consumer;
using System;
using System.Collections.Generic;
using System.Configuration;
namespace Bikewale.ImageIPC
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitMqImageConsumer rabbitMqImageConsumer = new RabbitMqImageConsumer();
            Random random = new Random();
            string hostName = ((List<string>)CreateConnection.nodes)[random.Next(((List<string>)CreateConnection.nodes).Count)];
            rabbitMqImageConsumer.RabbitMQExecution("RabbitMq-" + ConfigurationManager.AppSettings["QueueName"].ToUpper() + "-Queue", hostName);
        }
    }
}
