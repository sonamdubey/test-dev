using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    public sealed class BWConfiguration
    {
        private static BWConfiguration _instance = null;
        private static readonly object padlock = new object();
        private readonly string _royalEnFiedToken = String.Empty;
        public string RoyalEnFiedToken   { get { return _royalEnFiedToken; } }
        private BWConfiguration()
        {
            _royalEnFiedToken = ConfigurationManager.AppSettings["RoyalEnfieldToken"];
        }

        public static BWConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Take lock while creating the object
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new BWConfiguration();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
