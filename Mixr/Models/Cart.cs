
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

            [Display(Name = "Discount code")]
            public string DiscountCode { get; set; }
            public List<ProductDTO> CartCollection = new List<ProductDTO>();
        

            public void AddProductToCart(ProductDTO product)
            {
                CartCollection.Add(product);
            }

            public void RemoveProductFromCart(int id)
            {
                CartCollection.RemoveAll(r => r.Id == id);

            }

            public void UpdateProductQuantityFromCart(int id, int quantity)
            {
                var obj = CartCollection.FirstOrDefault(x => x.Id == id);
                if (obj != null) obj.Quantity = quantity;
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

                decimal result = CartCollection.Sum(x => x.Price * x.Quantity);
                return result;
            }

            public void UpdateTotalPrice()
            {
                Entities dbContext = new Entities();

                TotalPrice = CartCollection.Sum(x => x.Price * x.Quantity);

        }

    }

 
    }

        


