using System;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By: Snehal Dange on 22nd Nov 2017
    /// Description: Entity created for sub-footer categories and price list on make page
    /// </summary>
    [Serializable]
    public class MakeSubFooterEntity
    {
        public IEnumerable<MakeFooterCategory> FooterDescription { get; set; }
        public IEnumerable<BikeVersionPriceEntity> ModelPriceList { get; set; }
    }
}
