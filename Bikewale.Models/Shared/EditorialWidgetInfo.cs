using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.Shared
{
    public class EditorialWidgetInfo
    {
        public virtual EditorialWidgetType WidgetType { get; protected set; }
        public string Title { get; set; }
        public string TabId { get; set; }
        public bool ShowViewAll { get; set; }
        public string ViewAllUrl { get; set; }
        public string ViewAllTitle { get; set; }

        public string ViewAllText { get; set; }
    }
}
