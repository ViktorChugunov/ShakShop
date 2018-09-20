using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShakuroMarketplaceNetMVC.Models;

namespace ShakuroMarketplaceNetMVC.Controllers
{
    public class CatalogController : Controller
    {
        public ActionResult Catalog()
        {
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();//
                ViewBag.PageHeader = "Catalog";
                return View();
            }
        }

        public ActionResult Category(string category)
        {
            ViewBag.CategoriesData = GetCategoriesData();
            using (GoodContext db = new GoodContext())
            {                
                if (db.GoodCategories.Where(x => x.CategoryUrl == category).Any())
                {
                    ViewBag.PageHeader = db.GoodCategories.Where(x => x.CategoryUrl == category).First().CategoryName;//
                    ViewBag.SubcategoriesList = db.GoodCategories.Where(x => x.CategoryUrl == category).First().GoodSubcategories;//
                    ViewBag.CategoryUrl = category;
                    return View();
                }
                else {
                    return Redirect("/");
                }
            }
        }

        public ActionResult Subcategory(string category, string subcategory)
        {            
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                if (db.GoodCategories.Where(x => x.CategoryUrl == category).Any() && db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategory).Any())
                {
                    ViewBag.PageHeader = db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategory).First().SubcategoryName;
                    ViewBag.CategoryUrl = category;
                    ViewBag.SubcategoryUrl = subcategory;
                    int currentGoodSubcategoryId = db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategory).First().Id;
                    var GoodsList = db.Goods.Where(x => x.GoodSubcategoryId == currentGoodSubcategoryId)
                        .Select(p => new SubcategoryGoodsViewModel {
                            Id = p.Id,
                            GoodName = p.GoodName,
                            GoodBrand = p.GoodBrand,
                            GoodUrl = p.GoodUrl,
                            GoodColor = p.GoodColor,
                            GoodImagesUrls = p.GoodImagesUrls,
                            GoodPrice = p.GoodPrice,
                            NewGood = p.NewGood,
                            SalesGood = p.SalesGood,
                            RecommendedGood = p.RecommendedGood,
                            ReviewsNumber = p.Reviews.Count(),
                            GoodRating = p.Reviews.Any() ? p.Reviews.Average(x => x.Mark) : 0
                        }).ToList();
                    ViewBag.GoodsList = GoodsList;
                    return View();
                }
                else
                {
                    return Redirect("/");
                }                
            }
        }

        public ActionResult Good(string category, string subcategory, string goodName)
        {
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                if (db.GoodCategories.Where(x => x.CategoryUrl == category).Any() && 
                    db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategory).Any() && 
                    db.Goods.Where(x => x.GoodUrl == goodName).Any())
                {
                    ViewBag.PageHeader = db.Goods.Where(x => x.GoodUrl == goodName).First().GoodBrand + " " + 
                                         db.Goods.Where(x => x.GoodUrl == goodName).First().GoodName + ", " +
                                         db.Goods.Where(x => x.GoodUrl == goodName).First().GoodColor;
                    ViewBag.CategoryUrl = category;
                    ViewBag.SubcategoryUrl = subcategory;
                    ViewBag.GoodUrl = goodName;
                    int currentGoodId = db.Goods.Where(x => x.GoodUrl == goodName).First().Id;
                    var GoodData = db.Goods.Where(x => x.Id == currentGoodId)
                        .Select(p => new GoodViewModel
                        {
                            Id = p.Id,
                            GoodName = p.GoodName,
                            GoodBrand = p.GoodBrand,
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
                        }).First();
                    ViewBag.GoodData = GoodData;
                    ViewBag.Rev = db.Goods.First().Reviews.First().Mark;


                    Random rnd = new Random();
                    int value = rnd.Next(1, db.Goods.Count());

                    var RandomGoodData = db.Goods.Where(x => x.Id == value)
                        .Select(p => new GoodViewModel
                        {
                            Id = p.Id,
                            GoodName = p.GoodName,
                            GoodBrand = p.GoodBrand,
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
                        }).First();
                    ViewBag.RandomGoodData = RandomGoodData;
                    ViewBag.RandomGoodUrl = db.Goods.Where(x => x.Id == value).First().GoodUrl;
                    ViewBag.RandomGoodSubcategory = db.Goods.Where(x => x.Id == value).First().GoodSubcategory.SubcategoryUrl;
                    ViewBag.RandomGoodCategory = db.Goods.Where(x => x.Id == value).First().GoodSubcategory.GoodCategory.CategoryUrl;


                    return View();
                }
                else
                {
                    return Redirect("/");
                }
            }
        }

        public ActionResult Reviews(string category, string subcategory, string goodName)
        {
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                if (db.GoodCategories.Where(x => x.CategoryUrl == category).Any() && 
                    db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategory).Any() && 
                    db.Goods.Where(x => x.GoodUrl == goodName).Any())
                {
                    ViewBag.PageHeader = db.Goods.Where(x => x.GoodUrl == goodName).First().GoodBrand + " " + 
                                         db.Goods.Where(x => x.GoodUrl == goodName).First().GoodName + " Reviews";
                    ViewBag.CategoryUrl = category;
                    ViewBag.SubcategoryUrl = subcategory;
                    ViewBag.GoodUrl = goodName;
                    int currentGoodId = db.Goods.Where(x => x.GoodUrl == goodName).First().Id;                    
                    var viewModel = new ReviewViewModel()
                    {
                        goodRating = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Any() ? 
                                     db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Average(x => x.Mark) : 0,
                        reviewsNumberList = GetReviewsNumberList(currentGoodId),
                        goodReviewsList = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.ToList()                        
                    };
                    return View(viewModel);
                }
                else
                {
                    return Redirect("/");
                }
            }
        }

        public ActionResult Overview(string category, string subcategory, string goodName)
        {
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                if (db.GoodCategories.Where(x => x.CategoryUrl == category).Any() &&
                    db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategory).Any() &&
                    db.Goods.Where(x => x.GoodUrl == goodName).Any())
                {
                    ViewBag.PageHeader = db.Goods.Where(x => x.GoodUrl == goodName).First().GoodBrand + " " +
                                         db.Goods.Where(x => x.GoodUrl == goodName).First().GoodName + " Overview";
                    ViewBag.CategoryUrl = category;
                    ViewBag.SubcategoryUrl = subcategory;
                    ViewBag.GoodUrl = goodName;
                    int currentGoodId = db.Goods.Where(x => x.GoodUrl == goodName).First().Id;
                    ViewBag.goodOverview = db.Goods.Where(x => x.Id == currentGoodId).First().Overviews.First().Text;
                    return View();
                }
                else
                {
                    return Redirect("/");
                }
            }
        }

        public ActionResult Discussions(string category, string subcategory, string goodName)
        {
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                if (db.GoodCategories.Where(x => x.CategoryUrl == category).Any() &&
                    db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategory).Any() &&
                    db.Goods.Where(x => x.GoodUrl == goodName).Any())
                {
                    ViewBag.PageHeader = db.Goods.Where(x => x.GoodUrl == goodName).First().GoodBrand + " " +
                                         db.Goods.Where(x => x.GoodUrl == goodName).First().GoodName + " Discussions";
                    ViewBag.CategoryUrl = category;
                    ViewBag.SubcategoryUrl = subcategory;
                    ViewBag.GoodUrl = goodName;
                    int currentGoodId = db.Goods.Where(x => x.GoodUrl == goodName).First().Id;
                    DateTime currentDate = DateTime.Now.Date;
                    var viewModel = new DiscussionViewModel()
                    {
                        todayReviewsNumber = db.Goods.Where(x => x.Id == currentGoodId).First().Discussions.Where(c => c.Date >= currentDate && c.Date <= currentDate.AddDays(1)).Count(),
                        lastWeekReviewsNumber = db.Goods.Where(x => x.Id == currentGoodId).First().Discussions.Where(c => c.Date >= currentDate.AddDays(-7) && c.Date <= currentDate.AddDays(1)).Count(),
                        lastMonthReviewsNumber = db.Goods.Where(x => x.Id == currentGoodId).First().Discussions.Where(c => c.Date >= currentDate.AddMonths(-1) && c.Date <= currentDate.AddDays(1)).Count(),
                        lastYearReviewsNumber = db.Goods.Where(x => x.Id == currentGoodId).First().Discussions.Where(c => c.Date >= currentDate.AddYears(-1) && c.Date <= currentDate.AddDays(1)).Count(),
                        allReviewsNumber = db.Goods.Where(x => x.Id == currentGoodId).First().Discussions.Count(),
                        discussions = db.Goods.Where(x => x.Id == currentGoodId).First().Discussions.GroupBy(p => p.DiscussionGroup).ToList()
                    };
                    return View(viewModel);
                }
                else
                {
                    return Redirect("/");
                }
            }
        }

        public int[] GetReviewsNumberList(int currentGoodId)
        {
            using (GoodContext db = new GoodContext())
            {
                int reviewsNumber = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Any() ? db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Count() : 0;
                int reviewsNumberWithMarkOne = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 1).Any() ? 
                                                db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 1).Count() : 0;
                int reviewsNumberWithMarkTwo = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 2).Any() ? 
                                                db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 2).Count() : 0;
                int reviewsNumberWithMarkThree = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 3).Any() ? 
                                                db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 3).Count() : 0;
                int reviewsNumberWithMarkFour = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 4).Any() ? 
                                                db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 4).Count() : 0;
                int reviewsNumberWithMarkFive = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 5).Any() ? 
                                                db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 5).Count() : 0;
                int[] reviewsNumberList = new[] { reviewsNumberWithMarkOne, reviewsNumberWithMarkTwo, reviewsNumberWithMarkThree, reviewsNumberWithMarkFour, reviewsNumberWithMarkFive };
                return reviewsNumberList;
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
    }
}