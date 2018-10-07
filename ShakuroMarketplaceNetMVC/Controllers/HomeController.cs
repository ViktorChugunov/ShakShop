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
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                MainPageViewModel viewModel = new MainPageViewModel { };
                List<GoodViewModel> viewedGoodsList = new List<GoodViewModel>() { };

                if ((List<int>)Session["recentlyViewedGoods"] != null)
                {                    
                    foreach (int currentGoodId in (List<int>)Session["recentlyViewedGoods"])
                    {
                        int goodSubcategoryId = db.Goods.Where(x => x.Id == currentGoodId).First().GoodSubcategoryId;
                        int goodCategoryId = db.GoodSubcategories.Where(x => x.Id == goodSubcategoryId).First().GoodCategoryId;

                        string goodSubcategoryUrl = db.GoodSubcategories.Where(x => x.Id == goodSubcategoryId).First().SubcategoryUrl;
                        string goodCategoryUrl = db.GoodCategories.Where(x => x.Id == goodCategoryId).First().CategoryUrl;
                        GoodViewModel goodInfo = db.Goods.Where(x => x.Id == currentGoodId)
                            .Select(p => new GoodViewModel
                            {
                                Id = p.Id,
                                GoodName = p.GoodName,
                                GoodBrand = p.GoodBrand,
                                GoodCategoryUrl = goodCategoryUrl,
                                GoodSubcategoryUrl = goodSubcategoryUrl,
                                GoodUrl = p.GoodUrl,
                                GoodColor = p.GoodColor,
                                GoodImagesUrls = p.GoodImagesUrls,
                                GoodPrice = p.GoodPrice,
                                SalesGood = p.SalesGood
                            }).First();
                        viewedGoodsList.Add(goodInfo);
                    }                    
                }

                List<GoodViewModel> interestingGoodsList = new List<GoodViewModel>() { };
                if (HttpContext.Request.RawUrl == "/sales-goods")
                {
                    interestingGoodsList = db.Goods.Where(x => x.SalesGood == true)
                        .Select(p => new GoodViewModel
                        {
                            Id = p.Id,
                            GoodName = p.GoodName,
                            GoodBrand = p.GoodBrand,
                            GoodCategoryUrl = db.GoodCategories.Where(x => x.Id == db.Goods.Where(y => y.Id == p.Id).FirstOrDefault().GoodSubcategory.GoodCategoryId).FirstOrDefault().CategoryUrl,
                            GoodSubcategoryUrl = db.Goods.Where(x => x.Id == p.Id).FirstOrDefault().GoodSubcategory.SubcategoryUrl,
                            GoodUrl = p.GoodUrl,
                            GoodColor = p.GoodColor,
                            GoodImagesUrls = p.GoodImagesUrls,
                            GoodPrice = p.GoodPrice,
                            NewGood = p.NewGood,
                            SalesGood = p.SalesGood,
                            RecommendedGood = p.RecommendedGood,
                            ReviewsNumber = p.Reviews.Count(),
                            GoodRating = p.Reviews.Any() ? p.Reviews.Average(x => x.Mark) : 0
                        }).ToList().GetRange(0, 8);
                }
                else if (HttpContext.Request.RawUrl == "/recommended-goods")
                {
                    interestingGoodsList = db.Goods.Where(x => x.RecommendedGood == true)
                        .Select(p => new GoodViewModel
                        {
                            Id = p.Id,
                            GoodName = p.GoodName,
                            GoodBrand = p.GoodBrand,
                            GoodCategoryUrl = db.GoodCategories.Where(x => x.Id == db.Goods.Where(y => y.Id == p.Id).FirstOrDefault().GoodSubcategory.GoodCategoryId).FirstOrDefault().CategoryUrl,
                            GoodSubcategoryUrl = db.Goods.Where(x => x.Id == p.Id).FirstOrDefault().GoodSubcategory.SubcategoryUrl,
                            GoodUrl = p.GoodUrl,
                            GoodColor = p.GoodColor,
                            GoodImagesUrls = p.GoodImagesUrls,
                            GoodPrice = p.GoodPrice,
                            NewGood = p.NewGood,
                            SalesGood = p.SalesGood,
                            RecommendedGood = p.RecommendedGood,
                            ReviewsNumber = p.Reviews.Count(),
                            GoodRating = p.Reviews.Any() ? p.Reviews.Average(x => x.Mark) : 0
                        }).ToList().GetRange(0, 8);
                }
                else if (HttpContext.Request.RawUrl == "/new-goods")
                {
                    interestingGoodsList = db.Goods.Where(x => x.NewGood == true )
                        .Select(p => new GoodViewModel
                        {
                            Id = p.Id,
                            GoodName = p.GoodName,
                            GoodBrand = p.GoodBrand,
                            GoodCategoryUrl = db.GoodCategories.Where(x => x.Id == db.Goods.Where(y => y.Id == p.Id).FirstOrDefault().GoodSubcategory.GoodCategoryId).FirstOrDefault().CategoryUrl,
                            GoodSubcategoryUrl = db.Goods.Where(x => x.Id == p.Id).FirstOrDefault().GoodSubcategory.SubcategoryUrl,
                            GoodUrl = p.GoodUrl,
                            GoodColor = p.GoodColor,
                            GoodImagesUrls = p.GoodImagesUrls,
                            GoodPrice = p.GoodPrice,
                            NewGood = p.NewGood,
                            SalesGood = p.SalesGood,
                            RecommendedGood = p.RecommendedGood,
                            ReviewsNumber = p.Reviews.Count(),
                            GoodRating = p.Reviews.Any() ? p.Reviews.Average(x => x.Mark) : 0
                        }).ToList().GetRange(0, 8);
                }
                else
                {
                    interestingGoodsList = db.Goods.Where(x => x.SalesGood == true || x.NewGood == true || x.RecommendedGood == true)
                        .Select(p => new GoodViewModel
                        {
                            Id = p.Id,
                            GoodName = p.GoodName,
                            GoodBrand = p.GoodBrand,
                            GoodCategoryUrl = db.GoodCategories.Where(x => x.Id == db.Goods.Where(y => y.Id == p.Id).FirstOrDefault().GoodSubcategory.GoodCategoryId).FirstOrDefault().CategoryUrl,
                            GoodSubcategoryUrl = db.Goods.Where(x => x.Id == p.Id).FirstOrDefault().GoodSubcategory.SubcategoryUrl,
                            GoodUrl = p.GoodUrl,
                            GoodColor = p.GoodColor,
                            GoodImagesUrls = p.GoodImagesUrls,
                            GoodPrice = p.GoodPrice,
                            NewGood = p.NewGood,
                            SalesGood = p.SalesGood,
                            RecommendedGood = p.RecommendedGood,
                            ReviewsNumber = p.Reviews.Count(),
                            GoodRating = p.Reviews.Any() ? p.Reviews.Average(x => x.Mark) : 0
                        }).ToList().GetRange(0, 8);
                }
                viewModel = new MainPageViewModel { InterestingGoodsList = interestingGoodsList, ViewedGoodsList = viewedGoodsList };
                return View(viewModel);
            }
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

                var GoodSearchResultList = db.Goods.Where(x => (x.GoodBrand + " " + x.GoodName + " " + x.GoodColor).Contains(searchInputData)
                                                            || (x.GoodBrand + " " + x.GoodColor + " " + x.GoodName).Contains(searchInputData)
                                                            || (x.GoodName + " " + x.GoodBrand + " " + x.GoodColor).Contains(searchInputData)
                                                            || (x.GoodName + " " + x.GoodColor + " " + x.GoodBrand).Contains(searchInputData)
                                                            || (x.GoodColor + " " + x.GoodName + " " + x.GoodBrand).Contains(searchInputData)
                                                            || (x.GoodColor + " " + x.GoodBrand + " " + x.GoodName).Contains(searchInputData))
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
                var goodList = db.Goods.Where(x => (x.GoodBrand + " " + x.GoodName + " " + x.GoodColor).Contains(searchInputData)
                                                || (x.GoodBrand + " " + x.GoodColor + " " + x.GoodName).Contains(searchInputData)
                                                || (x.GoodName + " " + x.GoodBrand + " " + x.GoodColor).Contains(searchInputData)
                                                || (x.GoodName + " " + x.GoodColor + " " + x.GoodBrand).Contains(searchInputData)
                                                || (x.GoodColor + " " + x.GoodName + " " + x.GoodBrand).Contains(searchInputData)
                                                || (x.GoodColor + " " + x.GoodBrand + " " + x.GoodName).Contains(searchInputData))
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