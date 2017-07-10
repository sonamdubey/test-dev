namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Jul 2017
    /// Description :   Interface for Manufacturer Lead Handler
    /// </summary>
    internal interface IManufacturerLeadHandler
    {
        bool Process(ManufacturerLeadEntityBase leadEntity);
    }
}
