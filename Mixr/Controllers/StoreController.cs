using Mixr.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Mixr.Controllers
{
    public class StoreController : Controller
    {
        Entities db = new Entities();

        StoreViewModel store = new StoreViewModel();

        Cart cart = new Cart();

        // GET: Store
        public ActionResult Index()
        {
            var Model = store.GetAllProduct();

            return View(Model);
        }

        // GET: Store/Products
        public ActionResult Products(string categoryName)
        {
            if (categoryName == null)
                return RedirectToAction("Index");

            var Model = store.GetAllProduct().Where(x => x.Category_Id == categoryName);

            if (Model.Count() == 0)
                return RedirectToAction("Index");

            return View(Model);
        }

        // GET: Store/Product/{id}
        public ActionResult Product(int id)
        {
            ProductViewModel productViewModel = new ProductViewModel();
            Review review = new Review();
            UserModel user = new UserModel();

            // build product view model
            var Products = new ProductViewModel()
            {
                ProductsCollection = productViewModel.GetProduct(id),
                ProductGalleryCollection = productViewModel.GetProductGalleryCollection(id),
                ProductSpecificationCollection = productViewModel.GetProductSpecificationCollection(id),
                ProductFeatureCollection = productViewModel.GetProductFeatureCollection(id),
                ProductReviewsCollection = productViewModel.GetProductReviewCollection(id),
                ProductReviewAverage = review.GetRatingAverage(id),
                ProductReviewCount = review.GetRatingCount(id),
                ReviewUserCollection = user.GetUser(User.Identity.Name),
            };


            ViewBag.SessionString = Session["productQuantity"];

            return View(Products);
        }


        // GET: Store/Product/{id}/Review
        public ActionResult Review(int id)
        {
            UserModel user = new UserModel();


            // build product view model
            var Review = new ReviewViewModel()
            {
                ReviewUserCollection = user.GetUser(User.Identity.Name),
            };

            return View(Review);
        }

        // POST: Store/Product/{id}/Review  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Review(int id, ReviewViewModel model)
        {
            UserModel user = new UserModel();
            Review review = new Review();

            // build product view model
            var Review = new ReviewViewModel()
            {
                ReviewUserCollection = user.GetUser(User.Identity.Name),
            };

            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                    review.AddReview(id, model.Review, model.CustomerName, model.Rating);
                else
                    review.AddReview(id, model.Review, model.Rating, model.CustomerName);


                return RedirectToAction("Product",
                    new { id = id });
            }

            // If we got this far, something failed, redisplay form
            return View(Review);
        }

        // POST: Store/Product/{id}/AddToCart  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int id, ProductViewModel model)
        {


            // build product view model
            var Product = new ProductViewModel()
            {
                Quantity = cart.AddProductPurchaseQuantity(model.Quantity),
            };


            ProductDTO prods = new ProductDTO();
            var prodt = prods.GetProduct(id);
            ProductDTO product = null;
            int productId = 0;

            foreach (var prod in prodt)
            {
                productId = prod.Id;
                product = new ProductDTO(prod.Id, prod.Name, prod.Price, prod.Image, model.Quantity);
            }



            // create cart object if not yet created
            if (Session["cart"] == null)
            {
                Cart cart = new Cart();
                cart.AddProductToCart(product);
                Session["cart"] = cart;
            }
            else
            {
                Cart cart = Session["cart"] as Cart;
                if (cart.ProductExists(productId)) // update product quantity
                {
                    cart.UpdateProductQuantityFromCart(productId, model.Quantity);
                }
                else // add product to cart
                {
                    cart.AddProductToCart(product);
                }
                Session["cart"] = cart;
            }


            TempData["notificationSuccess"] = "Product added to your cart";
            return RedirectToAction("Cart");
            return RedirectToAction("Product",
                    new { id = id });


            // If we got this far, something failed, redisplay form
            return View(Product);
        }

        // GET: Store/Cart
        public ActionResult Cart()
        {
            Cart cart = Session["cart"] as Cart;

            ViewBag.NotificationSuccess = TempData["notificationSuccess"];
            ViewBag.NotificationDanger = TempData["notificationDanger"];

            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CartProductRemove(int id)
        {

            Cart cart = Session["cart"] as Cart;
            cart.RemoveProductFromCart(id);
            Session["cart"] = cart;

            TempData["notificationSuccess"] = "Product Removed";
            return RedirectToAction("Cart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CartProductUpdateQuantity(int id, Cart model)
        {

            Cart cart = Session["cart"] as Cart;
            cart.UpdateProductQuantityFromCart(id, model.Quantity);
            Session["cart"] = cart;

            TempData["notificationSuccess"] = "Product Quantity Updated";
            return RedirectToAction("Cart");
        }

        // GET: Store/Checkout
        public ActionResult Checkout()
        {


            Cart cart = Session["cart"] as Cart;

            UserModel user = new UserModel();
            ProductDTO product = new ProductDTO();

            // build product view model
            var Checkout = new CheckoutViewModel()
            {
                UserCollection = user.GetUser(User.Identity.Name),
            };

            return View(Checkout);

        }

        // POST: Store/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutViewModel model)
        {
            

            if (ModelState.IsValid)
            {
                return View(model);
            }


            // build product view model
            UserModel user = new UserModel();
            var Checkout = new CheckoutViewModel()
            {
                UserCollection = user.GetUser(User.Identity.Name),
            };

            return View(Checkout);

        }

        // POST: Store/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDiscountCode(Cart model)
        {
            // validate discount code

            var discountValid = db.Discounts.Any(x => x.Code == model.DiscountCode);

            if (discountValid)
            {
                // get discount 
                Discount discount = db.Discounts.Where(x => x.Code == model.DiscountCode).First();

                Cart cart = Session["cart"] as Cart;
                cart.UpdateTotalPriceWithDiscount(discount);
                Session["cart"] = cart;

                TempData["notificationSuccess"] = "Discount applied to your cart";
            }
            else
            {
                TempData["NotificationDanger"] = "Discount code invalid";
            }
            
            return RedirectToAction("Cart");
        }

        // POST: Store/Checkout
        public JsonResult CheckStock(string location, int id)
        {
            
            var product = db.Products.Where(x => x.Id == id);
         
            Product prod = null;

            foreach (Product p in product)
            {
                prod = new Product(
                p.Id,
                p.Name,
                p.Price,
                p.Image,
                p.GetProductQuantity(location),
                p.Category_Id
                );
            }

            List<Product> eList = new List<Product>();
            eList.Add(prod);

            string ans = JsonConvert.SerializeObject(eList, Formatting.Indented);
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonObject = serializer.DeserializeObject(ans);

            return Json(jsonObject, JsonRequestBehavior.AllowGet);
        }
            // POST: Store/Checkout
            public JsonResult CalculateShipping(string shippingPostcode)
        {
            // Set your API key: remember to change this to your live API key in production
            string apiKey = "23019725-2d64-4a72-8274-b9b0fce5dd0e";

            // Define the service input parameters
            string fromPostcode = "2000";
            string toPostcode = shippingPostcode;
            int parcelLengthInCMs = 22;
            int parcelWidthInCMs = 16;
            double parcelHeighthInCMs = 7.7;
            double parcelWeightInKGs = 1.5;


            // Set the URL for the Domestic Parcel Calculation service
            string urlPrefix = "digitalapi.auspost.com.au";
            string calculateRateURL = "https://" + urlPrefix + "/postage/parcel/domestic/calculate.json?"
                + "from_postcode=" + fromPostcode
                + "&to_postcode=" + toPostcode
                + "&length=" + parcelLengthInCMs
                + "&width=" + parcelWidthInCMs
                + "&height=" + parcelHeighthInCMs
                + "&weight=" + parcelWeightInKGs
                + "&service_code=" + "AUS_PARCEL_REGULAR";


            Uri objURI = new Uri(calculateRateURL);
            HttpWebRequest objwebreq = (HttpWebRequest)WebRequest.Create(objURI);
            
            objwebreq.Method = "Get";
            objwebreq.Timeout = 15000;


            objwebreq.Headers.Set("AUTH-KEY", apiKey);//For Localhost


            HttpWebResponse objWebResponse = (HttpWebResponse)objwebreq.GetResponse();
            Stream objStream = objWebResponse.GetResponseStream();
            StreamReader objStreamReader = new StreamReader(objStream);

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonObject = serializer.DeserializeObject(objStreamReader.ReadToEnd());

            return Json(jsonObject, JsonRequestBehavior.AllowGet);

        }

        public static List<SelectListItem> GetDropDownListForYears()
        {
            List<SelectListItem> ls = new List<SelectListItem>();

            for (int i = 2014; i <= 2099; i++)
            {
                ls.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return ls;
        }

    }
}


