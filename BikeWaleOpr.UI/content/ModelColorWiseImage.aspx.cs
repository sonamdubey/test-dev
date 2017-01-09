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
    public class ModelColorWiseImage : Page
    {
        protected Repeater rptModelColor;
        protected DropDownList cmbMake, cmbModel;
        public int modelId;
        private IBikeMakes _objMakesRepo = null;
        private IBikeModels _objModelsRepo = null;
        public IEnumerable<ModelColorImage> modelColors = null;
        public ushort modelColorCount = 0;
        ManageModelColor objManageModelColor = null;
        protected Button btnSubmit;
        protected HiddenField hdnModelId;

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
            }
            if (hdnModelId != null && hdnModelId.Value != null)
                modelId = Convert.ToUInt16(hdnModelId.Value);
        }

        private void BindMakeModel()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeMakes, BikeMakesRepository>()
                .RegisterType<IBikeModels, BikeModelsRepository>()
                .RegisterType<IBikeVersions, BikeVersionsRepository>();
                _objMakesRepo = container.Resolve<IBikeMakes>();
                _objModelsRepo = container.Resolve<IBikeModels>();
            }

            IEnumerable<BikewaleOpr.Entities.BikeData.BikeMakeEntityBase> makes = _objMakesRepo.GetMakes("NEW");
            cmbMake.DataSource = makes;
            cmbMake.DataTextField = "MakeName";
            cmbMake.DataValueField = "MakeId";
            cmbMake.DataBind();
        }

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

        void BtnSubmit_Click(object Sender, EventArgs e)
        {
            if (modelId > 0)
            {
                objManageModelColor = new ManageModelColor();
                BindModelColorRepeater();
            }

        }
    }//Class
}// Namespace