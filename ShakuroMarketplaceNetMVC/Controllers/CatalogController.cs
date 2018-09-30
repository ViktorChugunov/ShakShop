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
                ViewBag.CategoriesData = GetCategoriesData();
                ViewBag.PageHeader = "Catalog";                
                return View();
            }
        }

        public ActionResult Category(string categoryUrl)
        {
            ViewBag.CategoriesData = GetCategoriesData();
            using (GoodContext db = new GoodContext())
            {                
                if (db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).Any())
                {
                    string pageHeader = db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).First().CategoryName;
                    ViewBag.PageHeader = pageHeader;
                    ViewBag.CategoryUrl = categoryUrl;
                    ViewBag.SubcategoriesList = db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).First().GoodSubcategories;                    
                    return View();
                }
                else {
                    return Redirect("/");
                }
            }
        }

        public ActionResult Subcategory(string categoryUrl, string subcategoryUrl)
        {            
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                if (db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).Any() && db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).Any())
                {
                    string pageHeader = db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).First().SubcategoryName;
                    ViewBag.PageHeader = pageHeader;
                    string categoryName = db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).First().CategoryName;

                    ViewBag.CategoryUrl = categoryUrl;
                    ViewBag.SubcategoryUrl = subcategoryUrl;
                    int currentGoodSubcategoryId = db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).First().Id;
                    //
                    List<GoodViewModel> SubcategoryGoodsList = db.Goods.Where(x => x.GoodSubcategoryId == currentGoodSubcategoryId)
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
                            GoodRating = p.Reviews.Any() ? p.Reviews.Average(x => x.Mark) : 0
                        }).ToList();
                    var viewModel = new SubcategoryGoodsViewModel { SubcategoryGoodsList = SubcategoryGoodsList };
                    List<string> breadcrumbList = new List<string>() { pageHeader, categoryName, "Catalog", "Main" };
                    ViewBag.breadCrumbList = breadcrumbList;
                    return View(viewModel);
                }
                else
                {
                    return Redirect("/");
                }                
            }
        }

        public ActionResult Good(string categoryUrl, string subcategoryUrl, string goodUrl)
        {
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                if (db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).Any() && 
                    db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).Any() && 
                    db.Goods.Where(x => x.GoodUrl == goodUrl).Any())
                {
                    string goodBrand = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodBrand;
                    string goodName = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodName;
                    string goodColor = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodColor;
                    string pageHeader = goodBrand + " " + goodName + ", " + goodColor;
                    string subcategoryName = db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).First().SubcategoryName;
                    string categoryName = db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).First().CategoryName;

                    ViewBag.PageHeader = pageHeader;
                    ViewBag.CategoryUrl = categoryUrl;
                    ViewBag.SubcategoryUrl = subcategoryUrl;
                    ViewBag.GoodUrl = goodUrl;
                    int currentGoodId = db.Goods.Where(x => x.GoodUrl == goodUrl).First().Id;
                    
                    var viewModel = db.Goods.Where(x => x.Id == currentGoodId)
                        .Select(p => new GoodViewModel
                        {
                            Id = p.Id,
                            GoodName = p.GoodName,
                            GoodBrand = p.GoodBrand,
                            GoodUrl = p.GoodUrl,
                            GoodColor = p.GoodColor,
                            SameGoodColorsAndLinksList = db.Goods.Where(x => x.GoodName == goodName).Select(x => new SameGoodColorsAndLinks { GoodColor = x.GoodColor, GoodUrl = x.GoodUrl }).ToList(),
                            GoodImagesUrls = p.GoodImagesUrls,
                            GoodPrice = p.GoodPrice,
                            NewGood = p.NewGood,
                            SalesGood = p.SalesGood,
                            RecommendedGood = p.RecommendedGood,
                            ReviewsNumber = p.Reviews.Count(),
                            GoodRating = p.Reviews.Any() ? p.Reviews.Average(x => x.Mark) : 0,
                            Characteristics = p.Characteristics
                        }).First();
                    return View(viewModel);
                }
                else
                {
                    return Redirect("/");
                }
            }
        }

        public PartialViewResult SimilarOffers(string CategoryUrl, string SubcategoryUrl, string goodUrl)
        {
            using (GoodContext db = new GoodContext())
            {            
                int currentGoodId = db.Goods.Where(x => x.GoodUrl == goodUrl).First().Id;
                ViewBag.CategoryUrl = CategoryUrl;
                int[] randomItemsIndexes = GetRandomItemsIndexesList(currentGoodId);
                List<GoodViewModel> RandomGoodsList = new List<GoodViewModel>(4);
                for (int i = 0; i < 4; i++)
                {
                    int randomGoodItem = randomItemsIndexes[i];
                    var RandomGoodData = db.Goods.Where(x => x.Id == randomGoodItem)
                    .Select(p => new GoodViewModel
                    {
                        Id = p.Id,
                        GoodName = p.GoodName,
                        GoodBrand = p.GoodBrand,
                        GoodCategoryUrl = db.Goods.Where(x => x.Id == randomGoodItem).FirstOrDefault().GoodSubcategory.GoodCategory.CategoryUrl,
                        GoodSubcategoryUrl = db.Goods.Where(x => x.Id == randomGoodItem).FirstOrDefault().GoodSubcategory.SubcategoryUrl,
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
                    }).FirstOrDefault();
                    RandomGoodsList.Add(RandomGoodData);
                }
                var viewModel = new RandomGoodViewModel { RandomGoodsList = RandomGoodsList };
                return PartialView("SimilarOffers", viewModel);
            }            
        }

        public int[] GetRandomItemsIndexesList(int currentGoodId)
        {
            using (GoodContext db = new GoodContext())
            {
                int[] randomItemsIndexes = new int[4];
                List<int> goodsIdList = db.Goods.Select(p => p.Id).ToList();
                goodsIdList.RemoveAt(goodsIdList.IndexOf(currentGoodId));
                Random randomNumber = new Random();
                for (int i = 0; i < 4; i++)
                {
                    int randomIndex = randomNumber.Next(0, goodsIdList.Count());
                    randomItemsIndexes[i] = goodsIdList[randomIndex];
                    goodsIdList.RemoveAt(randomIndex);
                }
                return randomItemsIndexes;
            }
        }            

        public ActionResult Reviews(string categoryUrl, string subcategoryUrl, string goodUrl)
        {
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                if (db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).Any() && 
                    db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).Any() && 
                    db.Goods.Where(x => x.GoodUrl == goodUrl).Any())
                {
                    string goodBrand = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodBrand;
                    string goodName = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodName;
                    string goodColor = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodColor;
                    string fullGoodName = goodBrand + " " + goodName + ", " + goodColor;
                    string subcategoryName = db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).First().SubcategoryName;
                    string categoryName = db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).First().CategoryName;
                    ViewBag.PageHeader = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodBrand + " " + 
                                         db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodName + " Reviews";
                    ViewBag.CategoryUrl = categoryUrl;
                    ViewBag.SubcategoryUrl = subcategoryUrl;
                    ViewBag.GoodUrl = goodUrl;
                    ViewBag.InitiallyShowedReviewsNumber = 2;
                    int currentGoodId = db.Goods.Where(x => x.GoodUrl == goodUrl).First().Id;
                    ViewBag.GoodId = currentGoodId;
                    ViewBag.GoodName = db.Goods.Find(currentGoodId).GoodName;
                    var viewModel = new ReviewViewModel()
                    {
                        goodRating = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Any() ? 
                                     db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.Average(x => x.Mark) : 0,
                        reviewsNumberList = GetReviewsNumberList(currentGoodId),
                        goodReviewsList = db.Goods.Where(x => x.Id == currentGoodId).First().Reviews.ToList().GetRange(0, 2)
                    };
                    List<string> breadcrumbList = new List<string>() { "Reviews", fullGoodName, subcategoryName, categoryName, "Catalog", "Main" };
                    ViewBag.breadCrumbList = breadcrumbList;
                    return View(viewModel);
                }
                else
                {
                    return Redirect("/");
                }
            }
        }

        [HttpGet]
        public JsonResult ShowMoreReviews(int goodId, int showedReviewsNumber, int addedReviewsNumber)
        {
            using (GoodContext db = new GoodContext())
            {
                var reviewsList = db.Goods.Where(x => x.Id == goodId).First().Reviews
                    .Select(x => new { Reviewer = x.Reviewer, ReviewerAvatarSrc = x.ReviewerAvatarSrc, Date = x.Date.ToShortDateString(), Advantages = x.Advantages, Disadvantages = x.Disadvantages, Comment = x.Comment, Mark = x.Mark, LikesNumber = x.LikesNumber, DislikesNumber = x.DislikesNumber, ExperienceOfUse = x.ExperienceOfUse })
                    .ToList();
                int lastShowedReviewIndex = showedReviewsNumber + addedReviewsNumber;
                int reviewsListLength = reviewsList.Count();
                var addedReviewsList = reviewsList;
                string allReviewsShowed = "false";

                if (lastShowedReviewIndex < reviewsListLength)
                {
                    addedReviewsList = reviewsList.GetRange(showedReviewsNumber, addedReviewsNumber);
                }
                else
                {
                    addedReviewsList = reviewsList.GetRange(showedReviewsNumber, reviewsListLength - showedReviewsNumber);
                    allReviewsShowed = "true";
                }
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string addedReviewsJsonList = oSerializer.Serialize(addedReviewsList);
                string[] jsonResult = { allReviewsShowed, addedReviewsJsonList };
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddReview(string advantages, string disadvantages, string comment, string experienceOfUse, int mark, string categoryUrl, string subcategoryUrl, string goodUrl, string goodName)
        {
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                if (db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).Any() && 
                    db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).Any() && 
                    db.Goods.Where(x => x.GoodUrl == goodUrl).Any())
                {                    
                    Review newReview = new Review {
                        Reviewer = "asdasd",
                        ReviewerAvatarSrc = "",
                        Date = DateTime.Now.Date.ToUniversalTime(),
                        Advantages = advantages,
                        Disadvantages = disadvantages,
                        Comment = comment,
                        ExperienceOfUse = experienceOfUse,
                        Mark = mark,
                        LikesNumber = 0,
                        DislikesNumber = 0                        
                    };
                    db.Reviews.Add(newReview);

                    List<Good> goodList = db.Goods.Where(x => x.GoodName == goodName).ToList();
                    foreach (var item in goodList)
                    {
                        Good good = db.Goods.Find(item.Id);                    
                        good.Reviews.Add(newReview);
                    }
                    db.SaveChanges();
                    return Redirect("/catalog/" + categoryUrl + "/" + subcategoryUrl + "/" + goodUrl + "/Reviews");
                }
                else
                {
                    return Redirect("/");
                }
            }
        }

        public ActionResult Overview(string categoryUrl, string subcategoryUrl, string goodUrl)
        {
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                if (db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).Any() &&
                    db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).Any() &&
                    db.Goods.Where(x => x.GoodUrl == goodUrl).Any())
                {
                    string goodBrand = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodBrand;
                    string goodName = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodName;
                    string goodColor = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodColor;
                    string fullGoodName = goodBrand + " " + goodName + ", " + goodColor;
                    string subcategoryName = db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).First().SubcategoryName;
                    string categoryName = db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).First().CategoryName;

                    ViewBag.PageHeader = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodBrand + " " +
                                         db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodName + " Overview";
                    ViewBag.CategoryUrl = categoryUrl;
                    ViewBag.SubcategoryUrl = subcategoryUrl;
                    ViewBag.GoodUrl = goodUrl;
                    int currentGoodId = db.Goods.Where(x => x.GoodUrl == goodUrl).First().Id;
                    ViewBag.goodOverview = db.Goods.Where(x => x.Id == currentGoodId).First().Overviews.Any() == true ? db.Goods.Where(x => x.Id == currentGoodId).First().Overviews.First().Text : "<p class='overview-header'>There is no overview</p>";
                    List<string> breadcrumbList = new List<string>() { "Overview", fullGoodName, subcategoryName, categoryName, "Catalog", "Main" };
                    ViewBag.breadCrumbList = breadcrumbList;
                    return View();
                }
                else
                {
                    return Redirect("/");
                }
            }
        }

        public ActionResult Discussions(string categoryUrl, string subcategoryUrl, string goodUrl)
        {
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                if (db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).Any() &&
                    db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).Any() &&
                    db.Goods.Where(x => x.GoodUrl == goodUrl).Any())
                {
                    string goodBrand = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodBrand;
                    string goodName = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodName;
                    string goodColor = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodColor;
                    string fullGoodName = goodBrand + " " + goodName + ", " + goodColor;
                    string subcategoryName = db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).First().SubcategoryName;
                    string categoryName = db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).First().CategoryName;

                    ViewBag.PageHeader = db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodBrand + " " +
                                         db.Goods.Where(x => x.GoodUrl == goodUrl).First().GoodName + " Discussions";
                    ViewBag.CategoryUrl = categoryUrl;
                    ViewBag.SubcategoryUrl = subcategoryUrl;
                    ViewBag.GoodUrl = goodUrl;
                    ViewBag.InitiallyShowedDiscussionsNumber = 2;

                    int currentGoodId = db.Goods.Where(x => x.GoodUrl == goodUrl).First().Id;
                    ViewBag.GoodId = currentGoodId;
                    ViewBag.GoodName = db.Goods.Find(currentGoodId).GoodName;
                    DateTime currentDate = DateTime.Now.Date;
                    var viewModel = new DiscussionViewModel()
                    {
                        todayReviewsNumber = db.Goods.Where(x => x.Id == currentGoodId).First().Discussions.Where(c => c.Date >= currentDate && c.Date <= currentDate.AddDays(1)).Count(),
                        lastWeekReviewsNumber = db.Goods.Where(x => x.Id == currentGoodId).First().Discussions.Where(c => c.Date >= currentDate.AddDays(-7) && c.Date <= currentDate.AddDays(1)).Count(),
                        lastMonthReviewsNumber = db.Goods.Where(x => x.Id == currentGoodId).First().Discussions.Where(c => c.Date >= currentDate.AddMonths(-1) && c.Date <= currentDate.AddDays(1)).Count(),
                        lastYearReviewsNumber = db.Goods.Where(x => x.Id == currentGoodId).First().Discussions.Where(c => c.Date >= currentDate.AddYears(-1) && c.Date <= currentDate.AddDays(1)).Count(),
                        allReviewsNumber = db.Goods.Where(x => x.Id == currentGoodId).First().Discussions.Count(),
                        discussions = db.Goods.Where(x => x.Id == currentGoodId).First().Discussions.GroupBy(p => p.DiscussionGroup).ToList().GetRange(0, 2)
                    };
                    List<string> breadcrumbList = new List<string>() { "Discussions", fullGoodName, subcategoryName, categoryName, "Catalog", "Main" };
                    ViewBag.breadCrumbList = breadcrumbList;
                    return View(viewModel);
                }
                else
                {
                    return Redirect("/");
                }
            }
        }

        [HttpGet]
        public JsonResult ShowMoreDiscussions(int goodId, int showedDiscussionsNumber, int addedDiscussionsNumber)
        {
            using (GoodContext db = new GoodContext())
            {                
                var discussionsList = db.Goods.Where(x => x.Id == goodId).First().Discussions
                    .Select(x => new {
                        Message = x.Message,
                        AuthorName = x.AuthorName,
                        AuthorAvatarSrc = x.AuthorAvatarSrc,
                        Date = x.Date,
                        StringDate = x.Date.ToShortDateString(),
                        FirstDiscussionMessage = x.FirstDiscussionMessage,
                        DiscussionGroup = x.DiscussionGroup
                    })
                    .OrderByDescending(p => p.FirstDiscussionMessage).ThenBy(p => p.Date).GroupBy(p => p.DiscussionGroup).ToList();
                int lastShowedDiscussionIndex = showedDiscussionsNumber + addedDiscussionsNumber;
                int discussionsListLength = discussionsList.Count();
                var addedDiscussionsList = discussionsList;
                string allDiscussionsShowed = "false";

                if (lastShowedDiscussionIndex < discussionsListLength)
                {
                    addedDiscussionsList = discussionsList.GetRange(showedDiscussionsNumber, addedDiscussionsNumber);
                }
                else
                {
                    addedDiscussionsList = discussionsList.GetRange(showedDiscussionsNumber, discussionsListLength - showedDiscussionsNumber);
                    allDiscussionsShowed = "true";
                }
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string addedDiscussionsJsonList = oSerializer.Serialize(addedDiscussionsList);
                string[] jsonResult = { allDiscussionsShowed, addedDiscussionsJsonList };
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddMessage(string message, string categoryUrl, string subcategoryUrl, string goodUrl, string goodName, int discussionGroup)
        {
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                if (db.GoodCategories.Where(x => x.CategoryUrl == categoryUrl).Any() &&
                    db.GoodSubcategories.Where(x => x.SubcategoryUrl == subcategoryUrl).Any() &&
                    db.Goods.Where(x => x.GoodUrl == goodUrl).Any())
                {
                    Discussion newDiscussion = new Discussion
                    {
                        AuthorName = "Viktor Chugunov",
                        AuthorAvatarSrc = "",
                        Date = DateTime.Now.Date.ToUniversalTime(),
                        Message = message,
                        FirstDiscussionMessage = discussionGroup == -1 ? true : false,
                        DiscussionGroup = discussionGroup == -1 ? db.Discussions.Select(p => p.DiscussionGroup).Max() + 1 : discussionGroup
                    };
                    db.Discussions.Add(newDiscussion);

                    List<Good> goodList = db.Goods.Where(x => x.GoodName == goodName).ToList();
                    foreach (var item in goodList)
                    {
                        Good good = db.Goods.Find(item.Id);
                        good.Discussions.Add(newDiscussion);
                    }
                    db.SaveChanges();
                    return Redirect("/catalog/" + categoryUrl + "/" + subcategoryUrl + "/" + goodUrl + "/Discussions");
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

        public PartialViewResult BreadCrumbs(string pageUrl)
        {
            using (GoodContext db = new GoodContext())
            {
                string[] pageUrlList = pageUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                List<BreadCrumb> breadCrumbsList = new List<BreadCrumb>();

                if (pageUrlList.Length == 1 && pageUrlList[0].ToLower() == "catalog")
                {
                    BreadCrumb breadCrumbCatalog = new BreadCrumb { Name = "Catalog", Link = "/catalog" };
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
                var viewModel = new BreadCrumbsListViewModel { BreadCrumbsList = breadCrumbsList };
                return PartialView("_BreadCrumbs", viewModel);
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