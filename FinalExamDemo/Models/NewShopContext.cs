using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FinalExamDemo.Models;

public partial class NewShopContext : DbContext
{
    public NewShopContext()
    {
    }

    public NewShopContext(DbContextOptions<NewShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<NavItem> NavItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Categories");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.NameVn)
                .HasMaxLength(50)
                .HasColumnName("NameVN");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.Property(e => e.ComContent).HasMaxLength(200);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasMaxLength(10);

            entity.HasOne(d => d.Product).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_Products");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Customers");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Fullname).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.RePassword).HasMaxLength(50);
        });

        modelBuilder.Entity<NavItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.NavItems");

            entity.Property(e => e.NavName).HasMaxLength(50);
            entity.Property(e => e.NavNameVn)
                .HasMaxLength(50)
                .HasColumnName("NavNameVN");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Orders");

            entity.Property(e => e.OrderDate).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.Orders_dbo.Customers_CustomerId");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.OrderDetails");

            entity.Property(e => e.ProductId).HasMaxLength(10);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_dbo.OrderDetails_dbo.Orders_OrderId");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_dbo.OrderDetails_dbo.Products_ProductId");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Products");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_dbo.Products_dbo.Categories_CategoryId");

            entity.HasOne(d => d.Provider).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Providers");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.ProviderName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
