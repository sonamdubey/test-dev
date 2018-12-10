using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carwale.UI.Common;
using Carwale.Service;
using Carwale.Interfaces.CarData;
using Carwale.BL.CMS;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;

namespace Carwale.UI.Controls
{
    public class SubNavigation : UserControl
    {
        //protected string ClassNameAP = string.Empty, ClassNameInt = string.Empty, ClassNameExt = string.Empty;

        //public UCSubNavigation();
        private string _make = "";
        private string _pageId = "";

        public string Make
        {
            get { return _make; }
            set { _make = value; }
        }

        public string PageId
        {
            get { return _pageId; }
            set { _pageId = value; }
        }

        public string TrackingFor360 { get; set; }
        public string MaskingName { get; set; }
        public int VideoCount { get; set; }
        public bool OfferExists { get; set; }
        //public int NewsCount { get; set; }
        public bool IsMileageAvail { get; set; }
        //public bool IsColorVisible { get; set; }
        public bool IsReviewsAvial { get; set; }
        public bool IsUsedCarAvail { get; set; }
        public string ClassNameAP { get; set; }
        public string ClassNameInt { get; set; }
        public string ClassNameExt { get; set; }
        public int PQPageId { get; set; }
        private bool _isExpertReviewAvial = false;
        public bool IsExpertReviewAvial
        {
            get { return _isExpertReviewAvial; }
            set { _isExpertReviewAvial = value; }
        }
        private bool _isUserReviewsAvailable = true;
        public bool IsUserReviewsAvailable
        {
            get
            {
                return _isUserReviewsAvailable;
            }
            set
            {
                _isUserReviewsAvailable = value;
            }
        }
        private bool _isOverviewPage = false;
        public bool IsOverviewPage
        {
            get { return _isOverviewPage; }
            set { _isOverviewPage = value; }
        }
        private bool _isVersionDetailPage = false;
        public bool IsVersionDetailPage
        {
            get { return _isVersionDetailPage; }
            set { _isVersionDetailPage = value; }
        }
        private bool _isMileagePage = false;
        public bool IsMileagePage
        {
            get { return _isMileagePage; }
            set { _isMileagePage = value; }
        }
        private bool _subNavOnCarCompare = false;
        public bool subNavOnCarCompare
        {
            get { return _subNavOnCarCompare; }
            set { _subNavOnCarCompare = value; }
        }
        public string ModelName { get; set; }
        public string MakeId { get; set; }
        public int ModelId { get; set; }
        public string MakeName { get; set; }
        public Repeater rptPages { get; set; }
        private string cityId = CookiesCustomers.CityId;
        public string cityIdFromCookie = CookiesCustomers.CityId;
        public string cityNameFromCookie = CookiesCustomers.City;
        public string CityId
        {
            get { return cityId; }
            set { cityId = value; }
        }
        private string cityName = CookiesCustomers.MasterCity;
        public string CityName
        {
            get { return cityName; }
            set { cityName = value; }
        }
        public bool ShowImagePopup = false;
        public bool is360Available { get; set; }
        public int ImageCount { get; set; }
        public string Category { get; set; }
        public bool ShowColorsLink { get; set; }
        public ThreeSixtyViewCategory Default360Category { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

    }
}