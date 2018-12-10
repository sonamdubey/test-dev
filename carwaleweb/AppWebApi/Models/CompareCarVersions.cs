using AppWebApi.Common;
using Carwale.DAL.CompareCars;
using Carwale.Entity;
using Carwale.Interfaces.PriceQuote;
using Carwale.Service;
using Carwale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace AppWebApi.Models
{
    public class CompareCarVersions
    {
        private string ModelId { get; set; }
        private bool serverErrorOccured = false;
        [JsonIgnore]
        public bool ServerErrorOccured
        {
            get { return serverErrorOccured; }
            set { serverErrorOccured = value; }
        }

        public List<VersionItem> versions = new List<VersionItem>();
        protected ICarPriceQuoteAdapter _iPrice = UnityBootstrapper.Resolve<ICarPriceQuoteAdapter>();

        public CompareCarVersions(string modelId)
        {
            ModelId = modelId;
            BindVersionsToList();
        }

        private void BindVersionsToList()
        {
            DataTable DtModels = new DataTable();
            DataSet ds = new DataSet();
            CompareCarsRepository compRepo = new CompareCarsRepository();
            ds = compRepo.GetVersionsListForComapre(ModelId);
            var prices = _iPrice.GetAllVersionPriceByModelCity(int.Parse(ModelId), 10);
            Dictionary<int, VersionPrice> versionPrice = new Dictionary<int, VersionPrice>();

            foreach (var price in prices)
            {
                if (!versionPrice.ContainsKey(price.VersionBase.VersionId))
                {
                    versionPrice.Add(price.VersionBase.VersionId, price);
                }
            }

            DtModels = ds.Tables[0];
            if (DtModels.Rows.Count > 0)
            {
                DataRow[] dtRows = DtModels.Select("New = 1 AND IsSpecsAvailable = 1");
                if (dtRows.Length > 0)
                {
                    DtModels = dtRows.CopyToDataTable();
                    DtModels.DefaultView.Sort = "Value ASC";
                }
                else
                {
                    DtModels.Rows.Clear();
                    DtModels.AcceptChanges();
                }
            }

            foreach (DataRow dRow in DtModels.Rows)
            {
                VersionPrice version;
                int currentPrice = 0;
                versionPrice.TryGetValue(Convert.ToInt32(dRow["Value"].ToString()), out version);
                if (version != null && version.VersionBase != null &&  version.PriceStatus == (int)PriceBucket.HaveUserCity)
                {
                    currentPrice = version.VersionBase.ExShowroomPrice;
                }

                versions.Add(new VersionItem
                                            {
                                                VersionId = Convert.ToInt32(dRow["Value"].ToString()),
                                                CarName = dRow["FullCarName"].ToString(),
                                                VersionName = dRow["Text"].ToString(),
                                                MakeName = dRow["MakeName"].ToString(),
                                                ModelName = dRow["ModelName"].ToString(),
                                                SmallPicUrl = dRow["HostURL"].ToString() + ImageSizes._110X61 + dRow["OriginalImgPath"].ToString(),
                                                LargePicUrl = dRow["HostURL"].ToString() + ImageSizes._210X118 + dRow["OriginalImgPath"].ToString(),
                                                HostUrl = dRow["HostURL"].ToString(),
                                                OriginalImgPath = dRow["OriginalImgPath"].ToString(),
                                                Price = CommonOpn.GetPrice(currentPrice.ToString())
                                            }
                            );
            }
        }
    }
}