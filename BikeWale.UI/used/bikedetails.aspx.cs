using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using System.Text.RegularExpressions;

namespace Bikewale.Used
{
    /// <summary>
    ///     Class to show the details of the bike
    /// </summary>
    public class BikeDetails : Page
    {
        protected HtmlGenericControl scrollable_imgs, requestPhotos, other_models, salersNote, navigate_img, addDetails;
        protected Repeater rptPhotos, rptBikeDetails;

        public ClassifiedInquiryDetails objInquiry;
        public ClassifiedInquiryPhotos objPhotos;

        protected Repeater rptCompareList = null;
        
        // variable used for paging photos
        public int pageNo = 1;
        public int scale = 10;

        public string profileId = "", sellInqId = "";
        public bool isDealer = false;

        public string oem = "", bodyType = "", subSegment = "";
        protected string customerId = "", bikeCanonical = string.Empty;

        /// <summary>
        ///     selected Bikes list and compareCaption variables are used for add or remove the Bike in Bikes compare list
        /// </summary>
        //protected string selectedBikesList = string.Empty;
        //protected bool compareCaption = true;

        //protected BikesFromSameOwner dealerBikes;

        protected string researchBaseUrl = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            customerId = CurrentUser.Id;
            
            ValidateProfileId();

            objInquiry = new ClassifiedInquiryDetails(profileId);
            objPhotos = new ClassifiedInquiryPhotos();

            if (!IsPostBack)
            {
                // Validate profile id. Profile id is a unique id if each classified listing
                // if listed Bike is of a 'Dealer' than profile id will be like D78243(primary key of the table prepand by 'D')
                // if listed Bike is of a 'Individual' than profile id will be like S78243(primary key of the table prepand by 'S')				

                

                // if fake/wrong profile id passed in query string
                //Modified By : Ashwini Todkar added soldoutstatus fake or invalid
                if (objInquiry.AskingPrice == "" || objInquiry.AskingPrice == "-1" || objInquiry.SoldOutStatus == "4" || objInquiry.SoldOutStatus == "2")
                {
                    Response.Redirect("/pageNotFound.aspx",false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }

                if (objInquiry.SoldOutStatus == "3")
                    Response.Redirect("/used/BikeSold.aspx?profile=" + profileId);

                // Check seller type(Individual or Dealer)
                isDealer = CommonOpn.CheckIsDealerFromProfileNo(profileId);

                // If Seller is a Dealer, Show other Bikes if has showroom.
                //dealerBikes.SellerId = objInquiry.SellerId;
                //dealerBikes.ProfileNo = profileId;

                //dealerBikes.Price = int.Parse(objInquiry.AskingPrice.Replace(",", ""));

                //dealerBikes.Records = 5;

                // Bind photos 
                bool isAprooved = true;
                
                objPhotos.BindWithRepeater(sellInqId, isDealer, rptPhotos, isAprooved);

                if (objInquiry.VersionId != "" && objInquiry.CityId != "")
                {
                    ClassifiedCrossSelling objCrossSell = new ClassifiedCrossSelling();
                    objCrossSell.GetOtherModels(objInquiry.VersionId, objInquiry.ModelId, objInquiry.CityId, rptBikeDetails, profileId);
                }

                if (rptBikeDetails.Items.Count == 0) other_models.Style.Add("display", "none");

                // If photos not uploaded by the seller
                // Hide photos contents and make 'requestPhotos' div visible where buyer can request seller to upload photos.
                if (objPhotos.ClassifiedImageCount == 0)
                {
                    scrollable_imgs.Visible = false;

                    ClassifiedBuyerDetails objCBD = new ClassifiedBuyerDetails();
                    objCBD.GetBuyerDetails();

                    if (objCBD.BuyerId == "")
                        requestPhotos.Visible = true;
                    else if (!ClassifiedInquiryPhotos.IsPhotoRequestDone(sellInqId, objCBD.BuyerId, isDealer))
                        requestPhotos.Visible = true;
                }
                else if (objPhotos.ClassifiedImageCount <= 9)
                {
                    navigate_img.Visible = false;
                }

                // If seller's note not provided, Hide the relavent container
                if (objInquiry.CustomersNote == "")
                    salersNote.Visible = false;

                if (objInquiry.Warranties == "--" && objInquiry.Modifications == "--")
                    addDetails.Visible = false;


                // Format research base url id user want to view standard specs, features and colours
                //researchBaseUrl = "/" + UrlRewrite.FormatSpecial(objInquiry.MakeName) + "-bikes/" + UrlRewrite.FormatSpecial(objInquiry.ModelName) + "/" + UrlRewrite.FormatSpecial(objInquiry.VersionName);
                //bikeCanonical = "/used-bikes-in-" + UrlRewrite.FormatSpecial(objInquiry.CityName) + "/" + UrlRewrite.FormatSpecial(objInquiry.MakeName) + "-" + UrlRewrite.FormatSpecial(objInquiry.ModelName) + "-" + profileId + "/";

                researchBaseUrl = "/" + objInquiry.MakeMaskingName  + "-bikes/" + objInquiry.ModelMaskingName + "/" ;
                bikeCanonical = "/used/bikes-in-" + objInquiry.CityMaskingName + "/" + objInquiry.MakeMaskingName + "-" + objInquiry.ModelMaskingName + "-" + profileId + "/";
                
                
                // Summary : Get selected Bikes list from cookies
                //selectedBikesList = ClassifiedCookies.GetCompareList;

                //compareCaption = GetCompareCaption();

                //ClassifiedCompareBikes compareBikesObj = new ClassifiedCompareBikes();
                //compareBikesObj.BindCompareBikesData(rptCompareList);

            }
        }   // End of page load

        /// <summary>
        ///     Function to validate profile id
        /// </summary>
        void ValidateProfileId()
        {
            if (Request["bike"] != null && Request.QueryString["bike"] != "")
            {
                profileId = Request.QueryString["bike"];

                // Modified By Ashish G. Kamble on 14/3/2012
                // First check whether the bike profile id is valid or not then process next operations.
                if (!Bikewale.Common.Validations.IsValidProfileId(profileId))
                {
                    Response.Redirect("/pageNotFound.aspx",false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                else
                {
                    sellInqId = CommonOpn.GetProfileNo(profileId);

                    if (!CommonOpn.CheckId(sellInqId))
                    {
                        Response.Redirect("/pageNotFound.aspx",false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
            }
            else
            {
                Response.Redirect("/pageNotFound.aspx",false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }   //  End of ValidateProfileId

        public string GetColorCode()
        {
            string returnHtml = "";

            if (objInquiry.ExtiriorCode != "")
                returnHtml = "<div class=\"bike-color\" title=\"" + objInquiry.ExtiriorColor + "\" style=\"background-color:#" + objInquiry.ExtiriorCode + ";\"></div>";
            else
                returnHtml = objInquiry.ExtiriorColor;

            return returnHtml;
        }   // End of GetColorCode

        protected string GetImagePath(string imgName, string directoryPath, string hostUrl)
        {
            Trace.Warn("directoryPath : ", directoryPath);
            Trace.Warn("hostUrl : ", hostUrl);
            return Bikewale.Common.ImagingFunctions.GetPathToShowImages(directoryPath, hostUrl) + imgName;
        }

        protected string GetOriginalImagePath(string imgName, string hostUrl, string size)
        {
            Trace.Warn("hostUrl : ", hostUrl);
            return Bikewale.Utility.Image.GetPathToShowImages(imgName, hostUrl,size);
        }

        public string GetPageItemContainer()
        {
            string returnItem = "";

            if ((pageNo % scale) == 0)
            {
                pageNo = 1;
                returnItem = "</div><div>";
            }
            pageNo++;
            return returnItem;
        }

        /// <summary>
        /// function for returning back to the search page with all parameters
        /// </summary>
        /// <returns></returns>
        public string GetBackToSearch()
        {
            string backLink = Request.ServerVariables["QUERY_STRING"];

            if (backLink.Split('&').Length > 1)
            {
                backLink = Regex.Replace(backLink, "(bike=[S|D][0-9]*&)|(&bike=[S|D][0-9]*)", "");
            }
            else
            {
                backLink = "make=" + objInquiry.MakeId;
            }
            return backLink;
        }

    }   // End of class
}   // End of namespace