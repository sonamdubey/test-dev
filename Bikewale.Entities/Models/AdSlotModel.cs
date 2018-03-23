using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public AdSlotModel(string viewSize)
            : this()
        {
            ViewSizes.Add(viewSize);
        }

        /// <summary>
        /// Created By : Deepak Israni on 21 March 2018
        /// Description: Constructor for when there are multiple view sizes for an ad slot.
        /// </summary>
        public AdSlotModel(IEnumerable<string> viewSizes)
            : this()
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
        public ICollection<string> ViewSizes { get; private set; }


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
            return string.Empty;
        }

    }

    /// <summary>
    /// Created By : Deepak Israni on 21 March 2018
    /// Description: Class to store all the ad slot sizes.
    /// </summary>
    public static class AdSlotSize
    {
        public const string _200x211 = "200x211";
        public const string _200x216 = "200x216";
        public const string _200x253 = "200x253";
        public const string _292x399 = "292x399";
        public const string _292x359 = "292x359";
        public const string _292x360 = "292x360";
        public const string _300x100 = "300x100";
        public const string _300x250 = "300x250";
        public const string _320x50 = "320x50";
        public const string _320x100 = "320x100";
        public const string _320x150 = "320x150";
        public const string _320x400 = "320x400";
        public const string _970x90 = "970x90";
        public const string _976x204 = "976x204";
        public const string _976x400 = "976x400";
    }

    /// <summary>
    /// Created By : Deepak Israni on 22 March 2018
    /// Description: Class to store all the ad view slot sizes.
    /// </summary>
    public static class ViewSlotSize
    {
        public const string _200x211 = "[200, 211]";
        public const string _200x216 = "[200, 216]";
        public const string _200x253 = "[200, 253]";
        public const string _300x100 = "[300, 100]";
        public const string _300x250 = "[300, 250]";
        public const string _320x50 = "[320, 50]";
        public const string _320x100 = "[320, 100]";
        public const string _320x150 = "[320, 150]";
        public const string _320x300 = "[320, 300]";
        public const string _320x350 = "[320, 350]";
        public const string _320x400 = "[320, 400]";
        public const string _320x425 = "[320, 425]";
        public const string _320x450 = "[320, 450]";
    }

}
