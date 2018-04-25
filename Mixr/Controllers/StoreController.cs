using Mixr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mixr.Controllers
{
    public class StoreController : Controller
    {
        StoreViewModel store = new StoreViewModel();

        Cart cart = new Cart();

        // GET: Store
        public ActionResult Index()
        {
            var Model = store.GetAllProduct();

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


