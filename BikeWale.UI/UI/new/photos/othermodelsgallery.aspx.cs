using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Controls;
using Bikewale.Common;
using Bikewale.Entities.CMS;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using Bikewale.Interfaces.Pager;
using Microsoft.Practices.Unity;
using Bikewale.BAL.Pager;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Pager;

namespace Bikewale.New.PhotoGallery
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 6 Oct 2014
    /// Modified methods to get model gallery photos from carwale api
    /// </summary>
    public class OtherModelsGallery : System.Web.UI.Page
    {

        protected NoFollowPagerControl spanPager;
        protected Repeater rptModelPhotos;
        protected HtmlGenericControl divPager;
        protected string categoryIdList = string.Empty,_modelId, QsSortCriteria = string.Empty, QsSortOrder = string.Empty, _pageNumber = "1",_baseUrl = string.Empty;
        private const int _pageSize = 6;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            Trace.Warn("Inside PAGE LOAD");
            ProcessQS();
            GetOtherModelPhotos();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessQS()
        {
            Trace.Warn("INSIDE query_string : " + Request.ServerVariables["QUERY_STRING"]);
            string query_string = Request.ServerVariables["QUERY_STRING"];
       
            NameValueCollection qsCollection = Request.QueryString;

            if (!String.IsNullOrEmpty(qsCollection.Get("moId")))
            {
                _modelId = qsCollection.Get("moId");
                if (!String.IsNullOrEmpty(qsCollection.Get("pn")))
                    _pageNumber = qsCollection.Get("pn");

                if (CommonOpn.CheckId(_modelId))
                    categoryIdList = (int)EnumCMSContentType.RoadTest + "," + (int)EnumCMSContentType.PhotoGalleries + "," + (int)EnumCMSContentType.ComparisonTests;
                else
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", true);
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 6 Oct 2014
        /// Summary    : method to fetch other models and model Photos from carwale api
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="categoryIdList"></param>
        private async void GetOtherModelPhotos()
        {
            try
            {                
                CMSImage _objPhotos = null;

                // get pager instance
                IPager objPager = GetPager();

                int _startIndex = 0, _endIndex = 0;
                objPager.GetStartEndIndex(_pageSize, Convert.ToInt32(_pageNumber) , out _startIndex, out _endIndex);


                string _apiUrl = "webapi/image/othermodelphotolist/?applicationid=2&startindex=" + _startIndex + "&endindex=" + _endIndex + "&modelid=" + _modelId + "&categoryidlist=" + categoryIdList;
                
                using(Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    _objPhotos = await objClient.GetApiResponse<CMSImage>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objPhotos);
                }
                
                if (_objPhotos != null)
                {
                    BindOtherModelsPhotos(_objPhotos.Images);

                    if (_objPhotos.RecordCount > _pageSize)
                        BindLinkPager(objPager, Convert.ToInt32(_objPhotos.RecordCount));
                    else
                        divPager.Visible = false;
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        private void BindOtherModelsPhotos(List<ModelImage> list)
        {
            rptModelPhotos.DataSource = list;
            rptModelPhotos.DataBind();
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 6 Oct 2014
        /// Summary    : method to create pager
        /// </summary>
        /// <param name="objPager"></param>
        /// <param name="recordCount"></param>
        private void BindLinkPager(IPager objPager, int recordCount)
        {
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;
            _baseUrl = "/new/photos/othermodelsgallery.aspx?moId=" + _modelId;

            try
            {
                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = _baseUrl;
                _pagerEntity.PageNo = Convert.ToInt32(_pageNumber); //Current page number
                _pagerEntity.PagerSlotSize = 5; // 5 links on a page
                _pagerEntity.PageUrlType = "&pn=";
                _pagerEntity.TotalResults = recordCount; //total models count
                _pagerEntity.PageSize = _pageSize;        //No. of model photos to be displayed on a page

                _pagerOutput = objPager.GetPager<PagerOutputEntity>(_pagerEntity);

                // for RepeaterPager
                spanPager.PagerOutput = _pagerOutput;
                spanPager.CurrentPageNo = Convert.ToInt32(_pageNumber);
                spanPager.TotalPages = objPager.GetTotalPages(recordCount, _pageSize);
                spanPager.BindPagerList();
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        //PopulateWhere to create Pager instance
        private IPager GetPager()
        {
            IPager _objPager = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IPager, Pager>();
                _objPager = container.Resolve<IPager>();
            }
            return _objPager;
        }
        

        //Commented By : Ashwini Todkar on 6 oct 2014

        //modefied By Sadhana Upadhyay on 4 July 2014
        //private string GetSelectClause()
        //{
        //    return " ROW_NUMBER() OVER (PARTITION BY BMo.Id ORDER BY CB.EntryDate DESC) as Rnk, "
        //    + " CB.Id AS BasicId, CB.CategoryId,CB.EntryDate, BMa.Name AS MakeName, "
        //    + " BMa.MaskingName AS MakeMaskingName, BMo.MaskingName AS ModelMaskingName,BMo.Name AS ModelName, "
        //    + " BMo.HostURL, BMo.LargePic AS LargePicPath ";
        //}

        ////modefied By Sadhana Upadhyay on 4 July 2014
        //private string GetFromClause()
        //{
        //    return " Con_EditCms_Basic CB WITH(NOLOCK) "
        //    + " INNER JOIN Con_EditCms_Images CEI WITH(NOLOCK) ON CEI.BasicId = CB.Id "
        //    + " INNER JOIN BikeModels as BMo WITH(NOLOCK) ON BMo.ID= CEI.ModelId "
        //    + " INNER JOIN BikeMakes as BMa WITH(NOLOCK) ON BMa.ID= CEI.MakeId "
        //    + " INNER JOIN [dbo].[fnSplitCSV](@CategoryId) AS FN ON FN.ListMember=CB.CategoryId ";
        //}

        ////modefied By Sadhana Upadhyay on 4 July 2014
        //private string GetWhereClause()
        //{
        //    string whereClause = " CEI.MakeId = @MakeId AND CEI.ModelId <> @ModelId "
        //    + " AND CEI.IsActive = 1 AND CEI.IsActive = 1 AND CB.IsPublished=1 AND BMa.IsDeleted = 0 AND BMo.IsDeleted = 0 AND BMa.New = 1 AND BMo.New = 1 AND Bmo.Futuristic = 0 ";
        //    return whereClause;
        //}

        ////modefied By Sadhana Upadhyay on 4 July 2014
        //private string GetOrderByClause()
        //{
        //    string retVal = string.Empty;

        //    retVal = " ModelName ";

        //    return retVal;
        //}

        ////modefied By Sadhana Upadhyay on 4 July 2014
        //private string GetRecordCountQry()
        //{
        //    return "Select Count(*) From (SELECT " + GetSelectClause() + " From " + GetFromClause() + " Where " + GetWhereClause() + " )tbl WHERE Rnk = 1";
        //}
    } // class
} // namespace