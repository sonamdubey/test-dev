﻿using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Mobile.Controls;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Bikewale.Mobile.New
{
    public class CompareBike : System.Web.UI.Page
    {
        protected HtmlGenericControl ddlMake1, ddlMake2;
        protected CompareBikeMin ctrlCompareBikes;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadCompareBikeMakes();
                ctrlCompareBikes.TotalRecords = 3;
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 24 Sept 2014
        /// Summary : To load bike makes
        /// </summary>
        private void LoadCompareBikeMakes()
        {
            try
            {
                string retVal1 = "", retVal2 = "";
                List<BikeMakeEntityBase> makeList = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakes<BikeMakeEntity, int>>()
                        .RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>();
                    var objMake = container.Resolve<IBikeMakesCacheRepository<int>>();

                    makeList = objMake.GetMakesByType(EnumBikeType.NewBikeSpecification).ToList();
                    for (int i = 0; i < makeList.Count; i++)
                    {
                        retVal1 += "<li><a id='" + makeList[i].MakeId + "' MaskingName= '" + makeList[i].MaskingName + "' onClick='ShowModel(this);' type='1'>" + makeList[i].MakeName + "</a></li>";
                        retVal2 += "<li><a id='" + makeList[i].MakeId + "' MaskingName= '" + makeList[i].MaskingName + "' onClick='ShowModel(this);' type='2'>" + makeList[i].MakeName + "</a></li>";
                    }
                }
                ddlMake1.InnerHtml = retVal1;
                ddlMake2.InnerHtml = retVal2;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

    }
}