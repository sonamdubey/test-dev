using Bikewale.BAL.UserReviews;
using Bikewale.Common;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    public class TopUserReviews : System.Web.UI.UserControl
    {
        protected List<ReviewEntity> objReviewList = null;
        protected string modelMaskingName = String.Empty, makeMaskingName = String.Empty;
        protected uint totalReviews = 0;
        protected Repeater rptUserReviews;
        protected string _headerText = String.Empty;

        public uint ModelId { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public uint TopCount { get; set; }
        public FilterBy Filter { get; set; }

        public string HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; }
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                    GetTopUserReviews();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void GetTopUserReviews()
        {
            IUserReviews objUserReviews = null;

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IUserReviews, UserReviews>();

                objUserReviews = container.Resolve<IUserReviews>();

                objReviewList = objUserReviews.GetBikeReviewsList(1, TopCount, ModelId, 0, Filter, out totalReviews);

                if (totalReviews > 0)
                {
                    modelMaskingName = ModelMaskingName;
                    makeMaskingName = MakeMaskingName;

                    rptUserReviews.DataSource = objReviewList;
                    rptUserReviews.DataBind();
                }
            }
        }
    }
}