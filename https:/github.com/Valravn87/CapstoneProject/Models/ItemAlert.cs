//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CapstoneProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ItemAlert
    {
        public int Id { get; set; }
        public int AlertId { get; set; }
        public int RestaurantItemId { get; set; }
        public int StockCode { get; set; }
    
        public virtual RestaurantItem RestaurantItem { get; set; }
        public virtual Stock Stock { get; set; }
        public virtual Alert Alert { get; set; }
    }
}
