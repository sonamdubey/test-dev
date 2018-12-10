using Carwale.DAL.CoreDAL;
using Carwale.DTOs.Autocomplete;
using Carwale.DTOs.Elastic;
using Carwale.DTOs.Elastic.Autocomplete.Area;
using Carwale.Notifications;
using Carwale.Utility;
using com.calitha.carsuggest;
using com.calitha.carversussuggest;
using com.calitha.versussuggest;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Carwale.BL.Elastic;
using Carwale.Entity.Enum;
using Carwale.Notifications.Logs;

namespace Carwale.BL.AutoComplete
{

    public class ESAutoComplete
    {
        private static List<string> contextG;
        private static CarSuggestParser carSuggestParser;
        private static VersusSuggestParser versusSuggestParser;
        private static CarVersusSuggestParser carVersusParser;
        private static string VersusSuggestionString = "VERSUS";
        private static string CarSuggestionString = "CAR";
        private static string FirstCarSuggestionString = "CAR1";
        private static string SecondCarSuggestionString = "CAR2";
        private static string LinksSuggestionString = "LINKS";

        static ESAutoComplete()
        {
            carSuggestParser = new CarSuggestParser(new MemoryStream(Properties.Resources.carSuggestion));
            versusSuggestParser = new VersusSuggestParser(new MemoryStream(Properties.Resources.versus));
            carVersusParser = new CarVersusSuggestParser(new MemoryStream(Properties.Resources.CarWithVersus));
        }
        
        public List<LabelValueDTO> GetCarSuggestion(string textValue, List<SuggestionTypeEnum>types, bool isModel, string index, int platformId, int size , bool pqModel, bool showFeaturedCar = false,
            bool isNcf = false)
        {
            List<LabelValueDTO> suggestionlist = null;
            try
            {
                suggestionlist = new List<LabelValueDTO>();

                List<Nest.SuggestOption<ESCarDocument>> _suggestionList = null;
                List<Nest.SuggestOption<ESCarDocument>> _suggestionList2 = null;
                List<Nest.SuggestOption<ESCarDocument>> _linkSuggestionList = null;

                bool versusFlag = false;
                SuggestionBL _suggestion = new SuggestionBL();
                Tuple<List<Nest.SuggestOption<ESCarDocument>>, List<Nest.SuggestOption<ESCarDocument>>, List<Nest.SuggestOption<ESCarDocument>>> _result = _suggestion.GetSuggestionForCars<ESCarDocument>(textValue, ref versusFlag, types, size);
                if (_result == null)
                    return suggestionlist;
                _suggestionList = _result.Item1;
                _suggestionList2 = _result.Item2;
                _linkSuggestionList = _result.Item3;
                int count = 0;
                var modelSuggestions = new List<LabelValueDTO>();
                var versusSuggestions = new List<LabelValueDTO>();
           
                LabelValueDTO output = null;
                if (_suggestionList2.Count == 0)
                {
                    modelSuggestions.AddRange(GetModelSuggestion(_suggestionList, isModel, size, showFeaturedCar));
                    count = modelSuggestions.Count;
                    if ((_suggestionList.Any() || versusFlag) && !pqModel && !platformId.IsFromApps()) 
                    {
                        foreach (var item in _suggestionList)
                        {
                            if (count >= size) break;
                            CarPayLoad payload = item.Source.payload;
                            if (payload.CarComparisionList != null)
                            {
                                List<CarPayLoad> payloadList = payload.CarComparisionList;
                                foreach (CarPayLoad cpl in payloadList)
                                {
                                    if (count >= size)
                                    {
                                        break;
                                    }
                                    output = new LabelValueDTO();
                                    output.Label = item.Source.output + " vs " + cpl.OutputName;
                                    if (isModel)
                                        output.Value = (string.IsNullOrEmpty(cpl.ModelId) || cpl.ModelId.Equals("0")) ? (cpl.MakeName + ":" + cpl.MakeId + "|" + "make") : payload.MakeName + "-" + payload.MaskingName + ":" + payload.ModelId + "|" + cpl.MakeName + "-" + cpl.MaskingName + ":" + cpl.ModelId + (cpl.IsUpcoming ? "|upcoming" : "");
                                    else
                                        output.Value = (string.IsNullOrEmpty(cpl.RootId) || cpl.RootId.Equals("0")) ? cpl.MakeName + ":" + cpl.MakeId : cpl.MakeName + ":" + cpl.MakeId + "|" + cpl.RootName + ":" + cpl.RootId;
                                    versusSuggestions.Add(output);
                                    count++;
                               }
                            }
                        }
                    }
                }
                else if (!pqModel && !platformId.IsFromApps())
                {
                    foreach (var item in _suggestionList)
                    {
                        if (count >= size)
                        {
                            break;
                        }
                        CarPayLoad payload = item.Source.payload;
                        if (payload.ModelId != null && payload.ModelId != "0")
                        {
                            foreach (var item2 in _suggestionList2)
                            {
                                CarPayLoad payload2 = item2.Source.payload;
                                if (payload2.ModelId != null && payload2.ModelId != "0" && payload.ModelId != payload2.ModelId && (!payload.IsUpcoming && !payload2.IsUpcoming))
                                {
                                    if (count >= size)
                                    {
                                        break;
                                    }
                                    output = new LabelValueDTO();
                                    output.Label = item.Source.output + " vs " + item2.Source.output;
                                    if (isModel)
                                        output.Value = (string.IsNullOrEmpty(payload.ModelId) || payload.ModelId.Equals("0")) ? (payload.MakeName + ":" + payload.MakeId + "|" + "make") : payload.MakeName + "-" + payload.MaskingName + ":" + payload.ModelId + "|" + payload2.MakeName + "-" + payload2.MaskingName + ":" + payload2.ModelId ;
                                    else
                                        output.Value = (string.IsNullOrEmpty(payload.RootId) || payload.RootId.Equals("0")) ? payload.MakeName + ":" + payload.MakeId : payload.MakeName + ":" + payload.MakeId + "|" + payload.RootName + ":" + payload.RootId;
                                    versusSuggestions.Add(output);
                                     count++;
                                }
                            }
                        }
                    }
                    if(versusSuggestions.Count == 0)
                    {
                        modelSuggestions.AddRange(GetModelSuggestion(_suggestionList, isModel, size, showFeaturedCar));
                    }
                }
                if (versusFlag)
                {
                    suggestionlist.AddRange(versusSuggestions);
                    if (suggestionlist.Count < size)
                    {
                        suggestionlist.AddRange(modelSuggestions);
                        // condition for ncf Link to add in search result
                        if(isNcf && !pqModel && !platformId.IsFromApps())
                        {
                            AddNcfLink(suggestionlist, modelSuggestions.Count, size);
                        }
                        
                    }
               }
                else if(modelSuggestions.Count > 0)
                {
                    suggestionlist.AddRange(modelSuggestions);
                    // condition for ncf Link to add in search result
                    if (isNcf && !pqModel && !platformId.IsFromApps())
                    {
                        AddNcfLink(suggestionlist, modelSuggestions.Count, size);
                    }
                    suggestionlist.AddRange(versusSuggestions);
                }
                if (_linkSuggestionList.Count > 0 && count < size)
                {
                    foreach (var item in _linkSuggestionList)
                    {
                        if (count >= size) break;
                        CarPayLoad payload = item.Source.payload;
                        if (!string.IsNullOrWhiteSpace(payload.DesktopLink) || !string.IsNullOrWhiteSpace(payload.MobileLink))
                        {
                            output = new LabelValueDTO();
                            output.Label = item.Source.output;
                            output.Value = "desktoplink:" + (payload.DesktopLink ?? string.Empty) + "|mobilelink:" + (payload.MobileLink ?? string.Empty);
                            suggestionlist.Add(output);
                        }
                        count++;
                    }
                }
                if (isNcf && !pqModel && !platformId.IsFromApps() && suggestionlist.Count == 0)
                {
                    LabelValueDTO NCFlink = new LabelValueDTO();
                    NCFlink.Label = "Click here to find the right car for you";
                    NCFlink.Value = "ncfLink:/find-car/";
                    suggestionlist.Add(NCFlink);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return suggestionlist;
        }

        private void AddNcfLink(List<LabelValueDTO> suggestionlist, int ModelCount, int size)
        {
            //Adding NCFlink 
            LabelValueDTO NCFlink = new LabelValueDTO();
            NCFlink.Label = "Click here to find the right car for you";
            NCFlink.Value = "ncfLink:/find-car/";

            if (ModelCount >= 1)
            {
                if (suggestionlist.Count >= size)
                {
                    suggestionlist.Insert(size, NCFlink);
                }
                else
                {
                    suggestionlist.Add(NCFlink);
                }
            }
        }
        private List<LabelValueDTO> GetModelSuggestion(List<Nest.SuggestOption<ESCarDocument>> _suggestionList,bool isModel, int size, bool showFeaturedCar = false)
        {
            var modelSuggestions = new List<LabelValueDTO>();
            LabelValueDTO output = null;
            bool featureCarAdded = false;
            foreach (var item in _suggestionList)
            {
                CarPayLoad payload = item.Source.payload;
                output = new LabelValueDTO();
                output.Label = item.Source.output;
                if (isModel)
                    output.Value = (string.IsNullOrEmpty(payload.ModelId) || payload.ModelId.Equals("0")) ? (payload.MakeName + ":" + payload.MakeId + "|" + "make") : payload.MakeName + ":" + payload.MakeId + "|" + payload.MaskingName + ":" + payload.ModelId + (payload.IsUpcoming ? "|upcoming" : "");
                else
                    output.Value = (string.IsNullOrEmpty(payload.RootId) || payload.RootId.Equals("0")) ? payload.MakeName + ":" + payload.MakeId : payload.MakeName + ":" + payload.MakeId + "|" + payload.RootName + ":" + payload.RootId;

                modelSuggestions.Add(output);
                if (showFeaturedCar && !(featureCarAdded) && payload.FeaturedCar != null && payload.FeaturedCar.ModelId > 0)
                {
                    output = new LabelValueDTO();
                    output.Label = payload.FeaturedCar.OutputName;
                    output.Value = string.Format(payload.FeaturedCar.IsUpcoming ? "{0}:{1}|{2}:{3}|upcoming|sponsor|{4}" : "{0}:{1}|{2}:{3}|sponsor|{4}", Format.FormatURL(payload.FeaturedCar.MakeName), payload.FeaturedCar.MakeId, payload.FeaturedCar.MaskingName, payload.FeaturedCar.ModelId, (modelSuggestions.Count + 1));
                    modelSuggestions.Add(output);
                    featureCarAdded = true;
                }
                if (modelSuggestions.Count >= size)
                {
                    break;
                }
            }
            return modelSuggestions;
        }
        public List<LabelValueDTO> GetCitySuggestionForUsedCars(string textValue, string index, bool isApp = false, int size = 10)
        {
            List<LabelValueDTO> suggestionlist = null;
            try
            {
                suggestionlist = new List<LabelValueDTO>();
                var suggestions = GetSuggestion<CitySuggestion>(textValue, System.Configuration.ConfigurationManager.AppSettings["UsedCarCityCompletionField"], size, index, null);

                if (suggestions == null)
                    return suggestionlist;

                foreach (var item in suggestions)
                {
                    LabelValueDTO output = new LabelValueDTO();
                    CityPayLoad payload = item.Source.payload;

                    output.Label = payload.IsDuplicate ? item.Source.output : item.Source.output.Split(',')[0]; // remove state name only if city name is unique.
                    output.Value = payload.Id.ToString();
                    suggestionlist.Add(output);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetCitySuggestion");
                objErr.LogException();
            }
            return suggestionlist;
        }

        public List<LabelValueDTO> GetCitySuggestion(string textValue, string index, bool isApp=false,int size = 10)
        {
            List<LabelValueDTO> suggestionlist = null;
            try
            {
                suggestionlist = new List<LabelValueDTO>();
                var suggestions = GetSuggestion<CitySuggestion>(textValue, System.Configuration.ConfigurationManager.AppSettings["CityCompletionField"], size, index, null);

                if (suggestions == null)
                    return suggestionlist;

                foreach (var item in suggestions)
                {
                    LabelValueDTO output = new LabelValueDTO();
                    CityPayLoad payload = item.Source.payload;

                    //showing state name on desktop and mobile sites
                    output.Label = item.Source.output;
                    //but not on apps ,exception for where the city has duplicate names like akbharpur has duplicates in 3 states
                    if (!payload.IsDuplicate && isApp && !string.IsNullOrWhiteSpace(output.Label))
                    { 
                        output.Label = output.Label.Split(',')[0];
                    }
                    //

                    output.Value = payload.Id.ToString();
                    suggestionlist.Add(output);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetCitySuggestion");
                objErr.LogException();
            }
            return suggestionlist;
        }

        public IEnumerable<Nest.SuggestOption<T>> GetSuggestion<T>(string searchTerm, string completion_field, int size, string index, List<string> context) where T : class
        {
            IEnumerable<Nest.SuggestOption<T>> _suggestionList = null;
            try
            {
                ElasticClient client = ElasticClientInstance.GetInstance();
                Func<SearchDescriptor<T>, SearchDescriptor<T>> selectorWithContext = null;
                Func<SuggestContextQueriesDescriptor<T>, IPromise<System.Collections.Generic.IDictionary<string, System.Collections.Generic.IList<ISuggestContextQuery>>>> contentDict = null;
                if (context != null)
                {
                    contentDict =
                        new Func<SuggestContextQueriesDescriptor<T>, IPromise<IDictionary<string, IList<ISuggestContextQuery>>>>(cc => cc
                            .Context("types", context.Select<string, Func<SuggestContextQueryDescriptor<T>, ISuggestContextQuery>>
                                    (v => cd => cd.Context(v)).ToArray()));
                     selectorWithContext = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(sd => sd.Index(index)
                .Suggest(s => s.Completion(completion_field, c => c.Field(completion_field).Prefix(searchTerm).Contexts(contentDict).Size(size))));
                }
                Func<SearchDescriptor<T>, SearchDescriptor<T>> selectorWithoutContext = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(sd => sd
                    .Index(index).Suggest(s => s
                        .Completion(completion_field, c => c
                            .Prefix(searchTerm)
                            .Field(completion_field).Size(size)))
                            );
                

                ISearchResponse<T> _result = client.Search<T>(context == null ? selectorWithoutContext : selectorWithContext);

                if (_result.Suggest[completion_field][0].Options.Count <= 0)
                {
                    Func<SearchDescriptor<T>, SearchDescriptor<T>> selectorWithoutContextAndFuzyy = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(
                        sd => sd.Index(index).Suggest(s => s.Completion(completion_field, c => c.Field(completion_field)
                            .Fuzzy(ff => ff.MinLength(2).PrefixLength(0).Fuzziness(Fuzziness.EditDistance(1))).Prefix(searchTerm).Size(size))));
                    Func<SearchDescriptor<T>, SearchDescriptor<T>> selectorWithContextAndFuzyy = null;
                    if (context != null)
                    {
                        selectorWithContextAndFuzyy = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(sd => sd.Index(index)
                            .Suggest(s => s.Completion(completion_field, c => c
                                .Field(completion_field)
                                .Fuzzy(ff => ff.MinLength(2).PrefixLength(0).Fuzziness(Fuzziness.EditDistance(1)))
                                .Size(size)
                                .Contexts(contentDict)
                                .Prefix(searchTerm))));
                    }
                    _result = client.Search<T>(context == null ? selectorWithoutContextAndFuzyy : selectorWithContextAndFuzyy);
                }
                _suggestionList = _result.Suggest[completion_field][0].Options;
            }
            catch (Exception ex)
            {
                string msg = ("search term : " + searchTerm ?? "") + ",completion  field : " + (completion_field ?? "") + ", size : " + size + ",context : " + (context != null ? context.First() : "");
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetSuggestion() " + msg + "  inner exception : " + ex.InnerException);
                objErr.LogException();
            }
            return _suggestionList;
        }
        private Dictionary<string, Suggest<T>[]> GetSuggestionForCars<T>(string searchTerm, string completion_field, int size, string index, List<string> context) where T : class
        {
            try
            {
                bool fetchLinks = false;
                if (context.Contains("links"))
                {
                    context.Remove("links");
                    fetchLinks = true;
                }
                ElasticClient client = ElasticClientInstance.GetInstance();
                searchTerm = searchTerm.ToLower().Trim();
                contextG = context;
                int SecondCarCount = 15;
                // Func<FluentDictionary<string, object>, FluentDictionary<string, object>> contentDict = new Func<FluentDictionary<string, object>, FluentDictionary<string, object>>(cc => cc.Add("types", context));
                Func<SuggestContextQueriesDescriptor<T>, IPromise<System.Collections.Generic.IDictionary<string, System.Collections.Generic.IList<ISuggestContextQuery>>>> contentDict = 
                    new Func<SuggestContextQueriesDescriptor<T>, IPromise<IDictionary<string, IList<ISuggestContextQuery>>>>(cc => cc
                        .Context("types", context.Select<string, Func < SuggestContextQueryDescriptor<T>, ISuggestContextQuery >>
                                (v => cd => cd.Context(v)).ToArray()));
                Func <SearchDescriptor<T>, SearchDescriptor<T>> selectorWithoutContext = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(sd => sd.Index(index).Suggest(s => s.Completion(completion_field, c => c.Prefix(searchTerm).Field(completion_field).Size(size))));
                Func<SearchDescriptor<T>, SearchDescriptor<T>> selectorWithContext = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(
                    comp =>
                    {
                        comp = comp.Index(index);
                        string rContext;
                        Tuple<string, string> tuple;
                        SuggestContainerDescriptor<T> suggest = new SuggestContainerDescriptor<T>();
                        if ((rContext = versusSuggestParser.Parse(searchTerm)) != null)
                        {
                            suggest = suggest.Completion(VersusSuggestionString, c => c.Field("mm_suggest").Size(size).Prefix(searchTerm).Contexts(contentDict))
                                                    .Completion(CarSuggestionString, c => c.Field("mm_suggest").Size(size).Prefix(rContext).Contexts(contentDict));
                        }
                        else if ((tuple = carVersusParser.Parse(searchTerm)) != null)
                        {
                            suggest = suggest.Completion(FirstCarSuggestionString, c => c.Field("mm_suggest").Size(size).Prefix(tuple.Item1).Contexts(contentDict))
                                                    .Completion(SecondCarSuggestionString, c => c.Field("mm_suggest").Size(SecondCarCount).Prefix(tuple.Item2).Contexts(contentDict));
                        }
                        else if ((rContext = carSuggestParser.Parse(searchTerm)) != null)
                        {
                            suggest = suggest.Completion(CarSuggestionString, c => c.Field("mm_suggest").Size(size).Prefix(rContext).Contexts(contentDict));
                        }
                        if(fetchLinks) suggest = suggest.Completion(LinksSuggestionString, c => c.Field("mm_suggest").Prefix(searchTerm).Contexts(cn=>cn.Context("types",con=>con.Context("links"))).Size(size).Fuzzy(ff => ff.MinLength(2).PrefixLength(0).Fuzziness(Fuzziness.EditDistance(1))));
                        return comp.Suggest(s => suggest);
                    }
                );

                ISearchResponse<T> response = client.Search<T>(context == null ? selectorWithoutContext : selectorWithContext);
                Suggest<T>[] Links = response.Suggest.ContainsKey(LinksSuggestionString) && response.Suggest[LinksSuggestionString][0].Options.Any() ? response.Suggest[LinksSuggestionString] : null;

                Suggest<T>[] VersusNonFuzzySuggestions = response.Suggest.ContainsKey(VersusSuggestionString) && response.Suggest[VersusSuggestionString][0].Options.Any() ? response.Suggest[VersusSuggestionString] : null;
                if (response.Suggest.ContainsKey(CarSuggestionString) && response.Suggest[CarSuggestionString][0].Options.Count <= 0)
                {
                    Func<SearchDescriptor<T>, SearchDescriptor<T>> selectorWithoutContextAndFuzyy = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(sd => sd.Index(index).Suggest(s => s.Completion(completion_field, c => c.Field(completion_field).Prefix(searchTerm).Fuzzy(ff => ff.MinLength(2).PrefixLength(0).Fuzziness(Fuzziness.EditDistance(1))).Size(size))));
                    Func<SearchDescriptor<T>, SearchDescriptor<T>> selectorWithContextAndFuzyy = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(
                        comp =>
                        {
                            comp = comp.Index(index);
                            SuggestContainerDescriptor<T> suggest = new SuggestContainerDescriptor<T>();
                            string rContext;
                            if ((rContext = versusSuggestParser.Parse(searchTerm)) != null)
                            {
                                if (VersusNonFuzzySuggestions == null) suggest = suggest.Completion(VersusSuggestionString, c => c.Field("mm_suggest").Prefix(searchTerm).Contexts(contentDict).Size(size).Fuzzy(ff => ff.MinLength(2).PrefixLength(0).Fuzziness(Fuzziness.EditDistance(1))));
                                suggest = suggest.Completion(CarSuggestionString, c => c.Field("mm_suggest").Prefix(rContext).Contexts(contentDict).Size(size).Fuzzy(ff => ff.MinLength(2).PrefixLength(0).Fuzziness(Fuzziness.EditDistance(1))));
                            }
                            else if ((rContext = carSuggestParser.Parse(searchTerm)) != null)
                            {
                                suggest = suggest.Completion(CarSuggestionString, c => c.Field("mm_suggest").Prefix(rContext).Contexts(contentDict).Size(size).Fuzzy(ff => ff.MinLength(2).PrefixLength(0).Fuzziness(Fuzziness.EditDistance(1))));
                            }
                            return comp.Suggest(s=>suggest);
                        });

                    response = client.Search<T>(context == null ? selectorWithoutContextAndFuzyy : selectorWithContextAndFuzyy);
                    if (!response.IsValid)
                    {
                        ExceptionHandler objErr = new ExceptionHandler(response.OriginalException, "Invalid elasticsearch query!");
                        objErr.LogException();
                    }
                    if (VersusNonFuzzySuggestions != null)
                            response.Suggest[VersusSuggestionString][0]=VersusNonFuzzySuggestions[0];
                }
                else if (response.Suggest.ContainsKey(FirstCarSuggestionString) &&
                    (response.Suggest[FirstCarSuggestionString][0].Options.Count <= 0 || response.Suggest[SecondCarSuggestionString][0].Options.Count <= 0))
                {
                    Func<SearchDescriptor<T>, SearchDescriptor<T>> selectorWithoutContextAndFuzyy = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(sd => sd.Index(index).Suggest(s => s.Completion(completion_field, c => c.Field(completion_field).Prefix(searchTerm).Size(size).Fuzzy(ff => ff.MinLength(2).PrefixLength(0).Fuzziness(Fuzziness.EditDistance(1))))));
                    Func<SearchDescriptor<T>, SearchDescriptor<T>> selectorWithContextAndFuzyy = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(
                        comp =>
                        {
                            comp = comp.Index(index);
                            Tuple<string, string> tuple = carVersusParser.Parse(searchTerm);
                            SuggestContainerDescriptor<T> suggest = new SuggestContainerDescriptor<T>();

                            if (response.Suggest[FirstCarSuggestionString][0].Options.Count <= 0)
                            {
                                suggest = suggest.Completion(FirstCarSuggestionString, c => c.Field("mm_suggest").Size(size).Contexts(contentDict).Prefix(tuple.Item1).Fuzzy(ff => ff.MinLength(2).PrefixLength(0).Fuzziness(Fuzziness.EditDistance(1))));
                            }
                            else
                            {
                                suggest = suggest.Completion(FirstCarSuggestionString, c => c.Field("mm_suggest").Size(size).Prefix(tuple.Item1).Contexts(contentDict));
                            }
                            if (response.Suggest[SecondCarSuggestionString][0].Options.Count <= 0)
                            {
                                suggest = suggest.Completion(SecondCarSuggestionString, c => c.Field("mm_suggest").Contexts(contentDict).Prefix(tuple.Item2).Size(SecondCarCount).Fuzzy(ff => ff.MinLength(2).PrefixLength(0).Fuzziness(Fuzziness.EditDistance(1))));
                            }
                            else
                            {
                                suggest = suggest.Completion(SecondCarSuggestionString, c => c.Field("mm_suggest").Size(SecondCarCount).Prefix(tuple.Item2).Contexts(contentDict));
                            }

                            return comp.Suggest(s=>suggest);
                        });

                    response = client.Search<T>(context == null ? selectorWithoutContextAndFuzyy : selectorWithContextAndFuzyy);
                }
                Dictionary<string, Suggest<T>[]> dict = response.Suggest.ToDictionary(k=>k.Key,v=>v.Value);
                if (fetchLinks) dict[LinksSuggestionString] = Links;

                return dict;

            }
            catch (Exception ex)
            {
                string msg = (searchTerm != null ? "search term : " + searchTerm : "") + ",completion  field : " + (completion_field != null ? completion_field : "") + ", size : " + size + ",context : " + (context != null ? context.First() : "");
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetSuggestion() " + msg + "  inner exception : " + ex.InnerException);
                objErr.LogException();
            }

            return null;
        }

        public List<LabelValueDTO> GetAreaSuggestion(string textValue, List<string> context, string index, int size = 5)
        {
            List<LabelValueDTO> suggestionlist = null;
            try
            {
                suggestionlist = new List<LabelValueDTO>();
                var suggestions = GetSuggestion<AreaDocument>(textValue, System.Configuration.ConfigurationManager.AppSettings["AreaCompletionField"], size, index, context);

                if (suggestions == null)
                {
                    return suggestionlist;
                }

                foreach (var item in suggestions)
                {
                    var output = new LabelValueDTO();
                    var payload = item.Source.payload;

                    output.Label = item.Source.output;
                    output.Value = JsonConvert.SerializeObject(item.Source.payload);
                    suggestionlist.Add(output);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetCitySuggestion");
                objErr.LogException();
            }
            return suggestionlist;
        }

        public List<LabelValueDTO> GetCarRootSuggestion(string textValue, List<string> context, string index, int size = 10)
        {
            List<LabelValueDTO> suggestionlist = null;
            try
            {
                suggestionlist = new List<LabelValueDTO>();
                
                var _result = GetSuggestionForCars<ESCarDocument>(textValue, "mm_suggest", size, index, context);

                List<SuggestOption<ESCarDocument>> _suggestionList = new List<SuggestOption<ESCarDocument>>();

                if (_result == null)
                    return suggestionlist;

                _suggestionList.AddRange(_result[CarSuggestionString][0].Options.ToList<Nest.SuggestOption<ESCarDocument>>());
                
                foreach (var item in _suggestionList)
                {
                        LabelValueDTO output = new LabelValueDTO();
                        CarPayLoad payload = item.Source.payload;
                        output.Label = item.Source.output;
                        output.Value = payload.RootId;

                        suggestionlist.Add(output);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetCarRootSuggestion");
                objErr.LogException();
            }
            return suggestionlist;
        }
    }
}
