using Carwale.Entity.Accessories.Tyres;
using Carwale.Entity.Common;
using Carwale.Interfaces.Accessories.Tyres;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Accessories.Tyres
{
    public class AccessoryRepository : RepositoryBase, IAccessoryRepo
    {
        public ItemData GetAccessoryDataByItemId(int itemId)
        {            
            try
            {
                ItemData itemData = new ItemData();

                var param = new DynamicParameters();
                param.Add("v_ItemId", itemId > 0 ? itemId : Convert.DBNull);

                using (var conn = AccessoriesMySqlReadConnection)
                {
                    var itemIdResult = conn.QueryMultiple("GetAccessoryDataByItemId", param, commandType: CommandType.StoredProcedure);
                    itemData.ItemSummary = itemIdResult.Read<ItemSummary>().FirstOrDefault();
                    itemData.FeatureCategories = itemIdResult.Read<IdName>().AsList();
                    itemData.ItemFeatures = itemIdResult.Read<ItemFeature>().AsList();

                    return itemData;
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return null;
        }
    }
}
