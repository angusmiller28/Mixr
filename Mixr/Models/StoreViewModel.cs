
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Web.Mvc;

namespace Mixr.Models
    {
        public class StoreViewModel
        {
            public string Name { get; set; }
            public string Price { get; set; }
            public string Image { get; set; }

        public IEnumerable<Product> GetAllProduct()
        {
            Entities dbContext = new Entities();

            var query = from Name in dbContext.Products
                        select Name;
            IEnumerable<Product> products = query.ToList();

            return products;
        }
    }

    public class ReviewModel
    {
        public string Review1 { get; set; }
        public int Rating { get; set; }
        public string Avatar { get; set; }
        public string UserName { get; set; }
        public string PostDate { get; set; }
    }

    public class ProductViewModel
    {
        Entities dbContext = new Entities();
        public string Name { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
        public string Number { get; set; }

        // shipping
        [Display (Name = "Enter your postcode")]
        public string ShippingPostcode { get; set; }

        // Cart 
        [Required]
        public int Quantity { get; set; }


        // Example collections for each of your types
        public double ProductReviewAverage { get; set; }
        public int ProductReviewCount { get; set; }
        public IEnumerable<ProductGallery> ProductGalleryCollection { get; set; }
        public IEnumerable<ProductSpecification> ProductSpecificationCollection { get; set; }
        public IEnumerable<ProductFeature> ProductFeatureCollection { get; set; }
        public IEnumerable<Product> ProductsCollection { get; set; }
        public IEnumerable<ReviewModel> ProductReviewsCollection { get; set; }

        public IEnumerable<AspNetUser> ReviewUserCollection { get; set; }
        public IEnumerable<Product> GetProduct(int id)
        {
            Entities dbContext = new Entities();
            var query = from Product in dbContext.Products
                        where Product.Id == id
                        select Product;

            IEnumerable<Product> product = query.ToList();

            return product;
        }
        public IEnumerable<ProductGallery> GetProductGalleryCollection(int id)
        {
            Entities dbContext = new Entities();
            var query = from ProductGallery in dbContext.ProductGalleries
                        where ProductGallery.Product_Id == id
                        select ProductGallery;

            IEnumerable<ProductGallery> productGallery = query.ToList();

            return productGallery;
        }
        public IEnumerable<ProductSpecification> GetProductSpecificationCollection(int id)
        {
            Entities dbContext = new Entities();
            var query = from ProductSpecification in dbContext.ProductSpecifications
                        where ProductSpecification.Product_Id == id
                        select ProductSpecification;

            IEnumerable<ProductSpecification> productSpecification = query.ToList();

            return productSpecification;
        }
        public IEnumerable<ProductFeature> GetProductFeatureCollection(int id)
        {
            Entities dbContext = new Entities();
            var query = from ProductFeature in dbContext.ProductFeatures
                        where ProductFeature.Product_Id == id
                        select ProductFeature;

            IEnumerable<ProductFeature> productSpecification = query.ToList();

            return productSpecification;
        }
        public IEnumerable<ReviewModel> GetProductReviewUserCollection(int id)
        {
            Entities dbContext = new Entities();
            // select all review from the customers on a product page who are registered
            var query = from r in dbContext.Reviews
                        from u in dbContext.AspNetUsers
                        where r.Product_Id == id && u.Id == r.User_Id 
                        select new ReviewModel
                        {
                            Review1 = r.Review1,
                            Rating = r.Rating,
                            Avatar = u.Avatar,
                            UserName = u.UserName,
                            PostDate = r.Post_Date
                        };

            IEnumerable<ReviewModel> productReviews = query.ToList();

            return productReviews;
        }
        public IEnumerable<ReviewModel> GetProductReviewCustomerCollection(int id)
        {
            Entities dbContext = new Entities();
            var query = from Review in dbContext.Reviews
                        where Review.Product_Id == id && Review.Customer_Name != null
                        select new ReviewModel
                        {
                            Review1 = Review.Review1,
                            Rating = Review.Rating,
                            UserName = Review.Customer_Name,
                            Avatar = "default.jpg",
                            PostDate = Review.Post_Date
                        };

            IEnumerable<ReviewModel> productReviews = query.ToList();

            return productReviews;
        }

        public IEnumerable<ReviewModel> GetProductReviewCollection(int id)
        {
            // combine user and customer reviews together
            IEnumerable<ReviewModel> first = GetProductReviewUserCollection(id);
            IEnumerable<ReviewModel> second = GetProductReviewCustomerCollection(id);

            IEnumerable<ReviewModel> combined = first.Union(second);

            // order by date earliest 

            return combined;
        }

    }

    public class ReviewViewModel
    {
        [Required (ErrorMessage = "Customer name is required.")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required (ErrorMessage = "Rating is required.")]
        [Display(Name = "Rating")]
        public int Rating { get; set; }

        [Required (ErrorMessage = "A review is required.")]
        [Display(Name = "Review")]
        public string Review { get; set; }


        public string Username { get; set; }
        public string Avatar { get; set; }
        public IEnumerable<AspNetUser> ReviewUserCollection { get; set; }

    }

    public class CartViewModel
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string Image { get; set; }
        public string Quantity { get; set; }
    }

    public class CheckoutViewModel
    {

        // billing customer data
        [Required (ErrorMessage = "The Customer name is required.")]
        [StringLength(30)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required (ErrorMessage = "The Billing Address is required.")]
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }

        [Required (ErrorMessage = "The Postal code is required.")]
        [Display(Name = "Zip or Postal code")]
        public string ZipCode { get; set; }

        [Required (ErrorMessage = "The Phone number is required.")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        // card data
        [Required (ErrorMessage = "The Card number is required.")]
        [CreditCard]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Required (ErrorMessage = "The Card expiration month is required.")]
        [Display(Name = "Card Expiration Month")]
        public int ExpirationDateMonth { get; set; }

        [Required (ErrorMessage = "The Card expiration year is required.")]
        [Display(Name = "Card Expiration Year")]
        public int ExpirationDateYear { get; set; }

        [Required(ErrorMessage = "The Card CVV is required.")]
        [RegularExpression(@"\A\d{3,4}\Z", ErrorMessage = "CVV must be between 3 - 4 digits.")]
        [Display(Name = "Card CVV")]
        public string CardCVV { get; set; }

        // user data
        public string Username { get; set; }
        public string Avatar { get; set; }
        public IEnumerable<AspNetUser> UserCollection { get; set; }
       

        public List<SelectListItem> GetDropDownListForYears()
        {
            List<SelectListItem> ls = new List<SelectListItem>();

            for (int i = DateTime.Now.Year; i <= (DateTime.Now.Year + 50); i++)
            {
                ls.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return ls;
        }

        public List<SelectListItem> GetDropDownListForMonths()
        {
            List<SelectListItem> ls = new List<SelectListItem>();

            for (int i = 1; i <= (12); i++)
            {
                ls.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return ls;
        }

    }
}

        


