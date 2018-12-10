using AppWebApi.Common;
using Carwale.DAL.CompareCars;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace AppWebApi.Models
{
    public class CompareCarsDetails
    {
        public Hashtable response;
        public bool ServerErrorOccurred = false;
        private static string ModelId1 { get; set; }
        private static string ModelId2 { get; set; }

        public List<VersionItem> versionsList1 = new List<VersionItem>();
        public List<VersionItem> versionsList2 = new List<VersionItem>();

        //Added by supriya on 12/6/2014
        [JsonProperty("tinyShareUrl")]
        public string TinyShareUrl { get; set; }

        public CompareCarsDetails(int version1, int version2)
        {
            try
            {
                response = GetCarData(version1, version2);
                CompareCarVersions cv1 = new CompareCarVersions(ModelId1);
                CompareCarVersions cv2 = new CompareCarVersions(ModelId2);
                versionsList1 = cv1.versions;
                versionsList2 = cv2.versions;
            }
            catch (Exception)
            {
                ServerErrorOccurred = true;
            }
        }

        /*
        Author:Amit Verma 
        Date Created:  ‎March ‎12, ‎2014
        Desc: get detailed comparision of cars
        */
        Hashtable GetCarData(int version1, int version2)
        {
            //DataSet ds = getdata(2197, 2199);
            DataSet ds = getdata(version1, version2);

            Hashtable ht = new Hashtable();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ht[row["NodeCode"].ToString()] = new Cat(row["CategoryName"].ToString(), new Hashtable(), row["SortOrder"].ToString());
            }

            foreach (DataRow row in ds.Tables[1].Rows)
            {
                ((Cat)ht[row["NodeCode"].ToString().Substring(0, 3)]).ht[row["NodeCode"].ToString()] = new SubCat(row["CategoryName"].ToString(), new List<CData>(), row["SortOrder"].ToString());
            }


            foreach (DataRow row in ds.Tables[2].Rows)
            {
                CData data = new CData(row["Name"].ToString(), row["Value1"].ToString(), row["Value2"].ToString(), row["SortOrder"].ToString());
                ((SubCat)((Cat)ht[row["NodeCode"].ToString().Substring(0, 3)]).ht[row["NodeCode"].ToString()]).lt.Add(data);
            }

            ht["/0/"] = new SubCat("Overview", new List<CData>(), "0");

            foreach (DataRow row in ds.Tables[3].Rows)
            {
                CData data = new CData(row["Name"].ToString(), row["Value1"].ToString(), row["Value2"].ToString(), row["SortOrder"].ToString());
                ((SubCat)ht["/0/"]).lt.Add(data);
            }

            ht["/3/"] = new Hashtable();
            ((Hashtable)ht["/3/"])[version1] = ds.Tables[4];
            ((Hashtable)ht["/3/"])[version2] = ds.Tables[5];
            
            DataTable dt = ds.Tables[6];

            ModelId1 = dt.Rows[0]["ModelId"].ToString();
            ModelId2 = dt.Rows[1]["ModelId"].ToString();

            string url1 = CommonOpn.FormatSpecial(getCV(0, "MakeName", dt)) + "-" + getCV(0, "MaskingName", dt);
            string url2 = CommonOpn.FormatSpecial(getCV(1, "MakeName", dt)) + "-" + getCV(1, "MaskingName", dt);
            string url3 = "car1=" + getCV(0, "VersionId", dt) + "&car2=" + getCV(1, "VersionId", dt);
            ht["shareUrl"] = "https://www.carwale.com/comparecars/" + url1 +"-vs-" + url2 + "?" +url3;

            return ht;
        }

        static DataSet getdata(int version1, int version2)
        {
            CompareCarsRepository compRepo = new CompareCarsRepository();
            DataSet ds = new DataSet();
            ds = compRepo.GetCarVersionsDataForCompare(version1, version2);
            return ds;           
        }

        static string getCV(int rowNumber, string colName, DataTable dt)
        {
            return dt.Rows[rowNumber][colName].ToString();
        }
    }

    class Cat
    {
        public string n;
        public Hashtable ht;
        public string s;

        public Cat(string n, Hashtable ht, string s)
        {
            this.n = n;
            this.ht = ht;
            this.s = s;
        }
    }

    class SubCat
    {
        public string n;
        public List<CData> lt;
        public string s;

        public SubCat(string n, List<CData> lt, string s)
        {
            this.n = n;
            this.lt = lt;
            this.s = s;
        }
    }

    class CData
    {
        public string n;
        public string v1;
        public string v2;
        public string s;

        public CData(string n, string v1, string v2, string s)
        {
            this.n = n;
            this.v1 = v1;
            this.v2 = v2;
            this.s = s;
        }
    }
}