using System;

namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 06 Jul 2017
    /// Description :   This is default Manufacturer Lead handler
    /// </summary>
    internal class DefaultManufacturerLeadHandler : ManufacturerLeadHandler
    {
        /// <summary>
        /// Type initializer
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="urlAPI"></param>
        /// <param name="isAPIEnabled"></param>
        public DefaultManufacturerLeadHandler(uint manufacturerId, string urlAPI, bool isAPIEnabled) : base(manufacturerId, urlAPI, isAPIEnabled)
        {
        }
        protected override string PushLeadToManufacturer(ManufacturerLeadEntityBase leadEntity)
        {
            throw new NotImplementedException();
        }
    }
}
