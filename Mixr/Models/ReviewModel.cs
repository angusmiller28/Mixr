using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mixr.Models
{
    public partial class Review
    {
        public void AddReview(int productId, string reviewText, int rating, string CustomerName)
        {
            Entities db = new Entities();
            Review review = new Review
            {
                Review1 = reviewText,
                Rating = rating,
                Product_Id = productId,
                Customer_Name = CustomerName,
                Post_Date = DateTime.Now.ToString("dd/MM/yyyy")
            };

            db.Reviews.Add(review);
            db.SaveChanges();
        }

        public void AddReview(int productId, string reviewText, string userId, int rating)
        {
            Entities db = new Entities();
            Review review = new Review
            {
                Review1 = reviewText,
                Rating = rating,
                Product_Id = productId,
                User_Id = userId,
                Post_Date = DateTime.Now.ToString("dd/MM/yyyy")
            };

            db.Reviews.Add(review);
            db.SaveChanges();
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
}

