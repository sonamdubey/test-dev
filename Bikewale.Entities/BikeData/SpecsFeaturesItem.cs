using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 15 Mar 2018.
    /// Description : Entity to hold data related of a item for multiple versions OR models.
    /// </summary>
    public class SpecsFeaturesItem
    {
        /// <summary>
        /// Item name to be displayed to user, Ex. Displacement
        /// </summary>
        public string DisplayText { get; set; }
        /// <summary>
        /// Id of Item.
        /// </summary>
        public uint Id { get; set; }
        /// <summary>
        /// List of values of items for multiple versions OR models, Ex. [373.30, 346] for 2 versions.
        /// </summary>
        public IEnumerable<string>  ItemValues { get; set; }
        /// <summary>
        /// Unit of item to be used with item value, Ex. cc, kmpl
        /// </summary>
        public string UnitTypeText { get; set; }
    }
}
    