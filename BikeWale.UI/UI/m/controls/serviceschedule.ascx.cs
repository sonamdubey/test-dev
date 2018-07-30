using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.ServiceCenters;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By :  Sangram Nandkhile on 09-Nov-2016
    /// Description : Service Schedule for make
    /// </summary>
    public class ServiceSchedule : UserControl
    {
        public uint MakeId { get; set; }
        public string MakeName { get; set; }
        public string jsonBikeSchedule;
        protected IEnumerable<ModelServiceSchedule> BikeScheduleList = null;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BindServiceSchedule scheduleViewModel = new BindServiceSchedule();
            BikeScheduleList = scheduleViewModel.GetServiceScheduleList(MakeId);
            if (BikeScheduleList != null)
                jsonBikeSchedule = Newtonsoft.Json.JsonConvert.SerializeObject(BikeScheduleList);
        }

    }
}