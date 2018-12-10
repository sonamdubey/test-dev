using System;
using System.Collections.Generic;
using Nest;
using Carwale.DAL.CoreDAL;
using System.Linq;
using Carwale.Notifications.Logs;
using Carwale.DTOs.Elastic;

namespace Carwale.BL.Elastic
{
    public class SuggestionBL {
        private static string index;
        public static readonly string completion_field = "mm_suggest";
        public static readonly string VersusSuggestionString = "VERSUS";
        public static readonly string CarSuggestionString = "CAR";
        public static readonly string FirstCarSuggestionString = "CAR1";
        public static readonly string SecondCarSuggestionString = "CAR2";
        public static readonly string LinksSuggestionString = "LINKS";
        static SuggestionBL() {
            index = System.Configuration.ConfigurationManager.AppSettings["AutocompleteCarIndex"];
        }

        public Tuple<List<Nest.SuggestOption<T>>, List<Nest.SuggestOption<T>>, List<Nest.SuggestOption<T>>> GetSuggestionForCars<T>(string searchTerm,  ref bool versusFlagStatus, List<Carwale.Entity.Enum.SuggestionTypeEnum> types, int size = 10) where T : class {
            List<string> context = null;
            Tuple<List<string>, bool> tuple = null;
            try
            {                
                ElasticClient client = ElasticClientInstance.GetInstance();
                if (types != null && types.Count > 0) context = GetModelContext(types).Distinct().ToList();
                tuple = ParseSearchTerms(searchTerm);
                if (context.Count > 0 && tuple.Item1.Count > 0)
                {
                    List<string> searchTerms = tuple.Item1;
                    bool versusFlag = tuple.Item2;
                    versusFlagStatus = versusFlag;
                    int secondCarCount = 15;
                    bool fetchLinks = false;
                    if (context.Contains("links"))
                    {
                        context.Remove("links");
                        fetchLinks = true;
                    }
                    Func<SuggestContextQueriesDescriptor<T>, IPromise<System.Collections.Generic.IDictionary<string, System.Collections.Generic.IList<ISuggestContextQuery>>>> contentDict = new Func<SuggestContextQueriesDescriptor<T>, IPromise<IDictionary<string, IList<ISuggestContextQuery>>>>(con => con.Context("types", context.Select<string, Func<SuggestContextQueryDescriptor<T>, ISuggestContextQuery>>
                                    (v => cd => cd.Context(v)).ToArray()));
                   
                    Func<SearchDescriptor<T>, SearchDescriptor<T>> selectorWithContext = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(comp =>
                    {
                        comp = comp.Index(index);
                        SuggestContainerDescriptor<T> suggest = new SuggestContainerDescriptor<T>();
                        if (context.Count > 0)
                        {
                            if (searchTerms.Count > 1)
                            {
                                suggest = suggest.Completion(FirstCarSuggestionString, c => c.Field(completion_field).Size(size).Prefix(searchTerms[0]).Contexts(contentDict)).Completion(SecondCarSuggestionString, c => c.Field(completion_field).Size(secondCarCount).Prefix(searchTerms[1]).Contexts(contentDict));
                            }
                            else
                            {
                                if (versusFlag) suggest = suggest.Completion(VersusSuggestionString, c => c.Field(completion_field).Size(size).Prefix(searchTerms[0]).Contexts(contentDict));
                                suggest = suggest.Completion(CarSuggestionString, c => c.Field(completion_field).Size(size).Prefix(searchTerm).Contexts(contentDict));
                            }
                        }
                        if (fetchLinks) suggest = suggest.Completion(LinksSuggestionString, c => c.Field(completion_field).Prefix(searchTerm).Contexts(cn => cn.Context("types", con => con.Context("links"))).Size(size));
                        return comp.Suggest(s => suggest);
                    });

                    ISearchResponse<T> response = client.Search<T>(selectorWithContext);
                    ISearchResponse<T> fuzzyResponse = null;
                    bool haveResponse = (response.Suggest.ContainsKey(CarSuggestionString) && response.Suggest[CarSuggestionString].ElementAt(0).Options.Any()) || (response.Suggest.ContainsKey(VersusSuggestionString) && response.Suggest[VersusSuggestionString].ElementAt(0).Options.Any()) || ((response.Suggest.ContainsKey(FirstCarSuggestionString) &&
                        (response.Suggest[FirstCarSuggestionString].ElementAt(0).Options.Any() || response.Suggest[SecondCarSuggestionString].ElementAt(0).Options.Any())) ||(fetchLinks && response.Suggest[LinksSuggestionString].ElementAt(0).Options.Any()));

                    if ((response.Suggest.ContainsKey(CarSuggestionString) && response.Suggest[CarSuggestionString].ElementAt(0).Options.Count <= 0) || ((response.Suggest.ContainsKey(FirstCarSuggestionString) &&
                        (response.Suggest[FirstCarSuggestionString].ElementAt(0).Options.Count <= 0 || response.Suggest[SecondCarSuggestionString].ElementAt(0).Options.Count <= 0))) || ((response.Suggest.ContainsKey(LinksSuggestionString) &&
                        (response.Suggest[LinksSuggestionString].ElementAt(0).Options.Count <= 0))))
                    {
                        Func<SearchDescriptor<T>, SearchDescriptor<T>> selectorWithContextAndFuzyy = new Func<SearchDescriptor<T>, SearchDescriptor<T>>(
                            comp =>
                            {
                                comp = comp.Index(index);
                                SuggestContainerDescriptor<T> suggest = new SuggestContainerDescriptor<T>();
                                if (context.Count > 0)
                                {
                                    if (searchTerms.Count > 1)
                                    {
                                        if (response.Suggest[FirstCarSuggestionString].ElementAt(0).Options.Count <= 0)
                                        {
                                            suggest = suggest.Completion(FirstCarSuggestionString, c => c.Field(completion_field).Size(size).Fuzzy(ff => ff.MinLength(2).PrefixLength(0).Fuzziness(Fuzziness.EditDistance(1))).Prefix(searchTerms[0]).Contexts(contentDict));

                                        }
                                        if (response.Suggest[SecondCarSuggestionString].ElementAt(0).Options.Count <= 0)
                                        {
                                            suggest = suggest.Completion(SecondCarSuggestionString, c => c.Field(completion_field).Size(secondCarCount).Fuzzy(ff => ff.MinLength(2).PrefixLength(0).Fuzziness(Fuzziness.EditDistance(1))).Prefix(searchTerms[1]).Contexts(contentDict));
                                        }
                                    }
                                    else
                                    {
                                        if (versusFlag && response.Suggest[VersusSuggestionString].ElementAt(0).Options.Count <= 0)
                                            suggest = suggest.Completion(VersusSuggestionString, c => c.Field(completion_field).Size(size).Fuzzy(ff => ff.MinLength(2).PrefixLength(0).Fuzziness(Fuzziness.EditDistance(1))).Prefix(searchTerms[0]).Contexts(contentDict));
                                        if(response.Suggest[CarSuggestionString].ElementAt(0).Options.Count <= 0)suggest = suggest.Completion(CarSuggestionString, c => c.Field(completion_field).Size(size).Fuzzy(ff => ff.MinLength(2).PrefixLength(0).Fuzziness(Fuzziness.EditDistance(1))).Prefix(searchTerm).Contexts(contentDict));
                                    }
                                }
                                if (fetchLinks && searchTerms.Count <= 1 && !haveResponse) suggest = suggest.Completion(LinksSuggestionString, c => c.Field(completion_field).Prefix(searchTerm).Contexts(cn => cn.Context("types", con => con.Context("links"))).Size(size).Fuzzy(ff => ff.MinLength(2).PrefixLength(0).Fuzziness(Fuzziness.EditDistance(1))));
                                return comp.Suggest(s=>suggest);
                            });
                        fuzzyResponse = client.Search<T>(selectorWithContextAndFuzyy);                                             
                    }
                    var carSuggestionDictionary = GetCarSuggestionDictionary(fuzzyResponse, response, searchTerms.Count, versusFlag);
                    return ProcessSuggestion(carSuggestionDictionary);
                }
            }
            catch (Exception ex)
            {
                string msg = (searchTerm != null ? "search term : " + searchTerm : "") + ",completion  field : " + (completion_field != null ? completion_field : "") + ", size : " + size + ",context : " + (context != null ? context.First() : "");
                Logger.LogException(ex, "GetSuggestion() " + msg + "  inner exception : " + ex.InnerException);                
            }
            return null;
        }

        private Tuple<List<SuggestOption<T>>, List<SuggestOption<T>>, List<SuggestOption<T>>> ProcessSuggestion<T>(Dictionary<string, Suggest<T>[]> _result) where T : class
        {
            List<Nest.SuggestOption<T>> _suggestionList = new List<Nest.SuggestOption<T>>();
            List<Nest.SuggestOption<T>> _suggestionList2 = new List<Nest.SuggestOption<T>>();
            List<Nest.SuggestOption<T>> _linkSuggestion = new List<Nest.SuggestOption<T>>();
            if (_result.ContainsKey(SuggestionBL.CarSuggestionString) && _result[SuggestionBL.CarSuggestionString].ElementAt(0).Options.Any())
            {
                _suggestionList.AddRange(_result[SuggestionBL.CarSuggestionString].First().Options);
            }
            else if (_result.ContainsKey(SuggestionBL.VersusSuggestionString) && _result[SuggestionBL.VersusSuggestionString].ElementAt(0).Options.Any())
            {
                _suggestionList.AddRange(_result[SuggestionBL.VersusSuggestionString].First().Options);
            }
            else if (_result.ContainsKey(SuggestionBL.FirstCarSuggestionString) && _result.ContainsKey(SuggestionBL.SecondCarSuggestionString))
            {
                if (_result[SuggestionBL.FirstCarSuggestionString].ElementAt(0).Options.Any())
                {
                    _suggestionList.AddRange(_result[SuggestionBL.FirstCarSuggestionString].First().Options);
                    _suggestionList2.AddRange(_result[SuggestionBL.SecondCarSuggestionString].First().Options);
                }
                else
                {
                    _suggestionList.AddRange(_result[SuggestionBL.SecondCarSuggestionString].First().Options);
                    _suggestionList2.AddRange(_result[SuggestionBL.FirstCarSuggestionString].First().Options);
                }
            }
            if (_result.ContainsKey(SuggestionBL.LinksSuggestionString))
            {
                _linkSuggestion.AddRange(_result[SuggestionBL.LinksSuggestionString].First().Options);
            }
            return new Tuple<List<Nest.SuggestOption<T>>, List<Nest.SuggestOption<T>>, List<Nest.SuggestOption<T>>>(_suggestionList, _suggestionList2, _linkSuggestion);
        }

        private Dictionary<string, Suggest<T>[]> GetCarSuggestionDictionary<T>(ISearchResponse<T> fuzzyResponse, ISearchResponse<T> response, int termCount, bool versusFlag) where T : class
        {
            Dictionary<string, Suggest<T>[]> carSuggestion = new Dictionary<string, Suggest<T>[]>();
            if (termCount > 1)
            {
                if (fuzzyResponse != null && fuzzyResponse.Suggest.ContainsKey(FirstCarSuggestionString))
                {
                    carSuggestion[FirstCarSuggestionString] = fuzzyResponse.Suggest[FirstCarSuggestionString];
                }
                else
                {
                    carSuggestion[FirstCarSuggestionString] = response.Suggest[FirstCarSuggestionString];
                }

                if (fuzzyResponse != null && fuzzyResponse.Suggest.ContainsKey(SecondCarSuggestionString))
                {
                    carSuggestion[SecondCarSuggestionString] = fuzzyResponse.Suggest[SecondCarSuggestionString];
                }
                else 
                {
                    carSuggestion[SecondCarSuggestionString] = response.Suggest[SecondCarSuggestionString];
                }
            }
            else
            {
                if (versusFlag)
                {
                    if (fuzzyResponse != null && fuzzyResponse.Suggest.ContainsKey(VersusSuggestionString))
                    {
                        carSuggestion[VersusSuggestionString] = fuzzyResponse.Suggest[VersusSuggestionString];
                    }
                    else if(response.Suggest.ContainsKey(VersusSuggestionString))
                    {
                        carSuggestion[VersusSuggestionString] = response.Suggest[VersusSuggestionString];
                    }
                }

                if (fuzzyResponse != null && fuzzyResponse.Suggest.ContainsKey(CarSuggestionString))
                {
                    carSuggestion[CarSuggestionString] = fuzzyResponse.Suggest[CarSuggestionString];
                }
                else if(response.Suggest.ContainsKey(CarSuggestionString))
                {
                    carSuggestion[CarSuggestionString] = response.Suggest[CarSuggestionString];
                }
            }
            if (fuzzyResponse != null && fuzzyResponse.Suggest.ContainsKey(LinksSuggestionString))
            {
                carSuggestion[LinksSuggestionString] = fuzzyResponse.Suggest[LinksSuggestionString];
            }
            else if(response.Suggest.ContainsKey(LinksSuggestionString))
            {
                carSuggestion[LinksSuggestionString] = response.Suggest[LinksSuggestionString];
            }
            return carSuggestion;
        }

        private List<string> GetModelContext(List<Entity.Enum.SuggestionTypeEnum> _types) {
            List<string> tempContext = new List<string>();
            foreach (var type in _types)
            {
                switch (type)
                {
                    case Entity.Enum.SuggestionTypeEnum.Make:
                        tempContext.Add("newmake");
                        break;
                    case Entity.Enum.SuggestionTypeEnum.DualCompare:
                        tempContext.Add("pqmodel");
                        break;
                    case Entity.Enum.SuggestionTypeEnum.NewModel:
                        tempContext.Add("pqmodel");
                        break;
                    case Entity.Enum.SuggestionTypeEnum.UpcomingModel:
                        tempContext.Add("upcoming");
                        break;
                    case Entity.Enum.SuggestionTypeEnum.Link:
                        tempContext.Add("links");
                        break;
                    case Entity.Enum.SuggestionTypeEnum.DiscontinuedModel:
                        tempContext.Add("discontinued");
                        break;
                    default:
                        break;
                }
            }
            return tempContext;
        }
        private Tuple<List<string>, bool> ParseSearchTerms(string searchTerm) {
            List<string> searchTerms = new List<string>();
            string[] arr = searchTerm.ToLower().Trim().Split(null);
            string car = string.Empty;
            bool versusFlag = false;
            for (int index = 0; index < arr.Length; index++)
            {
                if (index != 0 && arr[index] == "vs")
                {
                    if (!string.IsNullOrWhiteSpace(car))
                    {
                        searchTerms.Add(car.Trim());
                        car = "";
                    }
                    versusFlag = true;
                    continue;
                }
                car += arr[index] + " ";
            }
            if (!string.IsNullOrWhiteSpace(car)) searchTerms.Add(car.Trim());
            return new Tuple<List<string>, bool>(searchTerms, versusFlag);
        }
    }
}
