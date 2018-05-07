using System.Collections;

namespace Bikewale.RabbitMq.LeadProcessingConsumer.Interface
{
    /// <summary>
    /// Honda model cache method reference
    /// </summary>
    public interface IHondaModelCache
    {
        Hashtable GetHondaModelMapping();
    }
}
