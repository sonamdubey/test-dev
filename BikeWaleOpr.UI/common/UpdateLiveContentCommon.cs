using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Mail;
using System.Collections;
using System.IO;
using System.Xml;

namespace BikeWaleOpr.Common 
{
	public class UpdateLiveContentCommon
	{
		protected string listingId = "", isModel = "";
		
		XmlDocument xmlDoc = new XmlDocument();
		XmlTextWriter xw;
	
		public void UpdateRoadTests()
		{
			string filename = "";
			
			if ( HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf( "bikewale.com" ) >= 0 )
		    {
				  filename = CommonOpn.ImagePathForSavingImages("/Content/") + "rt.xml";
		    }
		    else
		    {
				  filename = CommonOpn.ResolvePhysicalPath("/Content/") + "rt.xml";
		    }
			
			this.xw = new XmlTextWriter(filename, Encoding.UTF8);
			this.xw.Formatting = Formatting.Indented;
			this.xw.WriteStartDocument();
			this.xw.WriteStartElement("RoadTests");
			
			FetchRoadTests();
			
			this.xw.WriteEndElement();
			this.xw.Flush();
			this.xw.Close();
		}
		
		public void UpdateUpcomingBikes()
		{
			string filename = "";
			
			if ( HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf( "bikewale.com" ) >= 0 )
		    {
				  filename = CommonOpn.ImagePathForSavingImages("/Contents/") + "uc.xml";
		    }
		    else
		    {
				  filename = CommonOpn.ResolvePhysicalPath("/Contents/") + "uc.xml";
		    }
			
			//filename = Server.MapPath("~/Contents/uc.xml");
			
			this.xw = new XmlTextWriter(filename, Encoding.UTF8);
			this.xw.Formatting = Formatting.Indented;
			this.xw.WriteStartDocument();
			this.xw.WriteStartElement("UpcomingBikes");
			
			FetchUpcomingBikes();
			
			this.xw.WriteEndElement();
			this.xw.Flush();
			this.xw.Close();
		}
		
		public void UpdateFeaturedBike()
		{
			string filename = "";
			
			if ( HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf( "bikewale.com" ) >= 0 )
		    {
				  filename = CommonOpn.ImagePathForSavingImages("/Content/") + "FeaturedBikes.xml";
		    }
		    else
		    {
				  filename = CommonOpn.ResolvePhysicalPath("/Content/") + "FeaturedBikes.xml";
		    }
			
			//filename = Server.MapPath("~/Contents/FeaturedBikes.xml");
			//filename = @"Y:\Contents\FeaturedBikes.xml";
			this.xw = new XmlTextWriter(filename, Encoding.UTF8);
			this.xw.Formatting = Formatting.Indented;
			this.xw.WriteStartDocument();
			this.xw.WriteStartElement("FeaturedBikes");
			
			FetchFeaturedBikes();
			
			this.xw.WriteEndElement();
			this.xw.Flush();
			this.xw.Close();
		}
		
		public void UpdateComparisons()
		{
			string filename = "";
			
			if ( HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf( "bikewale.com" ) >= 0 )
		    {
				  filename = CommonOpn.ImagePathForSavingImages("/Content/") + "comparisons.xml";
		    }
		    else
		    {
				  filename = CommonOpn.ResolvePhysicalPath("/Content/") + "comparisons.xml";
		    }
			
			this.xw = new XmlTextWriter(filename, Encoding.UTF8);
			this.xw.Formatting = Formatting.Indented;
			this.xw.WriteStartDocument();
			this.xw.WriteStartElement("ComparisonBikes");
			
			FetchComparisonBikes();
			
			this.xw.WriteEndElement();
			this.xw.Flush();
			this.xw.Close();
		}
		
		private void FetchRoadTests()
		{
            //string sql = " SELECT TOP 5 CRT.*, CMA.Name + ' ' + CMO.Name + ' ' + ISNULL(CV.Name, '') AS Bike  "
            //           + " FROM	Con_RoadTest CRT "
            //           + " LEFT JOIN BikeVersions CV"
            //           + " ON CRT.BikeVersionId = CV.Id"
            //           + " LEFT JOIN BikeModels CMO"
            //           + " ON CRT.BikeModelId = CMO.Id"
            //           + " LEFT JOIN BikeMakes CMA"
            //           + " ON CMO.BikeMakeId = CMA.Id"
            //           + " WHERE CRT.IsActive = 1 AND CRT.IsPublished = 1"
            //           + " ORDER BY CRT.DisplayDate DESC";
					   
            //Database db = new Database();
            //SqlDataReader dr = null;
            //try
            //{
            //    dr = db.SelectQry(sql);
            //    while(dr.Read())
            //    {
            //        AddRoadTest(dr["Id"].ToString(),
            //                    dr["Bike"].ToString(),
            //                    dr["BikeModelId"].ToString(),
            //                    dr["BikeVersionId"].ToString(),
            //                    dr["Title"].ToString(),
            //                    dr["AuthorName"].ToString(),
            //                    dr["MainImgPath"].ToString(),
            //                    dr["Description"].ToString()	
            //        );	
            //    }
            //}
            //catch(Exception ex)
            //{
            //    //Response.Write(ex.Message);
            //    ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.ConsumeError();
            //}
            //finally
            //{
            //    dr.Close();
            //    db.CloseConnection();
            //}		   	

		}
		
		private void AddRoadTest(string id, string bike, string bikeModelId, string bikeVersionId, string title, string author, string mainImgPath, string description)
		{
			this.xw.WriteStartElement("RoadTest");
			
			this.xw.WriteStartElement("Id");
			this.xw.WriteString(id);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("Bike");
			this.xw.WriteString(bike);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("BikeModelId");
			this.xw.WriteString(bikeModelId);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("BikeVersionId");
			this.xw.WriteString(bikeVersionId);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("Title");
			this.xw.WriteCData(title);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("Author");
			this.xw.WriteCData(author);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("MainImgPath");
			this.xw.WriteCData(mainImgPath);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("Description");
			if (description.Length > 200)
			{
				this.xw.WriteCData(description.Substring(0, description.Substring(0, 200).LastIndexOf(" ")));
			}
			else
			{
				this.xw.WriteCData(description);
			}
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("DetailUrl");
			this.xw.WriteCData("/research/roadtests/view.aspx?id=" + id);
			this.xw.WriteEndElement();
			
			this.xw.WriteEndElement();
		}	
		
		private void FetchUpcomingBikes()
		{
            throw new Exception("Method not used/commented");

            //string sql  = " SELECT	"
            //            + " Top 5 ECL.*, CMA.Name + ' ' + ECL.ModelName AS Bike"
            //            + " FROM	"
            //            + " ExpectedBikeLaunches ECL, BikeMakes CMA"
            //            + " WHERE	"
            //            + " ECL.IsLaunched = 0"
            //            + " AND ECL.BikeMakeId = CMA.ID"
            //            + " ORDER	BY ECL.Sort ";
					   
            //Database db = new Database();
            //SqlDataReader dr = null;
            //try
            //{
            //    dr = db.SelectQry(sql);
            //    while(dr.Read())
            //    {
            //        AddUpcomingBike(dr["Id"].ToString(),
            //                    dr["Bike"].ToString(),
            //                    dr["BikeMakeId"].ToString(),
            //                    dr["ModelName"].ToString(),
            //                    dr["PhotoName"].ToString(),
            //                    dr["Description"].ToString(),
            //                    dr["ExpectedLaunch"].ToString(),
            //                    dr["EstimatedPrice"].ToString()
            //        );	
            //    }
            //}
            //catch(Exception ex)
            //{
            //    ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    dr.Close();
            //    db.CloseConnection();
            //}		   	

		}
		
		private void AddUpcomingBike(string id, string bike, string bikeMakeId, string modelName, string photoName, string description, string expectedLaunch, string estimatedPrice)
		{
			this.xw.WriteStartElement("UpcomingBike");
			
			this.xw.WriteStartElement("Id");
			this.xw.WriteString(id);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("Bike");
			this.xw.WriteString(bike);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("BikeMakeId");
			this.xw.WriteString(bikeMakeId);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("ModelName");
			this.xw.WriteString(modelName);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("PhotoName");
			this.xw.WriteString(photoName);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("Description");
			if (description.Length > 175)
			{
				this.xw.WriteCData(description.Substring(0, description.Substring(0, 175).LastIndexOf(" ")));
			}
			else
			{
				this.xw.WriteCData(description);
			}
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("DetailUrl");
			this.xw.WriteCData("/new/upcoming/upcomingbikes.aspx?id=" + id);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("ExpectedLaunch");
			this.xw.WriteString(expectedLaunch);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("EstimatedPrice");
			this.xw.WriteString(estimatedPrice);
			this.xw.WriteEndElement();
			
			this.xw.WriteEndElement();
			
		}	
		
		private void FetchFeaturedBikes()
		{
            throw new Exception("Method not used/commented");

        //    string sql = "";
        //    CheckForModel();
			
        //    if (isModel == "1")
        //    {
        //        sql = " SELECT FL.ID, CMO.Id AS ModelId, CMA.Name AS MakeName, CMO.Name AS ModelName, '' AS VersionName,"
        //            + " IsModel, Description, ShowResearch, ShowPrice, ShowRoadTest, Link"
        //            + " FROM Con_FeaturedListings AS FL, BikeMakes AS CMA, BikeModels AS CMO"
        //            + " WHERE FL.BikeId = CMO.Id AND CMO.BikeMakeId = CMA.Id AND FL.IsModel = 1"
        //            + " AND FL.ID = " + listingId;
        //    }
        //    else
        //    {
        //        sql = " SELECT FL.ID, CV.Id AS ModelId, CMA.Name AS MakeName, CMO.Name AS ModelName, CV.Name AS VersionName,"
        //            + " IsModel, Description, ShowResearch, ShowPrice, ShowRoadTest, Link"
        //            + " FROM Con_FeaturedListings AS FL, BikeMakes AS CMA, BikeModels AS CMO, BikeVersions AS CV"
        //            + " WHERE FL.BikeId = CV.Id AND CV.BikeModelId = CMO.Id AND CMO.BikeMakeId = CMA.Id AND FL.IsModel = 0"
        //            + " AND FL.ID = " + listingId;
        //    }
		
        //    Database db = new Database();
        //    SqlDataReader dr = null;
        //    try
        //    {
        //        dr = db.SelectQry(sql);
        //        if(dr.Read())
        //        {
        //            AddFeaturedBike(dr["id"].ToString(),
        //                           dr["MakeName"].ToString() + " " + dr["ModelName"].ToString() + " " + dr["VersionName"].ToString(),
        //                           dr["Description"].ToString(),
        //                           Convert.ToBoolean(dr["IsModel"]),
        //                           Convert.ToBoolean(dr["ShowResearch"]),	
        //                           Convert.ToBoolean(dr["ShowPrice"]),
        //                           Convert.ToBoolean(dr["ShowRoadTest"]),
        //                           dr["Link"].ToString(),
        //                           dr["MakeName"].ToString(),
        //                           dr["ModelName"].ToString(),
        //                           dr["VersionName"].ToString(),
        //                           dr["ModelId"].ToString()		
        //            );	
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        //Response.Write(ex.Message);
        //        ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.ConsumeError();
        //    }
        //    finally
        //    {
        //        dr.Close();
        //        db.CloseConnection();
        //    }
        //}
		
        //private void CheckForModel()
        //{
        //    isModel = "";
        //    listingId = "";
        //    string sql = "SELECT TOP 1 Id, IsModel FROM Con_FeaturedListings WHERE IsVisible = 1 AND IsActive = 1 ORDER BY ID DESC";
		
        //    Database db = new Database();
        //    SqlDataReader dr = null;
			
        //    try
        //    {
        //        dr = db.SelectQry(sql);
        //        if (dr.Read())
        //        {
        //            if (Convert.ToBoolean(dr["IsModel"]))
        //                isModel = "1";
        //            else
        //                isModel = "0";
						
        //            listingId = dr["Id"].ToString();			
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.ConsumeError();
        //    }
        //    finally
        //    {
        //        dr.Close();
        //        db.CloseConnection();
        //    }	
		}
		
		private void AddFeaturedBike(string id, string bike, string description, bool isModel, bool showResearch, bool showPrice, bool showRoadTest, string link, string makeName, string modelName, string versionName, string modelId)
		{
			this.xw.WriteStartElement("FeaturedBike");	
			
			this.xw.WriteStartElement("ListingId");	
			this.xw.WriteString(id);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("BikeName");	
			this.xw.WriteString(bike);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("Description");	
			this.xw.WriteString(description);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("IsModel");
			if (isModel)	
				this.xw.WriteString("1");
			else
				this.xw.WriteString("0");
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("ShowResearch");	
			if (showResearch)	
				this.xw.WriteString("1");
			else
				this.xw.WriteString("0");
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("ShowPrice");	
			if (showPrice)	
				this.xw.WriteString("1");
			else
				this.xw.WriteString("0");
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("ShowRoadTest");	
			if (showRoadTest)	
				this.xw.WriteString("1");
			else
				this.xw.WriteString("0");
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("Link");	
			this.xw.WriteString(link);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("MakeName");	
			this.xw.WriteString(makeName);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("ModelName");	
			this.xw.WriteString(modelName);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("VersionName");	
			this.xw.WriteString(versionName);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("ModelId");	
			this.xw.WriteString(modelId);
			this.xw.WriteEndElement();
			
			this.xw.WriteEndElement();
		}
		
		private void FetchComparisonBikes()
		{
            throw new Exception("Method not used/commented");

            //string sql 	= " SELECT	TOP 3 "
            //            + " CCL.VersionId1 AS Bike1, "
            //            + " CCL.VersionId2 AS Bike2,"
            //            + " CMA.Name + ' ' + CMO.Name AS BikeName1, "
            //            + " CMAK.Name + ' ' + CMOD.Name AS BikeName2 "
            //            + " FROM	"
            //            + " Con_BikeComparisonList CCL, "
            //            + " BikeVersions CV, BikeModels CMO, BikeMakes CMA,"
            //            + " BikeVersions CVE, BikeModels CMOD, BikeMakes CMAK"
            //            + " WHERE	"
            //            + " CCL.IsActive = 1"
            //            + " AND		CCL.VersionId1 = CV.ID "
            //            + " AND		CV.BikeModelId = CMO.ID"
            //            + " AND		CMO.BikeMakeId = CMA.ID"
            //            + " AND		CCL.VersionId2 = CVE.ID "
            //            + " AND		CVE.BikeModelId = CMOD.ID"
            //            + " AND		CMOD.BikeMakeId = CMAK.ID"
            //            + " ORDER	BY CCL.EntryDate DESC";
					   
            //Database db = new Database();
            //SqlDataReader dr = null;
            //try
            //{
            //    dr = db.SelectQry(sql);
            //    while(dr.Read())
            //    {
            //        AddComparisonBike(dr["Bike1"].ToString(),
            //                         dr["Bike2"].ToString(),
            //                         dr["BikeName1"].ToString(),
            //                         dr["BikeName2"].ToString()	
            //                        );	
            //    }
            //}
            //catch(Exception ex)
            //{
            //    ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.ConsumeError();
            //}
            //finally
            //{
            //    dr.Close();
            //    db.CloseConnection();
            //}		   	
		}
		
		private void AddComparisonBike(string bike1, string bike2, string bikeName1, string bikeName2)
		{
			this.xw.WriteStartElement("ComparisonBike");
			
			this.xw.WriteStartElement("Bike1");
			this.xw.WriteString(bike1);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("Bike2");
			this.xw.WriteString(bike2);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("BikeName1");
			this.xw.WriteString(bikeName1);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("BikeName2");
			this.xw.WriteString(bikeName2);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("DetailUrl");
			this.xw.WriteCData("/content/comparebikes/bikecomparison.aspx?bike1="+ bike1 +"&bike2=" + bike2);
			this.xw.WriteEndElement();
	
			this.xw.WriteEndElement();
		}	
		
		public void UpdateNewLaunches()
		{
			string filename = "";
			
			if ( HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf( "bikewale.com" ) >= 0 )
		    {
				  filename = CommonOpn.ImagePathForSavingImages("/Content/") + "nl.xml";
		    }
		    else
		    {
				  filename = CommonOpn.ResolvePhysicalPath("/Content/") + "nl.xml";
		    }
			
			this.xw = new XmlTextWriter(filename, Encoding.UTF8);
			this.xw.Formatting = Formatting.Indented;
			this.xw.WriteStartDocument();
			this.xw.WriteStartElement("NewLaunches");
			
			FetchNewLaunches();
			
			this.xw.WriteEndElement();
			this.xw.Flush();
			this.xw.Close();
		}
		
		private void FetchNewLaunches()
		{
            throw new Exception("Method not used/commented");

            //string sql 	= " SELECT Top 3	"
            //            + " NL.Id, CMA.Name AS Make, CMO.Name AS Model, NL.ModelId, CMO.SmallPic"
            //            + " FROM"
            //            + " NewLaunches NL, BikeModels CMO, BikeMakes CMA"
            //            + " WHERE"
            //            + " NL.ModelId = CMO.Id"
            //            + " AND CMO.BikeMakeId = CMA.ID"
            //            + " ORDER BY Id DESC";
					   
            //Database db = new Database();
            //SqlDataReader dr = null;
            //try
            //{
            //    dr = db.SelectQry(sql);
            //    while(dr.Read())
            //    {
            //        AddNewLaunch(dr["Id"].ToString(),
            //                     dr["ModelId"].ToString(),
            //                     dr["Make"].ToString(),
            //                     dr["Model"].ToString(),
            //                     dr["SmallPic"].ToString()	
            //                    );	
            //    }
            //}
            //catch(Exception ex)
            //{
            //    ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.ConsumeError();
            //}
            //finally
            //{
            //    dr.Close();
            //    db.CloseConnection();
            //}		   	
		}
		
		private void AddNewLaunch(string id, string modelId, string make, string model, string smallPic)
		{
			this.xw.WriteStartElement("NewLaunch");
			
			this.xw.WriteStartElement("Id");
			this.xw.WriteString(id);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("ModelId");
			this.xw.WriteString(modelId);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("Make");
			this.xw.WriteString(make);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("Model");
			this.xw.WriteString(model);
			this.xw.WriteEndElement();
			
			this.xw.WriteStartElement("SmallPic");
			this.xw.WriteString(smallPic);
			this.xw.WriteEndElement();
			
			this.xw.WriteEndElement();
		}	
		
		
	}
}		