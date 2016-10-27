﻿using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Compare;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    public class CompareBikesMin : System.Web.UI.UserControl
    {
        protected Repeater rptCompareBike;
        private static readonly string _WriteReviewLink = "/content/userreviews/writereviews.aspx?bikem={0}";
        private static readonly string _ExistingReviewLink = "/{0}-bikes/{1}/user-reviews/";
        private static readonly string _ReviewCountString = "{0} reviews";
        private static readonly string _WriteReviewString = "Write reviews";
        private static readonly string _Bike1VsBike2 = "{0} vs {1}";
        private static readonly string _ComparisonURL = "/comparebikes/{0}-{1}-vs-{2}-{3}";

        private uint m_TotalRecords;
        public uint TotalRecords
        {
            get
            {
                return m_TotalRecords;
            }
            set
            {
                m_TotalRecords = value;
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

        private string m_Bike1ReviewLink;
        public string Bike1ReviewLink
        {
            get
            {
                return m_Bike1ReviewLink;
            }
            set
            {
                m_Bike1ReviewLink = value;
            }
        }

        private string m_Bike2ReviewLink;
        public string Bike2ReviewLink
        {
            get
            {
                return m_Bike2ReviewLink;
            }
            set
            {
                m_Bike2ReviewLink = value;
            }
        }

        private string m_Bike1ReviewText;
        public string Bike1ReviewText
        {
            get
            {
                return m_Bike1ReviewText;
            }
            set
            {
                m_Bike1ReviewText = value;
            }
        }

        private string m_Bike2ReviewText;
        public string Bike2ReviewText
        {
            get
            {
                return m_Bike2ReviewText;
            }
            set
            {
                m_Bike2ReviewText = value;
            }
        }

        private string m_TopCompareImage;
        public string TopCompareImage
        {
            get
            {
                return m_TopCompareImage;
            }
            set
            {
                m_TopCompareImage = value;
            }
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
            objComp.TotalRecords = this.m_TotalRecords;
            objComp.FetchBikeCompares();
            objComp.BindBikeCompare(rptCompareBike, 1);
            this.m_TopRecord = objComp.FetchTopRecord();
            this.m_FetchedRecordsCount = objComp.FetchedRecordCount;
        }


        protected string FormatComparisonUrl(string make1MaskName, string model1MaskName, string make2MaskName, string model2MaskName)
        {
            string url = String.Empty;
            url = String.Format(_ComparisonURL, make1MaskName, model1MaskName, make2MaskName, model2MaskName);
            return url;
        }


        protected string FormatBikeCompareAnchorText(string bike1, string bike2)
        {
            string anchorText = String.Empty;
            anchorText = String.Format(_Bike1VsBike2, bike1, bike2);
            return anchorText;
        }

        public override void Dispose()
        {
            rptCompareBike.DataSource = null;
            rptCompareBike.Dispose();

            base.Dispose();
        }
    }
}