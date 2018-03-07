using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public class BWESDocumentBuilder
    {
        public static void PushToQueue(NameValueCollection nvc)
        {
            RabbitMqPublish publish = new RabbitMqPublish();
            publish.PublishToQueue(BWOprConfiguration.Instance.BWEsBuilderQueue, nvc);
        }
    }
}
