using Bikewale.BindViewModels.Webforms.Used;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Used;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Mobile.Used
{
    /// <summary>
    /// Created by : Subodh Jain 29 Dec 2016
    /// Summary: Get Used bikes by make in cities
    /// </summary>
    public class MakesInCity : System.Web.UI.Page
    {
        protected IEnumerable<UsedBikeCities> UsedBikeCityCountTopList = null;
        protected IEnumerable<UsedBikeCities> UsedBikeCityCountList = null;
        protected BikeMakeEntityBase MakeDetails;
        protected uint makeId;
        protected string pgTitle = string.Empty, pgDescription = string.Empty, pgCanonical = string.Empty, pgKeywords = string.Empty, makeMaskingName = string.Empty, pgAlternative = string.Empty;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        protected void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }
        /// <summary>
        /// Created by : Subodh Jain 29 Dec 2016
        /// Summary: Get Used bikes by make in cities
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {

            ProcessQueryString();
            if (MakeDetails != null)
                BindCities();
        }
        /// <summary>
        /// Created by : Subodh Jain 29 Dec 2016
        /// Summary: for processing querystring
        /// Modified by: Dhruv Joshi
        /// Dated: 19th July 2018
        /// Description: Processing Masking Name
        /// </summary>
        private void ProcessQueryString()
        {

            try
            {                
                makeMaskingName = Request.QueryString["make"];
                if (!string.IsNullOrEmpty(makeMaskingName))
                {
                    makeMaskingName = makeMaskingName.ToLower();
                    MakeMaskingResponse objMake = new MakeHelper().GetMakeByMaskingName(makeMaskingName);
                    if (objMake != null)
                        MakeDetails = new MakeHelper().GetMakeNameByMakeId(Convert.ToUInt16(objMake.MakeId));
                }
                    
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "MakesInCity.ProcessQueryString");
            }
        }
        /// <summary>
        /// Created By Subodh Jain on 29 dec 2016
        /// Desc : Bind cities on City page accoding to make
        /// Modified by: Dhruv Joshi
        /// Dated: 19th July 2018
        /// Description: Set Make Masking Name
        /// </summary>
        private void BindCities()
        {
            try
            {
                BindUsedBikesByMakeCity objBikeCity = new BindUsedBikesByMakeCity();
                objBikeCity.MakeName = MakeDetails.MakeName;
                objBikeCity.MakeMaskingName = MakeDetails.MaskingName;
                UsedBikeCityCountList = objBikeCity.GetUsedBikeByMakeCityWithCount(Convert.ToUInt16(MakeDetails.MakeId));
                objBikeCity.CreateMetas();
                UsedBikeCityCountTopList = UsedBikeCityCountList.Where(x => x.Priority > 0); ;
                UsedBikeCityCountList = UsedBikeCityCountList.OrderBy(c => c.CityName);
                pgKeywords = objBikeCity.keywords;
                pgTitle = objBikeCity.title;
                pgDescription = objBikeCity.description;
                pgCanonical = objBikeCity.canonical;
                pgAlternative = objBikeCity.alternative;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "MakesInCity.BindCities");

            }
        }
    }
}