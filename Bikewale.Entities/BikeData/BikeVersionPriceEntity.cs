using System;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By: SnehaL Dange on 22nd Nov 2017
    /// Description: Enitity created for sub-footer model price list
    /// </summary>
    [Serializable]
    public class BikeVersionPriceEntity
    {
        public BikeMakeBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public int VersionPrice { get; set; }
    }
}
