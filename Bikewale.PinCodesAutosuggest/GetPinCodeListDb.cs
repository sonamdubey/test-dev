using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
namespace Bikewale.PinCodesAutosuggest
{
    /// <summary>
    /// Created By : Sushil Kumar on 9th March 2017
    /// Description : Dataccess and business logic for pincodes suggestion
    /// </summary>
    public class GetPinCodeListDb
    {
        private static string _con = ConfigurationManager.ConnectionStrings["ReadOnlyConnectionString"].ConnectionString;

        /// <summary>
        /// Created By : Sushil Kumar on 9th March 2017
        /// Description : To fetch pincodes from database 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<PayLoad> GetPinCodeList(uint parameter)
        {
            IList<PayLoad> lstPinCodes = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getallpincodes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pincodetype", DbType.Int32, parameter));
                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            lstPinCodes = new List<PayLoad>();
                            while (reader.Read())
                            {
                                PayLoad pinCode = new PayLoad();
                                pinCode.PinCodeId = Convert.ToUInt32(reader["Id"]);
                                pinCode.PinCode = Convert.ToString(reader["PinCode"]);
                                pinCode.District = Convert.ToString(reader["District"]);
                                pinCode.Taluka = Convert.ToString(reader["Taluka"]);
                                pinCode.Area = Convert.ToString(reader["Area"]);
                                pinCode.State = Convert.ToString(reader["State"]);
                                pinCode.Region = Convert.ToString(reader["Region"]);
                                lstPinCodes.Add(pinCode);
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(MethodBase.GetCurrentMethod().Name, ex);
                Console.WriteLine("Exception Message  : " + ex.Message);
            }

            return lstPinCodes;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 9th March 2017
        /// Description : To create suggestion list and populate the same in elastic search
        /// </summary>
        /// <param name="objPinCodes"></param>
        /// <returns></returns>
        public static IEnumerable<PinCodeList> GetSuggestList(IEnumerable<PayLoad> objPinCodes)
        {
            IList<PinCodeList> objSuggestList = null;

            int count = objPinCodes.Count();
            try
            {
                objSuggestList = new List<PinCodeList>();
                foreach (var pinCode in objPinCodes)
                {
                    PinCodeList ObjTemp = new PinCodeList();

                    ObjTemp.Id = Convert.ToString(pinCode.PinCodeId);
                    ObjTemp.name = string.Format("{0}_{1}", pinCode.PinCodeId, pinCode.PinCode);
                    string displayName = string.Format("{0}, {1} - {2}", pinCode.PinCode, pinCode.Area, pinCode.District);

                    ObjTemp.mm_suggest = new PinCodeSuggestion();
                    ObjTemp.output = displayName;

                    ObjTemp.payload = pinCode;

                    ObjTemp.mm_suggest.Weight = count;

                    ObjTemp.mm_suggest.input = new List<string>();


                    string tokenName = string.Format("{0} {1} {2}", pinCode.PinCode, pinCode.Area, pinCode.District);

                    var tokens = tokenName.Split(' ');
                    int length = Math.Min(tokens.Length, 5);


                    ObjTemp.mm_suggest.input = new List<string>();

                    // For creating input in mm_suggest
                    for (int index = 1; index < 1 << length; index++)
                    {
                        int temp_value = index, jindex = 0;
                        string value = string.Empty;
                        while (temp_value > 0)
                        {
                            if ((temp_value & 1) > 0)
                                value = string.Format("{0} {1}", value, tokens[jindex]);
                            temp_value >>= 1;
                            jindex++;
                        }
                        if (!string.IsNullOrEmpty(value))
                            ObjTemp.mm_suggest.input.Add(value);

                    }

                    ObjTemp.mm_suggest.contexts = new Context();
                    ObjTemp.mm_suggest.contexts.types = new List<string>();
                    ObjTemp.mm_suggest.contexts.types.Add("AreaPinCodes");
                    objSuggestList.Add(ObjTemp);
                    count--;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get Suggest List Exception  : " + ex.Message);
                Logs.WriteErrorLog(MethodBase.GetCurrentMethod().Name + " :Exception in Generating Suggestion List for pin codes :", ex);
            }
            return objSuggestList;
        }



    }   //class
}   //namespace

