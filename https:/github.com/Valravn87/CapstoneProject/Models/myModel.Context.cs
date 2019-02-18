﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CapstoneDBEntities : DbContext
    {
        public CapstoneDBEntities()
            : base("name=CapstoneDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<RestaurantGroup> RestaurantGroups { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<ItemAlert> ItemAlerts { get; set; }
        public virtual DbSet<RestaurantItem> RestaurantItems { get; set; }
        public virtual DbSet<ScheduleDay> ScheduleDays { get; set; }
        public virtual DbSet<Alert> Alerts { get; set; }
    }
}