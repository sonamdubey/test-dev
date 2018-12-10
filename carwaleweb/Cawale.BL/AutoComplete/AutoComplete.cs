using Carwale.DTOs;
using Carwale.DTOs.Autocomplete;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Carwale.Utility;
using Carwale.Entity.Enum;
using System.Linq;
using Carwale.Notifications.Logs;

namespace Carwale.BL.AutoComplete
{
    public class AutoComplete
    {
        private static string cityIndex;
        private static string carIndex;
        private static string _areaIndex;
        private static string UsedCarCityIndex;
        
        static AutoComplete()
        {
            UsedCarCityIndex = System.Configuration.ConfigurationManager.AppSettings["ElasticIndexName"];
            cityIndex = System.Configuration.ConfigurationManager.AppSettings["AutocompleteCityIndex"];
            carIndex = System.Configuration.ConfigurationManager.AppSettings["AutocompleteCarIndex"];
            _areaIndex = System.Configuration.ConfigurationManager.AppSettings["AutocompleteAreaIndex"];
        }

        public List<LabelValueDTO> GetResults(NameValueCollection nvc)
        {
            List<LabelValueDTO> resultList = null;
            try
            {
                ESAutoComplete _esAuto = new ESAutoComplete();
                {
                    string source = nvc["source"], term = nvc["value"];

                    term = term.Trim();
                    term = term.Replace("-", " ");
                    term = Regex.Replace(term, "[^/\\-0-9a-zA-Z\\s]*", "");
                    string _size = nvc["size"];
                    string sourceId = nvc["SourceId"];
                    int noOfRecords;
                    bool pqModel = nvc["p"] != null;
                    bool isNcf = nvc["isNcf"] == "1";
                    string _showFeaturedCar = nvc["showFeaturedCar"];
                    bool showFeaturedCar = !string.IsNullOrWhiteSpace(_showFeaturedCar) ? Boolean.Parse(_showFeaturedCar) : false;
                    if (String.IsNullOrWhiteSpace(term))
                        return new List<LabelValueDTO>();

                    if (source.Equals("1") || source.Equals("2"))
                    {
                        List<SuggestionTypeEnum> types = GetTypes(nvc);
                        bool isModel = source.Equals("1");
                        var platformId = !string.IsNullOrEmpty(sourceId) ? int.Parse(sourceId) : 1;
                        int size = !string.IsNullOrEmpty(_size) ? Convert.ToInt32(_size) : 10;
                        resultList = _esAuto.GetCarSuggestion(term, types, isModel, carIndex, platformId, size, pqModel, showFeaturedCar,isNcf);                       
                    }
                    else
                    {
                        if (source.Equals("3"))
                        {

                            ESAutoComplete _esCitySuggest = new ESAutoComplete();
                            int sourceID;
                            int.TryParse(nvc["sourceIDHeader"], out sourceID);

                            if (!string.IsNullOrEmpty(_size))
                            {
                                Int32.TryParse(_size, out noOfRecords);
                                resultList = _esCitySuggest.GetCitySuggestionForUsedCars(term, cityIndex, sourceID.IsFromApps(), noOfRecords);
                            }
                            else
                            {
                                resultList = _esCitySuggest.GetCitySuggestionForUsedCars(term, cityIndex, sourceID.IsFromApps());
                            }
                        }
                        else if (source.Equals("6") || (!source.Equals("7") && !source.Equals("9")))
                        {
                            ESAutoComplete _esCitySuggest = new ESAutoComplete();
                            {
                                int sourceID;
                                int.TryParse(nvc["sourceIDHeader"], out sourceID);

                                if (!string.IsNullOrEmpty(_size))
                                {
                                    Int32.TryParse(_size, out noOfRecords);
                                    resultList = _esCitySuggest.GetCitySuggestion(term, cityIndex, sourceID.IsFromApps(), noOfRecords);
                                }
                                else
                                    resultList = _esCitySuggest.GetCitySuggestion(term, cityIndex, sourceID.IsFromApps());
                            }
                        }
                        else if (source.Equals("7"))
                        {
                            var cityId = nvc["cityId"];
                            ESAutoComplete _esAreaSuggest = new ESAutoComplete();
                            var context = new List<string>();
                            context.Add(cityId);

                            if (!string.IsNullOrEmpty(_size))
                            {
                                Int32.TryParse(_size, out noOfRecords);
                                resultList = _esAreaSuggest.GetAreaSuggestion(term, context, _areaIndex, noOfRecords);
                            }
                            else
                            {
                                resultList = _esAreaSuggest.GetAreaSuggestion(term, context, _areaIndex);
                            }
                        }
                        else if (source.Equals("9"))
                        {
                            ESAutoComplete _esCarRootSuggest = new ESAutoComplete();
                            List<string> context = new List<string>();
                            context.Add("tyre");

                            if (!string.IsNullOrEmpty(_size))
                            {
                                Int32.TryParse(_size, out noOfRecords);
                                resultList = _esCarRootSuggest.GetCarRootSuggestion(term, context, carIndex, Convert.ToInt32(noOfRecords));
                            }
                            else
                            {
                                resultList = _esCarRootSuggest.GetCarRootSuggestion(term, context, carIndex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return resultList;
        }
        private List<SuggestionTypeEnum> GetTypes(NameValueCollection nvc)
        {
            var types = new List<SuggestionTypeEnum>();
            string typesGroup = nvc["t"];
            if (nvc["n"] == null && nvc["p"] == null)
            {
                types.Add(SuggestionTypeEnum.Make);
                types.Add(SuggestionTypeEnum.NewModel);
                types.Add(SuggestionTypeEnum.UpcomingModel);
            }
            else
            {
                if (nvc["n"] != null)
                {
                     int sourceId;
                     int.TryParse(nvc["sourceIDHeader"], out sourceId);
                     if (typesGroup != null && (typesGroup.Equals("2,4") || typesGroup.Equals("1,2,4")) && sourceId.IsFromApps())
                     {
                         types.Add(SuggestionTypeEnum.NewModel);
                     }
                    else if (typesGroup != null && (typesGroup.Equals("2,4") || typesGroup.Equals("1,2,4")))
                    {
                        types.Add(SuggestionTypeEnum.NewModel);
                        types.Add(SuggestionTypeEnum.UpcomingModel);
                    }
                    else if(typesGroup != null && typesGroup.Equals("2,4,9"))
                    {
                        types.Add(SuggestionTypeEnum.NewModel);
                        types.Add(SuggestionTypeEnum.DiscontinuedModel);
                    }
                    else 
                    {
                        types.Add(SuggestionTypeEnum.Make);
                        types.Add(SuggestionTypeEnum.NewModel);
                        types.Add(SuggestionTypeEnum.UpcomingModel);
                    }
                }
                if (nvc["p"] != null)
                {
                    types.Add(SuggestionTypeEnum.NewModel);
                }
            }          
            if (typesGroup != null && typesGroup.Contains("8")) types.Add(SuggestionTypeEnum.Link);
            types = types.Distinct().OrderBy(x => (int)x).ToList();
            return types;
        }

        public T GetSuggestObject<T>(NameValueCollection nvc) where T : new()
        {
            dynamic result = new T();

            try
            {
                var source = nvc["source"];
                var suggestions = GetResults(nvc);

                if (source.Equals("1") || source.Equals("2"))
                {
                    var newsfilter = nvc["newsfilter"];
                    suggestions.ForEach(item => result.Add(FormatToPayload(item, newsfilter)));
                    return result;
                }

                result = suggestions;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetCarSuggestion");
                objErr.LogException();
            }
            return result;
        }

        private Suggest FormatToPayload(LabelValueDTO item,string newsFilter)
        {
            Suggest suggest = new Suggest();
            suggest.Result = item.Label;


            string[] keys = item.Value.Split('|');
            int keysCount = keys.Length;
            string[] makeInfo = keysCount > 0 ? keys[0].Split(':') : null;
            string[] modelInfo = keysCount > 1 ? keys[1].Split(':') : null;
            
            var displayName = item.Label;
            if (!(keysCount > 1 && modelInfo.Length > 1) && newsFilter != null && newsFilter.Equals("1")) 
            {
                suggest.Result = item.Label.Replace("All ", string.Empty).Replace(" Cars", string.Empty).Trim();
            }
            
            suggest.Payload = new CarResultsDTO()
            {
                MakeName = keysCount > 0 && makeInfo.Length > 0 ? makeInfo[0] : string.Empty,
                MakeId = keysCount > 0 && makeInfo.Length > 1 ? makeInfo[1] : string.Empty,
                ModelName = keysCount > 1 && modelInfo.Length > 1 ? modelInfo[0] : string.Empty,
                ModelId = keysCount > 1 && modelInfo.Length > 1 ? modelInfo[1] : string.Empty
            };            
            return suggest;
        }


        
    }
}
