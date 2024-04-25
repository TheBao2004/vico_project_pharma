using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pharma.Models;

namespace Pharma.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<BrandModel> BrandModel { set; get; }
        public DbSet<CategoryModel> CategoryModel { set; get; }
        public DbSet<ProductModel> ProductModel { set; get; }
        public DbSet<AddressModel> AddressModel { set; get; }
        public DbSet<CommentModel> CommentModel { set; get; }
        public DbSet<UserModel> UserModel { set; get; }
        public DbSet<VoucherModel> VoucherModel { set; get; }
        public DbSet<DetailOrderModel> DetailOrderModel { set; get; }
        public DbSet<OrderModel> OrderModel { set; get; }
        public DbSet<CartModel> CartModel { set; get; }
        public DbSet<CityModel> CityModel { set; get; }
        public DbSet<DistrictModel> DistrictModel { set; get; }
        public DbSet<WardModel> WardModel { set; get; }
        public DbSet<KeywordModel> KeywordModel { set; get; }
	}
}
