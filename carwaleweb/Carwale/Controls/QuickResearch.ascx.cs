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
using Newtonsoft.Json;
using Microsoft.Practices.Unity;
using Carwale.Interfaces;
using Carwale.DAL.CarData;
using Carwale.Interfaces.CarData;
using Carwale.Cache.CarData;
using AEPLCore.Cache;
using Carwale.Entity.CarData;
using System.Collections.Generic;
using Carwale.BL.CarData;
using Carwale.Notifications;
using Carwale.UI.Common;
using CarwaleAjax;
using AEPLCore.Cache.Interfaces;

namespace Carwale.UI.Controls
{
    public class QuickResearch : UserControl
    {
        protected DropDownList drpMake, drpModel, drpVersion;
        protected HtmlGenericControl ulQuickResearch;
        protected HtmlInputHidden hdn_selModel, hdn_selVersion;
        protected HtmlInputHidden hdn_drpModel, hdn_drpVersion;
        protected string drpVersion_Id = "", drpModel_Id = "", drpModel_Name = "", drpVersion_Name = "", drpMake_Id = "", drpModelJson = string.Empty, drpVersionJson = string.Empty, drpMakeJson = string.Empty;
        protected string hdn_drpModel_Id = "", hdn_drpVersion_Id = "", hdn_selModel_Id = "", hdn_selVersion_Id = "";
        protected bool _VerticalDisplay = true;

        private string _MakeId = "-1", _ModelId = "-1";
        private DataSet _MakeContents = null;
        ///
        public bool VerticalDisplay
        {
            get
            {
                return _VerticalDisplay;
            }
            set
            {
                _VerticalDisplay = value;
            }
        }

        public string MakeId
        {
            get
            {
                return _MakeId;
            }
            set
            {
                _MakeId = value;
            }
        }

        public string ModelId
        {
            get
            {
                return _ModelId;
            }
            set
            {
                _ModelId = value;
            }
        }

        public string ModelContents
        {
            get
            {
                if (hdn_drpModel != null)
                    return hdn_drpModel.Value;
                else
                    return "";
            }
        }

        public string VersionContents
        {
            get
            {
                if (hdn_drpVersion != null)
                    return hdn_drpVersion.Value;
                else
                    return "";
            }
        }

        public string SelectedVersion
        {
            get
            {
                if (Request.Form[drpVersion_Name] != null && Request.Form[drpVersion_Name].ToString() != "")
                    return Request.Form[drpVersion_Name].ToString();
                else
                    return "-1";
            }
        }

        public string SelectedModel
        {
            get
            {
                if (Request.Form[drpModel_Name] != null && Request.Form[drpModel_Name].ToString() != "")
                    return Request.Form[drpModel_Name].ToString();
                else
                    return "-1";
            }
        }

        public DataSet MakeContents
        {
            get
            {
                return _MakeContents;
            }
            set
            {
                _MakeContents = value;
            }
        }
        public List<CarMakeEntityBase> CarMakesList { get; set; }
        public List<CarModelEntityBase> CarModelsList { get; set; }
        public List<CarVersionEntity> CarVersionsList { get; set; }
        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        void Page_Load(object sender, EventArgs e)
        {
            drpMake_Id = drpMake.ClientID.ToString();
            drpModel_Id = drpModel.ClientID.ToString();
            drpVersion_Id = drpVersion.ClientID.ToString();
            hdn_drpModel_Id = hdn_drpModel.ClientID.ToString();
            hdn_drpVersion_Id = hdn_drpVersion.ClientID.ToString();
            hdn_selModel_Id = hdn_selModel.ClientID.ToString();
            hdn_selVersion_Id = hdn_selVersion.ClientID.ToString();

            drpModel_Name = drpModel.ClientID.ToString().Replace("_", "$");
            drpVersion_Name = drpVersion.ClientID.ToString().Replace("_", "$");

            if (VerticalDisplay == false)
            {
                ulQuickResearch.Attributes.Add("class", "qr-hor");
            }
            if (!IsPostBack)
            {
                LoadLists();
            }
            else
            {
                //in case of post back update contents of the city and the area drop down
                AjaxFunctions aj = new AjaxFunctions();

                //update the contents for model
                aj.UpdateContents(drpModel, ModelContents, SelectedModel);

                //update the contents for version
                aj.UpdateContents(drpVersion, VersionContents, SelectedVersion);
            }
        } // Page_Load

        /// <summary>
        /// Written By : Rohan S.
        /// Summary :Binds Make Drop List
        ///         also binds Model drop list when at Make page(i.e Make is already selected),
        ///         also binds Versions drop list when at model page(i.e Model is already selected)
        /// </summary>
        private void LoadLists()
        {
            try
            {
                int makeId = 0, modelId = 0;

                // Validate
                int.TryParse(MakeId, out makeId);
                int.TryParse(ModelId, out modelId);

                if (CarMakesList != null)  // for model details page
                {
                    drpMakeJson = JsonConvert.SerializeObject(CarMakesList);
                }
                else
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<ICacheManager, CacheManager>()
                                  .RegisterType<ICarMakesCacheRepository, CarMakesCacheRepository>()
                                  .RegisterType<ICarMakesRepository, CarMakesRepository>();

                        ICarMakesCacheRepository carMakes = container.Resolve<ICarMakesCacheRepository>();
                        drpMakeJson = JsonConvert.SerializeObject(carMakes.GetCarMakesByType("new"));
                    }
                }
                if (makeId > 0)
                {
                    if (CarModelsList != null)// for model details page
                    {
                        drpModelJson = JsonConvert.SerializeObject(CarModelsList);
                    }
                    else
                    {
                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<ICarModelRepository, CarModelsRepository>()
                                     .RegisterType<ICacheManager, CacheManager>()
                                     .RegisterType<ICarModelCacheRepository, CarModelsCacheRepository>();

                            ICarModelCacheRepository carModelsCache = container.Resolve<ICarModelCacheRepository>();
                            drpModelJson = JsonConvert.SerializeObject(carModelsCache.GetCarModelsByType("new", makeId));
                        }
                    }
                }

                if (modelId > 0)
                {
                    if (CarVersionsList != null)// for model details page
                    {
                        drpVersionJson = JsonConvert.SerializeObject(CarVersionsList);
                    }
                    else
                    {
                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<ICarVersionRepository, CarVersionsRepository>()
                            .RegisterType<ICacheManager, CacheManager>()
                                .RegisterType<ICarVersionCacheRepository, CarVersionsCacheRepository>();

                            ICarVersionCacheRepository verl = container.Resolve<ICarVersionCacheRepository>();
                            List<CarVersionEntity> carVersionsList = (List<CarVersionEntity>)verl.GetCarVersionsByType("new", modelId);

                            drpVersionJson = JsonConvert.SerializeObject(carVersionsList);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   // End of LoadLists

    }   // class
}   // namespace