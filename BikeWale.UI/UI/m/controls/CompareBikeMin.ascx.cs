using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Compare;
using System;
using System.Collections.Generic;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Modified By : Sushil Kumar on 11th Nov 2016
    /// Description : Removed unused methods for reviews and topcompare image
    /// </summary>
    public class CompareBikeMin : System.Web.UI.UserControl
    {
        public uint TotalRecords { get; set; }
        public int FetchedRecordsCount { get; set; }
        protected IEnumerable<TopBikeCompareBase> compareList = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindControls();
        }

        /// <summary>
        /// Created By : Sushil Kumar on 11th Nov 2016
        /// Description : To bind comparisions bikes 
        /// </summary>
        private void BindControls()
        {
            BindBikeCompareControl objComp = new BindBikeCompareControl();
            objComp.TotalRecords = this.TotalRecords;
            objComp.FetchBikeCompares();
            this.FetchedRecordsCount = objComp.FetchedRecordCount;
            compareList = objComp.CompareList;

        }

        /// <summary>
        /// Created By : Sushil Kumar on 11th Nov 2016
        /// Description : To format comparision url 
        /// </summary>
        /// <param name="make1MaskName"></param>
        /// <param name="model1MaskName"></param>
        /// <param name="make2MaskName"></param>
        /// <param name="model2MaskName"></param>
        /// <param name="versionId1"></param>
        /// <param name="versionId2"></param>
        /// <returns></returns>
        protected string FormatComparisonUrl(string make1MaskName, string model1MaskName, string make2MaskName, string model2MaskName, uint versionId1, uint versionId2)
        {
            return String.Format("/m/comparebikes/{0}-{1}-vs-{2}-{3}/?bike1={4}&bike2={5}", make1MaskName, model1MaskName, make2MaskName, model2MaskName, versionId1, versionId2);
        }

        /// <summary>
        /// Created By : Sushil Kumar on 11th Nov 2016
        /// Description : To format anchor text
        /// </summary>
        /// <param name="bike1"></param>
        /// <param name="bike2"></param>
        /// <returns></returns>
        protected string FormatBikeCompareAnchorText(string bike1, string bike2)
        {
            return String.Format("{0} vs {1}", bike1, bike2);
        }
    }
}