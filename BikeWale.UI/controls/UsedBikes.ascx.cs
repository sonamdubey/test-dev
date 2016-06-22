using Bikewale.Cache.Core;
using Bikewale.Cache.UsedBikes;
using Bikewale.Common;
using Bikewale.DAL.UsedBikes;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.UsedBikes;
using Microsoft.Practices.Unity;
using Org.BouncyCastle.Asn1.Ocsp;
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
        public uint TopCount { get; set; }
        public int CityId { get; set; }
        public string makeName = string.Empty;
        public string makeMaskingName = string.Empty;
        public string cityMaskingName = string.Empty;

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
        /// Function to validate the data passed to the widget
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
        /// Created By : Vivek Gupta on 20-05-2016
        /// Description : Function to bind used bikes for the makes          
        /// </summary>
        protected void BindUsedBikes()
        {
            try
            {
                if (TopCount <= 0) { TopCount = 6; }


                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikesCacheRepository, UsedBikesCacheRepository>();
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    container.RegisterType<IUsedBikes, UsedBikesRepository>();

                    IUsedBikesCacheRepository _objUsedBikes = container.Resolve<IUsedBikesCacheRepository>();

                    objMostRecentBikes = _objUsedBikes.GetMostRecentUsedBikes(MakeId, TopCount, CityId);

                    if (objMostRecentBikes.Count() > 0)
                    {
                        makeName = objMostRecentBikes.FirstOrDefault().MakeName;
                        makeMaskingName = objMostRecentBikes.FirstOrDefault().MakeMaskingName;
                        cityMaskingName = objMostRecentBikes.FirstOrDefault().CityMaskingName;

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
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"] + " : BindUsedBikes");
                objErr.SendMail();
            }
        }
    }
}