using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Deepak Israni on 20 March 2018
    /// Description: Entity class that holds data pertinent to an ad slot.
    /// </summary>
    public class AdSlotModel
    {
        /// <summary>
        /// Created By : Deepak Israni on 21 March 2018
        /// Description: Base constructor to initialize ViewSizes list.
        /// </summary>
        public AdSlotModel()
        {
            ViewSizes = new List<String>();
        }

        /// <summary>
        /// Created By : Deepak Israni on 21 March 2018
        /// Description: Constructor for when only one view size is applicable for an ad.
        /// </summary>
        public AdSlotModel(string viewSize) : this()
        {
            ViewSizes.Add(viewSize);
        }

        /// <summary>
        /// Created By : Deepak Israni on 21 March 2018
        /// Description: Constructor for when there are multiple view sizes for an ad slot.
        /// </summary>
        public AdSlotModel(IEnumerable<string> viewSizes) : this()
        {
            foreach (var viewSize in viewSizes)
            {
                ViewSizes.Add(viewSize);
            }
        }

        public string AdId { get; set; }
        public string AdPath { get; set; }
        public uint DivId { get; set; }
        public uint Width { get; set; }
        public bool LoadImmediate { get; set; }
        public string Position { get; set; }
        public string Size { get; set; }
        public string ViewSize { get { return GetViewSize(); } }
        public ICollection<string> ViewSizes{ get; private set; }


        /// <summary>
        /// Created By : Deepak Israni on 21 March 2018
        /// Description: Function to return the string with all the view sizes in proper format for building an ad slot.
        /// </summary>
        private string GetViewSize()
        {
            int count = ViewSizes.Count;
            if (count > 0)
            {
                if (count == 1)
                {
                    return ViewSizes.ElementAt(0);
                }
                else
                {
                    StringBuilder sizeString = new StringBuilder("[");
                    foreach (String vSize in ViewSizes)
                    {
                        sizeString.Append(vSize + ",");
                    }
                    sizeString.Length--;
                    sizeString.Append("]");

                    return sizeString.ToString();
                }
            }
            return "";
        }
        
    }
}
