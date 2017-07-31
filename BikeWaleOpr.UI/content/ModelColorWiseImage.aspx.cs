using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Common;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface.BikeData;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.Content
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13 Jan 2017
    /// Summary: Page to assign images to models by color
    /// Modified by : Vivek Singh Tomar on 31 July 2017
    /// Description : Add function to prefill the details and bind the data if query string passes any data
    /// </summary>
    public class ModelColorWiseImage : Page
    {
        protected Repeater rptModelColor;
        protected DropDownList cmbMake, cmbModel;
        public int modelId;
        private IBikeMakes _objMakesRepo = null;
        private IBikeModelsRepository _objModelsRepo = null;
        public IEnumerable<ModelColorImage> modelColors = null;
        public ushort modelColorCount = 0;
        ManageModelColor objManageModelColor = null;
        protected Button btnSubmit;
        protected HiddenField hdnModelId;
        protected uint makeId;
        protected bool isQueryString;
        #region events

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSubmit.Click += new EventHandler(BtnSubmit_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
            if (!IsPostBack)
            {
                BindMakeModel();
                makeId = SqlReaderConvertor.ToUInt32(Request.QueryString["makeid"]);
                if (makeId != 0)
                {
                    ShowModelColorImageUsingQueryString();
                }
            }
            if (!isQueryString && hdnModelId != null && hdnModelId.Value != null)
            {
                modelId = Convert.ToUInt16(hdnModelId.Value);
            }
        }

        void BtnSubmit_Click(object Sender, EventArgs e)
        {
            if (modelId > 0)
            {
                objManageModelColor = new ManageModelColor();
                BindModelColorRepeater();
            }
        }
        #endregion

        #region functions
        /// <summary>
        /// Created by: Sangram Nandkhile on 13 Jan 2017
        /// Binds make and model dropdown
        /// </summary>
        private void BindMakeModel()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes, BikeMakesRepository>()
                    .RegisterType<IBikeModelsRepository, BikeModelsRepository>()
                    .RegisterType<IBikeVersions, BikeVersionsRepository>();
                    _objMakesRepo = container.Resolve<IBikeMakes>();
                    _objModelsRepo = container.Resolve<IBikeModelsRepository>();
                }

                IEnumerable<BikewaleOpr.Entities.BikeData.BikeMakeEntityBase> makes = _objMakesRepo.GetMakes("NEW");
                cmbMake.DataSource = makes;
                cmbMake.DataTextField = "MakeName";
                cmbMake.DataValueField = "MakeId";
                cmbMake.DataBind();
                cmbMake.Items.Insert(0, "-- Select Make --");
                cmbMake.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ModelColorWiseImage.BindMakeModel()");
            }

        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 13 Jan 2017
        /// Binds model color and respective images
        /// </summary>
        private void BindModelColorRepeater()
        {
            try
            {
                modelColors = objManageModelColor.FetchModelImagesByColors(modelId);
                modelColorCount = (ushort)modelColors.Count();

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ModelColorWiseImage.BindModelColorRepeater()");
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 31 July 2017
        /// Bind model color images to view page if data is passed through query string
        /// </summary>
        private void ShowModelColorImageUsingQueryString()
        {
            isQueryString = true;
            modelId = SqlReaderConvertor.ToInt32(Request.QueryString["modelid"]);
            cmbMake.SelectedIndex = SqlReaderConvertor.ToInt32(cmbMake.Items.IndexOf(cmbMake.Items.FindByValue(Convert.ToString(makeId))));
            var response = Common.CommonOpn.GetModelFromMake(cmbMake.SelectedValue);
            IEnumerable<BikeModelEntityBase> models = new List<BikeModelEntityBase>();
            if (response != null && response.Tables != null && response.Tables.Count > 0)
            {
                models = response.Tables[0].AsEnumerable()
               .Select(row => new BikeModelEntityBase
               {
                   ModelId = row.Field<int>(0),
                   ModelName = row.Field<string>(1)
               });
                cmbModel.DataSource = models;
                cmbModel.DataTextField = "ModelName";
                cmbModel.DataValueField = "ModelId";
                cmbModel.DataBind();
                cmbModel.Items.Insert(0, "Any");
                cmbModel.SelectedIndex = SqlReaderConvertor.ToInt32(cmbModel.Items.IndexOf(cmbModel.Items.FindByValue(Convert.ToString(modelId))));
                if (modelId > 0)
                {
                    objManageModelColor = new ManageModelColor();
                    BindModelColorRepeater();
                }
            }
        }

        #endregion
    }//Class
}// Namespace