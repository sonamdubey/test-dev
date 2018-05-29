using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.Shared
{
    /// <summary>
    /// Modified by : Sanskar Gupta on 15 May 2018
    /// Description : Added property `ReturnUrlForAmpPages`
    /// Modified by : Sanskar Gupta on 15 May 2018
    /// Description : Added propery named `CityId`
    /// </summary>
    public class EditorialWidgetInfo
    {
        public virtual EditorialWidgetType WidgetType { get; protected set; }
        public string Title { get; set; }
        public string TabId { get; set; }
        public bool ShowViewAll { get; set; }
        public string ViewAllUrl { get; set; }
        public string ViewAllTitle { get; set; }
        public string ViewAllText { get; set; }
        public string ReturnUrlForAmpPages { get; set; }
        public uint CityId { get; set; }
    }
}
