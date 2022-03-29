﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Northwind.Services.EntityFrameworkCore.Entities;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable
namespace Northwind.Services.EntityFrameworkCore.Context
{
    public partial class NorthwindContext : DbContext
    {
        public NorthwindContext()
        {
        }

        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AlphabeticalListOfProducts> AlphabeticalListOfProducts { get; set; }
        public virtual DbSet<CategoryEntity> Categories { get; set; }
        public virtual DbSet<CategorySaleFor1997> CategorySalesFor1997 { get; set; }
        public virtual DbSet<CurrentProductList> CurrentProductList { get; set; }
        public virtual DbSet<CustomerAndSupplierByCity> CustomerAndSuppliersByCity { get; set; }
        public virtual DbSet<CustomerCustomerDemo> CustomerCustomerDemo { get; set; }
        public virtual DbSet<CustomerDemographics> CustomerDemographics { get; set; }
        public virtual DbSet<CustomerEntity> Customers { get; set; }
        public virtual DbSet<EmployeeTerritory> EmployeeTerritories { get; set; }
        public virtual DbSet<EmployeeEntity> Employees { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderDetailExtended> OrderDetailsExtended { get; set; }
        public virtual DbSet<OrderSubtotal> OrderSubtotals { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderQry> OrdersQry { get; set; }
        public virtual DbSet<ProductSaleFor1997> ProductSalesFor1997 { get; set; }
        public virtual DbSet<ProductEntity> Products { get; set; }
        public virtual DbSet<ProductAboveAveragePrice> ProductsAboveAveragePrice { get; set; }
        public virtual DbSet<ProductByCategory> ProductsByCategory { get; set; }
        public virtual DbSet<QuarterlyOrder> QuarterlyOrders { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<SaleByCategory> SalesByCategory { get; set; }
        public virtual DbSet<SaleTotalByAmount> SalesTotalsByAmount { get; set; }
        public virtual DbSet<Shipper> Shippers { get; set; }
        public virtual DbSet<SummaryOfSaleByQuarter> SummaryOfSalesByQuarter { get; set; }
        public virtual DbSet<SummaryOfSaleByYear> SummaryOfSalesByYear { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Territory> Territories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlphabeticalListOfProducts>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Alphabetical list of products");
            });

            modelBuilder.Entity<CategoryEntity>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("CategoryName");
            });

            modelBuilder.Entity<CategorySaleFor1997>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Category Sales for 1997");
            });

            modelBuilder.Entity<CurrentProductList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Current Product List");

                entity.Property(e => e.ProductId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<CustomerAndSupplierByCity>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Customer and Suppliers by City");

                entity.Property(e => e.Relationship).IsUnicode(false);
            });

            modelBuilder.Entity<CustomerCustomerDemo>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.CustomerTypeId })
                    .IsClustered(false);

                entity.Property(e => e.CustomerId).IsFixedLength();

                entity.Property(e => e.CustomerTypeId).IsFixedLength();

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerCustomerDemo)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerCustomerDemo_Customers");

                entity.HasOne(d => d.CustomerType)
                    .WithMany(p => p.CustomerCustomerDemo)
                    .HasForeignKey(d => d.CustomerTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerCustomerDemo");
            });

            modelBuilder.Entity<CustomerDemographics>(entity =>
            {
                entity.HasKey(e => e.CustomerTypeId)
                    .IsClustered(false);

                entity.Property(e => e.CustomerTypeId).IsFixedLength();
            });

            modelBuilder.Entity<CustomerEntity>(entity =>
            {
                entity.HasIndex(e => e.City)
                    .HasName("City");

                entity.HasIndex(e => e.CompanyName)
                    .HasName("CompanyName");

                entity.HasIndex(e => e.PostalCode)
                    .HasName("PostalCode");

                entity.HasIndex(e => e.Region)
                    .HasName("Region");

                entity.Property(e => e.Id).IsFixedLength();
            });

            modelBuilder.Entity<EmployeeTerritory>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.TerritoryId })
                    .IsClustered(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeTerritories)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeTerritories_Employees");

                entity.HasOne(d => d.Territory)
                    .WithMany(p => p.EmployeeTerritories)
                    .HasForeignKey(d => d.TerritoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeTerritories_Territories");
            });

            modelBuilder.Entity<EmployeeEntity>(entity =>
            {
                entity.HasIndex(e => e.LastName)
                    .HasName("LastName");

                entity.HasIndex(e => e.PostalCode)
                    .HasName("PostalCode");

                entity.HasOne(d => d.ReportsToNavigation)
                    .WithMany(p => p.InverseReportsToNavigation)
                    .HasForeignKey(d => d.ReportsTo)
                    .HasConstraintName("FK_Employees_Employees");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Invoices");

                entity.Property(e => e.CustomerId).IsFixedLength();
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PK_Order_Details");

                entity.HasIndex(e => e.OrderId)
                    .HasName("OrdersOrder_Details");

                entity.HasIndex(e => e.ProductId)
                    .HasName("ProductsOrder_Details");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Details_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Details_Products");
            });

            modelBuilder.Entity<OrderDetailExtended>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Order Details Extended");
            });

            modelBuilder.Entity<OrderSubtotal>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Order Subtotals");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => e.CustomerId)
                    .HasName("CustomersOrders");

                entity.HasIndex(e => e.EmployeeId)
                    .HasName("EmployeesOrders");

                entity.HasIndex(e => e.OrderDate)
                    .HasName("OrderDate");

                entity.HasIndex(e => e.ShipPostalCode)
                    .HasName("ShipPostalCode");

                entity.HasIndex(e => e.ShipVia)
                    .HasName("ShippersOrders");

                entity.HasIndex(e => e.ShippedDate)
                    .HasName("ShippedDate");

                entity.Property(e => e.CustomerId).IsFixedLength();

                entity.Property(e => e.Freight).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Orders_Customers");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Orders_Employees");

                entity.HasOne(d => d.ShipViaNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShipVia)
                    .HasConstraintName("FK_Orders_Shippers");
            });

            modelBuilder.Entity<OrderQry>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Orders Qry");

                entity.Property(e => e.CustomerId).IsFixedLength();
            });

            modelBuilder.Entity<ProductSaleFor1997>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Product Sales for 1997");
            });

            modelBuilder.Entity<ProductEntity>(entity =>
            {
                entity.HasIndex(e => e.CategoryId)
                    .HasName("CategoryID");

                entity.HasIndex(e => e.Name)
                    .HasName("ProductName");

                entity.HasIndex(e => e.SupplierId)
                    .HasName("SuppliersProducts");

                entity.Property(e => e.ReorderLevel).HasDefaultValueSql("((0))");

                entity.Property(e => e.UnitPrice).HasDefaultValueSql("((0))");

                entity.Property(e => e.UnitsInStock).HasDefaultValueSql("((0))");

                entity.Property(e => e.UnitsOnOrder).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Products_Categories");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_Products_Suppliers");
            });

            modelBuilder.Entity<ProductAboveAveragePrice>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Products Above Average Price");
            });

            modelBuilder.Entity<ProductByCategory>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Products by Category");
            });

            modelBuilder.Entity<QuarterlyOrder>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Quarterly Orders");

                entity.Property(e => e.CustomerId).IsFixedLength();
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.HasKey(e => e.RegionId)
                    .IsClustered(false);

                entity.Property(e => e.RegionId).ValueGeneratedNever();

                entity.Property(e => e.RegionDescription).IsFixedLength();
            });

            modelBuilder.Entity<SaleByCategory>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Sales by Category");
            });

            modelBuilder.Entity<SaleTotalByAmount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Sales Totals by Amount");
            });

            modelBuilder.Entity<SummaryOfSaleByQuarter>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Summary of Sales by Quarter");
            });

            modelBuilder.Entity<SummaryOfSaleByYear>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Summary of Sales by Year");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasIndex(e => e.CompanyName)
                    .HasName("CompanyName");

                entity.HasIndex(e => e.PostalCode)
                    .HasName("PostalCode");
            });

            modelBuilder.Entity<Territory>(entity =>
            {
                entity.HasKey(e => e.TerritoryId)
                    .IsClustered(false);

                entity.Property(e => e.TerritoryDescription).IsFixedLength();

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Territories)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Territories_Region");
            });

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
