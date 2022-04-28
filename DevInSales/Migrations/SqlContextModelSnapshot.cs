﻿// <auto-generated />
using System;
using DevInSales.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevInSales.Migrations
{
    [DbContext(typeof(SqlContext))]
    partial class SqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DevInSales.Models.Delivery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Address_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Delivery_Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Delivery_Forecast")
                        .HasColumnType("datetime2");

                    b.Property<int>("Order_Id")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Delivery");
                });

            modelBuilder.Entity("DevInSales.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date_Order")
                        .HasColumnType("datetime2")
                        .HasColumnName("date_order");

                    b.Property<int>("Seller_Id")
                        .HasColumnType("int")
                        .HasColumnName("seller_id");

                    b.Property<string>("Shipping_Company")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("shipping_Company");

                    b.Property<float>("Shipping_Company_Price")
                        .HasColumnType("real")
                        .HasColumnName("shipping_company_price");

                    b.Property<int>("User_Id")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("DevInSales.Models.Order_Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("Order_Id")
                        .HasColumnType("int");

                    b.Property<int>("Product_Id")
                        .HasColumnType("int");

                    b.Property<float>("Unit_Price")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Order_Product");
                });

            modelBuilder.Entity("DevInSales.Models.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("Profile");
                });

            modelBuilder.Entity("DevInSales.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("birth_date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password");

                    b.Property<int>("ProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("DevInSales.Models.User", b =>
                {
                    b.HasOne("DevInSales.Models.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");
                });
#pragma warning restore 612, 618
        }
    }
}
