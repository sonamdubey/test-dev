using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.BAL.Used.Search;
using Bikewale.DAL.Used.Search;
using Bikewale.Entities.Used.Search;
using Bikewale.Interfaces.Used.Search;
using Microsoft.Practices.Unity;

namespace Bikewale.Mobile.Used
{
    public class Search : System.Web.UI.Page
    {
        protected Repeater rptUsedListings;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindSearchPageData();
        }

        /// <summary>
        /// Function to bind the search result to the repeater
        /// </summary>
        private void BindSearchPageData()
        {
            InputFilters objFilters = new InputFilters();
            objFilters.CityId = 1;
            objFilters.Makes = "1";
            objFilters.Models = "39+59";
            objFilters.Budget = "20000+50000";
            objFilters.Age = "2";
            objFilters.Kms = "40000";
            objFilters.Owners = "1,2,3";
            objFilters.ST = "1,2";
            objFilters.PN = 1;
            objFilters.PS = 20;
            objFilters.SO = 0;
            

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ISearch, SearchBikes>();
                container.RegisterType<ISearchFilters, ProcessSearchFilters>();
                container.RegisterType<ISearchQuery, SearchQuery>();
                container.RegisterType<ISearchRepository, SearchRepository>();

                ISearch searchRepo = container.Resolve<ISearch>();

                SearchResult objResult = searchRepo.GetUsedBikesList(objFilters);

                if (objResult != null && objResult.Result != null && objResult.Result.Count() > 0)
                {
                    rptUsedListings.DataSource = objResult.Result;
                    rptUsedListings.DataBind();
                }
                
            }
        } // End of BindSearchPageData


    } // class
}   // namespace