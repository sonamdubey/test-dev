using System.Collections.Generic;

namespace Bikewale.Entities.Models
{
    /// <summary>
    /// Created By : Sajal Gupta on 07-09-2017
    /// Summary : Class have properties for the BreadCrumb
    /// </summary>
    public class BreadCrumbsList
    {
        public string PageName { get; set; }
        public IEnumerable<BreadCrumb> Breadcrumbs { get; set; }
    }
}
