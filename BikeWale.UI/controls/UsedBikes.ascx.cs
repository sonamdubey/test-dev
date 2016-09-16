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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Author : Vivek Gupta
    /// Created Date : 21st june 2016
    /// Desc : recently uploaded used bikes control
    /// </summary>
    public class UsedBikes : UserControl
    {
        protected Repeater rptUsedBikeNoCity, rptRecentUsedBikes;

        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public uint TopCount { get; set; }
        public uint CityId { get; set; }
        public string makeName;
        public string cityName;
        public string modelName;
        public string makeMaskingName;
        public string modelMaskingName;
        public string cityMaskingName;
        public bool showWidget;
        public uint FetchedRecordsCount;
        public string headingName;
        IEnumerable<MostRecentBikes> objMostRecentBikes;
        MostRecentBikes objFirstListObject;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(Page_Load);
            showWidget = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (isValidData())
                BindUsedBikes();
        }

        /// <summary>
        /// Function to validate the data passed to the widget
        /// </summary>
        /// <returns></returns>
        private bool isValidData()
        {
            bool isValid = true;

            if (MakeId < 0 || ModelId < 0)
            {
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Created By : Sajal Gupta on 15/09/2016
        /// Description : Function to bind used bikes for the makes/models.          
        /// </summary>
        protected void BindUsedBikes()
        {
            try
            {
                objMostRecentBikes = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    container.RegisterType<IUsedBikes, Bikewale.BAL.UsedBikes.UsedBikes>();
                    container.RegisterType<IUsedBikesCache, UsedBikesCache>();

                    IUsedBikesCache _objUsedBikes = container.Resolve<IUsedBikesCache>();

                    objMostRecentBikes = _objUsedBikes.GetUsedBikes(MakeId, ModelId, CityId, TopCount);

                    FetchedRecordsCount = Convert.ToUInt32(objMostRecentBikes.Count());
                    if (FetchedRecordsCount > 0)
                    {
                        makeName = objMostRecentBikes.FirstOrDefault().MakeName;
                        cityName = objMostRecentBikes.FirstOrDefault().CityName;
                        makeMaskingName = objMostRecentBikes.FirstOrDefault().MakeMaskingName;
                        cityMaskingName = objMostRecentBikes.FirstOrDefault().CityMaskingName;


                        if (ModelId != 0)  //model-page
                        {
                            modelName = objMostRecentBikes.FirstOrDefault().ModelName;
                            headingName = modelName;
                            modelMaskingName = objMostRecentBikes.FirstOrDefault().ModelMaskingName;
                        }
                        else   //make-page
                        {
                            headingName = makeName;
                            modelMaskingName = null;
                        }

                        if (CityId > 0)  //City-Present
                        {
                            rptRecentUsedBikes.DataSource = objMostRecentBikes;
                            rptRecentUsedBikes.DataBind();
                            showWidget = true;
                        }
                        else  //City-not-present
                        {
                            rptUsedBikeNoCity.DataSource = objMostRecentBikes;
                            rptUsedBikeNoCity.DataBind();
                            showWidget = true;
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