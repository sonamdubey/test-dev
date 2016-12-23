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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUser.Id == "-1")
            {
                Response.Redirect("/users/login.aspx");
            }
            else if (CurrentUser.Id == "1225")
            {
                isShownNotification = true;
            }


            NotificationTrigger();
        }

        protected void NotificationTrigger()
        {
            dataObj = _objModelsRepo.GetLastSoldUnitData();

            if (isShownNotification && dataObj.IsEmailToSend == 1)
            {
                string[] arr1 = new string[] { "abhishek.singh@carwale.com", "sajal.gupta@carwale.com" };                
                ComposeEmailBase objEmail = new ModelSoldUnitMailTemplate("NandKishor Sanas", dataObj.LastUpdateDate);
                objEmail.Send("nandkishor.sanas@carwale.com", "Please update last month model sold data", "", arr1, null);                
            }
        }
    }
}
