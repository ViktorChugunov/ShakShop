using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ShakuroMarketplaceNetMVC.Models;

namespace ShakuroMarketplaceNetMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.CategoriesData = GetCategoriesData();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.CategoriesData = GetCategoriesData();
            ViewBag.Message = "Your application description page.";            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public PartialViewResult Register()
        {
            return PartialView("Register", new RegisterViewModel());
        }

        public PartialViewResult Login()
        {
            return PartialView("Login", new LoginViewModel());
        }

        [HttpGet]
        public ActionResult GoodSearch(string searchInputData)
        {
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                ViewBag.PageHeader = "Good Search Results";

                var GoodSearchResultList = db.Goods.Where(x => x.GoodName.Contains(searchInputData) || x.GoodBrand.Contains(searchInputData))
                    .Select(p => new GoodViewModel
                    {
                        Id = p.Id,
                        GoodName = p.GoodName,
                        GoodBrand = p.GoodBrand,
                        GoodCategoryUrl = db.Goods.Where(x => x.Id == p.Id).FirstOrDefault().GoodSubcategory.GoodCategory.CategoryUrl,
                        GoodSubcategoryUrl = db.Goods.Where(x => x.Id == p.Id).FirstOrDefault().GoodSubcategory.SubcategoryUrl,
                        GoodUrl = p.GoodUrl,
                        GoodColor = p.GoodColor,
                        GoodImagesUrls = p.GoodImagesUrls,
                        GoodPrice = p.GoodPrice,
                        NewGood = p.NewGood,
                        SalesGood = p.SalesGood,
                        RecommendedGood = p.RecommendedGood,
                        ReviewsNumber = p.Reviews.Count(),
                        GoodRating = p.Reviews.Any() ? p.Reviews.Average(x => x.Mark) : 0,
                        Characteristics = p.Characteristics
                    }).ToList();
                var viewModel = new GoodSearchResultsViewModel { GoodSearchResult = GoodSearchResultList };
                return View(viewModel);
            }
        }

        [HttpGet]
        public JsonResult FastGoodSearch(string searchInputData)
        {
            using (GoodContext db = new GoodContext())
            {   
                var goodList = db.Goods.Where(x => x.GoodName.Contains(searchInputData) || x.GoodBrand.Contains(searchInputData))
                    .Select(p => new {
                        GoodName = p.GoodName,
                        GoodBrand = p.GoodBrand,
                        GoodImageUrl = "/Content/Images/Goods/" 
                                       + db.GoodCategories.Where(x => x.Id == db.GoodSubcategories.Where(y => y.Id == db.Goods.Where(z => z.Id == z.Id).FirstOrDefault().GoodSubcategoryId).FirstOrDefault().GoodCategoryId).FirstOrDefault().CategoryUrl 
                                       + "/" + db.GoodSubcategories.Where(x => x.Id == db.Goods.Where(y => y.Id == y.Id).FirstOrDefault().GoodSubcategoryId).FirstOrDefault().SubcategoryUrl 
                                       + "/" + p.GoodUrl + "/" + p.GoodImagesUrls.Substring(0, p.GoodImagesUrls.IndexOf("::")),
                        GoodPageLink = "/catalog/" + db.GoodCategories.Where(x => x.Id == db.GoodSubcategories.Where(y => y.Id == db.Goods.Where(z => z.Id == z.Id).FirstOrDefault().GoodSubcategoryId).FirstOrDefault().GoodCategoryId).FirstOrDefault().CategoryUrl + "/" + db.GoodSubcategories.Where(x => x.Id == db.Goods.Where(y => y.Id == y.Id).FirstOrDefault().GoodSubcategoryId).FirstOrDefault().SubcategoryUrl + "/" + p.GoodUrl + "/",
                        GoodColor = p.GoodColor,
                        GoodPrice = p.GoodPrice
                    }).OrderBy(p => p.GoodColor).ToList();
                int goodListLength = goodList.Count();
                if (goodListLength <= 5)
                {
                    goodList = goodList.GetRange(0, goodListLength);
                }
                else
                {
                    goodList = goodList.GetRange(0, 5);
                }
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonGoodList = oSerializer.Serialize(goodList);
                return Json(jsonGoodList, JsonRequestBehavior.AllowGet);
            }
        }

        public IEnumerable<GoodCategory> GetCategoriesData()
        {
            using (GoodContext db = new GoodContext())
            {
                IEnumerable<GoodCategory> goodCategories = db.GoodCategories.ToList();
                return goodCategories;
            }
        }

        public PartialViewResult BreadCrumbs(string pageUrl)
        {
            using (GoodContext db = new GoodContext())
            {
                string[] pageUrlList = pageUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                List<BreadCrumb> breadCrumbsList = new List<BreadCrumb>();

                if (pageUrlList.Length == 2 && pageUrlList[0].ToLower() == "good-search-result")
                {
                    BreadCrumb breadCrumbCatalog = new BreadCrumb { Name = "Good search results", Link = "/good-search-result" };
                    BreadCrumb breadCrumbMain = new BreadCrumb { Name = "Main", Link = "/" };
                    breadCrumbsList.AddRange(new BreadCrumb[] { breadCrumbCatalog, breadCrumbMain });
                }
                else if (pageUrlList.Length == 2 && pageUrlList[0].ToLower() == "cart" && pageUrlList[1].ToLower() == "confirm-order")
                {
                    string categoryName = "Confirm order";
                    BreadCrumb breadCrumbMain = new BreadCrumb { Name = "Main", Link = "/" };
                    BreadCrumb breadCrumbCart = new BreadCrumb { Name = "Cart", Link = "/cart" };
                    BreadCrumb breadCrumbConfirmOrder = new BreadCrumb { Name = categoryName, Link = "/cart/confirm-order" };
                    breadCrumbsList.AddRange(new List<BreadCrumb>() { breadCrumbConfirmOrder, breadCrumbCart, breadCrumbMain });
                }
                else
                {
                    BreadCrumb breadCrumbMain = new BreadCrumb { Name = "Main", Link = "/" };
                    BreadCrumb breadCrumbUnknown = new BreadCrumb { Name = "Unknown Link", Link = "/" };
                    breadCrumbsList.AddRange(new List<BreadCrumb>() { breadCrumbUnknown, breadCrumbMain });
                }

                var viewModel = new BreadCrumbsListViewModel { BreadCrumbsList = breadCrumbsList };
                return PartialView("_BreadCrumbs", viewModel);
            }
        }
    }
}