using Carwale.Entity.Author;
using Carwale.Interfaces.Author;
using Carwale.Notifications;
using Carwale.Service;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MobileWeb.Authors
{
    public class Default : System.Web.UI.Page
    {
        protected Repeater rptAuthors;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetAuthorDetails();
            }
        }

        /// <summary>
        /// Created Date : 30/7/2014 
        /// Desc: Get the list of all authors
        /// </summary>
        private void GetAuthorDetails()
        {
            IAuthorRepository authorContainer = UnityBootstrapper.Resolve<IAuthorRepository>();
            try
            {
                List<AuthorList> authorDetails = authorContainer.GetAuthorsList();
                Trace.Warn("List cnt :" + authorDetails.Count);
                rptAuthors.DataSource = authorDetails;
                rptAuthors.DataBind();
            }
            catch (Exception ex)
            {
                Trace.Warn("ex : " + ex.Message);
                ExceptionHandler objErr = new ExceptionHandler(ex, "MobileWeb.Authors.Default.GetAuthorDetails()");
                objErr.LogException();
            }
        }
    }
}