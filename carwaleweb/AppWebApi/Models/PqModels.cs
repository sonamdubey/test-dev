using AppWebApi.Common;
using Carwale.Entity.CarData;
using Carwale.Interfaces.CarData;
using Carwale.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace AppWebApi.Models
{
    public class PqModels
    {
        private bool serverErrorOccured = false;
        [JsonIgnore]
        public bool ServerErrorOccured
        {
            get { return serverErrorOccured; }
            set { serverErrorOccured = value; }
        }

        private string MakeId { get; set; }

        [JsonProperty("models")]
        public List<Item> Models = new List<Item>();

        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         Desc:  get list of models for price quote
         */
        public PqModels(string makeId)
        {
            MakeId = makeId;
            PopulateModels();    
        }

        /*
         Author: Rakesh Yadav
         Date Created: 16 Oct 2013
         Desc: populate mkodels for price quote
         */
        private void PopulateModels()
        {
            ICarModelCacheRepository modelCache = UnityBootstrapper.Resolve<ICarModelCacheRepository>();
            List<CarModelEntityBase> carModels = modelCache.GetCarModelsByType("new", Convert.ToInt16(MakeId));
            Item item;
            foreach (var model in carModels)
            {
               item = new Item();
               item.Name = model.ModelName;
               item.Id = model.ModelId.ToString();
               item.Url = CommonOpn.ApiHostUrl + "PqVersionsAndCities/?modelId=" + model.ModelId;
               Models.Add(item);
            }
        }
    }
}