using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Controls;
using Carwale.UI.Common;
using Microsoft.Practices.Unity;
using Carwale.Service;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Interfaces.CarData;
using Carwale.Entity.CarData;
using System.Collections.Generic;
using Carwale.Entity.Classified;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Classified.MyListings;

namespace Carwale.UI.MyCarwale.MyInquiries
{
    public class EditSellCar : Page
    {
        protected HtmlGenericControl spnError, alertObj;
        protected DropDownList cmbVersion;
        protected Button btnSave;
        protected DateControl calMakeYear;
        protected TextBox txtRegistrationNo, txtKilometers, txtPrice, txtColor, txtIColor, txtComments;
        protected Label lblMake, lblModel;
        protected DateControl calInsuranceExpiry;

        // *** Details Declarations *** 
        protected TextBox txtRegistrationPlace, txtInteriorColor, txtMileage, txtWarranties;
        protected DropDownList drpOwners, drpOneTimeTax, drpInsurance, drpAdditionalFuel,drpCarRegistrationType;

        public int ModelId, VersionId;
        public string inquiryId = "";
       
        private ISellCarRepository _sellCarRepo;
        private ICarVersionCacheRepository _carVersionCacheRepo;
        private IMyListingsRepository _mylistingsRepo;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _sellCarRepo = container.Resolve<ISellCarRepository>();
                _carVersionCacheRepo = container.Resolve<ICarVersionCacheRepository>();
                _mylistingsRepo = container.Resolve<IMyListingsRepository>();
            }
            base.Load += new EventHandler(Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
        }

        public string SelectedModel
        {
            get
            {
                if (Request.Form["drpModel"] != null)
                    return Request.Form["drpModel"].ToString();
                else
                    return "-1";
            }
        }

        public string ModelContents
        {
            get
            {
                if (Request.Form["hdn_drpModel"] != null)
                    return Request.Form["hdn_drpModel"].ToString();
                else
                    return "";
            }
        }

        public string SelectedVersion
        {
            get
            {
                if (Request.Form["drpVersion"] != null)
                    return Request.Form["drpVersion"].ToString();
                else
                    return "-1";
            }
        }

        public string VersionContents
        {
            get
            {
                if (Request.Form["hdn_drpVersion"] != null)
                    return Request.Form["hdn_drpVersion"].ToString();
                else
                    return "";
            }
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (CurrentUser.Id == "-1")
                Response.Redirect("/Users/login.aspx?returnUrl=/MyCarwale/MyInquiries/EditSellCar.aspx");

            if (!(Request["id"] != null && CommonOpn.CheckId(Request["id"])))
            {
                Response.Redirect("/mycarwale/");
            }
            else
            {
                inquiryId = Request["id"].ToString();
            }

            if (!_sellCarRepo.IsCustomerAuthorizedToManageCar(Convert.ToInt32(CurrentUser.Id), Convert.ToInt32(inquiryId)))
            {
                Response.Redirect("/mycarwale/", true);
            }
            int id = 0;
            if (Int32.TryParse(inquiryId,out id) &&  !_mylistingsRepo.IsCarCustomerEditable(id)) // check if inquiry id is editable i.e inquiry is preimum
            {
                Response.Redirect("/mycarwale/myinquiries/mysellinquiry.aspx?ispremium=true", true);
            }

            if (!IsPostBack)
            {
                if (_sellCarRepo.GetSellCarStepsCompleted(Convert.ToInt32(inquiryId)) == (int)Carwale.Entity.Classified.SellCarUsed.SellCarSteps.Confirmation)
                {
                    FillExisting(Convert.ToInt32(inquiryId));
                }
                else
                {
                    Response.Redirect("/mycarwale/myinquiries/mysellinquiry.aspx", true);
                }
            }
        }

        void FillExisting(int inquiryId)
        {
            CustomerSellInquiryVehicleData vehicleData = _sellCarRepo.GetCustomerSellInquiryVehicleDetails(inquiryId);
            SellInquiriesOtherDetails otherDetails = _sellCarRepo.GetSellCarOtherDetails(inquiryId);
            SellCarConditions carConditions = _sellCarRepo.GetSellCarCondition(inquiryId);

            if (vehicleData != null && otherDetails != null && carConditions != null)
            {
                ModelId = vehicleData.ModelId;
                VersionId = vehicleData.VersionId;
                CityId = vehicleData.CityId.ToString();
                PinCode = vehicleData.PinCode.ToString();
                FillVersions(ModelId, VersionId);

                lblMake.Text = vehicleData.MakeName;
                lblModel.Text = vehicleData.ModelName;
                txtRegistrationNo.Text = otherDetails.RegistrationNumber;
                txtKilometers.Text = vehicleData.Kilometers.ToString();
                txtPrice.Text = vehicleData.Price.ToString();
                txtColor.Text = otherDetails.Color;
                txtComments.Text = otherDetails.Comments;
                txtRegistrationPlace.Text = otherDetails.RegistrationPlace;
                txtIColor.Text = carConditions.InteriorColor;
                txtMileage.Text = carConditions.CityMileage;
                txtWarranties.Text = carConditions.Warranties;

                calMakeYear.Value = vehicleData.MakeYear != null ? vehicleData.MakeYear : DateTime.Today.AddMonths(-3);
                calInsuranceExpiry.Value = otherDetails.InsuranceExpiry != null ? otherDetails.InsuranceExpiry : DateTime.Today;
                FillExistingDrpOwners(otherDetails.Owners);
                drpOneTimeTax.SelectedIndex = drpOneTimeTax.Items.IndexOf(drpOneTimeTax.Items.FindByValue(otherDetails.OneTimeTax));
                drpInsurance.SelectedIndex = drpInsurance.Items.IndexOf(drpInsurance.Items.FindByValue(otherDetails.Insurance));
                drpAdditionalFuel.SelectedIndex = drpAdditionalFuel.Items.IndexOf(drpAdditionalFuel.Items.FindByText(carConditions.AdditionalFuel));
                drpCarRegistrationType.SelectedIndex = drpCarRegistrationType.Items.IndexOf(drpCarRegistrationType.Items.FindByText(otherDetails.RegType.ToString()));
            }
        }

        void FillVersions(int modelId, int versionId)
        {
            List<CarVersionEntity> carVersionsList = _carVersionCacheRepo.GetCarVersionsByType("Used", modelId);
            cmbVersion.DataSource = carVersionsList;
            cmbVersion.DataTextField = "Name";
            cmbVersion.DataValueField = "ID";
            cmbVersion.DataBind();
            cmbVersion.Items.Insert(0, new ListItem("--Select Version--", "0"));

            int index = cmbVersion.Items.IndexOf(cmbVersion.Items.FindByValue(versionId.ToString()));
            cmbVersion.SelectedIndex = index == -1 ? 1 : index;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            bool isDone = false;
            int inqId;
            int owners = -1;
            Int32.TryParse(drpOwners.SelectedValue, out owners);
            inqId = _sellCarRepo.SaveSellCarBasicInfo(new SellCarBasicInfo
            {
                InquiryId = Convert.ToInt32(inquiryId),
                CustomerId = Convert.ToInt32(CurrentUser.Id),
                MakeYear = calMakeYear.Value,
                Kms = Convert.ToInt32(txtKilometers.Text.Trim()),
                Price = Convert.ToInt32(txtPrice.Text.Trim()),
                CarVersionId = Convert.ToInt32(cmbVersion.SelectedItem.Value),
                CityId = Convert.ToInt32(CityId),
                PinCode = PinCode != "" ? Convert.ToInt32(PinCode) : (int?)null,
                Owners = owners
            });
            if (inqId > 0)
            {
                isDone = _sellCarRepo.SaveSellCarOtherDetails(new SellInquiriesOtherDetails
                {
                    Id = Convert.ToInt32(inquiryId),
                    RegistrationNumber = txtRegistrationNo.Text.Trim(),
                    Comments = txtComments.Text.Trim().Length > 1000 ? txtComments.Text.Trim().Substring(0, 999) : txtComments.Text.Trim(),
                    Owners = Convert.ToInt32(drpOwners.SelectedItem.Value),
                    RegistrationPlace = txtRegistrationPlace.Text.Trim(),
                    Insurance = drpInsurance.SelectedItem.Value,
                    InsuranceExpiry = drpInsurance.SelectedItem.Value != "N/A" ? calInsuranceExpiry.Value : DateTime.MinValue,
                    OneTimeTax = drpOneTimeTax.SelectedItem.Value,
                    Color = CarColor,
                    ColorCode = CarColorCode,
                    RegType = (CarRegistrationType)Convert.ToInt32(drpCarRegistrationType.SelectedValue)
                });
            }
            if (isDone)
            {
                isDone = _sellCarRepo.SaveSellCarCondition(new SellCarConditions
                {
                    InquiryId = Convert.ToInt32(inquiryId),
                    AdditionalFuel = drpAdditionalFuel.SelectedIndex > 0 ? drpAdditionalFuel.SelectedItem.Text : String.Empty,
                    CityMileage = txtMileage.Text.Trim(),
                    Warranties = txtWarranties.Text.Trim().Length > 1000 ? txtWarranties.Text.Trim().Substring(0, 999) : txtWarranties.Text.Trim(),
                    InteriorColor = InteriorColor,
                    InteriorColorCode = InteriorColorCode                  
                });
            }
            if (isDone)
            {
                Response.Redirect("confirmmessage.aspx?t=d&car=" + inquiryId);
            }
        }

        public string CarColor
        {
            get
            {
                return txtColor.Text.Trim() != "" ? txtColor.Text.Trim() : null;
            }
        }

        public string CarColorCode;

        public string CityId
        {
            get
            {
                if (ViewState["_CityId"] != null)
                {
                    return ViewState["_CityId"].ToString();
                }
                else
                    return "";
            }
            set
            {
                ViewState["_CityId"] = value;
            }
        }

        public string PinCode
        {
            get
            {
                if (ViewState["_PinCode"] != null)
                {
                    return ViewState["_PinCode"].ToString();
                }
                else
                    return "";
            }
            set
            {
                ViewState["_PinCode"] = value;
            }
        }
        public string InteriorColor
        {
            get
            {
                return txtIColor.Text.Trim();
            }
        }

        public string InteriorColorCode;

        private void FillExistingDrpOwners(int? owners)
        {
            if (owners != null)
            {
                switch (owners) 
                {
                    case (int)UsedCarOwnerTypes.FirstOwner :
                        owners = (int)UsedCarOwnerTypes.FirstOwner;
                        break;
                    case (int)UsedCarOwnerTypes.SecondOwner:
                        owners = (int)UsedCarOwnerTypes.SecondOwner;
                        break;
                    case (int)UsedCarOwnerTypes.ThirdOwner:
                        owners = (int)UsedCarOwnerTypes.ThirdOwner;
                        break;
                    default:
                        owners = (int)UsedCarOwnerTypes.FourOrMoreOwners;
                        break;
                }
                drpOwners.SelectedIndex = drpOwners.Items.IndexOf(drpOwners.Items.FindByValue(owners.ToString()));
            }
        }
    }
}