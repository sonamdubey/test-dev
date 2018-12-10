using Carwale.BL.CMS;
using Carwale.Entity;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Deals;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Deals.Cache;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.SponsoredCar;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Carwale.BL.CompareCars
{
	public class CCData : ICompareCarsBL
	{
		private readonly ICompareCarsCacheRepository _compareCacheRepo;
		private readonly ISponsoredCarBL _sponsoredBL;
		private readonly IDeals _cardeals;
		private readonly IDealsCache _carDealsCache;
		private readonly ICarPriceQuoteAdapter _iPrice;
		private readonly ICampaign _campaignBl;
		private readonly ICarModelCacheRepository _modelCache;
		private readonly IDealerAdProvider _dealerAd;
		private readonly string _apiHostUrl = "http://" + ConfigurationManager.AppSettings["HostUrl"] ?? "";
		public CCData(ISponsoredCarBL sponsoredBL, ICompareCarsCacheRepository compareCacheRepo, IDeals cardeals,
			IDealsCache carDealsCache, ICarPriceQuoteAdapter iprice, ICarModelCacheRepository modelCache, IDealerAdProvider dealerAd, ICampaign campaignBl)
		{
			_sponsoredBL = sponsoredBL;
			_compareCacheRepo = compareCacheRepo;
			_cardeals = cardeals;
			_carDealsCache = carDealsCache;
			_iPrice = iprice;
			_modelCache = modelCache;
			_dealerAd = dealerAd;
			_campaignBl = campaignBl;
		}

		public CCarData Get(List<int> versionIds, bool getFeaturedVersion, Location custLocation = null, int platform = -1, bool isOrp = false)
		{
			CCarData carData = new CCarData();
			int cityId = custLocation != null ? custLocation.CityId : -1;
			try
			{
				int featuredVersionId = -1;

				if (getFeaturedVersion)
				{
					carData.FeaturedCarData = _sponsoredBL.GetFeaturedCarData(string.Join(",", versionIds), (int)CampaignCategory.SponsoredCarComparison, cityId);

					if (carData.FeaturedCarData.FeaturedVersionId > 0 && !versionIds.Contains(carData.FeaturedCarData.FeaturedVersionId) && platform == (int)Platform.CarwaleDesktop)
					{
						featuredVersionId = carData.FeaturedCarData.FeaturedVersionId;//set featured versionId to -1 if platform is not desktop
						versionIds.Add(featuredVersionId);
					}
				}
				string specsKey = "/1/";
				string featuresKey = "/2/";

				carData.FeaturedVersionId = featuredVersionId;

				Hashtable htcarData = new Hashtable();

				Tuple<Hashtable, Hashtable> tSubCat = _compareCacheRepo.GetSubCategories();

				htcarData[specsKey] = tSubCat.Item1;
				htcarData[featuresKey] = tSubCat.Item2;

				Hashtable htItemData = _compareCacheRepo.GetItems();

				Hashtable htVersionValues = new Hashtable();

				foreach (int versionId in versionIds)
				{
					Tuple<Hashtable, List<Color>, CarWithImageEntity> tVersionData = _compareCacheRepo.GetVersionData(versionId);
					Hashtable tempHt = tVersionData.Item1;
					List<Color> tempColors = tVersionData.Item2;
					int modelId = tVersionData.Item3.ModelId;
					CarWithImageEntity cwie = tVersionData.Item3;

					PriceOverview price = _iPrice.GetAvailablePriceForVersion(versionId, cityId, isOrp, cwie.IsNew);
					if (price != null)
					{
						cwie.MinAvgPrice = price.Price;
						cwie.PriceOverview = price;
					}

					if (cwie.PriceOverview == null) cwie.PriceOverview = new PriceOverview();
					cwie.DiscountSummary = custLocation != null ? GetDiscountSummary(modelId, cityId, versionId, carData.FeaturedVersionId) : new DealsStock();
					if (versionId != carData.FeaturedVersionId)
						carData.ValidVersionIds.Add(versionId);
					htVersionValues[versionId] = tempHt;
					ValueData obj = (ValueData)((Hashtable)htVersionValues[versionId])["12"];
					if (obj != null && !string.IsNullOrEmpty(obj.Value)) obj.Value = Math.Round(Convert.ToDouble(obj.Value), 2).ToString();
					obj = (ValueData)((Hashtable)htVersionValues[versionId])["46"];
					if (obj != null && !string.IsNullOrEmpty(obj.Value)) obj.Value = Math.Round(Convert.ToDouble(obj.Value), 2).ToString();

					CarIdEntity carEntity = new CarIdEntity()
					{
						ModelId = modelId,
						MakeId = tVersionData.Item3.MakeId,
						VersionId = tVersionData.Item3.VersionId
					};
					if (custLocation != null && cwie.IsNew)
					{
						cwie.SponsoredCampaign = _dealerAd.GetDealerAd(carEntity, custLocation, platform, 1, -1, 0);
						if (cwie.SponsoredCampaign == null)
							cwie.ShowCampaignLink = _campaignBl.IsCityCampaignExist(carEntity.ModelId, custLocation, platform, (int)Application.CarWale);
					}
					SetColorsFlag(cwie);
					carData.CarDetails.Add(cwie);
					carData.Colors.Add(tempColors);
				}
				var validVersionIdsWithFC = new List<int>();
				foreach (int versionId in carData.ValidVersionIds)
				{
					validVersionIdsWithFC.Add(versionId);
				}
				if (carData.FeaturedVersionId != -1)
					validVersionIdsWithFC.Add(carData.FeaturedVersionId);

				foreach (int versionId in validVersionIdsWithFC)
				{
					foreach (string key in htItemData.Keys)
					{
						string value = "-";
						var item = (ItemData)htItemData[key];

						ValueData obj = (ValueData)((Hashtable)htVersionValues[versionId])[key];
						if (obj != null)
							value = obj.Value;

						item.Values.Add(value);
					}
				}

				foreach (string key in htItemData.Keys)
				{
					string nodeCode = ((ItemData)htItemData[key]).NodeCode;
					var item = (ItemData)htItemData[key];

					SubCategoryData tempSubCat = (SubCategoryData)((Hashtable)htcarData[nodeCode.Substring(0, 3)])[nodeCode];

					if (!isEmpty(item.Values))
					{
						int sortOrder;
						if (tempSubCat != null)
						{
							int.TryParse(item.SortOrder, out sortOrder);
							tempSubCat.Items.Add(new Item()
							{
								ItemMasterId = item.ItemMasterId,
								Name = item.Name,
								Values = item.Values,
								UnitType = item.UnitType,
								SortOrder = sortOrder
							});
						}

						if (item.IsOverviewable)
						{
							int.TryParse(item.OverviewSortOrder, out sortOrder);
							carData.OverView.Add(new Item()
							{
								ItemMasterId = item.ItemMasterId,
								Name = item.Name,
								Values = item.Values,
								UnitType = item.UnitType,
								SortOrder = sortOrder
							});
						}
					}
				}

				foreach (string key in ((Hashtable)htcarData[specsKey]).Keys)
				{
					SubCategoryData scd = (SubCategoryData)(((Hashtable)htcarData[specsKey])[key]);
					if (scd.Items.Count > 0)
					{
						scd.Items.Sort();
						int sortOrder;
						int.TryParse(scd.SortOrder, out sortOrder);
						carData.Specs.Add(new SubCategory()
						{
							Name = scd.Name,
							Items = scd.Items,
							SortOrder = sortOrder
						});
					}
				}

				foreach (string key in ((Hashtable)htcarData[featuresKey]).Keys)
				{
					SubCategoryData scd = (SubCategoryData)(((Hashtable)htcarData[featuresKey])[key]);
					if (scd.Items.Count > 0)
					{
						scd.Items.Sort();
						int sortOrder;
						int.TryParse(scd.SortOrder, out sortOrder);
						carData.Features.Add(new SubCategory()
						{
							Name = scd.Name,
							Items = scd.Items,
							SortOrder = sortOrder
						});
					}
				}

				carData.Specs.Sort();
				carData.Features.Sort();
				carData.OverView.Sort();
			}
			catch (Exception ex)
			{
				ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.CompareCars.CCData.Get()");
				objErr.LogException();
			}
			return carData;
		}

		void SetColorsFlag(CarWithImageEntity cwie)
		{
			try
			{
				var colorConditionDict = new Dictionary<int, bool>();

				int modelId = cwie.ModelId;
				if (!colorConditionDict.ContainsKey(modelId))
				{
					colorConditionDict.Add(modelId,
						CMSCommon.IsModelColorPhotosPresent(_modelCache.GetModelColorsByModel(modelId))
						);
				}
				cwie.ShowAllColorsLink = colorConditionDict[modelId];
			}
			catch (Exception ex)
			{
				Logger.LogException(ex, "Carwale.BL.CompareCars.CCData.SetColorsFlag()");
			}
		}

		DealsStock GetDiscountSummary(int modelId, int cityId, int versionId, int featuredVersionId)
		{
			DealsStock discountSummary = (_cardeals.IsShowDeals(cityId, true) && versionId != featuredVersionId) ? _carDealsCache.GetAdvantageAdContent(modelId, (cityId > 0 ? cityId : 0), 0, versionId) : null;
			return discountSummary != null ? discountSummary : new DealsStock() { Offers = string.Empty };
		}

		static bool isEmpty(List<string> lst)
		{
			bool resp = true;
			foreach (string str in lst)
			{
				if (str != "-")
				{
					resp = false;
					break;
				}
			}
			return resp;
		}

		public MetaTagsEntity GetCanonical(List<CarWithImageEntity> carData, int featuredVersionId)
		{
			MetaTagsEntity objMeta = new MetaTagsEntity();
			objMeta.ModelPageUrl = "/";

			StringBuilder canonical = new StringBuilder("/comparecars/");
			StringBuilder title = new StringBuilder();
			StringBuilder youHere = new StringBuilder("");

			var sortedCarData = carData.Where(x => x.VersionId != featuredVersionId)
												?.GroupBy(x => new { x.ModelId })
												?.Select(y => y.FirstOrDefault())
												?.OrderByDescending(z => z.ModelId).ToList();
			if (sortedCarData != null)
			{
				foreach (var car in sortedCarData)
				{
					canonical.AppendFormat("{0}-{1}-vs-", Format.RemoveSpecialCharacters(car.MakeName), car.MaskingName);

					title.AppendFormat("{0} {1} vs ", car.MakeName, car.ModelName);
				}
				canonical.Remove(canonical.Length - 4, 4); //to remove -vs-
				canonical.Append("/");
				title.Remove(title.Length - 4, 4);
				if (sortedCarData.Count == 1)
				{
					objMeta.ModelPageUrl = ManageCarUrl.CreateModelUrl(carData[0].MakeName, carData[0].MaskingName);
				}
			}
			for (int i = 0; i < carData.Count; i++)
			{
				if (carData[i].VersionId != featuredVersionId)
				{
					youHere.AppendFormat("c{0}={1}&", i + 1, carData[i].VersionId);
				}
			}
			if (youHere.Length > 0)
			{
				youHere.Remove(youHere.Length - 1, 1);
			}
			objMeta.Canonical = canonical.ToString();
			objMeta.Title = title.ToString();
			objMeta.YouHere = youHere.ToString();
			return objMeta;
		}

		/// <summary>
		/// Written By : Ashwini Todkar on 25 July 2015
		/// Method to get top comparison cars page wise 
		/// </summary>
		/// <param name="page"></param>
		/// <Modified>get price based on city</Modified>
		/// <returns></returns>
		public List<HotCarComparison> GetHotComaprisons(Pagination page, int cityId, bool isOrp)
		{
			List<HotCarComparison> _topComparison = null;

			try
			{
				var _allCar = _compareCacheRepo.GetHotComaprisons(50);
				ushort _startIndex, _endIndex;
				Carwale.Utility.Calculation.GetStartEndIndex(page.PageNo, page.PageSize, out _startIndex, out _endIndex);

				_topComparison = _allCar.Where((x, i) => i >= _startIndex - 1 && i < _endIndex).ToList<HotCarComparison>();

				if (_topComparison != null && _topComparison.Count > 0)
				{
					var _versionsPrice = _iPrice.GetVersionsPriceForDifferentModel(_topComparison.SelectMany(x => x.HotCars).Select(y => y.VersionId).Distinct().ToList(), cityId, isOrp);

					if (_versionsPrice != null)
					{
						_topComparison.ForEach(cc => cc.HotCars.ForEach(y =>
													{
														PriceOverview versionPriceOverview; _versionsPrice.TryGetValue(y.VersionId, out versionPriceOverview); y.PriceOverview = (versionPriceOverview != null ? versionPriceOverview : new PriceOverview());
													}));
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelsBL.GetHotComaprisons()\n Exception : " + ex.Message);
				objErr.LogException();
			}
			return _topComparison;
		}

		/// <summary>
		/// Written By : Jitendra solanki on 22 mar 2016
		/// Method to get Carcompare list based on index
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public CompareCarsDetails GetCompareCarList(Pagination page)
		{

			CompareCarsDetails _compareCars = null;
			try
			{
				var _topComparison = _compareCacheRepo.GetCompareCarsDetails(page);
				int _carCount = _compareCacheRepo.GetCompareCarCount();

				if (_topComparison != null && _topComparison.Count > 0)
				{
					_compareCars = new CompareCarsDetails();
					_compareCars.CompareCars = _topComparison;
					if ((page.PageNo * page.PageSize) >= _carCount)
					{
						_compareCars.NextPageUrl = "";
					}
					else
					{
						_compareCars.NextPageUrl = _apiHostUrl + "/api/v1/comparecars/?pageno=" + (Convert.ToInt32(page.PageNo) + 1) + "&pagesize=" + page.PageSize;
					}
				}

			}
			catch (Exception ex)
			{
				ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelsBL.GetHotComaprisons()\n Exception : " + ex.Message);
				objErr.LogException();
			}
			return _compareCars;
		}
	}
}
