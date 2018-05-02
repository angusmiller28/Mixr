using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mixr.Models
{
    public class SearchViewModel
    {
        public IEnumerable<ProductDTO> ProductsCollection { get; set; }
        public IEnumerable<Category> CategoriesCollection { get; set; }
    }
}