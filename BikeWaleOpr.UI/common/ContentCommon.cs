using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Text;

namespace BikeWaleOpr.Common
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 12 Mar 2014
    /// Summary : Class contains functions related to 
    /// </summary>
    public class ContentCommon
    {
        public static bool MapCities(string cityName, string cityId, string cwCityName)
        {
            bool added = false;
            string filePath = HttpContext.Current.Server.MapPath("/content/mappingfiles/MappedCityData.xml");

            if (CheckCityXmlFile(cityName, filePath) == false)
            {
                //add a node to the xml file
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(filePath);

                    XmlElement root = doc.DocumentElement;

                    if (cityName != "" && cityId != "")
                    {
                        //now move to solution
                        XmlNode nodeCity = root.SelectSingleNode("cities");

                        //now add the new element for city
                        XmlElement newElem = doc.CreateElement("city");

                        //add the attributes
                        newElem.SetAttribute("OEMCityName", cityName);

                        newElem.SetAttribute("BikewaleCityname", cwCityName);

                        newElem.SetAttribute("BikewaleCityid", cityId);

                        nodeCity.AppendChild(newElem);
                    }

                    // Save the document to a file and auto-indent the output.
                    using (XmlTextWriter writer = new XmlTextWriter(filePath, Encoding.UTF8))
                    {
                        writer.Formatting = Formatting.Indented;
                        doc.Save(writer);
                        writer.Flush();
                        writer.Close();
                    }

                    added = true;
                }
                catch (Exception err)
                {
                    HttpContext.Current.Trace.Warn(err.Message);
                    ErrorClass objErr = new ErrorClass(err, "ContentCommon.MapCities");
                    objErr.SendMail();
                }
            }

            return added;
        }

        public static bool UpdateCities(string cityName, string cityId, string cwCityName)
        {
            bool added = false;
            string filePath = HttpContext.Current.Server.MapPath("/content/mappingfiles/MappedCityData.xml");

            //add a node to the xml file
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(filePath);

                //get all the cities tags
                XmlNodeList list = doc.GetElementsByTagName("city");
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Attributes["OEMCityName"].Value == cityName)
                    {
                        list[i].Attributes["BikewaleCityname"].Value = cwCityName;
                        list[i].Attributes["BikewaleCityid"].Value = cityId;
                        break;
                    }
                }

                // Save the document to a file and auto-indent the output.
                using (XmlTextWriter writer = new XmlTextWriter(filePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    doc.Save(writer);
                    writer.Flush();
                    writer.Close();
                }
                added = true;
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, "ContentCommon.UpdateCities");
                objErr.SendMail();
            }

            return added;
        }

        static bool CheckCityXmlFile(string cityName, string filePath)
        {
            bool found = false;

            //get all the mapped cities and the versions
            using (XmlTextReader xr = new XmlTextReader(filePath))
            {
                xr.WhitespaceHandling = WhitespaceHandling.None;

                while (xr.Read())
                {
                    switch (xr.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (xr.Name)
                            {
                                case "city":
                                    string oemCityName = xr.GetAttribute("OEMCityName");

                                    if (oemCityName == cityName)
                                    {
                                        found = true;
                                        break;
                                    }
                                    break;

                                default:
                                    break;
                            }
                            break;

                        default:
                            break;
                    }
                }

                xr.Close();
            }
            return found;
        }

        public static bool MapBikes(string bikeName, string bikeId, string cwBikeName)
        {
            bool added = false;
            string filePath = HttpContext.Current.Server.MapPath("/content/mappingfiles/MappedBikeData.xml");

            if (CheckBikeXmlFile(bikeName, filePath) == false)
            {
                //add a node to the xml file
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(filePath);

                    XmlElement root = doc.DocumentElement;

                    if (bikeName != "" && bikeId != "")
                    {
                        //now move to solution
                        XmlNode nodeBike = root.SelectSingleNode("bikes");

                        //now add the new element for city
                        XmlElement newElem = doc.CreateElement("bike");

                        //add the attributes
                        newElem.SetAttribute("OEMBikeName", bikeName);

                        newElem.SetAttribute("BikewaleBikeName", cwBikeName);

                        newElem.SetAttribute("BikewaleBikeId", bikeId);

                        nodeBike.AppendChild(newElem);
                    }

                    // Save the document to a file and auto-indent the output.
                    using (XmlTextWriter writer = new XmlTextWriter(filePath, Encoding.UTF8))
                    {
                        writer.Formatting = Formatting.Indented;
                        doc.Save(writer);
                        writer.Flush();
                        writer.Close();
                    }
                    added = true;
                }
                catch (Exception err)
                {
                    HttpContext.Current.Trace.Warn(err.Message);
                    ErrorClass objErr = new ErrorClass(err, "ContentCommon.MapBike");
                    objErr.SendMail();
                }
            }

            return added;
        }


        public static bool UpdateBikes(string bikeName, string bikeId, string cwBikeName)
        {
            bool added = false;
            string filePath = HttpContext.Current.Server.MapPath("/content/mappingfiles/MappedBikeData.xml");

            //add a node to the xml file
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(filePath);

                XmlElement root = doc.DocumentElement;

                //get all the bikes tags
                XmlNodeList list = doc.GetElementsByTagName("bike");
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Attributes["OEMBikeName"].Value == bikeName)
                    {
                        list[i].Attributes["BikewaleBikeName"].Value = cwBikeName;
                        list[i].Attributes["BikewaleBikeId"].Value = bikeId;
                        break;
                    }
                }

                // Save the document to a file and auto-indent the output.
                using (XmlTextWriter writer = new XmlTextWriter(filePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    doc.Save(writer);
                    writer.Flush();
                    writer.Close();
                }
                added = true;
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, "ContentCommon.UpdateBike");
                objErr.SendMail();
            }

            return added;
        }


        static bool CheckBikeXmlFile(string bikeName, string filePath)
        {
            bool found = false;

            //get all the mapped cities and the versions
            using (XmlTextReader xr = new XmlTextReader(filePath))
            {
                xr.WhitespaceHandling = WhitespaceHandling.None;

                while (xr.Read())
                {
                    switch (xr.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (xr.Name)
                            {
                                case "bike":
                                    string oemBikeName = xr.GetAttribute("OEMBikeName");

                                    if (oemBikeName == bikeName)
                                    {
                                        found = true;
                                        break;
                                    }
                                    break;

                                default:
                                    break;
                            }
                            break;

                        default:
                            break;
                    }
                }

                xr.Close();
            }
            return found;
        }

    }
}