using Carwale.BL.CMS;
using Carwale.DTOs.CMS;
using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.UI.Filters;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Carwale.UI.Controllers.Editorial
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class ExpertReviewController : Controller
    {
        private readonly IUnityContainer _container;

        public ExpertReviewController(IUnityContainer container)
        {
            _container = container;
        }

        [DeviceDetectionFilter, Route("expert-reviews/details/{id:int}/{articleMaskingName}"), Route("expert-reviews/details/{id:int}/{makeName}/{maskingName}"), Route("expert-reviews/details/{id:int}/{articleMaskingName}/{makeName}/{maskingName}")]
        public ActionResult Index(ulong id, string articleMaskingName = null, string makeName = null, string maskingName = null)
        {
            ContentDetailPagesDTO expertReviewModel = null;
            try
            {
                _container.RegisterInstance<ulong>(id);
                _container.RegisterInstance<string>(string.Format("{0},{1}", (int)CMSContentType.ComparisonTests, (int)CMSContentType.RoadTest));
                _container.RegisterInstance<bool>(false);

                articleMaskingName = articleMaskingName ?? string.Empty;
                string articleUrl = string.Empty;

                if (!string.IsNullOrEmpty(makeName) && !string.IsNullOrEmpty(maskingName))
                {
                    if (!string.IsNullOrEmpty(articleMaskingName))
                    articleUrl = string.Format("/{0}-cars/{1}/expert-reviews/{2}", makeName, maskingName, articleMaskingName);
                    else
                        articleUrl = string.Format("/{0}-cars/{1}/expert-reviews", makeName, maskingName);
                }
                else
                    articleUrl = string.Format("/expert-reviews/{0}", articleMaskingName);

                IServiceAdapterV2 expertReviewAdapter = _container.Resolve<IServiceAdapterV2>("ExpertReviewDetails");
                expertReviewModel = expertReviewAdapter.Get<ContentDetailPagesDTO, string>(articleUrl);

                if (expertReviewModel.ArticlePages != null)
                {
                    if (expertReviewModel.IsRedirect)
                    {
                        return RedirectPermanent(expertReviewModel.ArticlePages.ArticleUrl);
                    }

                    List<string> listMakeName = new List<string>();
                    List<string> listModelName = new List<string>();
                    List<string> listMaskingName = new List<string>();
                    var VehiclTagsList = expertReviewModel.ArticlePages.VehiclTagsList;
                    int versionId = 0;
                    if (VehiclTagsList != null && VehiclTagsList.Count > 0)
                    {
                        int.TryParse(expertReviewModel.ArticlePages != null && expertReviewModel.ArticlePages.VehiclTagsList.Count > 0 ? expertReviewModel.ArticlePages.VehiclTagsList[0].VersionBase.ID.ToString() : "0", out versionId);
                        listModelName = expertReviewModel.ArticlePages.VehiclTagsList.Select(x => x.ModelBase.ModelName).ToList();
                        listMakeName = expertReviewModel.ArticlePages.VehiclTagsList.Select(x => x.MakeBase.MakeName).ToList();
                        listMaskingName = expertReviewModel.ArticlePages.VehiclTagsList.Select(x => x.ModelBase.MaskingName).ToList();
                    }
                    ViewBag.TaggedMakes = string.Join(",", listMakeName.ToArray());
                    ViewBag.TaggedModels = string.Join(",", listModelName.ToArray());
                    ViewBag.TaggedMaskingNames = string.Join(",", listMaskingName.ToArray());
                    ViewBag.VersionId = versionId;
                    ViewBag.CityName = CookiesCustomers.MasterCity;
                    string commaseparatedCarName = string.Join(",", expertReviewModel.ArticlePages.VehiclTagsList.Select(x => x.MakeBase.MakeName + " " + x.ModelBase.ModelName + " " + x.VersionBase.Name));
                    expertReviewModel.BreadcrumbEntitylist = BindBreadCrumb(expertReviewModel.ArticlePages.CategoryId, expertReviewModel.ArticlePages.Title, listMakeName.Count, listModelName.Count);
                    if (expertReviewModel.ArticlePages.CategoryId == 2)
                    {
                        ViewBag.Description = string.Format("A comprehensive comparison road test of {0}. Read the full comparison review to know how these cars fair against each other.", commaseparatedCarName);
                    }
                    else
                    {
                        ViewBag.Description = string.Format("CarWale tests {0}. Read the complete road test report to know how it performed.", commaseparatedCarName);
                    }
                    expertReviewModel.ArticlePages.Description = Format.GetPlainTextFromHTML(expertReviewModel.ArticlePages.Description);
                    return View("~/Views/Editorial/ExpertReview.cshtml", expertReviewModel);
                }
                else
                {
                    return new HttpStatusCodeResult(500);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ExpertReviewController.Index()\n Exception : " + ex.Message);
                objErr.LogException();                
            }
            return Redirect("/expert-reviews/");
        }
        private List<Carwale.Entity.BreadcrumbEntity> BindBreadCrumb(ushort categoryId, string text, int makeCount = 0, int modelCount = 0)
        {
            List<Carwale.Entity.BreadcrumbEntity> _BreadcrumbEntitylist = new List<Carwale.Entity.BreadcrumbEntity>();
            if (categoryId.Equals(2))
                _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title = "Comparison Test", Link = "/expert-reviews/", Text = "Expert Reviews" });
            else if (makeCount > 1 || modelCount > 1)
            {
                _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title = "Road Test", Link = "/expert-reviews/", Text = "Expert Reviews" });
            }
            else
            {
                _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title = string.Format("{0} Cars", ViewBag.TaggedMakes), Link = string.Format("/{0}-cars/", UrlRewrite.FormatSpecial(ViewBag.TaggedMakes)), Text = ViewBag.TaggedMakes });
                _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title = string.Format("{0} {1} Cars", ViewBag.TaggedMakes, ViewBag.TaggedModels), Link = string.Format("/{0}-cars/{1}/", UrlRewrite.FormatSpecial(ViewBag.TaggedMakes), ViewBag.TaggedMaskingNames), Text = ViewBag.TaggedModels });
                _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Title = "Road Test", Link = string.Format("/{0}-cars/{1}/expert-reviews/", UrlRewrite.FormatSpecial(ViewBag.TaggedMakes), ViewBag.TaggedMaskingNames), Text = "Expert Reviews" });
            }
            _BreadcrumbEntitylist.Add(new Entity.BreadcrumbEntity { Text = text });
            return _BreadcrumbEntitylist;
        }
    }
}