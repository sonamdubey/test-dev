using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Controls;
using Carwale.Entity.CMS;
using Carwale.UI.Common;
using Carwale.Interfaces.CMS.Photos;
using Carwale.BL.CMS.Photos;
using Carwale.Entity.CMS.Photos;
using Carwale.Service;
using Carwale.Entity.CMS.URIs;
using System.Linq;

namespace Carwale.UI.Editorial
{
    public class OtherModelsGallery : System.Web.UI.Page
    {
        protected RepeaterPagerPhotoGallery rpgListings;
        protected Repeater rptListings;
        protected HtmlGenericControl omAlertMsg; // In the case that there are no images for similar cars to the model, the message to be displayed.
        protected int PageSize = 6;
        // protected variables
        protected string QsSortCriteria = string.Empty, QsSortOrder = string.Empty, PageNumber = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            rptListings = (Repeater)rpgListings.FindControl("rptListings");
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                omAlertMsg.Visible = false;
                string query_string = Request.ServerVariables["QUERY_STRING"];
                NameValueCollection qsCollection = Request.QueryString;
                if (!String.IsNullOrEmpty(qsCollection.Get("mId")) && !String.IsNullOrEmpty(qsCollection.Get("moId")))
                {
                    string makeId = qsCollection.Get("mId");
                    string modelId = qsCollection.Get("moId");
                    Trace.Warn(makeId);

                    if (CommonOpn.CheckId(makeId) && CommonOpn.CheckId(modelId))
                    {
                        IPhotos getotherPhotos = UnityBootstrapper.Resolve<CMSPhotosBL>();
                        CMSImage otherPhotosList = new CMSImage();
                        RelatedPhotoURI relatedPhoto = new RelatedPhotoURI();
                        relatedPhoto.ApplicationId = (ushort)CMSAppId.Carwale;
                        List<int> categories = new List<int>();
                        List<int> allcategories = new List<int>();
                        foreach (var values in Enum.GetValues(typeof(CMSContentType)))
                        {
                            allcategories.Add((int)values);
                        }

                        allcategories = allcategories.Where(x => x != (int)CMSContentType.News && x != (int)CMSContentType.ComparisonTests).ToList();
                        relatedPhoto.CategoryIdList = string.Join(",", allcategories);
                        int ModelId;
                        int.TryParse(modelId, out ModelId);
                        relatedPhoto.ModelId = ModelId == 0 ? ModelId = 852 : ModelId;
                        rpgListings.BaseUrl = "/editorial/othermodelsgallery.aspx?" + query_string;

                        string pageNumber = qsCollection.Get("pn");
                        if (!String.IsNullOrEmpty(pageNumber) && CommonOpn.IsNumeric(pageNumber))
                            rpgListings.CurrentPageIndex = int.Parse(pageNumber);
                        else
                            rpgListings.CurrentPageIndex = 1;

                        relatedPhoto.StartIndex = (uint)((rpgListings.CurrentPageIndex - 1) * PageSize + 1);
                        relatedPhoto.EndIndex = (uint)(rpgListings.CurrentPageIndex * PageSize);
                        otherPhotosList = getotherPhotos.GetOtherModelPhotosList(relatedPhoto);
                        foreach (var photos in otherPhotosList.Images)
                        {
                            if (photos.MainImgCategoryId == (int)CMSContentType.RoadTest)
                                photos.ImageTitle = "Road Test: " + photos.MakeBase.MakeName + ' ' + photos.ModelBase.ModelName;
                        }
                        rpgListings.PhotosList = otherPhotosList;
                        rpgListings.InitializeGrid();//initialize the grid, and this will also bind the repeater

                        if (rpgListings.RecordCount == 0)
                        {
                            rpgListings.Visible = false;
                            omAlertMsg.Visible = true;
                        }
                    }
                }
            }
        }
    } // class
} // namespace