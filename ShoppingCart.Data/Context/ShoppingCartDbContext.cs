using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.Context
{
    public class ShoppingCartDbContext : DbContext
    {
        public ShoppingCartDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Task> Tasks {get;set;}

        public DbSet<TaskFile> TasksFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            //modelBuilder.Entity<Category>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }


    }
}
