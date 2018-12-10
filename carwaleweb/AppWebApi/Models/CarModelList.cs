using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using AppWebApi.Common;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.DAL.CompareCars;

namespace AppWebApi.Models {
    public class CarModelList : IDisposable {
        private bool serverErrorOccured = false;
        [JsonIgnore]
        public bool ServerErrorOccured {
            get { return serverErrorOccured; }
            set { serverErrorOccured = value; }
        }

        private string MakeId { get; set; }

        [JsonProperty("models")]
        public List<Item> Models = new List<Item>();

        DataSet ds = new DataSet();
        public CarModelList(string makeId) {
            MakeId = makeId;
            GetCarModels();
        }



        private void GetCarModels() {
            CompareCarsRepository compRepo = new CompareCarsRepository();
            ds = compRepo.GetCarModels(Convert.ToInt32(MakeId), 0, 0, 0, 1);
            foreach (DataRow dRow in ds.Tables[0].Rows)
            {
                Models.Add(new Item
                {
                    Name = dRow["Text"].ToString(),
                    Id = dRow["Value"].ToString(),
                    Url = CommonOpn.ApiHostUrl + "CompareCarVersions/?modelId=" + dRow["Value"].ToString()
                });
            }
        }
        public void Dispose() {
            this.ds.Dispose();
        }
    }
}