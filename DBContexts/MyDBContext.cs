using ezStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ezStore.DBContexts
{
public class MyDBContext : DbContext  
    {  
        public DbSet<Category> Categories { get; set; }  
        public DbSet<Product> Products { get; set; }  
  
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)  
        {   
        }  
  
        protected override void OnModelCreating(ModelBuilder modelBuilder)  
        {  
            // Use Fluent API to configure  
  
            // Map entities to tables  
            modelBuilder.Entity<Category>().ToTable("Categories");  
            modelBuilder.Entity<Product>().ToTable("Products");  
  
            // Configure Primary Keys  
            modelBuilder.Entity<Category>().HasKey(ug => ug.Id).HasName("PK_Categories");  
            modelBuilder.Entity<Product>().HasKey(u => u.Id).HasName("PK_Products");  
  
            // Configure indexes  
            modelBuilder.Entity<Category>().HasIndex(p => p.Name).IsUnique().HasDatabaseName("Idx_Name");  
            modelBuilder.Entity<Product>().HasIndex(u => u.Name).HasDatabaseName("Idx_Name"); 
  
            // Configure columns  
            modelBuilder.Entity<Category>().Property(ug => ug.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Category>().Property(ug => ug.Name).HasColumnType("nvarchar(100)").IsRequired();  
            modelBuilder.Entity<Category>().Property(ug => ug.Description).HasColumnType("nvarchar(250)").IsRequired();
            modelBuilder.Entity<Category>().Property(ug => ug.is_active).HasColumnType("bit").IsRequired();



            modelBuilder.Entity<Product>().Property(u => u.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            modelBuilder.Entity<Product>().Property(u => u.Name).HasColumnType("nvarchar(50)").IsRequired();  
            modelBuilder.Entity<Product>().Property(u => u.CategoryId).HasColumnType("int").IsRequired();  
            modelBuilder.Entity<Product>().Property(u => u.Cost).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Product>().Property(u => u.Description).HasColumnType("nvarchar(250)").IsRequired();
            modelBuilder.Entity<Product>().Property(u => u.is_active).HasColumnType("bit").IsRequired();

            // Configure relationships  
            modelBuilder.Entity<Product>().HasOne<Category>().WithMany().HasPrincipalKey(ug => ug.Id).HasForeignKey(u => u.CategoryId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("FK_Product_Categories");  
        }  
    }  
}
