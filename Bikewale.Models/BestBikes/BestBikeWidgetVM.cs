
using Bikewale.Entities.GenericBikes;
namespace Bikewale.Models
{
    /// <author>
    /// Create by: Sangram Nandkhile on 24-Mar-2017
    /// Summary:  View Model for Best bike widget
    /// </author>
    public class BestBikeWidgetVM
    {
        public string Title { get; set; }
        public EnumBikeBodyStyles? CurrentPage { get; set; }
    }
}
