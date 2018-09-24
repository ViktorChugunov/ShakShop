using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ShakuroMarketplaceNetMVC.Models
{
    public class GoodContext : DbContext
    {
        public GoodContext() : base("DefaultConnection")
        { }
        
        public DbSet<GoodCategory> GoodCategories { get; set; }
        public DbSet<GoodSubcategory> GoodSubcategories { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
    }
    /*
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Good>()
             .HasMany(u => u.Discussions)
             .WithMany(l => l.Goods)
             .Map(ul =>
             {
                 ul.MapLeftKey("GoodId");
                 ul.MapRightKey("DiscussionId");
                 ul.ToTable("DiscussionGoods");
             });
    }
    */
}