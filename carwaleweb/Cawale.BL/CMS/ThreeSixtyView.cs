using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.ThreeSixtyView;
using Carwale.Entity.Enum;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Carwale.BL.CMS
{
    public class ThreeSixtyView : IThreeSixtyView
    {
        private readonly string _cdnHostUrl = ConfigurationManager.AppSettings["CDNHostURL"];
        public static readonly List<int> _exteriorExceptionModels = ConfigurationManager.AppSettings["Exterior360ExceptionModels"].Split(',').Select(Int32.Parse).ToList();
        private readonly string _360CacheKey = "three-sixty-config-{0}-{1}-view";
        private readonly string _360ExteriorXMLCacheKey = "three-sixty-xml-{0}-{1}-{2}-{3}-{4}-view";
        private readonly string _360InteriorXMLCacheKey = "three-sixty-xml-{0}-{1}-{2}-view-hotspot-{3}-{4}";
        private readonly ICacheManager _cacheCore;
        private readonly ICarModelCacheRepository _carModelCache;
        private readonly IThreeSixtyCache _cacheThreeSixty;

        public ThreeSixtyView(ICacheManager cachecore, ICarModelCacheRepository carModelCache, IThreeSixtyCache cacheThreeSixty)
        {
            _cacheCore = cachecore;
            _carModelCache = carModelCache;
            _cacheThreeSixty = cacheThreeSixty;
        }

        public ThreeSixty GetExterior360Config(int modelId, ThreeSixtyViewCategory category, string qualityFactor, int imageCount)
        {
            try
            {
                string threeSixtyCategory = category.ToString().ToLower();
                ThreeSixty threeSixty = _cacheCore.GetFromCache<ThreeSixty>(string.Format(_360CacheKey, modelId, threeSixtyCategory));

                if (threeSixty == null)
                {
                    ThreeSixtyDTO threeSixtyDTO = new ThreeSixtyDTO();
                    threeSixtyDTO.ModelDetails = _carModelCache.GetModelDetailsById(modelId);
                    HotspotData data = _cacheThreeSixty.GetHotspots(modelId, category);
                    threeSixty = new ThreeSixty();

                    if (threeSixtyDTO.ModelDetails != null && CMSCommon.CheckCategoryAvailable(threeSixtyDTO.ModelDetails, category) && data.TotalImages>0)
                    {
                        threeSixty.ExteriorImages = new List<string>();

                        string rootPath = string.Format("/cw/360/{0}/{1}/{2}-door/", Format.FormatSpecial(threeSixtyDTO.ModelDetails.MakeName), modelId, threeSixtyCategory);

                        threeSixty.PreviewImage = string.Format("{0}1.jpg?q=20&wm=1{1}", rootPath, string.IsNullOrWhiteSpace(data.ImageVersion) ? string.Empty : ("&v=" + data.ImageVersion));
                        for (int i = 1; i <= data.TotalImages; i++) {
                            threeSixty.ExteriorImages.Add(string.Format("{0}{1}.jpg?wm=1&q=80{2}", rootPath, i, string.IsNullOrWhiteSpace(data.ImageVersion) ? string.Empty : ("&v=" + data.ImageVersion)));
                        }

                        threeSixty.HostUrl = _cdnHostUrl;
                        threeSixty.StartIndex = 0;

                        _cacheCore.StoreToCache<ThreeSixty>(string.Format(_360CacheKey, modelId, threeSixtyCategory), CacheRefreshTime.NeverExpire(), threeSixty);
                    }
                }

                if (imageCount != 72)
                {
                    int skipCount;
                    if (_exteriorExceptionModels.Contains(modelId))
                    {
                        skipCount = imageCount <= 20 ? 2 : 1; //since brezza has only 40 images ,its the only exception
                    }
                    else
                    {
                        skipCount = 72 / imageCount;
                    }
                    threeSixty.ExteriorImages = threeSixty.ExteriorImages.Where((image, index) => index % skipCount == 0).ToList();
                }

                return threeSixty;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return null;
            }
        }

        public ThreeSixty GetInterior360Config(int modelId, string qualityFactor)
        {
            try
            {
                ThreeSixty threeSixty = _cacheCore.GetFromCache<ThreeSixty>(string.Format(_360CacheKey, modelId, ThreeSixtyViewCategory.Interior.ToString().ToLower()));

                if (threeSixty == null)
                {
                    ThreeSixtyDTO threeSixtyDTO = new ThreeSixtyDTO();
                    threeSixtyDTO.ModelDetails = _carModelCache.GetModelDetailsById(modelId);
                    HotspotData data = _cacheThreeSixty.GetHotspots(modelId, ThreeSixtyViewCategory.Interior);

                    threeSixty = new ThreeSixty();

                    if (threeSixtyDTO.ModelDetails != null && CMSCommon.CheckCategoryAvailable(threeSixtyDTO.ModelDetails, ThreeSixtyViewCategory.Interior) && data != null && data.TotalImages > 0)
                    {
                        threeSixty.InteriorImages = new Dictionary<string, ThreeSixtyViewImage>();
                        threeSixty.CameraAngle = new ThreeSixtyCamera();

                        string rootPath = string.Format("/cw/360/{0}/{1}/interior/m/", Format.FormatSpecial(threeSixtyDTO.ModelDetails.MakeName), modelId);
                        string[] threeSixtyFaces = new string[] { "front", "right", "back", "left", "up", "down" }; //Do not change the order of array
                        for (int index = 0; index < 6; index++)
                        {
                            threeSixty.InteriorImages.Add(threeSixtyFaces[index], new ThreeSixtyViewImage { PreviewImagePath = rootPath + string.Format("{0}_preview.jpg?v={1}&ao=1", index + 1, data.ImageVersion), OriginalImagePath = rootPath + string.Format("{0}.jpg?v={1}&ao=1", index + 1, data.ImageVersion) });
                        }

                        threeSixty.CameraAngle.hLookAt = 0;
                        threeSixty.CameraAngle.vLookAt = 0;
                        
                        threeSixty.HostUrl = _cdnHostUrl;

                        _cacheCore.StoreToCache<ThreeSixty>(string.Format(_360CacheKey, modelId, ThreeSixtyViewCategory.Interior.ToString().ToLower()), CacheRefreshTime.NeverExpire(), threeSixty);
                    }
                }

                qualityFactor = "&q=" + qualityFactor;
                foreach (var keyValuePair in threeSixty.InteriorImages)
                {
                    keyValuePair.Value.OriginalImagePath = string.Format("{0}{1}", keyValuePair.Value.OriginalImagePath, qualityFactor);
                    keyValuePair.Value.PreviewImagePath = string.Format("{0}&q=20", keyValuePair.Value.PreviewImagePath);
                }

                return threeSixty;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return null;
            }
        }

        public string GetExterior360XML(int modelId, ThreeSixtyViewCategory type, bool getHotspots, bool isMsite, int imageCount = 72, int qualityFactor = 80)
        {
            string cachekey = string.Format(_360ExteriorXMLCacheKey, modelId, type.ToString().ToLower(), isMsite.ToString().ToLower(), imageCount,qualityFactor);
            string xml;
            double widthOffset, heightOffset;
            if (!isMsite)
            {
                widthOffset = (double)1280 / 1920;
                heightOffset = (double)720 / 1080;
            }
            else
            {
                widthOffset = (double)860 / 1920;
                heightOffset = (double)484 / 1080;
            }
            xml = _cacheCore.GetFromCache<string>(cachekey);

            if (string.IsNullOrWhiteSpace(xml))
            {
                HotspotData hotSpots = _cacheThreeSixty.GetHotspots(modelId, type);
                StringWriter stream = new StringWriter();
                XmlTextWriter writer = new XmlTextWriter(stream);
                int skipCount = 1;
                writer.WriteStartDocument();
                writer.WriteStartElement("config");
                writer.WriteStartElement("settings");
                writer.WriteStartElement("preloader");
                writer.WriteAttributeString("image", "1.jpg?q=20&wm=1" + (string.IsNullOrWhiteSpace(hotSpots.ImageVersion) ? string.Empty : ("&v=" + hotSpots.ImageVersion)));
                writer.WriteEndElement();
                writer.WriteStartElement("userInterface");
                writer.WriteAttributeString("showZoomButtons", "true");
                writer.WriteAttributeString("showToolTips", "true");
                writer.WriteAttributeString("showHotspotsButton", "true");
                writer.WriteAttributeString("showFullScreenButton", "true");
                writer.WriteAttributeString("showTogglePlayButton", "true");
                writer.WriteAttributeString("showArrows", "true");
                writer.WriteAttributeString("toolbarAlign", "center");
                writer.WriteAttributeString("toolbarBackColor", "#0A0101");
                writer.WriteAttributeString("toolbarHoverColor", "#808285");
                writer.WriteAttributeString("toolbarForeColor", "#A7A9AE");
                writer.WriteAttributeString("toolbarBackAlpha", "1");
                writer.WriteAttributeString("toolbarAlpha", "1");
                writer.WriteAttributeString("showProgressNumbers", "true");
                writer.WriteAttributeString("showFullScreenToolbar", "false");
                writer.WriteAttributeString("fullScreenBackColor", "#FFFFFF");
                writer.WriteEndElement();
                writer.WriteStartElement("control");
                writer.WriteAttributeString("dragSpeed", "0.6");
                writer.WriteAttributeString("doubleClickZooms", "false");
                writer.WriteAttributeString("disableMouseControl", "false");
                writer.WriteAttributeString("showHighresOnFullScreen", "true");
                writer.WriteAttributeString("mouseHoverDrag", "false");
                writer.WriteAttributeString("hideHotspotsOnLoad", "true");
                writer.WriteAttributeString("hideHotspotsOnZoom", "true");
                writer.WriteAttributeString("rowSensitivity", "15");
                writer.WriteAttributeString("dragSensitivity", "80");
                writer.WriteAttributeString("inBrowserFullScreen", "false");
                writer.WriteAttributeString("doubleClickFullscreen", "false");
                writer.WriteEndElement();
                writer.WriteStartElement("rotation");
                writer.WriteAttributeString("firstImage", "0");
                writer.WriteAttributeString("rotatePeriod", "4");
                writer.WriteAttributeString("bounce", "false");
                writer.WriteAttributeString("rotateDirection", "-1");
                writer.WriteAttributeString("forceDirection", "false");
                writer.WriteAttributeString("inertiaRelToDragSpeed", "true");
                writer.WriteAttributeString("inertiaTimeToStop", "50");
                writer.WriteAttributeString("inertiaMaxInterval", "80");
                writer.WriteAttributeString("useInertia", "true");
                writer.WriteAttributeString("flipHorizontalInput", "false");
                writer.WriteAttributeString("bounceRows", "true");
                writer.WriteEndElement();
                writer.WriteEndElement();

                if (getHotspots)
                {
                    writer.WriteStartElement("hotspots");
                    foreach (var hotspot in hotSpots.Hotspots)
                    {
                        writer.WriteStartElement("hotspot");
                        writer.WriteAttributeString("id", hotspot.Key.ToString());
                        writer.WriteAttributeString("renderMode", "3");
                        writer.WriteAttributeString("indicatorImage", ImageSizes._0X0 + "/cw/static/360icons/hotspot-indicator-30x30.png");
                        writer.WriteStartElement("spotinfo");
                        writer.WriteAttributeString("url", "Javascript:Hotspot.onHotspotClick(" + hotspot.Value.HotspotXmlId + ","+ Convert.ToInt16(type) + ")");
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                else
                {
                    writer.WriteStartElement("hotspots");
                    writer.WriteEndElement();
                }

                writer.WriteStartElement("images");
                writer.WriteAttributeString("rows", "1");

                //skipCount = (hotSpots.TotalImages >= 72 && hotSpots.TotalImages % 36 == 0 && isMsite) ? 2 : 1;
                skipCount = imageCount >= 72 || (imageCount < 72 && imageCount >= 36 && (hotSpots.TotalImages % 36 != 0)) ? 1 : ((imageCount < 72 && imageCount >= 36 && (hotSpots.TotalImages % 36 == 0)) ? 2 : ((imageCount >=18 && imageCount < 36 && hotSpots.TotalImages % 36 == 0) ? 4 : 2));
                for (int i = 1; i <= hotSpots.TotalImages; i += skipCount)
                {
                    writer.WriteStartElement("image");
                    writer.WriteAttributeString("src", string.Format("{0}.jpg?wm=1&q={1}{2}", i,qualityFactor, string.IsNullOrWhiteSpace(hotSpots.ImageVersion) ? string.Empty : ("&v=" + hotSpots.ImageVersion)));
                    if (getHotspots && hotSpots.HotspotPositions.ContainsKey(i))
                    {
                        foreach (var hotspot in hotSpots.HotspotPositions[i])
                        {
                            writer.WriteStartElement("hotspot");
                            writer.WriteAttributeString("source", hotspot.HotspotXmlId.ToString());
                            writer.WriteAttributeString("offsetX", (widthOffset * hotspot.PositionX).ToString());
                            writer.WriteAttributeString("offsetY", (heightOffset * hotspot.PositionY).ToString());
                            writer.WriteAttributeString("activateOnClick", "true");
                            writer.WriteEndElement();
                        }
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();

                writer.Flush();
                writer.Close();

                xml = stream.ToString();
                _cacheCore.StoreToCache(cachekey, CacheRefreshTime.NeverExpire(), xml);
            }

            return xml;
        }

        public string GetInterior360XML(int modelId, bool getHotspots, bool isMsite,int qualityFactor)
        {
            string cachekey = string.Format(_360InteriorXMLCacheKey, modelId, ThreeSixtyViewCategory.Interior.ToString().ToLower(), isMsite.ToString().ToLower(), getHotspots.ToString().ToLower(), qualityFactor.ToString().ToLower());
            string xml;

            xml = _cacheCore.GetFromCache<string>(cachekey);

            if (string.IsNullOrWhiteSpace(xml))
            {
                HotspotData hotSpots = _cacheThreeSixty.GetHotspots(modelId, ThreeSixtyViewCategory.Interior);
                MemoryStream memoryStream = new MemoryStream();
                XmlWriterSettings xms = new XmlWriterSettings();
                xms.Encoding = Encoding.UTF8;
                XmlWriter writer = XmlWriter.Create(memoryStream, xms);
                writer.WriteStartDocument();
                writer.WriteStartElement("panorama");
                writer.WriteAttributeString("appversion", "5.1");
                writer.WriteAttributeString("id", "node1");
                writer.WriteAttributeString("apprev", "15722");

                writer.WriteStartElement("input");
                writer.WriteAttributeString("tilescale", isMsite ? "1.005333" : "1.002667");
                writer.WriteAttributeString("levelingroll", "0");
                for (int i = 0; i < 6; i++)
                {
                    writer.WriteAttributeString(string.Format("tile{0}url", i), (i + 1) + ".jpg?"+(string.IsNullOrWhiteSpace(hotSpots.ImageVersion) ? "" : ("v=" + hotSpots.ImageVersion + "&"))+"ao=1&q="+qualityFactor);
                    writer.WriteAttributeString(string.Format("prev{0}url", i), (i + 1) + "_preview.jpg?"+(string.IsNullOrWhiteSpace(hotSpots.ImageVersion) ? string.Empty : ("v=" + hotSpots.ImageVersion + "&"))+"ao=1");
                }
                writer.WriteAttributeString("tilesize", "3750");
                writer.WriteAttributeString("levelingpitch", "0");

                writer.WriteStartElement("preview");
                writer.WriteAttributeString("color", "0x808080");
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteStartElement("view");
                writer.WriteAttributeString("fovmode", "0");
                writer.WriteAttributeString("pannorth", "0");
                writer.WriteStartElement("start");
                writer.WriteAttributeString("pan", "0");
                writer.WriteAttributeString("projection", "4");
                writer.WriteAttributeString("tilt", "0");
                writer.WriteAttributeString("fov", "70");
                writer.WriteEndElement();
                writer.WriteStartElement("flyin");
                writer.WriteAttributeString("pan", "0");
                writer.WriteAttributeString("projection", "9");
                writer.WriteAttributeString("tilt", "-90");
                writer.WriteAttributeString("fov", "170");
                writer.WriteEndElement();
                writer.WriteStartElement("min");
                writer.WriteAttributeString("pan", "0");
                writer.WriteAttributeString("tilt", "-90");
                writer.WriteAttributeString("fov", "5");
                writer.WriteEndElement();
                writer.WriteStartElement("max");
                writer.WriteAttributeString("pan", "360");
                writer.WriteAttributeString("scaletofit", "1");
                writer.WriteAttributeString("tilt", "90");
                writer.WriteAttributeString("fovstereographic", "270");
                writer.WriteAttributeString("fov", "80");
                writer.WriteAttributeString("fovfisheye", "360");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("userdata");
                writer.WriteAttributeString("author", string.Empty);
                writer.WriteAttributeString("tags", string.Empty);
                writer.WriteAttributeString("source", string.Empty);
                writer.WriteAttributeString("longitude", string.Empty);
                writer.WriteAttributeString("description", string.Empty);
                writer.WriteAttributeString("latitude", string.Empty);
                writer.WriteAttributeString("info", string.Empty);
                writer.WriteAttributeString("datetime", "12/04/17 12:16 PM");
                writer.WriteAttributeString("copyright", string.Empty);
                writer.WriteAttributeString("comment", string.Empty);
                writer.WriteAttributeString("customnodeid", string.Empty);
                writer.WriteAttributeString("title", string.Empty);
                writer.WriteEndElement();
                writer.WriteStartElement("hotspots");
                writer.WriteAttributeString("wordwrap", "1");
                writer.WriteAttributeString("width", "180");
                writer.WriteAttributeString("height", "20");
                writer.WriteStartElement("label");
                writer.WriteAttributeString("enabled", "0");
                writer.WriteEndElement();
                writer.WriteStartElement("polystyle");
                writer.WriteAttributeString("handcursor", "1");
                writer.WriteEndElement();

                if (getHotspots)
                {
                    foreach (var hotspot in hotSpots.Hotspots)
                    {
                        writer.WriteStartElement("hotspot");
                        writer.WriteAttributeString("url", string.Empty);
                        writer.WriteAttributeString("target", string.Empty);
                        writer.WriteAttributeString("tilt", hotspot.Value.Tilt.ToString());
                        writer.WriteAttributeString("pan", hotspot.Value.Pan.ToString());
                        writer.WriteAttributeString("id", hotspot.Value.HotspotXmlId.ToString());
                        writer.WriteAttributeString("skinid", "ht_info");
                        writer.WriteAttributeString("title", string.Empty);
                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                writer.WriteStartElement("media");
                writer.WriteEndElement();
                writer.WriteStartElement("transition");
                writer.WriteAttributeString("blendtime", "1");
                writer.WriteAttributeString("softedge", "0");
                writer.WriteAttributeString("zoomout", "0");
                writer.WriteAttributeString("enabled", "0");
                writer.WriteAttributeString("zoomspeed", "0");
                writer.WriteAttributeString("zoomoutpause", "1");
                writer.WriteAttributeString("type", "crossdissolve");
                writer.WriteAttributeString("zoomin", "0");
                writer.WriteAttributeString("zoomfov", "20");
                writer.WriteAttributeString("blendcolor", "0x000000");
                writer.WriteEndElement();
                writer.WriteStartElement("control");
                writer.WriteAttributeString("lockedkeyboard", "0");
                writer.WriteAttributeString("speedwheel", "2");
                writer.WriteAttributeString("lockedkeyboardzoom", "0");
                writer.WriteAttributeString("contextprojections", "0");
                writer.WriteAttributeString("dblclickfullscreen", "0");
                writer.WriteAttributeString("sensitivity", "8");
                writer.WriteAttributeString("hideabout", "0");
                writer.WriteAttributeString("simulatemass", "1");
                writer.WriteAttributeString("invertcontrol", "1");
                writer.WriteAttributeString("contextfullscreen", "0");
                writer.WriteAttributeString("lockedmouse", "0");
                writer.WriteAttributeString("invertwheel", "0");
                writer.WriteAttributeString("rubberband", "0");
                writer.WriteAttributeString("lockedwheel", "0");
                writer.WriteEndElement();
                writer.WriteEndElement();


                writer.Flush();
                writer.Close();

                xml = Encoding.UTF8.GetString(memoryStream.ToArray());
                _cacheCore.StoreToCache(cachekey, CacheRefreshTime.NeverExpire(), xml);
            }

            return xml;
        }
    }
}
