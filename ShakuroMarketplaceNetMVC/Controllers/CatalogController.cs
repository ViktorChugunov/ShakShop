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
                    ViewBag.GoodRating = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Any() ? db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Average(x => x.Mark) : 0;
                    ViewBag.OneMark = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 1).Any() ? db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Where(x => x.Mark == 1).Count() : 0;
                    var GoodReviewsList = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews;
                    ViewBag.GoodReviewsList = GoodReviewsList;
                    return View();
                }
                else
                {
                    return Redirect("/");
                }
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