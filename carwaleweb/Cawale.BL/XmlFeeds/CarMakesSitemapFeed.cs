using Carwale.Entity.CarData;
using Carwale.Entity.XmlFeed;
using Carwale.Interfaces;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;

namespace Carwale.BL.XmlFeeds
{
    public class CarMakesSitemapFeed : IXmlFeed
    {
        private readonly ICarMakesCacheRepository _makes;

        public CarMakesSitemapFeed(ICarMakesCacheRepository makes)
        {
            _makes = makes;
        }
        /// <summary>
        /// To Generate Xml Feeds For Makes
        /// Written By : Ashish Verma on 18/2/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<url> GenerateXmlFeed()
        {
            var modelUrlList = new List<url>();
            try
            {
                List<CarMakeEntityBase> makeList = (List<CarMakeEntityBase>)_makes.GetCarMakesByType("new");

                foreach (var make in makeList)
                {
                    modelUrlList.Add(new url()
                    {
                        loc = Utility.ManageCarUrl.CreateMakeUrl(make.MakeName,false,true)
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return modelUrlList;
        }

        List<SociomanticProduct> IXmlFeed.GenerateSociomanticXmlFeed()
        {
            throw new NotImplementedException();
        }
    }
}
