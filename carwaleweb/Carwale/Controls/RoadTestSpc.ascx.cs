using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Notifications;
using Carwale.Service;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Carwale.UI.Controls
{
    public class RoadTestSpc : UserControl
    {
        protected Repeater rptRoadTestSpc;
        public string Id = "";
        protected Label lblTitle;
        protected string _Car = "", _MakeId = "", _ModelId = "", _VersionId = "", _MaskingName = "";
        private int _ResultCount = 0, _count = 0, _descLength = 185;
        protected Label lblNotFound;
        protected string _IsExpandable = "1";


        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        public string Car
        {
            get
            {
                return _Car;
            }
            set
            {
                _Car = value;
            }
        }

        public string MakeId
        {
            get
            {
                return _MakeId;
            }
            set
            {
                _MakeId = value;
            }
        }

        public string ModelId
        {
            get
            {
                return _ModelId;
            }
            set
            {
                _ModelId = value;
            }
        }

        public string VersionId
        {
            get
            {
                return _VersionId;
            }
            set
            {
                _VersionId = value;
            }
        }

        public string IsExpandable
        {
            get
            {
                return _IsExpandable;
            }
            set
            {
                _IsExpandable = value;
            }
        }

        public int ResultCount
        {
            get
            {
                return _ResultCount;
            }
            set
            {
                _ResultCount = value;
            }
        }

        public int DescLength
        {
            get
            {
                return _descLength;
            }
            set
            {
                _descLength = value;
            }
        }

        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
            }
        }
        public string MaskingName
        {
            get
            {
                return _MaskingName;
            }
            set
            {
                _MaskingName = value;
            }
        }
        public string MakeName { get; set; }

        public List<ArticleSummary> ExpertReviews { get; set; }

        void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // FillDetails();
            }
        } // Page_Load



        public void GetCount()
        {
            try
            {
                ICMSContent contentContainer = UnityBootstrapper.Resolve<ICMSContent>();

                ResultCount = contentContainer.GetCMSRoadTestCount((string.IsNullOrEmpty(MakeId) ? -1 : Convert.ToInt32(MakeId)), (string.IsNullOrEmpty(ModelId) ? -1 : Convert.ToInt32(ModelId)), (string.IsNullOrEmpty(VersionId) ? -1 : Convert.ToInt32(VersionId)),1,(int)CMSAppId.Carwale);             
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }        

        protected string GetPubDate(string _PubDate)
        {
            if (_PubDate.ToString() == "")
                return "";
            else
                return "" + Convert.ToDateTime(_PubDate).ToString("dd MMM yyyy");
        }

        protected string GetDesc(string _textValue)
        {
            if (_textValue.Length < DescLength)
            {
                Trace.Warn("1");
                return _textValue;
            }
            else
            {
                _textValue = _textValue.Substring(0, DescLength);
                _textValue = _textValue.Substring(0, _textValue.LastIndexOf(" "));
                Trace.Warn("2");
                return _textValue + " ...";
            }

        }
    }
}