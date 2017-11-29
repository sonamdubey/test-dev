using BikewaleOpr.DALs.BikePricing;
using BikewaleOpr.Entity.BikePricing;
using BikewaleOpr.Interface.Dealers;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.UI.WebControls;

namespace BikewaleOpr.Campaign
{
    /// <summary>
    /// Created by : Aditi Srivastava on 18 Jan 2017
    /// Description: Add and view price categories
    /// </summary>
    public class ManageDealerPriceCategories : System.Web.UI.Page
    {
        protected ICollection<PriceCategoryEntity> priceCatList = null;
        protected Button btnAddCat;
        protected TextBox txtPriceCat;
        protected Label errAddCat;
        private readonly IDealerPriceRepository _objDealerPriceRepo = null;
        protected string msg = "";

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnAddCat.Click += new EventHandler(btnAddCat_Click);            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
              if(!IsPostBack)
                GetPriceCategories();
        }

        public ManageDealerPriceCategories()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceRepository, DealerPriceRepository>();
                _objDealerPriceRepo = container.Resolve<IDealerPriceRepository>();
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 18 Jan 2017
        /// Description: Get all price categories
        /// </summary>
        private void GetPriceCategories()
        {
            priceCatList = new Collection<PriceCategoryEntity>();
            try
            {
                priceCatList = _objDealerPriceRepo.GetAllPriceCategories();

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Campaign.ManageDealerPriceCategories.GetPriceCategories");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 18 Jan 2017
        /// Description: Add a price category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCat_Click(object sender, EventArgs e)
        {
            bool isSuccess=false;
            try
            {
                string priceCat = txtPriceCat.Text.Trim();
                     isSuccess = _objDealerPriceRepo.SaveBikeCategory(priceCat);
                    if (!isSuccess)
                        msg = "Could not add category!";
                    else
                        msg = string.Format("Category \"{0}\" added successfully",priceCat);

                    GetPriceCategories();
                                  
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Campaign.ManageDealerPriceCategories.btnAddCat_Click");
            }
        }
    }
}