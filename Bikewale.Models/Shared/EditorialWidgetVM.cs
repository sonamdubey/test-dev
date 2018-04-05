using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.Shared
{
    public class EditorialWidgetVM
    {
        public IDictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo> WidgetColumns { get; set; }
    }
}
