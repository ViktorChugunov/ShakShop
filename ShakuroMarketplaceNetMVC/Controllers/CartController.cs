using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShakuroMarketplaceNetMVC.Models;

using System.IO;
using System.Net;
using System.Net.Mail;

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
                        int goodSubcategoryId = db.Goods.Where(x => x.Id == currentGoodId).First().GoodSubcategoryId;
                        int goodCategoryId = db.GoodSubcategories.Where(x => x.Id == goodSubcategoryId).First().GoodCategoryId;

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

        public ActionResult ConfirmOrder(OrderInformation orderInformation)
        {
            using (GoodContext db = new GoodContext())
            {
                ViewBag.CategoriesData = GetCategoriesData();
                ViewBag.PageHeader = "Cart";
                Session["GoodIdList"] = null;


                string orderPrice = (orderInformation.goodsTotalPrice + orderInformation.deliveryPrice).ToString();
                string productsHTML = "";
                for (int i = 0; i < orderInformation.goodNameArray.Count(); i++)
                {
                    productsHTML += @"
                        <!-- Product -->
                        <tr>
                            <td>
                                <table width = '100%' style = 'border-collapse:collapse; border-top:1px solid #E5E5E5;'>   
                                    <tr><td height='24' colspan ='5'></td></tr>        
                                    <tr>        
                                        <td width='24'></td>         
                                        <td width='80'><img src='http://localhost:52248/" + orderInformation.goodImageUrlArray[i] + @"' width='80' alt='" + orderInformation.goodNameArray[i] + @"'></td>                  
                                        <td width='24'></td>                   
                                        <td >                   
                                            <a href='#' style='font-size:20px; color:#424A5B; text-decoration:none'>" + orderInformation.goodNameArray[i] + @"</span><br>                          
                                            <span style='font-size:12px; color:#939CA2;'>Purchased: " + DateTime.Now.ToUniversalTime() + @"</span >                               
                                        </td>                               
                                        <td width='124'>                                
                                            <table style = 'border-collapse:collapse'>                                 
                                                <tr>                                 
                                                    <td width='24'></td>                                  
                                                    <td width='80' style = 'color:#424A5B; text-align: right;'>
                                                         " + orderInformation.goodQuantityArray[i] + @" X $"
                                                        + orderInformation.goodPriceArray[i] + @"<br> $" 
                                                        + orderInformation.goodTotalPriceArray[i] + @"
                                                    </td >
                                                    <td width='24'></td>                                         
                                                </tr>                                         
                                            </table>                                         
                                        </td>                                         
                                    </tr>                                         
                                    <tr><td height='24' colspan='5'></td></tr>        
                                </table>                                              
                            </td>                                              
                        </tr>                                              
                        <!-- Product end -->                                                                                   
                    ";     
                }
                string mailBody = @"                    
                    <table align='center' bgcolor='#f9f8fb' style='font-family:Helvetica;border-collapse:collapse;width:100%;'>
			            <tr>
				            <td colspan='3' height='24'></td>
			            </tr>
			            <tr align='center'>
				            <td width='24'></td>				
				            <td>
					            <!-- Mail information -->
					            <table style='border-collapse:collapse; width:100%;'>
						            <!-- Header -->
						            <tr align='center'>
							            <td>
								            <table>
									            <tr width='100%' height='24'><td></td></tr>
									            <tr><td><a href='http://localhost:52248/'><img src='http://www.imageup.ru/img136/3124743/marketplace.png' width='114' alt='marketplace.png'></a></td></tr>
									            <tr width='100%' height='24'><td></td></tr>							
								            </table>
							            </td>
						            </tr>
						            <!-- Body -->
						            <tr align='center'>
							            <td>
								            <table bgcolor='#ffffff'  style='border-collapse:collapse; border: 1px solid #E5E5E5; padding: 0;'>
									            <tr>
										            <td height='24'></td>
									            </tr>
									            <!-- Greeting -->
									            <tr>
										            <td>
											            <table width='100%' style='border-collapse:collapse'>
												            <tr>
													            <td width='24'></td>
													            <td width='50' align='center'>
														            <img src='http://www.imageup.ru/img247/3125999/userpic.png' width='50' alt='userpic.png'>
													            </td>
													            <td>
														            <table bgcolor='#ffffff' style='border-collapse:collapse;border-bottom:none;' width='100%'>
															            <tr>
																            <td width='13'></td>
																            <td align='left' height='34' style='font-size:18px; color:#2C3241; vertical-align:middle;'>
																	            Dear, <span style='font-style: regular; color: #2C3241; font-weight: 600;'>" + orderInformation.customerName + @"</span>!<br>
																	            <span style='font-size: 14px;'>You have made an order! Order information is shown below.</span>
																            </td>										
															            </tr>
														            </table>
													            </td>
													            <td width='162'>
														            <table>
															            <tr>
																            <td width='18'></td>
																            <td width='126'>
																	            <a href='http://localhost:52248/'><img src='http://www.imageup.ru/img247/3126000/view_btn.png' width='126' alt='View profile'></a>
																            </td>					
																            <td width='18'></td>
															            </tr>
														            </table>
													            </td>
												            </tr>
											            </table>
										            </td>
									            </tr>
									            <!-- Greeting end -->
									            <tr>
										            <td height='24'></td>
									            </tr>"

                                                + productsHTML +

                                                @"
                                                <!-- Delivery method -->
									            <tr>
										            <td>
											            <table width='100%' style='height:49px; border-collapse:collapse; border-top:1px solid #E5E5E5;'>
												            <tr>
													            <td width='24'></td>
													            <td align='left' style='font-size:12px; color:#939CA2;'>" + orderInformation.deliveryMethods + @"</td>
													            <td align='right' style='font-size:16px; color:#424A5B;'>$ " + orderInformation.deliveryPrice + @"</td>
													            <td width='24'></td>
												            </tr>
											            </table>
										            </td>
									            </tr>
									            <!-- Delivery method end -->

                                                <!-- Total price -->
									            <tr>
										            <td>
											            <table width='100%' style='height:49px; border-collapse:collapse; border-top:1px solid #E5E5E5;'>
												            <tr>
													            <td width='24'></td>
													            <td align='left' style='font-size:12px; color:#939CA2;'>TOTAL</td>
													            <td align='right' style='font-size:16px; color:#424A5B;'>$ " + orderPrice + @"</td>
													            <td width='24'></td>
												            </tr>
											            </table>
										            </td>
									            </tr>
									            <!-- Total price end -->
								            </table>
							            </td>
						            </tr>
						            <tr>
							            <td align='center'>
								            <table>
									            <tr width='100%' height='24'><td></td></tr>
									            <tr><td align='center' style='font-size:12px; color:#7E8993;'><a href='http://localhost:52248/' style='font-size:12px; color:#7E8993; text-decoration:none;'>2018 © Marketplace.com</a>. All Rights Reserved</td></tr>
									            <tr><td align='center' style='font-size:12px; color:#7E8993;'><a href='#' style='font-size:10px;color:#7E8993;text-decoration:underline;'>Subscribe settings</a></td></tr>
									            <tr width='100%' height='24'><td></td></tr>							
								            </table>							
							            </td>
						            </tr>					
					            </table>
				            </td>
				            <td width='24'></td>				
			            </tr>
			            <tr>
				            <td colspan='3' height='24'></td>
			            </tr>
		            </table>                    
                ";
                
                using (MailMessage mm = new MailMessage("ShakuroMarketplace@gmail.com", "vik-chugun@yandex.ru"))
                {
                    mm.Subject = "Order №213";
                    mm.Body = mailBody;
                    mm.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("ShakuroMarketplace@gmail.com", "ShakuroMarketplace2018");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }
                }



                //return Redirect("cart/order-information");
                OrderInformation viewModel = orderInformation;
                return View(viewModel);
            }
        }
        /*
        public ActionResult OrderInformationRoute(viewModel)
        {

            return View(viewModel);
        }
        */
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
                else if (pageUrlList.Length == 2 && pageUrlList[0].ToLower() == "cart" && pageUrlList[1].ToLower() == "confirm-order")
                {
                    string categoryName = "Confirm order";
                    BreadCrumb breadCrumbMain = new BreadCrumb { Name = "Main", Link = "/" };
                    BreadCrumb breadCrumbCart = new BreadCrumb { Name = "Cart", Link = "/cart" };
                    BreadCrumb breadCrumbPaymentMethods = new BreadCrumb { Name = categoryName, Link = "/cart/confirm-order" };
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