using Carwale.Entity.XmlFeed;
using Carwale.Interfaces;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;

namespace Carwale.Service.Controllers
{
    public class XMLFeedsController : ApiController
    {
        private readonly IUnityContainer _container;
        public XMLFeedsController(IUnityContainer container)
        {
            _container = container;
        }
        public HttpResponseMessage GetCarModels()
        {
            IXmlFeed xmlFeed = _container.Resolve<IXmlFeed>("ModelFeeds");
            List<url> modelFeed = xmlFeed.GenerateXmlFeed();

            return GetFeedResponse(modelFeed);
        }

        public HttpResponseMessage GetCarMakes()
        {
            IXmlFeed xmlFeed = _container.Resolve<IXmlFeed>("MakeFeeds");
            List<url> modelFeed = xmlFeed.GenerateXmlFeed();

            return GetFeedResponse(modelFeed);
        }

        private HttpResponseMessage GetFeedResponse(List<url> feedList)
        {
            var response = new HttpResponseMessage();
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd\">");
            foreach (var s in feedList)
            {
                sb.Append("<url><loc>");
                sb.Append(s.loc.ToString());
                sb.Append("</loc></url>");
            }
            sb.Append("</urlset>");

            response.Content = new StringContent(sb.ToString(), Encoding.UTF8, "text/xml");

            return response;
        }       
        private HttpResponseMessage GetSociomanticFeedResponse(List<SociomanticProduct> modelfeedList)
        {
            var response = new HttpResponseMessage();
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode dataNode = xmlDoc.CreateElement("data");
            xmlDoc.AppendChild(dataNode);
            foreach (var feed in modelfeedList)
            {
                XmlNode productNode = xmlDoc.CreateElement("product");
                dataNode.AppendChild(productNode);                
                productNode.AppendChild(GetXmlNode(xmlDoc, "identifier", feed.Id.ToString()));
                productNode.AppendChild(GetXmlNode(xmlDoc, "fn", feed.Title));
                productNode.AppendChild(GetXmlNode(xmlDoc, "description", feed.Description));
                productNode.AppendChild(GetXmlNode(xmlDoc, "category", feed.Category));
                productNode.AppendChild(GetXmlNode(xmlDoc, "brand", feed.Brand));
                productNode.AppendChild(GetXmlNode(xmlDoc, "valid", "0"));
                productNode.AppendChild(GetXmlNode(xmlDoc, "price", feed.SalePrice.ToString()));
                productNode.AppendChild(GetXmlNode(xmlDoc, "amount", feed.RegularPrice.ToString()));
                productNode.AppendChild(GetXmlNode(xmlDoc, "currency", "INR"));
                productNode.AppendChild(GetXmlNode(xmlDoc, "url", feed.Url));
                productNode.AppendChild(GetXmlNode(xmlDoc, "photo", feed.ImageUrl));
            }
            response.Content = new StringContent(xmlDoc.InnerXml, Encoding.UTF8, "text/xml");

            return response;
        }

        private XmlNode GetXmlNode(XmlDocument xmlDoc,string key,string value)
        {
            XmlNode node = xmlDoc.CreateElement(key);
            node.InnerText = value;
            return node;
        }

        [HttpGet, Route("api/feed/sociomantic/")]
        public HttpResponseMessage GetSociomanticFeedForModels()
        {
            IXmlFeed xmlFeed = _container.Resolve<IXmlFeed>("ModelFeeds");
            List<SociomanticProduct> modelFeeds = xmlFeed.GenerateSociomanticXmlFeed();
            return GetSociomanticFeedResponse(modelFeeds);
        }
    }
}
