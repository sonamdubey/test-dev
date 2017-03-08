using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Bikewale.PinCodesAutosuggest
{
    public class GetPinCodeListDb
    {
        private static string _con = ConfigurationManager.AppSettings["ReadOnlyConnectionString"];
        public static List<PayLoad> GetPinCodeList()
        {
            List<PayLoad> lstPinCodes = null;
            PayLoad pinCode = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getAllIndiaPinCodes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            lstPinCodes = new List<PayLoad>();
                            while (reader.Read())
                            {
                                pinCode = new PayLoad();
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

        public static List<PinCodeList> GetSuggestList(List<PayLoad> objPinCodes)
        {
            List<PinCodeList> objSuggestList = null;

            int count = objPinCodes.Count;
            try
            {
                objSuggestList = new List<PinCodeList>();
                foreach (var pinCode in objPinCodes)
                {
                    PinCodeList ObjTemp = new PinCodeList();

                    ObjTemp.Id = pinCode.PinCodeId.ToString();
                    ObjTemp.name = string.Format("{0}_{1}", pinCode.PinCodeId, pinCode.PinCode);
                    string displayName = string.Format("{0}, {1} - {2}", pinCode.PinCode, pinCode.Area, pinCode.District);

                    ObjTemp.mm_suggest = new PinCodeSuggestion();
                    ObjTemp.mm_suggest.output = displayName;

                    ObjTemp.mm_suggest.payload = pinCode;

                    ObjTemp.mm_suggest.Weight = count;

                    ObjTemp.mm_suggest.input = new List<string>();


                    string tokenName = string.Format("{0} {1} {2}", pinCode.PinCode, pinCode.Area, pinCode.District);

                    var tokens = tokenName.Split(' ');
                    int l = tokens.Length;

                    for (int p = 1; p <= l; p++)                                        //  Break The Input into Different Tokens
                    {
                        printSeq(l, p, tokens, ObjTemp);
                    }

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


        private static void printSeqUtil(int n, int k, ref int len, int[] arr, string[] combination, PinCodeList obj)
        {
            // If length of current increasing sequence becomes k, print it
            if (len == k)
            {
                if (k == 1)
                    obj.mm_suggest.input.Add(String.Format("{0}", combination[arr[0] - 1].Trim()));
                else if (k == 2)
                    obj.mm_suggest.input.Add(String.Format("{0} {1}", combination[arr[0] - 1].Trim(), combination[arr[1] - 1].Trim()));
                else if (k == 3)
                    obj.mm_suggest.input.Add(String.Format("{0} {1} {2}", combination[arr[0] - 1].Trim(), combination[arr[1] - 1].Trim(), combination[arr[2] - 1].Trim()));
                else if (k == 4)
                    obj.mm_suggest.input.Add(String.Format("{0} {1} {2} {3}", combination[arr[0] - 1].Trim(), combination[arr[1] - 1].Trim(), combination[arr[2] - 1].Trim(), combination[arr[3] - 1].Trim()));
                else if (k == 5)
                    obj.mm_suggest.input.Add(String.Format("{0} {1} {2} {3} {4}", combination[arr[0] - 1].Trim(), combination[arr[1] - 1].Trim(), combination[arr[2] - 1].Trim(), combination[arr[3] - 1].Trim(), combination[arr[4] - 1].Trim()));
                else if (k == 6)
                    obj.mm_suggest.input.Add(String.Format("{0} {1} {2} {3} {4} {5}", combination[arr[0] - 1].Trim(), combination[arr[1] - 1].Trim(), combination[arr[2] - 1].Trim(), combination[arr[3] - 1].Trim(), combination[arr[4] - 1].Trim(), combination[arr[5] - 1].Trim()));
                else if (k == 7)
                    obj.mm_suggest.input.Add(String.Format("{0} {1} {2} {3} {4} {5} {6}", combination[arr[0] - 1].Trim(), combination[arr[1] - 1].Trim(), combination[arr[2] - 1].Trim(), combination[arr[3] - 1].Trim(), combination[arr[4] - 1].Trim(), combination[arr[5] - 1].Trim(), combination[arr[6] - 1].Trim()));
                else if (k == 8)
                    obj.mm_suggest.input.Add(String.Format("{0} {1} {2} {3} {4} {5} {6} {7}", combination[arr[0] - 1].Trim(), combination[arr[1] - 1].Trim(), combination[arr[2] - 1].Trim(), combination[arr[3] - 1].Trim(), combination[arr[4] - 1].Trim(), combination[arr[5] - 1].Trim(), combination[arr[6] - 1].Trim(), combination[arr[7] - 1].Trim()));
                else if (k == 9)
                    obj.mm_suggest.input.Add(String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}", combination[arr[0] - 1].Trim(), combination[arr[1] - 1].Trim(), combination[arr[2] - 1].Trim(), combination[arr[3] - 1].Trim(), combination[arr[4] - 1].Trim(), combination[arr[5] - 1].Trim(), combination[arr[6] - 1].Trim(), combination[arr[7] - 1].Trim(), combination[arr[8] - 1].Trim()));
                else if (k == 10)
                    obj.mm_suggest.input.Add(String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", combination[arr[0] - 1].Trim(), combination[arr[1] - 1].Trim(), combination[arr[2] - 1].Trim(), combination[arr[3] - 1].Trim(), combination[arr[4] - 1].Trim(), combination[arr[5] - 1].Trim(), combination[arr[6] - 1].Trim(), combination[arr[7] - 1].Trim(), combination[arr[8] - 1].Trim(), combination[arr[9] - 1].Trim()));
                else if (k == 11)
                    obj.mm_suggest.input.Add(String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}", combination[arr[0] - 1].Trim(), combination[arr[1] - 1].Trim(), combination[arr[2] - 1].Trim(), combination[arr[3] - 1].Trim(), combination[arr[4] - 1].Trim(), combination[arr[5] - 1].Trim(), combination[arr[6] - 1].Trim(), combination[arr[7] - 1].Trim(), combination[arr[8] - 1].Trim(), combination[arr[9] - 1].Trim(), combination[arr[10] - 1].Trim()));
                return;
            }

            int i = (len == 0) ? 1 : arr[len - 1] + 1;
            len++;	// Increase length
            // Put all numbers (which are greater than the previous element) at new position.
            while (i <= n)
            {
                arr[len - 1] = i;
                printSeqUtil(n, k, ref len, arr, combination, obj);
                i++;
            }
            // This is important. The variable 'len' is shared among all function calls in recursion tree. Its value must be brought back before next iteration of while loop
            len--;
        }

        private static void printSeq(int l, int p, string[] combination, PinCodeList obj)
        {
            int[] arr = new int[p];
            int len = 0;
            printSeqUtil(l, p, ref len, arr, combination, obj);
        }
    }   //class
}   //namespace

