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

                CartGoodsListViewModel viewModel = new CartGoodsListViewModel { };
                if ((List<int>)Session["GoodIdList"] != null)
                {
                    List<CartGoodViewModel> cartGoodsList = new List<CartGoodViewModel>() { };
                    foreach (int currentGoodId in (List<int>)Session["GoodIdList"])
                    {
                        int goodSubcategoryId = db.Goods.Find(currentGoodId).GoodSubcategoryId;
                        int goodCategoryId = db.GoodSubcategories.Find(goodSubcategoryId).GoodCategoryId;
                        string goodSubcategoryUrl = db.GoodSubcategories.Where(x => x.Id == goodSubcategoryId).First().SubcategoryUrl;
                        string goodCategoryUrl = db.GoodCategories.Where(x => x.Id == goodCategoryId).First().CategoryUrl;
                        CartGoodViewModel goodInfo = db.Goods.Where(x => x.Id == currentGoodId)
                            .Select(p => new CartGoodViewModel
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
                        cartGoodsList.Add(goodInfo);
                    }
                    viewModel = new CartGoodsListViewModel { CartGoodsList = cartGoodsList };
                }
                return View(viewModel);
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
            if ((Session["GoodIdList"] as List<int>).Contains(goodId))
            {
                (Session["GoodIdList"] as List<int>).Remove(goodId);
                if (!(Session["GoodIdList"] as List<int>).Any())
                {
                    Session["GoodIdList"] = null;
                }
            }
            return Json(Session["GoodIdList"], JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteGoodFromdCart(int goodId)
        {
            if ((Session["GoodIdList"] as List<int>).Contains(goodId))
            {
                (Session["GoodIdList"] as List<int>).Remove(goodId);
                if (!(Session["GoodIdList"] as List<int>).Any())
                {
                    Session["GoodIdList"] = null;
                }
            }
            return Redirect("/cart");
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

                if (pageUrlList.Length == 1 && pageUrlList[0].ToLower() == "cart")
                {
                    BreadCrumb breadCrumbCatalog = new BreadCrumb { Name = "Cart", Link = "/cart" };
                    BreadCrumb breadCrumbMain = new BreadCrumb { Name = "Main", Link = "/" };
                    breadCrumbsList.AddRange(new BreadCrumb[] { breadCrumbCatalog, breadCrumbMain });
                }
                else if (pageUrlList.Length == 2 && pageUrlList[0].ToLower() == "cart" && pageUrlList[1].ToLower() == "payment-methods")
                {
                    string categoryName = "Payment methods";
                    BreadCrumb breadCrumbMain = new BreadCrumb { Name = "Main", Link = "/" };
                    BreadCrumb breadCrumbCart = new BreadCrumb { Name = "Cart", Link = "/cart" };
                    BreadCrumb breadCrumbPaymentMethods = new BreadCrumb { Name = categoryName, Link = "/cart/payment-methods" };
                    breadCrumbsList.AddRange(new List<BreadCrumb>() { breadCrumbPaymentMethods, breadCrumbCart, breadCrumbMain });
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

    }
}