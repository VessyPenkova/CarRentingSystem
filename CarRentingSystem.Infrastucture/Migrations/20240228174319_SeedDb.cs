using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentingSystem.Infrastucture.Migrations
{
    public partial class SeedDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "DriversCars",
                columns: table => new
                {
                    DriverCarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriversCars", x => x.DriverCarId);
                    table.ForeignKey(
                        name: "FK_DriversCars_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarRoutes",
                columns: table => new
                {
                    CarRouteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PickUpAddress = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageUrlRouteGoogleMaps = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "money", precision: 18, scale: 2, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    DriverCarId = table.Column<int>(type: "int", nullable: false),
                    RenterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarRoutes", x => x.CarRouteId);
                    table.ForeignKey(
                        name: "FK_CarRoutes_ApplicationUser_RenterId",
                        column: x => x.RenterId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CarRoutes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarRoutes_DriversCars_DriverCarId",
                        column: x => x.DriverCarId,
                        principalTable: "DriversCars",
                        principalColumn: "DriverCarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApplicationUser",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "6d5800-d726-4fc8-83d9-d6b3ac1f582e", 0, "d135d5d5-502a-4af9-9dad-cd8ab64a31c3", "guest@mail.com", false, null, true, null, false, null, "guest@mail.com", "guest@mail.com", "AQAAAAEAACcQAAAAEBdhmhkd2NctHOIf0FKPKGjmoerpZ3j0VZXfaSmCjRoyJB+zNFUthXGYnqCxL8g8gg==", null, false, null, false, "guest@mail.com" },
                    { "dea1286-c198-4129-b3f3-b89d839581", 0, "07bac992-7c82-46fc-81e6-058e8d15f865", "agent@mail.com", false, null, true, null, false, null, "agent@mail.com", "agent@mail.com", "AQAAAAEAACcQAAAAEJ1fxz0POUhozYKBgzlG/qA/fmuSTErW0T0TLRvsbj2HUYSCCye0zhKUTCq2hQxjpg==", null, false, null, false, "agent@mail.com" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { 1, "InterCitySingle" },
                    { 2, "InterCityShared" },
                    { 3, "OneWayLocal" },
                    { 4, "RoundTripLocal" },
                    { 5, "Luxury" },
                    { 6, "Charter" }
                });

            migrationBuilder.InsertData(
                table: "DriversCars",
                columns: new[] { "DriverCarId", "PhoneNumber", "UserId" },
                values: new object[] { 1, "00359123456", "dea1286-c198-4129-b3f3-b89d839581" });

            migrationBuilder.InsertData(
                table: "CarRoutes",
                columns: new[] { "CarRouteId", "CategoryId", "DeliveryAddress", "Description", "DriverCarId", "ImageUrlRouteGoogleMaps", "IsActive", "PickUpAddress", "Price", "RenterId", "Title" },
                values: new object[,]
                {
                    { 11, 5, "Bulgaria, Sofia, Bul, Alexander Malinov, 78", "Whether you want a tourist tour from Plovdiv to Sofia, or simply busyness trip, this trip is private with a luxury limousine", 1, "https://le-cdn.hibuwebsites.com/8978d127e39b497da77df2a4b91f33eb/dms3rep/multi/opt/RSshutterstock_120889072-1920w.jpg", true, "Bulgaria, Plovdiv, Bul.Kniyaginya Maria Luiza, 31", 316.80m, "6d5800-d726-4fc8-83d9-d6b3ac1f582e", "Private Luxury" },
                    { 22, 2, "Bulgaria, Plovdiv, Bul.Kniyaginya Maria Luiza, 31", "Whether you want a tourist tour from Sofia to Plovdiv, or simply busyness trip, this trip is private with a luxury limousine", 1, "https://content.fortune.com/wp-content/uploads/2014/09/170030873.jpg?resize=1200,600", true, "Bulgaria, Sofia, Bul, Alexander malinov, 78", 316.80m, "6d5800-d726-4fc8-83d9-d6b3ac1f582e", "Shared" },
                    { 33, 2, "Bulgaria, Plovdiv, Bul.Kniyaginya Maria Luiza, 31", "Whether you want a tourist tour from Sofia to Plovdiv, or simply busyness trip, this trip is private with a luxury limousine", 1, "https://bulgaria-infoguide.com/wp-content/uploads/2018/10/green-taxi-1024x768.jpg", true, "Bulgaria, Sofia, Bul, Alexander malinov, 78", 158.40m, "6d5800-d726-4fc8-83d9-d6b3ac1f582e", "Shared with One" },
                    { 44, 3, "Antique Theater, str. Tsar Ivaylo 4, Plovdiv, Bulgaria", "Whether you want a tourist tour in Plovdiv, or simply busyness trip, this trip is will satisy your expectation with a luxury limousine", 1, "https://ekotaxi.bg/wp-content/uploads/2020/03/single_cab_redone-min-1-2048x1536.png", true, "Bulgaria, Plovdiv, Bul.Kniyaginya Maria Luiza, 31", 6.20m, "6d5800-d726-4fc8-83d9-d6b3ac1f582e", "OneWayLocal" },
                    { 55, 4, "Antique Theater, str. Tsar Ivaylo 4, Plovdiv, Bulgaria", "Whether you want a tourist tour in Plovdiv, or simply busyness trip, this trip is will satisfy your expectation with a luxury limousine", 1, "https://content.fortune.com/wp-content/uploads/2014/09/170030873.jpg?resize=1200,600", true, "Bulgaria, Plovdiv, Bul.Kniyaginya Maria Luiza, 31", 10.20m, "6d5800-d726-4fc8-83d9-d6b3ac1f582e", "RoundTripLocal" },
                    { 66, 6, "Hartmann Road, London E16 2PX", "Whether you want a tourist tour in Plovdiv, or simply busyness trip, this trip is will satisfy your expectation with a luxury limousine", 1, "https://th.bing.com/th/id/R.4f634d4c26e3f1a1cda6459f649713d1?rik=GYIFZQe3lUWPJA&pid=ImgRaw&r=0", true, "Krumovo 4009, Rodopi Municipality, Plovdiv District", 10.20m, "6d5800-d726-4fc8-83d9-d6b3ac1f582e", "Charter" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarRoutes_CategoryId",
                table: "CarRoutes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CarRoutes_DriverCarId",
                table: "CarRoutes",
                column: "DriverCarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarRoutes_RenterId",
                table: "CarRoutes",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_DriversCars_UserId",
                table: "DriversCars",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarRoutes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "DriversCars");

            migrationBuilder.DropTable(
                name: "ApplicationUser");
        }
    }
}
