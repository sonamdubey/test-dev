using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Compare;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Controls
{
    /// <summary>
    /// Modified By : Sushil Kumar on 27th Oct 2016
    /// Description : Removed unused methods for reviews and topcompare image
    /// </summary>
    public class ComparisonMin : System.Web.UI.UserControl
    {
        private static readonly string _Bike1VsBike2 = "{0} vs {1}";
        private static readonly string _ComparisonURL = "/comparebikes/{0}-{1}-vs-{2}-{3}/?bike1={4}&bike2={5}";
        protected IEnumerable<TopBikeCompareBase> compareList = null;

        private uint _totalRecords = 4;

        public uint TotalRecords
        {
            get { return _totalRecords; }
            set { _totalRecords = value; }
        }

        private TopBikeCompareBase m_TopRecord;
        public TopBikeCompareBase TopRecord
        {
            get
            {
                return m_TopRecord;
            }
            set
            {
                m_TopRecord = value;
            }
        }

        private int m_FetchedRecordsCount;
        public int FetchedRecordsCount
        {
            get
            {
                return m_FetchedRecordsCount;
            }
            set
            {
                m_FetchedRecordsCount = value;
            }
        }

        private bool _showCompButton = true;
        public bool ShowCompButton
        {
            get { return _showCompButton; }
            set { _showCompButton = value; }
        }


        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindControls();
        }

        private void BindControls()
        {
            BindBikeCompareControl objComp = new BindBikeCompareControl();
            objComp.TotalRecords = _totalRecords;
            objComp.FetchBikeCompares();
            this.m_TopRecord = objComp.FetchTopRecord();
            this.m_FetchedRecordsCount = objComp.FetchedRecordCount;
            if (m_TopRecord != null) compareList = objComp.CompareList.Skip(1);
        }

        protected string FormatComparisonUrl(string make1MaskName, string model1MaskName, string make2MaskName, string model2MaskName, string versionId1, string versionId2)
        {
            string url = String.Empty;
            url = String.Format(_ComparisonURL, make1MaskName, model1MaskName, make2MaskName, model2MaskName, versionId1, versionId2);
            return url;
        }


        protected string FormatBikeCompareAnchorText(string bike1, string bike2)
        {
            string anchorText = String.Empty;
            anchorText = String.Format(_Bike1VsBike2, bike1, bike2);
            return anchorText;
        }
    }
}