using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mixr.Models
{
    public partial class Category
    {
        public IEnumerable<Category> GetCategories()
        {
            Entities dbContext = new Entities();
            // select all review from the customers on a product page who are registered
            return (from p in dbContext.Categories
                    select new Category
                    {
                        Name = p.Name,

                    }).ToList();
        }
    }
}