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
    ///              The dictionary stores the array of strings of all the view slot sizes an advertisement would need for a certain size.
    /// </summary>
    public static class ViewSlotSize
    {
        public const string _200x211 = "[200, 211]";
        public const string _200x216 = "[200, 216]";
        public const string _200x253 = "[200, 253]";
        public const string _292x359 = "[292, 359]";
        public const string _292x360 = "[292, 360]";
        public const string _292x399 = "[292, 399]";
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
        public const string _728x90 = "[728, 90]";
        public const string _950x90 = "[950, 90]";
        public const string _960x60 = "[960, 60]";
        public const string _960x66 = "[960, 66]";
        public const string _960x90 = "[960, 90]";
        public const string _970x60 = "[970, 60]";
        public const string _970x66 = "[970, 66]";
        public const string _970x90 = "[970, 90]";
        public const string _970x150 = "[970, 150]";
        public const string _970x200 = "[970, 200]";
        public const string _976x100 = "[976, 100]";
        public const string _976x150 = "[976, 150]";
        public const string _976x200 = "[976, 200]";
        public const string _976x204 = "[976, 204]";
        public const string _976x250 = "[976, 250]";
        public const string _976x300 = "[976, 300]";
        public const string _976x350 = "[976, 350]";
        public const string _976x400 = "[976, 400]";
        public const string _976x450 = "[976, 450]";
        public const string _976x460 = "[976, 460]";



        public static readonly IDictionary<string, string[]> ViewSlotSizes = new Dictionary<string, string[]>()
        {
            {AdSlotSize._200x211, new String[] { ViewSlotSize._200x211 }},

            {AdSlotSize._200x216, new String[] { ViewSlotSize._200x216 }},

            {AdSlotSize._200x253, new String[] { ViewSlotSize._200x253 }},

            {AdSlotSize._292x359, new String[] { ViewSlotSize._292x359 }},

            {AdSlotSize._292x360,new String[] { ViewSlotSize._292x360 }},

            {AdSlotSize._292x399, new String[] { ViewSlotSize._292x399 }},

            {AdSlotSize._300x100,  new String[] { ViewSlotSize._300x100 }},

            {AdSlotSize._300x250, new String[] { ViewSlotSize._300x250 }},

            {AdSlotSize._320x50, new String[] { ViewSlotSize._320x50 }},

            {AdSlotSize._320x100, new String[] { ViewSlotSize._320x100, ViewSlotSize._320x50 }},

            {AdSlotSize._320x150, new String[] { ViewSlotSize._320x150, ViewSlotSize._320x50, ViewSlotSize._320x100, ViewSlotSize._320x425 }},

            {AdSlotSize._320x400,  new String[] { ViewSlotSize._320x300, ViewSlotSize._320x350, ViewSlotSize._320x400, ViewSlotSize._320x425, ViewSlotSize._320x450 }},

            {AdSlotSize._970x90 + "_A", new String[] { ViewSlotSize._970x200,ViewSlotSize._970x150,ViewSlotSize._960x60, ViewSlotSize._970x66, ViewSlotSize._960x90, ViewSlotSize._970x60, ViewSlotSize._728x90, ViewSlotSize._970x90, ViewSlotSize._960x66 }},

            {AdSlotSize._970x90 + "_B", new String[] { ViewSlotSize._950x90, ViewSlotSize._728x90, ViewSlotSize._960x60, ViewSlotSize._970x60, ViewSlotSize._970x90, ViewSlotSize._960x90 }},

            {AdSlotSize._970x90 + "_C", new String[] { ViewSlotSize._970x66, ViewSlotSize._970x60, ViewSlotSize._960x90, ViewSlotSize._950x90, ViewSlotSize._960x66, ViewSlotSize._728x90, ViewSlotSize._960x60, ViewSlotSize._970x90 }},

            {AdSlotSize._976x204, new String[] { ViewSlotSize._976x200, ViewSlotSize._976x250, ViewSlotSize._976x204 }},

            {AdSlotSize._976x400 + "_A", new String[] { ViewSlotSize._976x450, ViewSlotSize._976x300, ViewSlotSize._976x460, ViewSlotSize._976x250, ViewSlotSize._976x400 }},

            {AdSlotSize._976x400 + "_B", new String[] { ViewSlotSize._976x150, ViewSlotSize._976x100, ViewSlotSize._976x250, ViewSlotSize._976x300, ViewSlotSize._976x350, ViewSlotSize._976x400, ViewSlotSize._970x90, ViewSlotSize._976x200 }},
        };
    }




}



