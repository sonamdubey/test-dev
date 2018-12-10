using AppWebApi.Common;
using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Carwale.Service;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace AppWebApi.Models
{
    public class PqMakes
    {
        private bool serverErrorOccured = false;
        [JsonIgnore]
        public bool ServerErrorOccured
        {
            get { return serverErrorOccured; }
            set { serverErrorOccured = value; }
        }

        [JsonProperty("makes")]
        public List<Item> Makes = new List<Item>();

        public PqMakes()
        {
            PopulateMakes();    
        }

        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         Desc:  get makes for pricequotes
         */
        private void PopulateMakes()
        {
            ICarMakesCacheRepository makeCacheRepo = UnityBootstrapper.Resolve<ICarMakesCacheRepository>();
            List<CarMakeEntityBase> makeList = new List<CarMakeEntityBase>();
            makeList = makeCacheRepo.GetCarMakesByType("new");
            foreach (var make in makeList)
            {
                Item item = new Item();
                item.Name = make.MakeName;
                item.Id = make.MakeId.ToString();
                item.Url = CommonOpn.ApiHostUrl + "PqModels/?makeId=" + make.MakeId;
                Makes.Add(item);
            }
        }
    }
}