using Carwale.DTOs.NewCars;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.NewCars;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.NewCars {
	public class UpcomingCarListAdapter : IServiceAdapterV2 {
		protected IUnityContainer _container;		
		protected const int pagesize = 10;
		public UpcomingCarListAdapter(IUnityContainer container) {
			_container = container;
		}

		public T Get<T, U>(U input) {
			return (T)Convert.ChangeType(GetUpcomingListDTO<U>(input), typeof(T));
		}

		public UpcomingCarsDTO GetUpcomingListDTO<U>(U _input) {

		UpcomingCarsInputParam input = (UpcomingCarsInputParam)Convert.ChangeType(_input, typeof(U));

			UpcomingCarsDTO Model = new UpcomingCarsDTO();


			int pageNo = Model.PageNo = 1;
			Model.Url = input.RewriteUrl;
			//pagination input
			if (input.Nvc["pn"] != null && input.Nvc["pn"] != "")
			{
				if (Carwale.Utility.RegExValidations.IsPositiveNumber(input.Nvc["pn"]))
				{
					if (int.TryParse(input.Nvc["pn"], out pageNo)) Model.PageNo = pageNo;
				}
			}
			Pagination page = new Pagination() { PageNo = (ushort)pageNo, PageSize = pagesize };
			//sort input
			int sort = 0;
			if (!String.IsNullOrEmpty(input.Nvc["sort"]) && int.TryParse(input.Nvc["sort"], out sort))
			{
				Model.Sort = sort;
			}
			var carModelsBl = _container.Resolve<ICarModels>();
			var carMakesCache = _container.Resolve<ICarMakesCacheRepository>();
			Model.CarMakes = carMakesCache.GetCarMakesByType("new");



			//make input
			int makeId = 0;
			if (!String.IsNullOrEmpty(input.Nvc["makeId"]) && int.TryParse(input.Nvc["makeId"], out makeId))
			{
				CarMakeEntityBase make = Model.CarMakes.Find(c => c.MakeId == makeId);
				Model.MakeId = makeId;
				Model.MakeName = make != null ? make.MakeName : string.Empty;
			}
			else
			{
				Model.MakeName = string.Empty;
			}

			//Fetching upcoming models
			Model.UpcomingCarModels = carModelsBl.GetUpcomingCarModels(page, makeId, sort);


			if (!input.IsMobile)
			{
				Model = GetSynopsis(Model);

				//Get 3 new launched models
				Model.NewLaunches = carModelsBl.GetLaunchedCarModelsV1(new Pagination() { PageNo = 1, PageSize = 3 });
			}
			Model = GetMeta(Model,input);

			return Model;
		}

		/// <summary>
		/// Get Synopsis for each model
		/// </summary>
		/// <param name="Model"></param>
		/// <returns></returns>
		protected UpcomingCarsDTO GetSynopsis(UpcomingCarsDTO Model) {
			ICMSContent _cmsContent = _container.Resolve<ICMSContent>();
			Model.CarSynopsis = new Dictionary<int, string>();
			//synopsis for each car
			foreach (var model in Model.UpcomingCarModels)
			{
				var synopsisList = _cmsContent.GetCarSynopsis(model.ModelId, (int)Carwale.Entity.Enum.Application.CarWale);
				string desc = synopsisList != null ? Carwale.Utility.Format.RemoveBasicHtmlTags(synopsisList.Description ?? string.Empty) : string.Empty;                
                desc = desc.Length > 300 ? desc.Substring(0,300) : desc;
				Model.CarSynopsis.Add(model.ModelId, desc);
			}
			return Model;
		}

		/// <summary>
		/// Get Meta Data for page
		/// </summary>
		/// <param name="Model"></param>
		/// <returns></returns>
		protected UpcomingCarsDTO GetMeta(UpcomingCarsDTO Model,UpcomingCarsInputParam input) {

			Model.TotalPages = (int)Math.Ceiling((Model.UpcomingCarModels.Count > 0 ? Model.UpcomingCarModels[0].RecordCount : 0) / (double)pagesize);
			bool makeSelected = !string.IsNullOrWhiteSpace(Model.MakeName);
			int currentYear = DateTime.Now.Year;
			Model.MetaData = new PageMetaTags
			{
				Title = (string.IsNullOrWhiteSpace(Model.MakeName) ? "Upcoming" : "Upcoming " + System.Text.RegularExpressions.Regex.Replace(Model.MakeName, "maruti suzuki", "Maruti", System.Text.RegularExpressions.RegexOptions.IgnoreCase)) + " Cars - Expected Launches in " + currentYear.ToString() + "/" + (currentYear + 1).ToString() + (input.IsMobile?" - CarWale":"") ,
				Description = string.Format("Find out upcoming new cars in {0}/{1} in India. From small to super-luxury, from announced to highly speculated models, from near future to end of year, know about every upcoming car launch in India this year.", currentYear, currentYear + 1),
				Canonical = "https://www.carwale.com" + Model.Url.Replace("/m/","/"),
				DeeplinkAlternativesAndroid = makeSelected ? string.Format("/m/{0}-cars/upcoming/", Model.MakeName) : "/m/upcoming-cars/",
				Alternate = "https://www.carwale.com/m" + Model.Url
			};
			string urlWithoutPagination = Model.Url.Split(new string[] { "page/" }, StringSplitOptions.None)[0];
			Model.MetaData.NextUrl = ((Model.PageNo + 1) > Model.TotalPages) ? string.Empty : "https://www.carwale.com" + urlWithoutPagination + "page/" + (Model.PageNo + 1) + "/";
			Model.MetaData.PrevUrl = Model.PageNo == 1 ? string.Empty : ("https://www.carwale.com" + urlWithoutPagination + ((Model.PageNo - 1 > 1) ? "page/" + (Model.PageNo - 1) + "/" : string.Empty));
			return Model;
		}
	}
}
