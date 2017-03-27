
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using System;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 25-Mar-2017
    /// Model for Used bike cities and bike count
    /// </summary>
    public class UsedBikeCitiesWidgetModel
    {
        private ICityCacheRepository _ICityCache = null;
        public string Title { get; private set; }
        public string Href { get; private set; }
        public UsedBikeCitiesWidgetModel(string title, string href, ICityCacheRepository cache)
        {
            _ICityCache = cache;
            Title = title;
            Href = !string.IsNullOrEmpty(href) ? href : "/bikes-in-india/";
        }
        /// <summary>
        /// Gets the data for used bike cities and count
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
        /// </returns>
        public UsedBikeCitiesWidgetVM GetData()
        {
            UsedBikeCitiesWidgetVM objData = null;
            try
            {
                objData = new UsedBikeCitiesWidgetVM();
                var cities = _ICityCache.GetUsedBikeByCityWithCount();
                objData.Cities = cities.Where(x => x.Priority > 0);
                objData.Title = Title;
                objData.Href = Href;
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "UsedBikeCitiesWidgetModel.GetData()");
            }
            return objData;
        }
    }
}