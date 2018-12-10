using AutoMapper;
using Carwale.BL.Elastic;
using Carwale.DTOs;
using Carwale.DTOs.Elastic;
using Carwale.DTOs.Elastic.Autocomplete.Area;
using Carwale.DTOs.Suggestion;
using Carwale.Interfaces.AutoComplete;
using Carwale.Notifications;
using System;
using Carwale.Utility;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using Carwale.Notifications.Logs;

namespace Carwale.BL.AutoComplete
{
    public class AutoComplete_v1 : IAutoComplete_v1
    {
        
        private static string _cityIndex;
        private static string _cityCompletionField;
        private static string _carIndex;
        private static string _areaIndex;
        private static string _areaCompletionField;
        private const int RecordCount = 10;
        private readonly SuggestionBL _suggestion;
        static AutoComplete_v1()
        {
            _cityIndex = System.Configuration.ConfigurationManager.AppSettings["AutocompleteCityIndex"] ?? "newcityautosuggest";
            _carIndex = System.Configuration.ConfigurationManager.AppSettings["AutocompleteCarIndex"] ?? "autosuggesttest";
            _areaIndex = System.Configuration.ConfigurationManager.AppSettings["AutocompleteAreaIndex"] ?? "locations2";
            _cityCompletionField = System.Configuration.ConfigurationManager.AppSettings["CityCompletionField"] ?? "mm_suggest";
            _areaCompletionField = System.Configuration.ConfigurationManager.AppSettings["AreaCompletionField"] ?? "areaSuggests";
        }
        public AutoComplete_v1(SuggestionBL suggestion)
        {
            _suggestion = suggestion;
        }
        public List<Suggest> GetAreaSuggestion(NameValueCollection nvc)
        {
            List<Suggest> areaResults = null;
            try
            {
                ESAutoComplete _esCitySuggest = new ESAutoComplete();
                var suggestions = _esCitySuggest.GetSuggestion<AreaDocument>(GetTerm(nvc["term"]), _areaCompletionField, GetNumberOfRecord(nvc["record"]), _areaIndex, GetContext(nvc["cityid"]));

                if (suggestions != null)
                {
                    areaResults = new List<Suggest>();
                    foreach (var item in suggestions)
                    {
                        AreaResultsDTO areaPayload = Mapper.Map<AreaResultsDTO>(item.Source.payload);
                        areaPayload.DisplayName = areaPayload.Name + ", "+areaPayload.PinCode;

                        areaResults.Add(new Suggest() { Result = areaPayload.DisplayName, Payload = areaPayload });
                    }
                }                 
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "AutoComplete_v1.GetAreaSuggestion()");
                objErr.LogException();
            }
            return areaResults;
        }

        public List<Suggest> GetCitySuggestion(NameValueCollection nvc)
        {
            List<Suggest> cityResults = null;
            try
            {
                ESAutoComplete _esCitySuggest = new ESAutoComplete();

                var suggestions = _esCitySuggest.GetSuggestion<CitySuggestion>(GetTerm(nvc["term"]), _cityCompletionField, GetNumberOfRecord(nvc["record"]), _cityIndex, null);

                if (suggestions != null)
                {
                    cityResults = new List<Suggest>();
                    foreach (var item in suggestions)
                    {
                        DTOs.PayLoad payload = Mapper.Map<CityResultsDTO>(item.Source.payload);
                        cityResults.Add(new Suggest() { Result = payload.DisplayName, Payload = payload});
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "AutoComplete_v1.GetCitySuggestion()");
                objErr.LogException();
            }

            return cityResults;                        
        }

        public string GetTerm(string term)
        {
            return Regex.Replace(term.Trim().Replace("-", " "), "[^/\\-0-9a-zA-Z\\s]*", "");
        }

        public int GetNumberOfRecord(string size)
        {
            int noOfRecords;
            return !string.IsNullOrEmpty(size) && int.TryParse(size, out noOfRecords) ? noOfRecords : RecordCount;
        }

        public List<string> GetContext(params string[] values)
        {
            var context = new List<string>();
            for (int paramCount = 0; paramCount < values.Length; paramCount++)
            { 
                context.Add(values[paramCount]); 
            }            
            return context;
        }
       
        public List<Carwale.DTOs.Suggestion.Base> GetSearchSuggestion(string _searchTerm, List<Carwale.Entity.Enum.SuggestionTypeEnum> _types,int _size, bool isAmp)  
        {
            List<Base> resultList = new List<Base>();
            List<Base> versusResultList = new List<Base>();
            List<Base> modelResultList = new List<Base>();
            List<Base> linkResultList = new List<Base>();
            List<Nest.SuggestOption<ESCarDocument>> _suggestionList1 = null;
            List<Nest.SuggestOption<ESCarDocument>> _suggestionList2 = null;
            List<Nest.SuggestOption<ESCarDocument>> _linkSuggestionList = null;
            try
            {
                bool versusFlag = false;
                Tuple<List<Nest.SuggestOption<ESCarDocument>>, List<Nest.SuggestOption<ESCarDocument>>, List<Nest.SuggestOption<ESCarDocument>>> _result = _suggestion.GetSuggestionForCars<ESCarDocument>(_searchTerm, ref versusFlag, _types, _size);
                if (_result == null)
                    return new List<Base>();
                _suggestionList1 = _result.Item1;
                _suggestionList2 = _result.Item2;
                _linkSuggestionList = _result.Item3;

                if (versusFlag && _types.Contains(Entity.Enum.SuggestionTypeEnum.DualCompare))
                {
                    versusResultList.AddRange(GetDualCompareCarSuggestion(_suggestionList1, _suggestionList2, _size, isAmp));
                    if (versusResultList.Count > 0 && _suggestionList2.Count > 0) return versusResultList.Count > _size ? versusResultList.GetRange(0, _size) : versusResultList;
                    _types.Remove(Entity.Enum.SuggestionTypeEnum.DualCompare);
                }
                if(_types.Contains(Entity.Enum.SuggestionTypeEnum.Make) && _types.Contains(Entity.Enum.SuggestionTypeEnum.NewModel))
                {
                    _types.Remove(Entity.Enum.SuggestionTypeEnum.Make);
                }
                foreach (var type in _types)
                {
                    switch(type)
                    {
                        case Entity.Enum.SuggestionTypeEnum.DualCompare:
                            versusResultList.AddRange(GetDualCompareCarSuggestion(_suggestionList1, _suggestionList2, _size , isAmp));
                            break;
                        case Entity.Enum.SuggestionTypeEnum.Make:
                        case Entity.Enum.SuggestionTypeEnum.NewModel:
                            modelResultList.AddRange(GetMakeModelSuggestion(_suggestionList1, _size,false, false, isAmp));
                            break;
                        case Entity.Enum.SuggestionTypeEnum.DiscontinuedModel:
                            modelResultList.AddRange(GetMakeModelSuggestion(_suggestionList1, _size, false, true, isAmp));
                            break;
                        case Entity.Enum.SuggestionTypeEnum.UpcomingModel:
                            modelResultList.AddRange(GetMakeModelSuggestion(_suggestionList1, _size,true, false, isAmp));
                            break;
                        case Entity.Enum.SuggestionTypeEnum.Link:
                            linkResultList.AddRange(GetLinks(_linkSuggestionList, _size));
                            break;
                        default:
                            break;
                    }
                }
                if (versusFlag)
                {
                    resultList.AddRange(versusResultList);
                    resultList.AddRange(modelResultList);
                }
                else
                {
                    resultList.AddRange(modelResultList);
                    resultList.AddRange(versusResultList);
                }
                resultList.AddRange(linkResultList);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return resultList.Count > _size ? resultList.GetRange(0,_size) : resultList;
        }
        public List<Base> GetVersusCar(List<Nest.SuggestOption<ESCarDocument>> _suggestionList,int _size, bool isAmp)
        {
            List<Base> results = new List<Base>();
            foreach (var item1 in _suggestionList)
            {
                CarPayLoad payload1 = item1.Source.payload;
                List<CarPayLoad> payloadList = payload1.CarComparisionList;
                short _modelId1 = 0, _modelId2 = 0, _versionId1 = 0, _versionId2 = 0;
                if (!payload1.IsUpcoming && payloadList.Count > 0 && !string.IsNullOrEmpty(payload1.ModelId) && !payload1.ModelId.Equals("0") )
                {
                    payload1.OutputName = item1.Source.output;
                    short.TryParse(payload1.ModelId, out _modelId1);
                    short.TryParse(payload1.VersionId, out _versionId1);
                    foreach (var payload2 in payloadList)
                    {
                        if (!string.IsNullOrEmpty(payload2.ModelId) && !payload2.ModelId.Equals("0")) short.TryParse(payload2.ModelId, out _modelId2);
                        short.TryParse(payload2.VersionId, out _versionId2);
                        if (_modelId2 > 0 && _versionId2 > 0 && _modelId1 != _modelId2 && !payload2.IsUpcoming)
                        {
                            string displayName = string.Format("{0} vs {1}", payload1.OutputName, (!string.IsNullOrEmpty(payload2.OutputName) ? payload2.OutputName : string.Empty));
                            //If request is from amp page then set the url otherwise make it null
                            string url = null;
                            if (isAmp)
                            {
                                url = ManageCarUrl.CreateCompareCarUrl(new List<Tuple<int, string>> { Tuple.Create(Convert.ToInt32(_modelId1), Format.FormatSpecial(payload1.MakeName) + "-" + payload1.MaskingName), Tuple.Create(Convert.ToInt32(_modelId2), Format.FormatSpecial(payload2.MakeName) + "-" + payload2.MaskingName) }, true, true);

                            }
                            Base _base = new Base()
                            {
                                SuggestionType = Entity.Enum.SuggestionTypeEnum.DualCompare,
                                DisplayName = displayName,
                                PayLoad = new DualCompareSuggestion()
                                {
                                    ModelId1 = _modelId1,
                                    ModelId2 = _modelId2,
                                    VersionId1 = _versionId1,
                                    VersionId2 = _versionId2,
                                    Url = url
                                }
                            };
                            results.Add(_base);
                            if (results.Count >= _size) break;
                        }
                    }
                    if (results.Count >= _size) break;
                }
            }
            return results;
        }
        public List<Base> GetCompareCars(List<Nest.SuggestOption<ESCarDocument>> _suggestionList, List<Nest.SuggestOption<ESCarDocument>> _suggestionList2, int _size, bool isAmp)
        {
            List<Base> results = new List<Base>();
            short _modelId1 = 0, _modelId2 = 0, _versionId1 = 0, _versionId2 = 0;
            foreach (var item1 in _suggestionList)
            {
                CarPayLoad payload1 = item1.Source.payload;
               
                if (!string.IsNullOrEmpty(payload1.ModelId) && !payload1.ModelId.Equals("0") && !payload1.IsUpcoming)
                {
                    short.TryParse(payload1.ModelId, out _modelId1);
                    short.TryParse(payload1.VersionId, out _versionId1);
                    foreach (var item2 in _suggestionList2)
                    {
                        CarPayLoad payload2 = item2.Source.payload;

                        if (!string.IsNullOrEmpty(payload2.ModelId) && !payload2.ModelId.Equals("0") && !payload2.IsUpcoming)
                        {
                            short.TryParse(payload2.ModelId, out _modelId2);
                            short.TryParse(payload2.VersionId, out _versionId2);
                            if (_modelId1 != _modelId2)
                            {
                                string displayName = string.Format("{0} vs {1}" , item1.Source.output ,item2.Source.output);
                                //If request is from amp page then set the url otherwise make it null
                                string url = null;
                                if (isAmp)
                                {
                                    url = ManageCarUrl.CreateCompareCarUrl(new List<Tuple<int, string>> { Tuple.Create(Convert.ToInt32(_modelId1), Format.FormatSpecial(payload1.MakeName) + "-" + payload1.MaskingName), Tuple.Create(Convert.ToInt32(_modelId2), Format.FormatSpecial(payload2.MakeName) + "-" + payload2.MaskingName) }, true, true);

                                }
                                Base _base = new Base()
                                {
                                    SuggestionType = Entity.Enum.SuggestionTypeEnum.DualCompare,
                                    DisplayName = displayName,
                                 
                                    PayLoad = new DualCompareSuggestion()
                                    {
                                        ModelId1 = _modelId1,
                                        ModelId2 = _modelId2,
                                        VersionId1 = _versionId1,
                                        VersionId2 = _versionId2,
                                        Url = url
                                    }
                                };

                                results.Add(_base);
                                if (results.Count >= _size) break;
                            }
                        }
                    }
                }
                if (results.Count >= _size) break;
            }
            return results;
        }
        
        public List<Base> GetDualCompareCarSuggestion(List<Nest.SuggestOption<ESCarDocument>> _suggestionList, List<Nest.SuggestOption<ESCarDocument>> _suggestionList2, int _size, bool isAmp)
        {
            List<Base> resultList = new List<Base>();
            if (_suggestionList2.Count > 0 && _suggestionList.Count > 0)
            {
                resultList.AddRange(GetCompareCars(_suggestionList, _suggestionList2, _size, isAmp));
            }
            else if (_suggestionList.Count > 0)
            {
                resultList.AddRange(GetVersusCar(_suggestionList, _size, isAmp));
            }
            else if (_suggestionList2.Count > 0)
            {
                resultList.AddRange(GetVersusCar(_suggestionList2, _size, isAmp));
            }
            return resultList;
        }
        public List<Base> GetMakeModelSuggestion(List<Nest.SuggestOption<ESCarDocument>> _suggestionList, int _size, bool isUpcoming, bool isDiscontinue, bool isAmp)
        {            
            List<Base> makeModelResultList = new List<Base>();
            foreach (var item in _suggestionList)
            {
                CarPayLoad payLoad = item.Source.payload;
                payLoad.IsDiscontinue =  item.Contexts["types"].Select(x => x.Category == "discontinued").FirstOrDefault();
                //If request is from amp page then set the url otherwise make it null
                string url = null;
                if (payLoad.IsUpcoming == isUpcoming && payLoad.IsDiscontinue == isDiscontinue && !string.IsNullOrEmpty(payLoad.ModelId) && !payLoad.ModelId.Equals("0"))
                {
                    short _modelId1;
                    short.TryParse(payLoad.ModelId, out _modelId1);
                    string _additionalInfo = payLoad.IsUpcoming ? "Coming soon" : string.Empty;
                    string _displayName = item.Source.output;
                    if (isAmp)
                    {
                        url = ManageCarUrl.CreateModelUrl(payLoad.MakeName, payLoad.MaskingName, true);
                    }
                    Base _base = new Base()
                    {
                        SuggestionType = isUpcoming ? Entity.Enum.SuggestionTypeEnum.UpcomingModel : (isDiscontinue ? Entity.Enum.SuggestionTypeEnum.DiscontinuedModel : Entity.Enum.SuggestionTypeEnum.NewModel),
                        DisplayName = _displayName,
                        AdditionalInfo = _additionalInfo,
                        PayLoad = new ModelSuggestion() { ModelId = _modelId1, Url = url }
                    };
                    makeModelResultList.Add(_base);
                    if (makeModelResultList.Count >= _size) break;
                }
                else if (!string.IsNullOrEmpty(payLoad.MakeId) && !payLoad.MakeId.Equals("0") && (string.IsNullOrEmpty(payLoad.ModelId) || payLoad.ModelId.Equals("0")) && !isUpcoming)
                {
                    short makeId = 0;
                    short.TryParse(payLoad.MakeId, out makeId);
                    if (isAmp)
                    {
                        url = ManageCarUrl.CreateMakeUrl(payLoad.MakeName, true, true);
                    }
                    string displayName = item.Source.output;
                    Base _base = new Base()
                    {
                        SuggestionType = Entity.Enum.SuggestionTypeEnum.Make,
                        DisplayName = displayName,
                        PayLoad = new MakeSuggestion() { MakeId = makeId, Url = url }

                    };
                    makeModelResultList.Add(_base);
                }
            }
            return makeModelResultList;
        }
        public List<Base> GetLinks(List<Nest.SuggestOption<ESCarDocument>> _suggestionList, int _size)
        {
            List<Base> resultList = new List<Base>();
            foreach (var item in _suggestionList)
            {
                CarPayLoad payload = item.Source.payload;
                if(!string.IsNullOrWhiteSpace(payload.DesktopLink) || !string.IsNullOrWhiteSpace(payload.MobileLink))
                {
                    Base _base = new Base()
                    {
                        SuggestionType = Entity.Enum.SuggestionTypeEnum.Link,
                        DisplayName = item.Source.output,
                        PayLoad = new LinkSuggestion() { Url = payload.DesktopLink ?? string.Empty }
                    };
                    resultList.Add(_base);
                    if (resultList.Count >= _size) break;
                }
            }
             return resultList;
        }

    }
}
