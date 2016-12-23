using Bikewale.Notifications;
using Bikewale.Notifications.MailTemplates;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.DALs.PopularComparisions;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.PopularComparisions;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Web.UI;

namespace BikeWaleOpr
{
    public class Default : Page
    {
        private IBikeModels _objModelsRepo = null;
        protected SoldUnitData dataObj = null;
        protected bool isShownNotification = false;

        public Default()
        {
            using (IUnityContainer container = new UnityContainer())
            {

                container.RegisterType<IPopularBikeComparisions, PopularBikeComparisionsRepository>()
                .RegisterType<IBikeMakes, BikeMakesRepository>()
                .RegisterType<IBikeModels, BikeModelsRepository>()
                .RegisterType<IBikeVersions, BikeVersionsRepository>();

                _objModelsRepo = container.Resolve<IBikeModels>();

            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified by : Sajal gupta on 22-12-2016
        /// Desc : Added condition for showing notification panel to nandu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUser.Id == "-1")
            {
                Response.Redirect("/users/login.aspx");
            }
            else if (CurrentUser.Id == "1225") //customer id of nandu
            {
                isShownNotification = true;
            }


            NotificationTrigger();
        }

        /// <summary>
        /// Created by Sajal Gupta o 22-12-2016
        /// Desc : Mail trigger to nandu and abhishek .
        /// </summary>
        protected void NotificationTrigger()
        {
            dataObj = _objModelsRepo.GetLastSoldUnitData();

            if (isShownNotification && dataObj.IsEmailToSend == 1)
            {
                string[] arr1 = new string[] { "abhishek.singh@carwale.com" };
                ComposeEmailBase objEmail = new ModelSoldUnitMailTemplate("NandKishor Sanas", dataObj.LastUpdateDate);
                objEmail.Send("nandkishor.sanas@carwale.com", "Please update last month model sold data", "", arr1, null);
            }
        }
    }
}
