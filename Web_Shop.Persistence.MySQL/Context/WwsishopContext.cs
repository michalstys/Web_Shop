using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WWSI_Shop.Persistence.MySQL.Model;

namespace WWSI_Shop.Persistence.MySQL.Context;

public partial class WwsishopContext : DbContext
{
    public WwsishopContext()
    {
    }

    public WwsishopContext(DbContextOptions<WwsishopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.IdAddress).HasName("PRIMARY");

            entity.ToTable("Address");

            entity.Property(e => e.BuildingNumber).HasMaxLength(45);
            entity.Property(e => e.City).HasMaxLength(45);
            entity.Property(e => e.Country).HasMaxLength(45);
            entity.Property(e => e.Email).HasMaxLength(45);
            entity.Property(e => e.Mobile).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.PostalCode).HasMaxLength(45);
            entity.Property(e => e.Province).HasMaxLength(45);
            entity.Property(e => e.Street).HasMaxLength(45);
            entity.Property(e => e.Surname).HasMaxLength(45);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.IdCart).HasName("PRIMARY");

            entity.ToTable("Cart");

            entity.HasIndex(e => e.IdCustomer, "fk_Cart_Customer1_idx");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.IdCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Cart_Customer1");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.IdCartItem).HasName("PRIMARY");

            entity.ToTable("CartItem");

            entity.HasIndex(e => e.IdCart, "fk_CartItem_Cart1_idx");

            entity.HasIndex(e => e.IdProduct, "fk_CartItem_Product1_idx");

            entity.HasOne(d => d.IdCartNavigation).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.IdCart)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_CartItem_Cart1");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_CartItem_Product1");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PRIMARY");

            entity.ToTable("Category");

            entity.HasIndex(e => e.ParentIdCategory, "fk_Category_Category1_idx");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(45);

            entity.HasOne(d => d.ParentIdCategoryNavigation).WithMany(p => p.InverseParentIdCategoryNavigation)
                .HasForeignKey(d => d.ParentIdCategory)
                .HasConstraintName("fk_Category_Category1");

            entity.HasMany(d => d.IdProducts).WithMany(p => p.IdCategories)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategory",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("IdProduct")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_ProductCategory_Product1"),
                    l => l.HasOne<Category>().WithMany()
                        .HasForeignKey("IdCategory")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_ProductCategory_Category1"),
                    j =>
                    {
                        j.HasKey("IdCategory", "IdProduct")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("ProductCategory");
                        j.HasIndex(new[] { "IdCategory" }, "fk_ProductCategory_Category1_idx");
                        j.HasIndex(new[] { "IdProduct" }, "fk_ProductCategory_Product1");
                    });
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.IdCustomer).HasName("PRIMARY");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Email, "Email_UNIQUE").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.PasswordHash).HasMaxLength(128);
            entity.Property(e => e.Surname).HasMaxLength(45);

            entity.HasMany(d => d.IdAddresses).WithMany(p => p.IdCustomers)
                .UsingEntity<Dictionary<string, object>>(
                    "CustomerAddress",
                    r => r.HasOne<Address>().WithMany()
                        .HasForeignKey("IdAddress")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_CustomerAddress_Address1"),
                    l => l.HasOne<Customer>().WithMany()
                        .HasForeignKey("IdCustomer")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_CustomerAddress_Customer1"),
                    j =>
                    {
                        j.HasKey("IdCustomer", "IdAddress")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("CustomerAddress");
                        j.HasIndex(new[] { "IdAddress" }, "fk_CustomerAddress_Address1_idx");
                    });
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.IdInvoice).HasName("PRIMARY");

            entity.ToTable("Invoice");

            entity.HasIndex(e => e.IdCustomer, "fk_Invoice_Customer1_idx");

            entity.Property(e => e.Discount).HasPrecision(10, 2);
            entity.Property(e => e.Number).HasMaxLength(100);
            entity.Property(e => e.Price).HasPrecision(10, 2);

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.IdCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Invoice_Customer1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PRIMARY");

            entity.ToTable("Order");

            entity.HasIndex(e => e.IdShippingAddress, "fk_Order_Address1_idx");

            entity.HasIndex(e => e.IdCustomer, "fk_Order_Customer_idx");

            entity.Property(e => e.OrderNumber).HasMaxLength(100);

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Order_Customer");

            entity.HasOne(d => d.IdShippingAddressNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdShippingAddress)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Order_Address1");

            entity.HasMany(d => d.IdInvoices).WithMany(p => p.IdOrders)
                .UsingEntity<Dictionary<string, object>>(
                    "InvoiceItem",
                    r => r.HasOne<Invoice>().WithMany()
                        .HasForeignKey("IdInvoice")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_InvoiceItem_Invoice1"),
                    l => l.HasOne<Order>().WithMany()
                        .HasForeignKey("IdOrder")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_InvoiceItem_Order1"),
                    j =>
                    {
                        j.HasKey("IdOrder", "IdInvoice")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("InvoiceItem");
                        j.HasIndex(new[] { "IdInvoice" }, "fk_InvoiceItem_Invoice1_idx");
                    });
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.IdOrderItem).HasName("PRIMARY");

            entity.ToTable("OrderItem");

            entity.HasIndex(e => e.IdOrder, "fk_OrderItem_Order1_idx");

            entity.HasIndex(e => e.IdProduct, "fk_OrderItem_Product1_idx");

            entity.Property(e => e.Discount).HasPrecision(10, 2);
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .HasColumnName("SKU");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_OrderItem_Order1");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_OrderItem_Product1");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PRIMARY");

            entity.ToTable("Product");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(128);
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .HasColumnName("SKU");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.IdProductReview).HasName("PRIMARY");

            entity.ToTable("ProductReview");

            entity.HasIndex(e => e.IdProduct, "fk_ProductReview_Product1_idx");

            entity.Property(e => e.IdProductReview).ValueGeneratedNever();
            entity.Property(e => e.Description).HasColumnType("text");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ProductReview_Product1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
