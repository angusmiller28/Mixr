
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    namespace Mixr.Models
    {
        public partial class Cart
        {
            public decimal TotalPrice { get; set; }
            public int Quantity { get; set; }
            public Discount Discount { get; set; }

            [Display(Name = "Discount code")]
            public string DiscountCode { get; set; }
            public List<ProductDTO> CartCollection = new List<ProductDTO>();
            


        public void AddProductToCart(ProductDTO product)
            {
                CartCollection.Add(product);
                UpdateTotalPrice();
            }

            public void RemoveProductFromCart(int id)
            {
                CartCollection.RemoveAll(r => r.Id == id);
                UpdateTotalPrice();

            }

            public void UpdateProductQuantityFromCart(int id, int quantity)
            {
                var obj = CartCollection.FirstOrDefault(x => x.Id == id);
                if (obj != null) obj.Quantity = quantity;

                if (Discount == null)
                {
                    UpdateTotalPrice();
                }   
                else
                {
                    UpdateTotalPrice();
                    UpdateTotalPriceWithDiscount(Convert.ToDecimal(Discount.Discount1) * TotalPrice);
                }
                    
            }

            public IEnumerable<Product> GetAllProduct()
            {
                Entities dbContext = new Entities();
                var query = from Name in dbContext.Products
                            select Name;
                IEnumerable<Product> products = query.ToList();

                return products;
            }

            public IEnumerable<Product> GetProduct(int id)
            {
                Entities dbContext = new Entities();
                var query = from Product in dbContext.Products
                            where Product.Id == id
                            select Product;

                IEnumerable<Product> product = query.ToList();

                return product;
            }

            public bool ProductExists(int id)
            {
                return CartCollection.Exists(f => f.Id == id);
            }

            public decimal GetTotalPrice()
            {
                Entities dbContext = new Entities();

                decimal result = TotalPrice;
                return result;
            }

            public void UpdateTotalPrice()
            {
                Entities dbContext = new Entities();

            // check if discount is already applied
            if (Discount == null)
            {
                TotalPrice = Math.Round(CartCollection.Sum(x => x.Price * x.Quantity), 2);
            }
            else
            {
                UpdateTotalPriceWithDiscount(Convert.ToDecimal(Discount.Discount1) * TotalPrice);
            }

            }

            public void UpdateTotalPriceWithDiscount(Discount discount)
            {
                Entities dbContext = new Entities();

                
                TotalPrice = Math.Round(CartCollection.Sum(x => x.Price * x.Quantity) - (Convert.ToDecimal(discount.Discount1) * CartCollection.Sum(x => x.Price * x.Quantity)), 2);
                Discount = discount;
            }

            public void UpdateTotalPriceWithDiscount(decimal price)
            {
                Entities dbContext = new Entities();

                TotalPrice = Math.Round(CartCollection.Sum(x => x.Price * x.Quantity) - (Convert.ToDecimal(Discount.Discount1) * CartCollection.Sum(x => x.Price * x.Quantity)), 2);
            }



    }

 
    }

        


