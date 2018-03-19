using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 15 Mar 2018.
    /// Description : Entity to hold list of list items for a specs & features category.
    /// </summary>
    public class SpecsFeaturesCategory
    {
        /// <summary>
        /// Category name to be displayed to user, Ex. Brakes, Wheels and Suspension
        /// </summary>
        public string DisplayText { get; set; }
        /// <summary>
        /// Icon html to be bound.
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// List of items under this category.
        /// </summary>
        public IEnumerable<SpecsFeaturesItem> SpecsItemList { get; set; }
    }
}
