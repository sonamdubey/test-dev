using Carwale.BL.Videos;
using Carwale.Cache.CMS;
using AEPLCore.Cache;
using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Interfaces;
using Carwale.Interfaces.CMS;
using Carwale.Notifications;
using Carwale.Service;
using Microsoft.Practices.Unity;
using System;
using System.Web.UI.WebControls;

public class VideoCarousel : System.Web.UI.UserControl
{
    protected Repeater rptFeatured;
    protected int topCount = -1;
    protected int category = 1;
    protected int pageId = -1;

    public string TopCount
    {
        get { return topCount.ToString(); }
        set { topCount = Convert.ToInt32(value); }
    }

    public int Category
    {
        get { return category; }
        set { category = value; }
    }

    public int PageId
    {
        get { return pageId; }
        set { pageId = value; }
    }

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
        if (!IsPostBack)
        {
            BindFeaturedAndLatestVideos();
        }
    } // Page_Load

    /// <summary>
    /// Binds List of videos to Repeater datasource
    /// </summary>
    void BindFeaturedAndLatestVideos()
    {
        try
        {
            IVideosBL blObj = UnityBootstrapper.Resolve<IVideosBL>();

            rptFeatured.DataSource = blObj.GetNewModelsVideosBySubCategory(getCategory(Category), CMSAppId.Carwale, 1, topCount);
            rptFeatured.DataBind();
        }
        catch (Exception ex)
        {
            ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
            objErr.SendMail();
        }
    }
    protected string FindSubString(string str, string strFrom, string strTo)
    {
        int pFrom = str.IndexOf(strFrom) + strFrom.Length; ;
        int pTo = str.IndexOf(strTo);
        return str.Substring(pFrom, pTo - pFrom);
    }

    protected string FormatSubCat(string subCat)
    {
        return subCat.Trim().ToLower().Replace(" ", "-");
    }
    protected EnumVideoCategory getCategory(int catid)
    {
        switch (catid)
        {
            case 1: return EnumVideoCategory.FeaturedAndLatest;
            case 2: return EnumVideoCategory.MostPopular;
            case 3: return EnumVideoCategory.ExpertReviews;
            case 4: return EnumVideoCategory.InteriorShow;
            case 5: return EnumVideoCategory.Miscelleneous;
            case 6: return EnumVideoCategory.JustLatest;
            default: return EnumVideoCategory.FeaturedAndLatest;
        }
    }
}