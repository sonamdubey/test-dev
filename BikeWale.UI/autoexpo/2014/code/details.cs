//<%# DataBinder.Eval(Container.DataItem,"MainImageSet").ToString() == "True" ? "<a rel='slide' href='" + CarWale.CommonCW.ImagingFunctions.GetImagePath("/ec/") + DataBinder.Eval(Container.DataItem,"BasicId") + "/img/m/" + DataBinder.Eval(Container.DataItem,"BasicId") + "_l.jpg'><img class='alignright size-thumbnail img-border-news' src='" + CarWale.CommonCW.ImagingFunctions.GetImagePath("/ec/") + DataBinder.Eval(Container.DataItem,"BasicId") + "/img/m/"+ DataBinder.Eval(Container.DataItem,"BasicId") +"_m.jpg' align='right' border='0' /></a>" : "" %>

using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using System.Configuration;
using System.Text.RegularExpressions;
using Carwale.CMS;
using System.Collections.Generic;
using Carwale.CMS.Entities;

namespace AutoExpo
{
    public class details : Page
	{


        protected string BasicId = string.Empty, Url = string.Empty, DisplayDate = string.Empty, Details = string.Empty,
                                 MainImageSet = string.Empty, HostURL = string.Empty, NewsTitle = string.Empty, AuthorName = string.Empty, ImagePathThumbNail = string.Empty, ImagePathLarge = string.Empty;
		
		protected override void OnInit( EventArgs e )		
        {
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( this.Page_Load );
            //rptSubCat.ItemDataBound += new RepeaterItemEventHandler(rptSubCat_ItemDataBound);
		}

		void Page_Load( object Sender, EventArgs e )
		{
            if (!IsPostBack)
            {
                CommonOpn op = new CommonOpn();

                if (Request["bid"] != null && Request.QueryString["bid"] != "")
                {
                    if (CommonOpn.CheckId(Request.QueryString["bid"]) == true)
                        BasicId = Request.QueryString["bid"];
                }

                FillNews();
            }			
			
		} // Page_Load
		
		
        //getting details from news id 
		private void FillNews()
		{
            Database db = new Database();
            NewsTitle = "Auto Expo Updates <span>Latest Bike Launch News</span>";
            var cateList = new List<int>();
            ICMSContent content = CMSFactory.GetInstance(EnumCMSContentType.AutoExpo);
            List<GenericEntitiyContentList> contentDetails = new List<GenericEntitiyContentList>();
            contentDetails = (List<GenericEntitiyContentList>)content.GetContentDetails<GenericEntitiyContentList>(Convert.ToInt32(BasicId));

            GenericEntitiyContentList detailsContent = (GenericEntitiyContentList) contentDetails[0];
            
            NewsTitle = detailsContent.Title.ToString();
            AuthorName = detailsContent.AuthorName;
            DisplayDate = detailsContent.DisplayDate.ToString();
            Url = detailsContent.Url;
            Details = detailsContent.Description;
            MainImageSet = detailsContent.IsMainImage.ToString();
            HostURL = detailsContent.HostURL;
            ImagePathLarge = detailsContent.ImagePathLarge;
            ImagePathThumbNail = detailsContent.ImagePathThumbNail;
		}
	} // class
} // namespace