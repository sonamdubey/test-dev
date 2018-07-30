
using Bikewale.Entities;
using Bikewale.Interfaces.App;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace Bikewale.DAL.App
{
    /// <summary>
    /// Author      :   Sangram Nandkhile
    /// Description :   Return Splash screen
    /// Created On  :   05 May 2015
    /// </summary>
    public class SplashScreenRepository : ISplashScreenRepository
    {
        public IEnumerable<SplashScreenEntity> GetAppSplashScreen()
        {
            List<SplashScreenEntity> splashScreenImages = null;
            try
            {
                splashScreenImages = new List<SplashScreenEntity>();
                using (DbCommand cmd = DbFactory.GetDBCommand("getappsplashscreen"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                var image = new SplashScreenEntity();
                                image.SplashImgUrl = Convert.ToString(dr["imagepath"].ToString());
                                image.SplashTimeOut = SqlReaderConvertor.ToUInt32(dr["timeout"]);
                                splashScreenImages.Add(image);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "SplashScreenRepository.GetAppSplashScreen()");
            }
            return splashScreenImages;
        }
    }
}
