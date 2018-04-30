using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mixr.Models
{
    public partial class Product
    {
        public int Quantity { get; set; }

        public Product(int id, string name, decimal price, string image, int quantity)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.Image = image;
            this.Quantity = quantity;
        }
        public int GetRatingCount(int id)
        {
            Entities db = new Entities();

            int ratingCount = db.Reviews
                                .Where(r => r.Product_Id == id)
                                .Count();

            return ratingCount;
        }

        public double GetRatingAverage(int id)
        {
            Entities db = new Entities();

            double ratingAverage = 0;

            if (GetRatingCount(id) != 0)
            {
                ratingAverage = db.Reviews
                .Where(r => r.Product_Id == id)
                .Average(r => r.Rating);
            }

            return Math.Round(ratingAverage, 2);
        }

    }

    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }

        

        public ProductDTO(int id, string name, decimal price, string image, int quantity)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.Image = image;
            this.Quantity = quantity;
        }

        public ProductDTO()
        {
        }

        public int GetRatingCount(int id)
        {
            Entities db = new Entities();

            int ratingCount = db.Reviews
                                .Where(r => r.Product_Id == id)
                                .Count();

            return ratingCount;
        }

        public double GetRatingAverage(int id)
        {
            Entities db = new Entities();

            double ratingAverage = 0;

            if (GetRatingCount(id) != 0)
            {
                ratingAverage = db.Reviews
                .Where(r => r.Product_Id == id)
                .Average(r => r.Rating);
            }

            return Math.Round(ratingAverage, 2);
        }

        public IEnumerable<ProductDTO> GetProducts()
        {
            Entities dbContext = new Entities();
            // select all review from the customers on a product page who are registered
            return (from p in dbContext.Products
                    select new ProductDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Image = p.Image,

                    }).ToList();
        }

        public IEnumerable<ProductDTO> GetProduct(int id)
        {
            Entities dbContext = new Entities();
            // select all review from the customers on a product page who are registered
            return (from p in dbContext.Products
                    where p.Id == id
                    select new ProductDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Image = p.Image,

                    }).ToList();
        }

        public IEnumerable<ProductDTO> GetProductByName(string value)
        {
            Entities dbContext = new Entities();
            // select all review from the customers on a product page who are registered
            return (from p in dbContext.Products
                    where p.Name.Contains(value)
                    select new ProductDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Image = p.Image,

                    }).ToList();
        }
    }
}