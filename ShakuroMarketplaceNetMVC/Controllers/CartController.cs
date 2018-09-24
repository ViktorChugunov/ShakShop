using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShakuroMarketplaceNetMVC.Models;

namespace ShakuroMarketplaceNetMVC.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                ViewBag.PageHeader = "Cart";
                return View();
            }
        }

        public PartialViewResult BreadCrumbs(string pageUrl)
        {
            using (GoodContext db = new GoodContext())
            {
                string[] pageUrlList = pageUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                List<BreadCrumb> breadCrumbsList = new List<BreadCrumb>();

                if (pageUrlList.Length == 1 && pageUrlList[0].ToLower() == "cart")
                {
                    BreadCrumb breadCrumbCatalog = new BreadCrumb { Name = "Cart", Link = "/cart" };
                    BreadCrumb breadCrumbMain = new BreadCrumb { Name = "Main", Link = "/" };
                    breadCrumbsList.AddRange(new BreadCrumb[] { breadCrumbCatalog, breadCrumbMain });
                }
                else if (pageUrlList.Length == 2 && pageUrlList[0].ToLower() == "catalog")
                {
                    string categoryUrl = pageUrlList[1];
                    string categoryName = db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).First().CategoryName;
                    BreadCrumb breadCrumbMain = new BreadCrumb { Name = "Main", Link = "/" };
                    BreadCrumb breadCrumbCatalog = new BreadCrumb { Name = "Catalog", Link = "/catalog" };
                    BreadCrumb breadCrumbCategory = new BreadCrumb { Name = categoryName, Link = "/catalog/" + categoryUrl };
                    breadCrumbsList.AddRange(new List<BreadCrumb>() { breadCrumbCategory, breadCrumbCatalog, breadCrumbMain });
                }
                else if (pageUrlList.Length == 3 && pageUrlList[0].ToLower() == "catalog")
                {
                    string categoryUrl = pageUrlList[1];
                    string subcategoryUrl = pageUrlList[2];
                    string categoryName = db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).First().CategoryName;
                    string subcategoryName = db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).First().SubcategoryName;
                    BreadCrumb breadCrumbMain = new BreadCrumb { Name = "Main", Link = "/" };
                    BreadCrumb breadCrumbCatalog = new BreadCrumb { Name = "Catalog", Link = "/catalog" };
                    BreadCrumb breadCrumbCategory = new BreadCrumb { Name = categoryName, Link = "/catalog/" + categoryUrl };
                    BreadCrumb breadCrumbSubcategory = new BreadCrumb { Name = subcategoryName, Link = "/catalog/" + categoryUrl + "/" + subcategoryUrl };
                    breadCrumbsList.AddRange(new List<BreadCrumb>() { breadCrumbSubcategory, breadCrumbCategory, breadCrumbCatalog, breadCrumbMain });
                }
                else if (pageUrlList.Length == 4 && pageUrlList[0].ToLower() == "catalog")
                {
                    string categoryUrl = pageUrlList[1];
                    string subcategoryUrl = pageUrlList[2];
                    string goodUrl = pageUrlList[3];

                    string categoryName = db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).First().CategoryName;
                    string subcategoryName = db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).First().SubcategoryName;
                    string goodName = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodBrand + " " +
                                      db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodName + ", " +
                                      db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodColor;

                    BreadCrumb breadCrumbMain = new BreadCrumb { Name = "Main", Link = "/" };
                    BreadCrumb breadCrumbCatalog = new BreadCrumb { Name = "Catalog", Link = "/catalog" };
                    BreadCrumb breadCrumbCategory = new BreadCrumb { Name = categoryName, Link = "/catalog/" + categoryUrl };
                    BreadCrumb breadCrumbSubcategory = new BreadCrumb { Name = subcategoryName, Link = "/catalog/" + categoryUrl + "/" + subcategoryUrl };
                    BreadCrumb breadCrumbGood = new BreadCrumb { Name = goodName, Link = "/catalog/" + categoryUrl + "/" + subcategoryUrl + "/" + goodUrl };
                    breadCrumbsList.AddRange(new List<BreadCrumb>() { breadCrumbGood, breadCrumbSubcategory, breadCrumbCategory, breadCrumbCatalog, breadCrumbMain });
                }
                else if (pageUrlList.Length == 5 && pageUrlList[0].ToLower() == "catalog" && (pageUrlList[4].ToLower() == "reviews" || pageUrlList[4].ToLower() == "discussions" || pageUrlList[4].ToLower() == "overview"))
                {
                    string categoryUrl = pageUrlList[1];
                    string subcategoryUrl = pageUrlList[2];
                    string goodUrl = pageUrlList[3];
                    string categoryName = db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).First().CategoryName;
                    string subcategoryName = db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).First().SubcategoryName;
                    string goodName = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodBrand + " " +
                                      db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodName + ", " +
                                      db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodColor;
                    BreadCrumb breadCrumbMain = new BreadCrumb { Name = "Main", Link = "/" };
                    BreadCrumb breadCrumbCatalog = new BreadCrumb { Name = "Catalog", Link = "/catalog" };
                    BreadCrumb breadCrumbCategory = new BreadCrumb { Name = categoryName, Link = "/catalog/" + categoryUrl };
                    BreadCrumb breadCrumbSubcategory = new BreadCrumb { Name = subcategoryName, Link = "/catalog/" + categoryUrl + "/" + subcategoryUrl };
                    BreadCrumb breadCrumbGood = new BreadCrumb { Name = goodName, Link = "/catalog/" + categoryUrl + "/" + subcategoryUrl + "/" + goodUrl };
                    BreadCrumb breadCrumbGoodOption = new BreadCrumb { Name = pageUrlList[4], Link = "/catalog/" + categoryUrl + "/" + subcategoryUrl + "/" + goodUrl + "/" + pageUrlList[4] };
                    breadCrumbsList.AddRange(new List<BreadCrumb>() { breadCrumbGoodOption, breadCrumbGood, breadCrumbSubcategory, breadCrumbCategory, breadCrumbCatalog, breadCrumbMain });
                }
                else
                {
                    BreadCrumb breadCrumbMain = new BreadCrumb { Name = "Main", Link = "/" };
                    breadCrumbsList.AddRange(new List<BreadCrumb>() { breadCrumbMain });
                }

                var viewModel = new BreadCrumbsListViewModel { BreadCrumbsList = breadCrumbsList };
                return PartialView("_BreadCrumbs", viewModel);
            }
        }

        [HttpGet]
        public JsonResult AddGoodToCart(int goodId)
        {
            if (Session["GoodIdList"] == null)
            {
                Session["GoodIdList"] = new List<int>();
            }
            if (!(Session["GoodIdList"] as List<int>).Contains(goodId))
            {
                (Session["GoodIdList"] as List<int>).Add(goodId);
            }         

            return Json(Session["GoodIdList"], JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RemoveGoodFromdCart(int goodId)
        {
            if (Session["GoodIdList"] == null)
            {
                Session["GoodIdList"] = new List<int>();
            }
            if ((Session["GoodIdList"] as List<int>).Contains(goodId))
            {
                (Session["GoodIdList"] as List<int>).Remove(goodId);
            }         

            return Json(Session["GoodIdList"], JsonRequestBehavior.AllowGet);
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