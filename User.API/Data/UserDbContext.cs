using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using User.API.Models;

namespace User.API.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //设置主键
            modelBuilder.Entity<AppUser>()
                .HasKey(x => x.Id);
            //双主键
            modelBuilder.Entity<AppUserProperty>()
                .HasKey(x => new { x.AppUserId, x.Key, x.Value });
            //指定长度
            modelBuilder.Entity<AppUserProperty>().Property(x => x.Key).HasMaxLength(100);
            modelBuilder.Entity<AppUserProperty>().Property(x => x.Value).HasMaxLength(100);

            modelBuilder.Entity<AppUserTag>()
                .HasKey(x => new { x.AppUserId, x.Tag });

            modelBuilder.Entity<AppUserTag>().Property(x => x.Tag).HasMaxLength(100);
            modelBuilder.Entity<AppUserTag>().Property(x => x.CreatedTime).ValueGeneratedOnAdd();
            //设置主键
            modelBuilder.Entity<BPFile>()
                .HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppUserProperty> AppUserProperties { get; set; }
        public DbSet<BPFile> BpFiles { get; set; }
        public DbSet<AppUserTag> AppUserTags { get; set; }
    }
}
