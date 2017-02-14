using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.GenericBikes;
using Bikewale.Notifications;
using System;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Sushil Kumar on 2nd Jan 2016
    /// Description : To bind desktop generic bike info control 
    /// Modified By :- subodh Jain 10 Feb 2017
    /// Summary :- BikeInfo Slug details
    /// </summary>
    public class GenericBikeInfoControl : System.Web.UI.UserControl
    {
        public uint ModelId { get; set; }
        protected GenericBikeInfo bikeInfo { get; set; }
        protected string bikeUrl = string.Empty, bikeName = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (ModelId > 0)
                {
                    BindBikeInfo genericBikeInfo = new BindBikeInfo();
                    genericBikeInfo.ModelId = ModelId;
                    bikeInfo = genericBikeInfo.GetBikeInfo();
                    if (bikeInfo != null)
                    {
                        if (bikeInfo.Make != null)
                            bikeUrl = string.Format("/{0}-bikes/{1}/", bikeInfo.Make.MaskingName, bikeInfo.Model.MaskingName);
                        if (bikeInfo.Model != null)
                            bikeName = string.Format("{0} {1}", bikeInfo.Make.MakeName, bikeInfo.Model.ModelName);
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Controls.GenericBikeInfoControl.Page_Load");
            }

        }
    }
}