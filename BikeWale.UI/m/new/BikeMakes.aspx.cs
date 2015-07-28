using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Bikewale.BAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Memcache;
using Bikewale.Common;

namespace Bikewale.Mobile
{
    public class BikeMakes : System.Web.UI.Page
    {
        protected Repeater rptSeries;

        protected string makeId = string.Empty, makeName = string.Empty, makeMaskingName = string.Empty, price = string.Empty;
        protected int count = 0;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessQueryString();
            if(!String.IsNullOrEmpty(makeId))
                BindSeiesList();
        }

        //Modified By : Sadhana Upadhyay to get price 
        protected void BindSeiesList()
        {
            int minPrice = 999999999;
            int maxPrice = 0;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakes<BikeMakeEntity, int>>();
                    IBikeMakes<BikeMakeEntity, int> objMake = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();

                    List<BikeModelsListEntity> objModelList = objMake.GetModelsList(Convert.ToInt32(makeId));

                    //Filtered with lambda expression
                    List<BikeModelsListEntity> objSeriesList = objModelList.FindAll(s => s.ModelRank == 1);

                    count = objSeriesList.Count;
                    if (count > 0)
                    {
                        rptSeries.DataSource = objSeriesList;
                        rptSeries.DataBind();

                        makeName = objSeriesList[0].MakeBase.MakeName;
                        makeMaskingName = objSeriesList[0].MakeBase.MaskingName;


                        for (int i = 0; i < objSeriesList.Count; i++)
                        {
                            if (!String.IsNullOrEmpty(objSeriesList[i].MinPrice.ToString()))
                            {
                                if (Convert.ToInt32(objSeriesList[i].MinPrice) < minPrice)
                                {
                                    minPrice = Convert.ToInt32(objSeriesList[i].MinPrice);
                                }

                                if (Convert.ToInt32(objSeriesList[i].MinPrice) > maxPrice)
                                {
                                    maxPrice = Convert.ToInt32(objSeriesList[i].MinPrice);
                                }
                            }
                        }

                        price = ((!String.IsNullOrEmpty(minPrice.ToString()) && minPrice != 999999999) ? "Rs." + CommonOpn.FormatPrice(minPrice.ToString()) : String.Empty) + ((!String.IsNullOrEmpty(maxPrice.ToString()) && maxPrice != 0) ? " to Rs." + CommonOpn.FormatPrice(maxPrice.ToString()) : String.Empty);
                    }
                    else
                    {
                        Response.Redirect("/m/pagenotfound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        void ProcessQueryString()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["make"]))
            {
                makeId = MakeMapping.GetMakeId(Request.QueryString["make"].ToLower());

                if (String.IsNullOrEmpty(makeId))
                {
                    Response.Redirect("/m/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            else
            {
                Response.Redirect("/m/pagenotfound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

    }   // class
}   // namespace