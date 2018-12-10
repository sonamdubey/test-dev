using Carwale.Cache.Core;
using Carwale.CarData.DAL;
using Carwale.CarData.Entities;
using Carwale.Notifications;
using System;
using System.Collections;

namespace Carwale.CarData
{
    public class CarModelsCache
    {
        static TimeSpan ts = CacheRefreshTime.DefaultRefreshTime();

         #warning Changing these key will affect refreshing it from opr.So go to OPR "web.config" and find the key name 'AllKeysRelatedToMaskingName' and update the key Template present there.

         const string s_strModelMaskingNames = "modelRootMaskingNames";
         const string s_strModelOldMaskingNames = "modelRootOldMaskingNames";

        public static Hashtable[] GetMaskingNamesFromMemcache()
        {
            var data = new MemcacheManager();
            return data.GetFromCache<Hashtable[]>(s_strModelMaskingNames, ts, () => new CarModels().GetMaskingNames());
        }

        public static Hashtable[] GetMaskingNamesFromLocalCache(bool refresh)
        {
            var data = new LocalCacheManager();

            if (refresh)
            {
                data.RefreshCache(s_strModelMaskingNames);
            }

            return data.GetFromCache<Hashtable[]>(s_strModelMaskingNames, ts, () => GetMaskingNamesFromMemcache());
        }

        public static Hashtable GetOldMaskingNamesFromMemcache()
        {
            var data = new MemcacheManager();
            return data.GetFromCache<Hashtable>(s_strModelOldMaskingNames, ts, () => new CarModels().GetOldMaskingNames());
        }

        public static Hashtable GetOldMaskingNamesFromLocalCache(bool refresh)
        {
            var data = new LocalCacheManager();

            if (refresh)
                data.RefreshCache(s_strModelOldMaskingNames);

            return data.GetFromCache<Hashtable>(s_strModelOldMaskingNames, ts, () => GetOldMaskingNamesFromMemcache());
        }

        public static CarModelMaskingResponse getMaskingNameDetails(string maskingName)
        {
            var cme = new CarModelMaskingEntity(-1, -1, string.Empty,string.Empty);
            var cmr = new Carwale.CarData.Entities.CarModelMaskingResponse();

            bool redirect = true;

            try
            {
                Hashtable[] arrHt = CarModelsCache.GetMaskingNamesFromLocalCache(false);
                if (arrHt[0][maskingName] != null)
                {
                    cme = (CarModelMaskingEntity)arrHt[0][maskingName];
                    redirect = false;
                }
                else
                {
                    arrHt = CarModelsCache.GetMaskingNamesFromLocalCache(true);
                    if (arrHt[0][maskingName] != null)
                    {
                        cme = (CarModelMaskingEntity)arrHt[0][maskingName];
                        redirect = false;
                    }
                    else
                    {//query for following needs modification
                        Hashtable ht = CarModelsCache.GetOldMaskingNamesFromLocalCache(false);
                        if (ht[maskingName] != null)
                        {
                            cme = (CarModelMaskingEntity)ht[maskingName];
                        }
                        else
                        {
                            ht = CarModelsCache.GetOldMaskingNamesFromLocalCache(true);
                            if (ht[maskingName] != null)
                            {
                                cme = (CarModelMaskingEntity)ht[maskingName];
                            }
                        }
                    }
                }

                if (cme.ModelId != -1)
                {
                    cmr.MakeId = cme.MakeId;
                    cmr.ModelId = cme.ModelId;
                    cmr.MakeName = cme.MakeName;
                    cmr.Redirect = redirect;
                    cmr.MaskingName = !redirect ? maskingName : (string)arrHt[1][cme.ModelId];
                    cmr.RootName = cme.RootName;
                }
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, string.Empty);
                objErr.LogException();
            }

            return cmr;
        }
    }
}