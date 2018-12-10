using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobileWeb.Common;
using Microsoft.Practices.Unity;
using Carwale.Service;
using System.Collections.Generic;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces.Geolocation;
using Carwale.Entity.Classified.CarValuation;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Classified.CarValuation;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Entity.CarData;
using System.Web.UI.HtmlControls;
using Carwale.BL.Classified.CarValuation;

namespace MobileWeb.Used
{
	public class ValuationRequest : Page
	{
		protected DropDownList ddlMake, ddlModel, ddlVersion, ddlOtherCity, ddlYear, ddlCity, ddlState, ddlValType, ddlOwners;
        protected HtmlGenericControl divOtherCity, divVersion, divModel;
        protected TextBox txtKms;
        protected bool isQSAvailable;
        protected int year, car, city, owner, kms;

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
			if (!Page.IsPostBack)
			{
				ddlCity.Attributes.Add("onchange", "javascript:CityChanged();");
                ddlYear.Attributes.Add("onchange", "javascript:YearChanged();");
                ddlMake.Attributes.Add("onchange", "javascript:MakeChanged();");
				ddlModel.Attributes.Add("onchange", "javascript:ModelChanged();");
				ddlState.Attributes.Add("onchange", "javascript:StateChanged();");
                FillYears();
                FillPopularCities();
                FillOwners();

                int paramCount = 0;
                if (Int32.TryParse(Request["year"], out year) && year >= 1998 && year <= DateTime.Now.Year)
                {
                    FillMakes();
                    ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByValue(year.ToString()));
                    if (ddlYear.SelectedIndex > 0)
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

                if ( Int32.TryParse(Request["city"], out city) && city > 0)
                {
                    ddlCity.SelectedIndex = ddlCity.Items.IndexOf(ddlCity.Items.FindByValue(city.ToString()));
                    if (ddlCity.SelectedIndex > 0 || PopulateCityDetails(city))
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
                if (Int32.TryParse(Request["kms"], out kms) && kms > 0) 
                {
                    txtKms.Text = kms.ToString();
                    paramCount++;
                }

                isQSAvailable = paramCount == 5;
            }
		}

        private void FillOwners()
        {
            ddlOwners.DataSource = ValuationBL.GetOwnerOptions();
            ddlOwners.DataTextField = "Value";
            ddlOwners.DataValueField = "Key";
            ddlOwners.DataBind();
            ddlOwners.Items.Insert(0, new ListItem("--Select Owners--", "0"));
        }

        private bool PopulateCityDetails(int cityId)
        {
            StateAndAllCities stateAndAllCities = _geoCitiesCacheRepo.GetStateAndAllCities(cityId);
            if (stateAndAllCities != null && stateAndAllCities.State != null && stateAndAllCities.State.StateId > 0)
            {
                FillStates();
                ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(stateAndAllCities.State.StateId.ToString()));
                FillCities(stateAndAllCities.Cities);
                ddlOtherCity.SelectedIndex = ddlOtherCity.Items.IndexOf(ddlOtherCity.Items.FindByValue(cityId.ToString()));
                if (ddlOtherCity.SelectedIndex > 0)
                {
                    divOtherCity.Style["display"] = "block";
                    ddlCity.SelectedIndex = ddlCity.Items.IndexOf(ddlCity.Items.FindByValue("-1"));
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
                ddlMake.SelectedIndex = ddlMake.Items.IndexOf(ddlMake.Items.FindByValue(versionDetails.MakeId.ToString()));
                if (ddlMake.SelectedIndex > 0)
                {
                    FillModels(versionDetails.MakeId);
                    divModel.Style["display"] = "block";
                    ddlModel.SelectedIndex = ddlModel.Items.IndexOf(ddlModel.Items.FindByValue(versionDetails.ModelId.ToString()));
                    if (ddlModel.SelectedIndex > 0)
                    {
                        divVersion.Style["display"] = "block";
                        FillVersions(versionDetails.ModelId);
                        ddlVersion.SelectedIndex = ddlVersion.Items.IndexOf(ddlVersion.Items.FindByValue(version.ToString()));
                        if (ddlVersion.SelectedIndex > 0)
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
                ddlYear.Items.Insert(DateTime.Today.Year - i + 1, new ListItem(i.ToString(), i.ToString()));
            }
        }

        private void FillMakes()
        {
            ddlMake.DataSource = _carMakesCacheRepo.GetCarMakesByType("USED");
            ddlMake.DataTextField = "MakeName";
            ddlMake.DataValueField = "MakeId";
            ddlMake.DataBind();
            ddlMake.Items.Insert(0, new ListItem("--Select Make--", "0"));
        }

        private void FillModels(int makeId)
        {
            ddlModel.DataSource = _carModelsCacheRepo.GetCarModelsByType("USED", makeId);
            ddlModel.DataTextField = "ModelName";
            ddlModel.DataValueField = "ModelId";
            ddlModel.DataBind();
            ddlModel.Items.Insert(0, new ListItem("--Select Model--", "0"));
        }

        private void FillVersions(int modelId)
        {
            ddlVersion.DataSource = _carVersionsCacheRepo.GetCarVersionsByType("USED", modelId);
            ddlVersion.DataTextField = "Name";
            ddlVersion.DataValueField = "ID";
            ddlVersion.DataBind();
            ddlVersion.Items.Insert(0, new ListItem("--Select Version--", "0"));
        }

        private void FillPopularCities()
        {
            ddlCity.DataSource = _geoCitiesCacheRepo.GetClassifiedPopularCities();
            ddlCity.DataTextField = "CityName";
            ddlCity.DataValueField = "CityId";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
            ddlCity.Items.Insert(ddlCity.Items.Count, new ListItem("Other", "-1"));
        }

        private void FillStates()
        {
            ddlState.DataSource = _geoCitiesCacheRepo.GetStates();
            ddlState.DataTextField = "StateName";
            ddlState.DataValueField = "StateId";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("--Select State--", "0"));
        }

        private void FillCities(List<City> cities)
        {
            ddlOtherCity.DataSource = cities;
            ddlOtherCity.DataTextField = "CityName";
            ddlOtherCity.DataValueField = "CityId";
            ddlOtherCity.DataBind();
            ddlOtherCity.Items.Insert(0, new ListItem("--Select City--", "0"));
        }
	}
}		