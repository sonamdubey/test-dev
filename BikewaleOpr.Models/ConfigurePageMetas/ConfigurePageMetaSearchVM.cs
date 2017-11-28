using BikewaleOpr.Entity.ConfigurePageMetas;
using System.Collections.Generic;

namespace BikewaleOpr.Models.ConfigurePageMetas
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 14-Aug-2017
    /// Description : Model for page meta search page.
    /// </summary>
    public class ConfigurePageMetaSearchVM
    {
        public IEnumerable<PageMetaEntity> PageMetaList { get; set; }

        public ushort PageMetaStatus { get; set; }

    }
}
