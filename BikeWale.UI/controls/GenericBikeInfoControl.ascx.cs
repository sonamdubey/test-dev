using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.GenericBikes;
using System;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Sushil Kumar on 2nd Jan 2016
    /// Description : To bind desktop generic bike info control 
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
            if (ModelId > 0)
            {
                BindGenericBikeInfo genericBikeInfo = new BindGenericBikeInfo();
                genericBikeInfo.ModelId = ModelId;
                bikeInfo = genericBikeInfo.GetGenericBikeInfo();
                if (bikeInfo != null)
                {
                    bikeUrl = string.Format("/{0}-bikes/{1}/", bikeInfo.Make.MaskingName, bikeInfo.Model.MaskingName);
                    bikeName = string.Format("{0} {1}", bikeInfo.Make.MakeName, bikeInfo.Model.ModelName);
                };
            }

        }
    }
}