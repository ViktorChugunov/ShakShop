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