//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mixr.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductsGallery
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public int Product_Id { get; set; }
        public string Name { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
