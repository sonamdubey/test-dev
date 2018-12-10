using System;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Carwale.Service;
using Carwale.Interfaces.Geolocation;
using System.Collections.Generic;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces;
using Carwale.Entity.CarData;
using Carwale.Interfaces.CarData;
using System.Web.UI.HtmlControls;
using Carwale.BL.Classified.CarValuation;
using System.Web.Mvc;

namespace Carwale.UI.Used.Valuation
{
    public class Valuation : ViewPage
    {
        protected DropDownList cmbMake, cmbModel, cmbVersion, cmbState, cmbCity, cmbYear, cmbValuationCity, cmbMonth, ddlOwners;
        protected HtmlTable tblStateCity;
        protected TextBox txtKms;
        protected bool isQSAvailable;
        protected int year, car, city, owner, kms, month;

        private IGeoCitiesCacheRepository _geoCitiesCacheRepo;
        private ICarMakesCacheRepository _carMakesCacheRepo;
        private ICarModelCacheRepository _carModelsCacheRepo;
        private ICarVersionCacheRepository _carVersionsCacheRepo;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _geoCitiesCacheRepo = container.Resolve<IGeoCitiesCacheRepository>();
                _carMakesCacheRepo = container.Resolve<ICarMakesCacheRepository>();
                _carModelsCacheRepo = container.Resolve<ICarModelCacheRepository>();
                _carVersionsCacheRepo = container.Resolve<ICarVersionCacheRepository>();
            }
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (!IsPostBack)
            {   
                FillOwners();
                FillYears();               
                FillPopularCities();

                cmbMake.Enabled = false;
                cmbModel.Enabled = false;
                cmbVersion.Enabled = false;
                int paramCount = 0;
                if (Int32.TryParse(Request["year"], out year) && year >=1998 && year <= DateTime.Now.Year)
                {
                    cmbMake.Enabled = true;
                    FillMakes();
                    cmbYear.SelectedIndex = cmbYear.Items.IndexOf(cmbYear.Items.FindByValue(year.ToString()));
                    if (cmbYear.SelectedIndex > 0)
                    {
                        paramCount++;
                    }
                }

                if (Int32.TryParse(Request["car"], out car) && car > 0)
                {
                    if (PopulateCarDetails(car))
                    {
                        paramCount++;
                    }
                }

                if (Int32.TryParse(Request["city"], out city) && city > 0)
                {
                    cmbValuationCity.SelectedIndex = cmbValuationCity.Items.IndexOf(cmbValuationCity.Items.FindByValue(city.ToString()));
                    if (cmbValuationCity.SelectedIndex > 0 || PopulateCityDetails(city))
                    {
                        paramCount++;
                    }
                }
                if (Int32.TryParse(Request["owner"], out owner) && owner > 0)
                {
                    ddlOwners.SelectedIndex = ddlOwners.Items.IndexOf(ddlOwners.Items.FindByValue(owner.ToString()));
                    if (ddlOwners.SelectedIndex > 0)
                    {
                        paramCount++;
                    }
                }
                if (Int32.TryParse(Request["month"], out month) && month > 0)
                {
                    cmbMonth.SelectedIndex = cmbMonth.Items.IndexOf(cmbMonth.Items.FindByValue(month.ToString()));
                    if (cmbMonth.SelectedIndex > 0)
                    {
                        paramCount++;
                    }
                }
                if (Int32.TryParse(Request["kms"], out kms) && kms > 0)
                {
                    txtKms.Text = kms.ToString();
                    paramCount++;
                }

                isQSAvailable = paramCount == 6;
            }
        }

        private bool PopulateCityDetails(int cityId)
        {
            StateAndAllCities stateAndAllCities = _geoCitiesCacheRepo.GetStateAndAllCities(cityId);
            if (stateAndAllCities != null && stateAndAllCities.State != null && stateAndAllCities.State.StateId > 0)
            {
                FillStates();
                cmbState.SelectedIndex = cmbState.Items.IndexOf(cmbState.Items.FindByValue(stateAndAllCities.State.StateId.ToString()));
                FillCities(stateAndAllCities.Cities);
                cmbCity.Enabled = true;
                cmbCity.SelectedIndex = cmbCity.Items.IndexOf(cmbCity.Items.FindByValue(cityId.ToString()));
                if (cmbCity.SelectedIndex > 0)
                {
                    tblStateCity.Style["display"] = "block";
                    cmbValuationCity.SelectedIndex = cmbValuationCity.Items.IndexOf(cmbValuationCity.Items.FindByValue("-1"));
                    return true;
                }
            }
            return false;
        }

        private bool PopulateCarDetails(int version)
        {
            CarVersionDetails versionDetails = _carVersionsCacheRepo.GetVersionDetailsById(version);
            if (versionDetails != null)
            {
                cmbMake.SelectedIndex = cmbMake.Items.IndexOf(cmbMake.Items.FindByValue(versionDetails.MakeId.ToString()));
                if (cmbMake.SelectedIndex > 0)
                {
                    FillModels(versionDetails.MakeId);
                    cmbModel.Enabled = true;
                    cmbModel.SelectedIndex = cmbModel.Items.IndexOf(cmbModel.Items.FindByValue(versionDetails.ModelId.ToString()));
                    if (cmbModel.SelectedIndex > 0)
                    {
                        cmbVersion.Enabled = true;
                        FillVersions(versionDetails.ModelId);
                        cmbVersion.SelectedIndex = cmbVersion.Items.IndexOf(cmbVersion.Items.FindByValue(version.ToString()));
                        if (cmbVersion.SelectedIndex > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void FillYears()
        {
            for (int i = DateTime.Today.Year; i >= 1998; i--)
            {
                cmbYear.Items.Insert(DateTime.Today.Year - i + 1, new ListItem(i.ToString(), i.ToString()));
            }
        }

        private void FillMakes()
        {
            cmbMake.DataSource = _carMakesCacheRepo.GetCarMakesByType("USED");
            cmbMake.DataTextField = "MakeName";
            cmbMake.DataValueField = "MakeId";
            cmbMake.DataBind();
            cmbMake.Items.Insert(0, new ListItem("--Select Make--", "0"));
        }

        private void FillModels(int makeId)
        {
            cmbModel.DataSource = _carModelsCacheRepo.GetCarModelsByType("USED", makeId);
            cmbModel.DataTextField = "ModelName";
            cmbModel.DataValueField = "ModelId";
            cmbModel.DataBind();
            cmbModel.Items.Insert(0, new ListItem("--Select Model--", "0"));
        }

        private void FillVersions(int modelId)
        {
            cmbVersion.DataSource = _carVersionsCacheRepo.GetCarVersionsByType("USED", modelId);
            cmbVersion.DataTextField = "Name";
            cmbVersion.DataValueField = "ID";
            cmbVersion.DataBind();
            cmbVersion.Items.Insert(0, new ListItem("--Select Version--", "0"));
        }

        private void FillPopularCities()
        {
            cmbValuationCity.DataSource = _geoCitiesCacheRepo.GetClassifiedPopularCities();
            cmbValuationCity.DataTextField = "CityName";
            cmbValuationCity.DataValueField = "CityId";
            cmbValuationCity.DataBind();
            cmbValuationCity.Items.Insert(0, new ListItem("--Select City--", "0"));
            cmbValuationCity.Items.Insert(cmbValuationCity.Items.Count, new ListItem("Other", "-1"));
        }

        private void FillStates()
        {
            cmbState.DataSource = _geoCitiesCacheRepo.GetStates();
            cmbState.DataTextField = "StateName";
            cmbState.DataValueField = "StateId";
            cmbState.DataBind();
            cmbState.Items.Insert(0, new ListItem("--Select State--", "0"));
        }

        private void FillCities(List<City> cities)
        {
            cmbCity.DataSource = cities;
            cmbCity.DataTextField = "CityName";
            cmbCity.DataValueField = "CityId";
            cmbCity.DataBind();
            cmbCity.Items.Insert(0, new ListItem("--Select City--", "0"));
        }
        private void FillOwners()
        {
            ddlOwners.DataSource = ValuationBL.GetOwnerOptions();
            ddlOwners.DataTextField = "Value";
            ddlOwners.DataValueField = "Key";
            ddlOwners.DataBind();
            ddlOwners.Items.Insert(0, new ListItem("--Select Owners--", "0"));
        }
    }
}