﻿
using Consumer;
using System;
using System.Configuration;
namespace Bikewale.DealerAreaCommuteDistance
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            try
            {
                Logs.WriteInfoLog("Started at : " + DateTime.Now);
                CommuteDistanceConsumer consumer = new CommuteDistanceConsumer();
                Random Random = new Random();
                string hostName = CreateConnection.nodes[Random.Next(CreateConnection.nodes.Count)];
                consumer.RabbitMQExecution("RabbitMq-" + ConfigurationManager.AppSettings["QueueName"].ToUpper() + "-Queue", hostName);
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception " + ex.Message);
            }
            finally
            {
                Logs.WriteInfoLog("End at : " + DateTime.Now);
            }
        }
    }
}
