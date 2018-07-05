using System;
using Consumer;
using RabbitMqPublishing;
using System.Configuration;
using System.Collections.Specialized;

namespace PriceQuoteConsumer
{
	class Program
	{
		/// <summary>
		/// Created By  : Rajan Chauhan on 19 June 2018
		/// Description : Method running Price Quote Consumer 
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			log4net.Config.XmlConfigurator.Configure();
			try
			{
				Logs.WriteInfoLog("Started at : " + DateTime.Now);
				PQConsumer consumer = new PQConsumer();
				consumer.ProcessMessages();
			}
			catch (Exception ex)
			{
				Logs.WriteErrorLog("Exception : " + ex.Message);
			}
			finally
			{
				Logs.WriteInfoLog("End at : " + DateTime.Now);
			}
		}
	}
}
