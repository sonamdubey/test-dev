using Bikewale.Entities.ServiceCenters;
using System.Collections.Generic;

namespace Bikewale.Models.ServiceCenters
{
    /// <summary>
    /// Created By Sajal Gupta on 23-03-2017
    /// This entity is wrapper to ServiceCenterDetails to hold make masking name and city masking name to make url for service center details widget.
    /// </summary>
    public class ServiceCenterDetailsWidgetVM
    {
        public IEnumerable<ServiceCenterDetails> ServiceCentersList { get; set; }
        public string MakeMaskingName { get; set; }
        public string CityMaskingName { get; set; }
    }
}
