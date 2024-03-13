﻿// <auto-generated />
using System;
using CarRentingSystem.Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarRentingSystem.Infrastucture.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CarRentingSystem.Infrastucture.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "dea1286-c198-4129-b3f3-b89d839581",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "92d895d2-3e9c-40d6-80e6-508a258e8563",
                            Email = "agent@mail.com",
                            EmailConfirmed = false,
                            IsActive = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "agent@mail.com",
                            NormalizedUserName = "agent@mail.com",
                            PasswordHash = "AQAAAAEAACcQAAAAEANSdAAEBFpzVCh+SE/OkAtaWys+l7afcPxH9qs8gOnQQqGIb9Nr1oMX/goMfhOgnQ==",
                            PhoneNumberConfirmed = false,
                            TwoFactorEnabled = false,
                            UserName = "agent@mail.com"
                        },
                        new
                        {
                            Id = "6d5800-d726-4fc8-83d9-d6b3ac1f582e",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "591b74f9-7c26-4110-99b0-08265ea55e77",
                            Email = "guest@mail.com",
                            EmailConfirmed = false,
                            IsActive = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "guest@mail.com",
                            NormalizedUserName = "guest@mail.com",
                            PasswordHash = "AQAAAAEAACcQAAAAEItCQPkWw1hou4xrAPhZ+EMUZMikh0eR2AcbDZA4iksuJOiwmI+RjvUk6LFcxepEYQ==",
                            PhoneNumberConfirmed = false,
                            TwoFactorEnabled = false,
                            UserName = "guest@mail.com"
                        });
                });

            modelBuilder.Entity("CarRentingSystem.Infrastucture.Data.CarRoute", b =>
                {
                    b.Property<int>("CarRouteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CarRouteId"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("DeliveryAddress")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("DriverCarId")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrlRouteGoogleMaps")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("PickUpAddress")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("money");

                    b.Property<string>("RenterId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CarRouteId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("DriverCarId");

                    b.HasIndex("RenterId");

                    b.ToTable("CarRoutes");

                    b.HasData(
                        new
                        {
                            CarRouteId = 11,
                            CategoryId = 5,
                            DeliveryAddress = "Bulgaria, Sofia, Bul, Alexander Malinov, 78",
                            Description = "Whether you want a tourist tour from Plovdiv to Sofia, or simply busyness trip, this trip is private with a luxury limousine",
                            DriverCarId = 1,
                            ImageUrlRouteGoogleMaps = "https://le-cdn.hibuwebsites.com/8978d127e39b497da77df2a4b91f33eb/dms3rep/multi/opt/RSshutterstock_120889072-1920w.jpg",
                            IsActive = true,
                            PickUpAddress = "Bulgaria, Plovdiv, Bul.Kniyaginya Maria Luiza, 31",
                            Price = 316.80m,
                            Title = "Private Luxury"
                        },
                        new
                        {
                            CarRouteId = 22,
                            CategoryId = 2,
                            DeliveryAddress = "Bulgaria, Plovdiv, Bul.Kniyaginya Maria Luiza, 31",
                            Description = "Whether you want a tourist tour from Sofia to Plovdiv, or simply busyness trip, this trip is private with a luxury limousine",
                            DriverCarId = 1,
                            ImageUrlRouteGoogleMaps = "https://content.fortune.com/wp-content/uploads/2014/09/170030873.jpg?resize=1200,600",
                            IsActive = true,
                            PickUpAddress = "Bulgaria, Sofia, Bul, Alexander malinov, 78",
                            Price = 316.80m,
                            Title = "Shared"
                        },
                        new
                        {
                            CarRouteId = 33,
                            CategoryId = 2,
                            DeliveryAddress = "Bulgaria, Plovdiv, Bul.Kniyaginya Maria Luiza, 31",
                            Description = "Whether you want a tourist tour from Sofia to Plovdiv, or simply busyness trip, this trip is private with a luxury limousine",
                            DriverCarId = 1,
                            ImageUrlRouteGoogleMaps = "https://bulgaria-infoguide.com/wp-content/uploads/2018/10/green-taxi-1024x768.jpg",
                            IsActive = true,
                            PickUpAddress = "Bulgaria, Sofia, Bul, Alexander malinov, 78",
                            Price = 158.40m,
                            Title = "Shared with One"
                        },
                        new
                        {
                            CarRouteId = 44,
                            CategoryId = 3,
                            DeliveryAddress = "Antique Theater, str. Tsar Ivaylo 4, Plovdiv, Bulgaria",
                            Description = "Whether you want a tourist tour in Plovdiv, or simply busyness trip, this trip is will satisy your expectation with a luxury limousine",
                            DriverCarId = 1,
                            ImageUrlRouteGoogleMaps = "https://ekotaxi.bg/wp-content/uploads/2020/03/single_cab_redone-min-1-2048x1536.png",
                            IsActive = true,
                            PickUpAddress = "Bulgaria, Plovdiv, Bul.Kniyaginya Maria Luiza, 31",
                            Price = 6.20m,
                            Title = "OneWayLocal"
                        },
                        new
                        {
                            CarRouteId = 55,
                            CategoryId = 4,
                            DeliveryAddress = "Antique Theater, str. Tsar Ivaylo 4, Plovdiv, Bulgaria",
                            Description = "Whether you want a tourist tour in Plovdiv, or simply busyness trip, this trip is will satisfy your expectation with a luxury limousine",
                            DriverCarId = 1,
                            ImageUrlRouteGoogleMaps = "https://content.fortune.com/wp-content/uploads/2014/09/170030873.jpg?resize=1200,600",
                            IsActive = true,
                            PickUpAddress = "Bulgaria, Plovdiv, Bul.Kniyaginya Maria Luiza, 31",
                            Price = 10.20m,
                            Title = "RoundTrip Local"
                        },
                        new
                        {
                            CarRouteId = 66,
                            CategoryId = 6,
                            DeliveryAddress = "Hartmann Road, London E16 2PX",
                            Description = "Whether you want a tourist tour in Plovdiv, or simply busyness trip, this trip is will satisfy your expectation with a luxury limousine",
                            DriverCarId = 1,
                            ImageUrlRouteGoogleMaps = "https://th.bing.com/th/id/R.4f634d4c26e3f1a1cda6459f649713d1?rik=GYIFZQe3lUWPJA&pid=ImgRaw&r=0",
                            IsActive = true,
                            PickUpAddress = "Krumovo 4009, Rodopi Municipality, Plovdiv District",
                            Price = 10.20m,
                            Title = "Charter"
                        });
                });

            modelBuilder.Entity("CarRentingSystem.Infrastucture.Data.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            Name = "InterCity Single"
                        },
                        new
                        {
                            CategoryId = 2,
                            Name = "InterCity Shared"
                        },
                        new
                        {
                            CategoryId = 3,
                            Name = "OneWay Local"
                        },
                        new
                        {
                            CategoryId = 4,
                            Name = "RoundTripLocal"
                        },
                        new
                        {
                            CategoryId = 5,
                            Name = "Luxury"
                        },
                        new
                        {
                            CategoryId = 6,
                            Name = "Charter"
                        });
                });

            modelBuilder.Entity("CarRentingSystem.Infrastucture.Data.DriverCar", b =>
                {
                    b.Property<int>("DriverCarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DriverCarId"), 1L, 1);

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DriverCarId");

                    b.HasIndex("UserId");

                    b.ToTable("DriversCars");

                    b.HasData(
                        new
                        {
                            DriverCarId = 1,
                            PhoneNumber = "00359123456",
                            UserId = "dea1286-c198-4129-b3f3-b89d839581"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CarRentingSystem.Infrastucture.Data.CarRoute", b =>
                {
                    b.HasOne("CarRentingSystem.Infrastucture.Data.Category", "Category")
                        .WithMany("CarRoutes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarRentingSystem.Infrastucture.Data.DriverCar", "DriverCar")
                        .WithMany()
                        .HasForeignKey("DriverCarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarRentingSystem.Infrastucture.Data.ApplicationUser", "Renter")
                        .WithMany()
                        .HasForeignKey("RenterId");

                    b.Navigation("Category");

                    b.Navigation("DriverCar");

                    b.Navigation("Renter");
                });

            modelBuilder.Entity("CarRentingSystem.Infrastucture.Data.DriverCar", b =>
                {
                    b.HasOne("CarRentingSystem.Infrastucture.Data.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CarRentingSystem.Infrastucture.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CarRentingSystem.Infrastucture.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarRentingSystem.Infrastucture.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CarRentingSystem.Infrastucture.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CarRentingSystem.Infrastucture.Data.Category", b =>
                {
                    b.Navigation("CarRoutes");
                });
#pragma warning restore 612, 618
        }
    }
}
