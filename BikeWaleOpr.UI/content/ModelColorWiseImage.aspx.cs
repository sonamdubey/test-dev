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
        public uint modelId;
        private IBikeMakesRepository _objMakesRepo = null;
        public IEnumerable<ModelColorImage> modelColors = null;
        public ushort modelColorCount = 0;
        ManageModelColor objManageModelColor = null;
        protected Button btnSubmit;
        protected HiddenField hdnModelId;
        protected uint makeId;
        protected IBikeMakes _bikeMakes = null;
        #region events

        /// <summary>
        /// Created by : Vivek Singh Tomar on 3rd Aug 2017
        /// Summary : Initialize dependencies
        /// </summary>
        public ModelColorWiseImage()
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
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "ModelColorWiseImage.ModelColorWiseImage()");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSubmit.Click += new EventHandler(BtnSubmit_Click);
        }

        /// <summary>
        /// Modified by : Vivek Singh Tomar on 3rd Aug 2107
        /// Summary : Made changes to bind data if request is made using query string on first hit only
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        void Page_Load(object Sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
            if (!IsPostBack)
            {
                BindMakeModel();
                BindModelAndImages();
            }
            else if(hdnModelId != null && hdnModelId.Value != null)
            {
                modelId = Convert.ToUInt32(hdnModelId.Value);
            }
        }

        void BtnSubmit_Click(object Sender, EventArgs e)
        {
            if (modelId > 0)
            {
                BindModelColorImages();
                makeId = Convert.ToUInt32(cmbMake.SelectedValue);
                BindModelDropdown(makeId, modelId);
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
        /// Summary : Binds model color and respective images
        /// Modified by : Vivek Singh Tomar on 3rd Aug 2017
        /// Summary : Changed the function name
        /// </summary>
        private void BindModelColorImages()
        {
            try
            {
                objManageModelColor = new ManageModelColor();
                modelColors = objManageModelColor.FetchModelImagesByColors(modelId);
                modelColorCount = (ushort)modelColors.Count();

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ModelColorWiseImage.BindModelColorImagesRepeater()");
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 31st July 2017
        /// Summary : Bind model dropdown and images using querystring
        /// </summary>
        private void BindModelAndImages()
        {
            try
            {
                ProcessQueryString();
                if(modelId > 0 && makeId > 0)
                {
                    BindModelDropdown(makeId, modelId);
                    BindModelColorImages();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ModelColorWiseImage.ShowModelColorImage");
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 3rd Aug 2017
        /// Summary : parse query string to get model and make IDs
        /// </summary>
        private void ProcessQueryString()
        {
            try
            {
                UInt32.TryParse(Request.QueryString["makeid"], out makeId);
                UInt32.TryParse(Request.QueryString["modelid"], out modelId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ModelColorWiseImage.ProcessQueryString");
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 1st July 2017
        /// Summary : function to bind model dropdown
        /// </summary>
        private void BindModelDropdown(uint ddlMakeId, uint ddlModelId)
        {
            try
            {
                cmbMake.SelectedValue = Convert.ToString(ddlMakeId);
                var models = _bikeMakes.GetModelsByMake(ddlMakeId);
                if (models != null && models.Count() > 0)
                {
                    cmbModel.DataSource = models;
                    cmbModel.DataTextField = "ModelName";
                    cmbModel.DataValueField = "ModelId";
                    cmbModel.DataBind();
                    cmbModel.Items.Insert(0, "--Select Model--");
                    cmbModel.SelectedValue = Convert.ToString(ddlModelId);
                }

                if (hdnModelId != null && hdnModelId.Value != null)
                {
                    hdnModelId.Value = Convert.ToString(ddlModelId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ModelColorWiseImage.BindModelDropdown");
            }
        }

        #endregion
    }//Class
}// Namespace