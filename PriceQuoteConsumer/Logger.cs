using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceQuoteConsumer
{
	public class Logger
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Logger));

		public static void LogError( string message, Exception ex)
		{
			log.Error(message, ex);
		}

		public static void LogInfo(string message)
		{
			log.Info(message);
		}
	}
}
