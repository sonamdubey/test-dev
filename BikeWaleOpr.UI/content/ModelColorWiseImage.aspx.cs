using Bikewale.Notifications;
using BikewaleOpr.Common;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface.BikeData;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
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
        private IBikeMakesRepository _objMakesRepo = null;
        public IEnumerable<ModelColorImage> modelColors = null;
        public ushort modelColorCount = 0;
        ManageModelColor objManageModelColor = null;
        protected Button btnSubmit;
        protected HiddenField hdnModelId;
        protected uint makeId;
        protected bool isQueryString;
        protected IBikeMakes _bikeMakes = null;
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
                UInt32.TryParse(Request.QueryString["makeid"], out makeId);
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
                    container.RegisterType<IBikeMakesRepository, BikeMakesRepository>()
                    .RegisterType<IBikeMakes, BikewaleOpr.BAL.BikeMakes>();
                    _objMakesRepo = container.Resolve<IBikeMakesRepository>();
                    _bikeMakes = container.Resolve<IBikeMakes>();
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
            Int32.TryParse(Request.QueryString["modelid"], out modelId);
            if(modelId > 0)
            {
                cmbMake.SelectedIndex = Convert.ToInt32(cmbMake.Items.IndexOf(cmbMake.Items.FindByValue(Convert.ToString(makeId))));
                var models = _bikeMakes.GetModelsByMake(makeId);
                if(models != null && models.Count() > 0)
                {
                    cmbModel.DataSource = models;
                    cmbModel.DataTextField = "ModelName";
                    cmbModel.DataValueField = "ModelId";
                    cmbModel.DataBind();
                    cmbModel.Items.Insert(0, "Any");
                    cmbModel.SelectedIndex = Convert.ToInt32(cmbModel.Items.IndexOf(cmbModel.Items.FindByValue(Convert.ToString(modelId))));
                    objManageModelColor = new ManageModelColor();
                    BindModelColorRepeater();
                }

                if (hdnModelId != null && hdnModelId.Value != null)
                {
                    hdnModelId.Value = Convert.ToString(modelId);
                }
            }
        }

        #endregion
    }//Class
}// Namespace