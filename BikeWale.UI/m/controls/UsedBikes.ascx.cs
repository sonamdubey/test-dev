using Bikewale.Cache.Core;
using Bikewale.Cache.UsedBikes;
using Bikewale.Common;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.UsedBikes;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created by Subodh Jain on 15 sep 2016
    /// Dec:- Used bike widget for model and make page mobile
    /// </summary>
    public class UsedBikes : System.Web.UI.UserControl
    {
        protected Repeater rptUsedBikeNoCity, rptRecentUsedBikes;

        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public uint TopCount { get; set; }
        public uint CityId { get; set; }
        public string makeName = string.Empty;
        public string modelName = string.Empty;
        public string cityName = string.Empty;
        public string makeMaskingName = string.Empty;
        public string modelMaskingName = string.Empty;
        public string cityMaskingName = string.Empty;
        public string pageHeading = string.Empty;
        IEnumerable<MostRecentBikes> objMostRecentBikes = null;

        public bool showWidget = false;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (isValidData())
                BindUsedBikes();
        }

        /// <summary>
        ///  Created by Subodh Jain on 15 sep 2016
        /// Desc:-Function to validate the data passed to the widget
        /// </summary>
        /// <returns></returns>
        private bool isValidData()
        {
            bool isValid = true;

            if (MakeId <= 0)
            {
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Created By :subodh jain on 14 sep 2016
        /// Description : Function to bind used bikes for the makes          
        /// </summary>
        protected void BindUsedBikes()
        {
            try
            {
                if (TopCount <= 0) { TopCount = 6; }


                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikes, Bikewale.BAL.UsedBikes.UsedBikes>();
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    container.RegisterType<IUsedBikesCache, UsedBikesCache>();
                    IUsedBikesCache _objUsedBikes = container.Resolve<IUsedBikesCache>();

                    objMostRecentBikes = _objUsedBikes.GetUsedBikes(MakeId, ModelId, CityId, TopCount);

                    if (objMostRecentBikes != null && objMostRecentBikes.Count() > 0)
                    {
                        MostRecentBikes ObjDetails = null;
                        ObjDetails = objMostRecentBikes.FirstOrDefault();
                        if (ObjDetails != null)
                        {
                            makeName = ObjDetails.MakeName;
                            if (ModelId > 0)
                            {
                                modelName = ObjDetails.ModelName;
                                modelMaskingName = ObjDetails.ModelMaskingName;
                            }

                            makeMaskingName = ObjDetails.MakeMaskingName;
                            if (CityId > 0)
                            {
                                cityName = ObjDetails.CityName;
                                cityMaskingName = ObjDetails.CityMaskingName;
                            }

                            pageHeading = String.Format("{0} {1}", makeName, modelName);
                            if (CityId > 0)
                            {
                                rptRecentUsedBikes.DataSource = objMostRecentBikes;
                                rptRecentUsedBikes.DataBind();
                                showWidget = true;
                            }

                            else if (CityId <= 0)
                            {
                                rptUsedBikeNoCity.DataSource = objMostRecentBikes;
                                rptUsedBikeNoCity.DataBind();
                                showWidget = true;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"] + " : BindUsedBikes");
                objErr.SendMail();
            }
        }
    }
}