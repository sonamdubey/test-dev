using AutoMapper;
using Carwale.DAL.CoreDAL;
using Carwale.DTOs.Elastic.Autocomplete.Area;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces.Geolocation;
using Carwale.Notifications;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.GeoLocation
{
    public class ElasticLocation: IElasticLocation
    {
        private static string _areaIndex;

        static ElasticLocation()
        {
            _areaIndex = System.Configuration.ConfigurationManager.AppSettings["AutocompleteAreaIndex"];
        }

        public Area GetLocation(double latitude, double longitude)
        {
            Area area = new Area();
            try
            {
                ElasticClient client = ElasticClientInstance.GetInstance();

                var results = client.Search<Area>(s => s.Index(System.Configuration.ConfigurationManager.AppSettings["areaindex"] ?? "locations").Type("area")
                                .Query(q => q.
                                    Bool(b => b
                                        .Filter(f => f
                                            .GeoDistance(n => n.Distance(75.0, DistanceUnit.Kilometers).Location(latitude, longitude).Field(ob => ob.location))

                                             //.GeoDistance(
                                        //       n => n.location,
                                        //       d => d.Distance(75.0, DistanceUnit.Kilometers).Location(latitude, longitude)

                                )))
                                .Sort(sort => sort
                                    .GeoDistance(d => d
                                            .Field(obj => obj.location)
                                            .Unit(DistanceUnit.Kilometers).Order(SortOrder.Ascending).Ascending()
                                            .PinTo(geoLocations: new double[] { latitude, longitude })
                                    //.PinTo(Lat: latitude, Lon: longitude)
                                ))
                                .Size(1)
                            );

                if (results.Documents.Count() > 0)
                {
                    area = results.Documents.First();
                    return area;
                }
                else
                {
                    results = client.Search<Area>(s => s.Index(System.Configuration.ConfigurationManager.AppSettings["areaindex"] ?? "locations").Type("area")
                                    .Query(q => q
                                        .Bool(b => b
                                        .Filter(f => f
                                              .GeoDistance(d => d.Distance(1000.0, DistanceUnit.Kilometers).Location(latitude, longitude).Field(ob => ob.location))

                                        )))
                                    .Sort(sort => sort
                                        .GeoDistance(d => d
                                            .Field(obj => obj.location)
                                            .Unit(DistanceUnit.Kilometers).Order(SortOrder.Ascending).Ascending()
                                            .PinTo(geoLocations: new double[] { latitude, longitude })
                                    ))
                                    .Size(1)
                                );

                    if (results.Documents.Count() > 0)
                    {
                        area = results.Documents.First();
                        return area;
                    }
                }


            }
            catch (Exception ex)
            {
                ExceptionHandler err = new ExceptionHandler(ex, "ElasticLocation.GetLocation() latitude=" + (latitude == null ? "null" : latitude.ToString()) + "longi=" + (longitude.ToString()));
                err.LogException();
            }
            return area;
        }

        public Area GetLocation(int areaId)
        {
            Area area = null;
            try
            {
                ElasticClient client = ElasticClientInstance.GetInstance();


                var results = client.Get<AreaDocument>(areaId, t => t.Index(_areaIndex).Type("areadocument"));

                if (results.Source != null)
                {
                    AreaDocument areaDocuments = results.Source;
                    if (areaDocuments != null)
                    {
                        area = Mapper.Map<AreaPayLoad, Area>(areaDocuments.payload);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler err = new ExceptionHandler(ex, "ElasticLocation.GetLocation() areaId=" + areaId.ToString());
                err.LogException();
            }
            return area;
        }

        public Location FormCompleteLocation(Location locationObj)
        {
            if (locationObj != null && locationObj.AreaId > 0)
            {
                Location locationElastic = Mapper.Map<Area, Location>(GetLocation(locationObj.AreaId));
                if (locationElastic != null)
                {
                    return locationElastic;
                } 
            }
            return locationObj != null ? new Location { CityId = locationObj.CityId, ZoneId =  locationObj.ZoneId } : new Location();
        }

        public static IEnumerable<AreaDocument> GetNearestAreas(double latitude, double longitude, int noOfRecords)
        {
            ElasticClient client = ElasticClientInstance.GetInstance();
            var results = client.Search<AreaDocument>(s => s.Index(System.Configuration.ConfigurationManager.AppSettings["areaindex"] ?? "locations").Type(Types.All)
                .Sort(sort => sort
                    .GeoDistance(d => d
                        .Field("payload.location")
                        .DistanceType(GeoDistanceType.Plane)
                        .Unit(DistanceUnit.Kilometers)
                        .Order(SortOrder.Ascending)
                        .PinTo(new Nest.GeoLocation(latitude, longitude))
                    )
                )
                .Size(noOfRecords)
            );
            return results.Documents;
        }
    }
}
