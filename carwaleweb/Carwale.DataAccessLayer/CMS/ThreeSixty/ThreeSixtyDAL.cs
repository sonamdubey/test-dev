using Carwale.Entity.CMS;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CMS;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.CMS.ThreeSixty
{
    public class ThreeSixtyDal : RepositoryBase, IThreeSixtyDal
    {
        public HotspotData GetHotspots(int modelId, ThreeSixtyViewCategory type)
        {
            HotspotData data = new HotspotData();
            data.Hotspots = new Dictionary<int, Hotspot>();
            data.HotspotPositions = new Dictionary<int, List<Hotspot>>();
            List<Hotspot> list;

            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId);
                param.Add("v_Type", type);
                using (var con = EditCmsMySqlReadConnection)
                {
                    var mapper = con.QueryMultiple("GetHotSpots", param, commandType: CommandType.StoredProcedure);
                    list = mapper.Read<Hotspot>().ToList(); 
                    dynamic row = mapper.Read().FirstOrDefault();
                    if (row != null)
                    {
                        data.TotalImages = row.imagecount;
                        data.ImageVersion = row.imageversion;
                    }
                }

                foreach (Hotspot hotspot in list)
                {
                    if (data.HotspotPositions.ContainsKey(hotspot.ImageId))
                        data.HotspotPositions[hotspot.ImageId].Add(hotspot);
                    else
                        data.HotspotPositions.Add(hotspot.ImageId, new List<Hotspot> { hotspot });

                    if (!data.Hotspots.ContainsKey(hotspot.HotspotXmlId))
                        data.Hotspots.Add(hotspot.HotspotXmlId, hotspot);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return data ?? new HotspotData();
        }

        public Dictionary<string, List<Hotspot>> GetHotspotDetails(int modelId, ThreeSixtyViewCategory category)
        {
            try
            {
                List<Hotspot> hotspotDetails;

                DynamicParameters param = new DynamicParameters();

                param.Add("v_ModelId", modelId, DbType.Int32, ParameterDirection.Input);
                param.Add("v_ThreeSixtyViewType", (int)category, DbType.Int16, ParameterDirection.Input);
                param.Add("v_GetAllHotspots", false, DbType.Boolean, ParameterDirection.Input);

                using (var con = EditCmsMySqlReadConnection)
                {
                    hotspotDetails = con.Query<Hotspot>("GetHotspotData", param, null, true, null, CommandType.StoredProcedure).ToList();
                }

                if (hotspotDetails != null && hotspotDetails.Count > 0)
                    return hotspotDetails.GroupBy(hotspot => hotspot.HotspotXmlId).ToDictionary(hotspot => hotspot.Key.ToString(), hotspot => hotspot.ToList());

                return new Dictionary<string, List<Hotspot>>();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }
    }
}
