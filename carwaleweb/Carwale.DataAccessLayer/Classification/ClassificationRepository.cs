using Carwale.Entity.Classification;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;
using Carwale.Interfaces.Classification;

namespace Carwale.DAL.Classification
{
    public class ClassificationRepository : IClassificationRepository
    {
        private static string _imgHostUrl = Carwale.Utility.CWConfiguration._imgHostUrl;

        public List<BodyType> GetBodyType() 
        {
            var carBodyType = new List<BodyType>();
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("GetCarBodyType_v17_11_1"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.CarDataMySqlReadConnection))                  
                    {

                        while (dr.Read())
                        {
                            carBodyType.Add(new BodyType()
                            {
                                Name = Convert.ToString(dr["Name"]),
                                Id = Convert.ToUInt16(dr["ID"]),
                                Image = _imgHostUrl + "/" + Convert.ToString(dr["ImageUrl"]),
                                Description = dr["Description"] != null ? Convert.ToString(dr["Description"]) : string.Empty,
                                Icon = _imgHostUrl + Convert.ToString(dr["Icon"]),
                                LineIcon = _imgHostUrl + Convert.ToString(dr["LineIcon"]),
								AppIcon = _imgHostUrl + Convert.ToString(dr["AppIcon"])
							});
                        }

                    }
                    
                }
            }
            catch (MySqlException ex)
            {
                var obj = new ExceptionHandler(ex, "Carwale.DAL.Classification.ClassificationRepository.GetBodyTypes()");
                obj.LogException();
            }
            catch (Exception ex)
            {
                var obj = new ExceptionHandler(ex, "Carwale.DAL.Classification.ClassificationRepository.GetBodyTypes()");
                obj.LogException();
            }
            return carBodyType;
        }

    }
}
