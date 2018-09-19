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
                        GoodRating = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Any() ? 
                                     db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Average(x => x.Mark) : 0,
                        ReviewsRationList = GetReviewsRationList(currentGoodId),
                        GoodReviewsList = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.ToList()                        
                    };
                    return View(viewModel);
                }
                else
                {
                    return Redirect("/");
                }
            }
        }

        public int[] GetReviewsRationList(int currentGoodId)
        {
            using (GoodContext db = new GoodContext())
            {
                int[] reviewsRationList;
                int reviewsNumber = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Any() ? db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Count() : 0;
                int reviewsNumberWithMarkOne = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 1).Any() ? db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 1).Count() : 0;
                int reviewsNumberWithMarkTwo = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 2).Any() ? db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 2).Count() : 0;
                int reviewsNumberWithMarkThree = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 3).Any() ? db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 3).Count() : 0;
                int reviewsNumberWithMarkFour = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 4).Any() ? db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 4).Count() : 0;
                int reviewsNumberWithMarkFive = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 5).Any() ? db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 5).Count() : 0;
                if (reviewsNumber == 0)
                {
                    reviewsRationList = new[] { 0, 0, 0, 0, 0 };
                }
                else
                {
                    int reviewsRationWithMarkOne = 100 * reviewsNumberWithMarkOne / reviewsNumber;
                    int reviewsRationWithMarkTwo = 100 * reviewsNumberWithMarkTwo / reviewsNumber;
                    int reviewsRationWithMarkThree = 100 * reviewsNumberWithMarkThree / reviewsNumber;
                    int reviewsRationWithMarkFour = 100 * reviewsNumberWithMarkFour / reviewsNumber;
                    int reviewsRationWithMarkFive = 100 - reviewsRationWithMarkOne - reviewsRationWithMarkTwo - reviewsRationWithMarkThree - reviewsRationWithMarkFour;
                    reviewsRationList = new[] { reviewsRationWithMarkOne, reviewsRationWithMarkTwo, reviewsRationWithMarkThree, reviewsRationWithMarkFour, reviewsRationWithMarkFive };
                }
                return reviewsRationList;
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