using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Xml;
using Carwale.UI.Common;
using System.Configuration;
using System.Collections.Generic;
using Carwale.Entity.Classified;
using Microsoft.Practices.Unity;
using Carwale.Interfaces.Classified;
using Carwale.DAL.Classified;
using Carwale.Cache.Classified;
using Carwale.Interfaces;
using AEPLCore.Cache;
using Carwale.Interfaces.CarData;
using Carwale.BL.CarData;
using Carwale.Cache.CarData;
using Carwale.DAL.CarData;
using Carwale.Notifications;
using AEPLCore.Cache.Interfaces;

namespace Carwale.UI.Controls
{
    public class UsedCarsCount : UserControl
    {
        protected int usedcarsCount = 0, minUsedcarsPrice = 0,_cityId = -1,_rootId = -1;
        protected string _MakeName = "", _ModelName = "", _MaskingName = string.Empty, liveListingcount = "", minLiveListingPrice = "", _MakeId = "", _ModelId = "";
        protected HtmlGenericControl divUsedCarsCount;

        public string MakeId
        {
            get { return _MakeId; }
            set { _MakeId = value; }
        }

        public int RootId
        {
            get { return _rootId; }
            set { _rootId = value; }
        }

        public int CityId 
        {
            get { return _cityId; }
            set { _cityId = value; }
        }

        public string ModelId
        {
            get { return _ModelId; }
            set { _ModelId = value; }
        }

        public string MakeName
        {
            get { return _MakeName; }
            set { _MakeName = value; }
        }

        public string ModelName
        {
            get { return _ModelName; }
            set { _ModelName = value; }
        }

        public string MaskingName
        {
            get { return _MaskingName; }
            set { _MaskingName = value; }
        }

        private bool _isUsedCarAvail = false;
        public bool IsUsedCarAvial
        {
            get { return _isUsedCarAvail; }
            set { _isUsedCarAvail = value; }
        }
        public UsedCarCount UsedCarCount = new UsedCarCount();

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void GetUsedCarsDetails()
        {
            try
            {
                divUsedCarsCount.Visible = false;
                if (UsedCarCount != null)  //for model details page 
                {
                    liveListingcount = UsedCarCount.LiveListingCount.ToString();
                    minLiveListingPrice = UsedCarCount.MinLiveListingPrice.ToString();

                    if ((!String.IsNullOrEmpty(liveListingcount)) || (!String.IsNullOrEmpty(minLiveListingPrice)))
                    {
                        usedcarsCount = Convert.ToInt32(liveListingcount);
                        minUsedcarsPrice = Convert.ToInt32(minLiveListingPrice);
                        minLiveListingPrice = CommonOpn.FormatNumeric(minLiveListingPrice);
                    }

                    //divUsedCarsCount is shown only when liveListingcount and minLiveListingPrice is not null or empty and usedcarsCount and minUsedcarsPrice are greater than zero.
                    if (((!String.IsNullOrEmpty(liveListingcount)) || (!String.IsNullOrEmpty(minLiveListingPrice))) && (usedcarsCount > 0 && minUsedcarsPrice > 0))
                    {
                        divUsedCarsCount.Visible = true;
                        IsUsedCarAvial = true;
                    }
                }
                else
                {
                    IUnityContainer container = new UnityContainer();
                    container.RegisterType<IStockCountRepository, StockCountRepository>()
                        .RegisterType<IStockCountCacheRepository, StockCountCacheRepository>()
                        .RegisterType<ICacheManager, CacheManager>();
                    IStockCountCacheRepository usedContainer = container.Resolve<IStockCountCacheRepository>();

                    UsedCarCount count = usedContainer.GetUsedCarsCount(RootId, CityId);

                    if (count != null) 
                    {
                        //Get live listing count of passed car model
                        liveListingcount = count.LiveListingCount.ToString();

                        //Get minimum available price of passed car model
                        minLiveListingPrice = count.MinLiveListingPrice.ToString();
                    }
                    
                    if ((!String.IsNullOrEmpty(liveListingcount)) || (!String.IsNullOrEmpty(minLiveListingPrice)))
                    {
                        Trace.Warn("in if 1");
                        usedcarsCount = Convert.ToInt32(liveListingcount);
                        minUsedcarsPrice = Convert.ToInt32(minLiveListingPrice);
                        minLiveListingPrice = CommonOpn.FormatNumeric(minLiveListingPrice);
                    }

                    //divUsedCarsCount is shown only when liveListingcount and minLiveListingPrice is not null or empty and usedcarsCount and minUsedcarsPrice are greater than zero.
                    if (((!String.IsNullOrEmpty(liveListingcount)) || (!String.IsNullOrEmpty(minLiveListingPrice))) && (usedcarsCount > 0 && minUsedcarsPrice > 0))
                    {
                        divUsedCarsCount.Visible = true;
                        IsUsedCarAvial = true;
                    }
                }
            }

            catch (SqlException ex)
            {
                Trace.Warn("special sql ex :" + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn("special ex :" + err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

    }//class
}//namespace