using RabbitMqPublishing;
using System.Collections.Specialized;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 21 Feb 2018
    /// Description : Class containing Function to Publish messages to queue. 
    /// </summary>
    public class BWESIndexUpdater
    {
        public static void PushToQueue(NameValueCollection nvc)
        {
            RabbitMqPublish publish = new RabbitMqPublish();
            publish.PublishToQueue(BWOprConfiguration.Instance.BWEsIndexUpdaterQueue, nvc);
        }
    }
}
