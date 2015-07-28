﻿using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Linq;
using Bikewale.Common;
using Bikewale.Memcache;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.BikeData;
using Bikewale.BAL.BikeData;

namespace Bikewale.Controls
{   
    /// <summary>
    /// Created By : Sadhana Upadhyay on 13th March 2014
    /// Summary : Class for add bike to comparison 
    /// </summary>
    public class AddBikeToCompare : UserControl
    {
        protected HtmlSelect drpMake, drpModel, drpVersion;
        public string versionId;
        public string VersionId
        {
            get
            {
                return versionId;
            }
            set
            {
                versionId = value;
            }
        }
		//Added By Sadhana Upadhyay on 27 Aug 2014
        protected bool _isFeatured = false;
        public bool IsFeatured
        {
            get { return _isFeatured; }
            set { _isFeatured = value; }
        }

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
            FillMake();
            Trace.Warn("sadhana",versionId);
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 13th March 2014
        /// Summary : Function to fill make dropdown
        /// Modified By : Sadhana Upadhyay on 29 Sept 2014
        /// Summary : fill make dropdown with makes whose specification for version are available.
        /// </summary>
        protected void FillMake()
        {
            try
            {
                List<BikeMakeEntityBase> makeList = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakes<BikeMakeEntity, int>>();
                    IBikeMakes<BikeMakeEntity, int> objMake = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();

                    makeList = objMake.GetMakesByType(EnumBikeType.NewBikeSpecification);

                    var makeListNew = makeList.Select(a => new { Value = a.MakeId + "_" + a.MaskingName, Text = a.MakeName });

                    drpMake.DataSource = makeListNew;
                    drpMake.DataValueField = "Value";
                    drpMake.DataTextField = "Text";
                    drpMake.DataBind();

                    ListItem items = new ListItem("--Select Make--", "0");
                    drpMake.Items.Insert(0, items);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}