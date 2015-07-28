using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Bikewale.BAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Common;
using Bikewale.Memcache;

namespace Bikewale.Mobile
{
    public class BikeSeries : System.Web.UI.Page
    {
        protected Repeater rptModels;

        protected string seriesId = string.Empty, seriesName = string.Empty, makeName = string.Empty, makeMaskingName = string.Empty, seriesMaskingName = string.Empty, price = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ProcessQueryString())
            {
                if (!IsPostBack)
                {
                    GetSeriesDetails();
                    BindModels();
                }
            }
            else
            {
                Response.Redirect("/m/pagenotfound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        private void GetSeriesDetails()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeSeries<BikeSeriesEntity, int>, BikeSeries<BikeSeriesEntity, int>>();
                    IBikeSeries<BikeSeriesEntity, int> objSeries = container.Resolve<IBikeSeries<BikeSeriesEntity, int>>();

                    BikeSeriesEntity objSeriesEntity = objSeries.GetById(Convert.ToInt32(seriesId));

                    seriesName = objSeriesEntity.SeriesName;
                    seriesMaskingName = objSeriesEntity.MaskingName;
                    makeName = objSeriesEntity.MakeBase.MakeName;
                    makeMaskingName = objSeriesEntity.MakeBase.MaskingName;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private bool ProcessQueryString()
        {
            bool isSuccess = true;
            if (!String.IsNullOrEmpty(Request.QueryString["series"]))
            {
                seriesId = SeriesMapping.GetSeriesId(Request.QueryString["series"].ToLower());

                if (String.IsNullOrEmpty(seriesId))
                {
                    isSuccess = false;
                }
            }
            else
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        //Modified By : Sadhana Upadhyay to get price
        protected void BindModels()
        {
            try
            {
                int minPrice = 999999999;
                int maxPrice = 0;

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeSeries<BikeSeriesEntity, int>, BikeSeries<BikeSeriesEntity, int>>();
                    IBikeSeries<BikeSeriesEntity, int> objSeries = container.Resolve<IBikeSeries<BikeSeriesEntity, int>>();

                    List<BikeModelEntity> objModelList = objSeries.GetModelsList(Convert.ToInt32(seriesId));

                    rptModels.DataSource = objModelList;
                    rptModels.DataBind();

                    for (int i = 0; i < objModelList.Count; i++)
                    {
                        if (!String.IsNullOrEmpty(objModelList[i].MinPrice.ToString()))
                        {
                            if (Convert.ToInt32(objModelList[i].MinPrice) < minPrice)
                            {
                                minPrice = Convert.ToInt32(objModelList[i].MinPrice);
                            }

                            if (Convert.ToInt32(objModelList[i].MinPrice) > maxPrice)
                            {
                                maxPrice = Convert.ToInt32(objModelList[i].MinPrice);
                            }
                        }
                    }

                    price = ((!String.IsNullOrEmpty(minPrice.ToString()) && minPrice != 999999999) ? " Rs." + CommonOpn.FormatPrice(minPrice.ToString()) : String.Empty) + ((!String.IsNullOrEmpty(maxPrice.ToString()) && maxPrice != 0) ? " to Rs." + CommonOpn.FormatPrice(maxPrice.ToString()) : String.Empty);

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

    }   // class
}   // namespace