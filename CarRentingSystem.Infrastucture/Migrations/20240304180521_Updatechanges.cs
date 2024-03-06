using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentingSystem.Infrastucture.Migrations
{
    public partial class Updatechanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarRoutes_ApplicationUser_RenterId",
                table: "CarRoutes");

            migrationBuilder.DropForeignKey(
                name: "FK_DriversCars_ApplicationUser_UserId",
                table: "DriversCars");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6d5800-d726-4fc8-83d9-d6b3ac1f582e", 0, "abc85a61-992f-4a51-891b-1376ff130d11", "guest@mail.com", false, null, true, null, false, null, "guest@mail.com", "guest@mail.com", "AQAAAAEAACcQAAAAEGXHYFb38kiJ41owjYuiGnGiYYH5TKswWf50ngyhYFGwZkd+61uwZf/zhejT4dL3KA==", null, false, null, false, "guest@mail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "dea1286-c198-4129-b3f3-b89d839581", 0, "c0ce6761-529f-4e09-a56f-0b1700a25974", "agent@mail.com", false, null, true, null, false, null, "agent@mail.com", "agent@mail.com", "AQAAAAEAACcQAAAAED7mAHh4q4SMvAYU4wmbYZVgJuV7c0vJ50Md6qwGY+KMyrnW3EwN08VYarAkuKrJlg==", null, false, null, false, "agent@mail.com" });

            migrationBuilder.AddForeignKey(
                name: "FK_CarRoutes_AspNetUsers_RenterId",
                table: "CarRoutes",
                column: "RenterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DriversCars_AspNetUsers_UserId",
                table: "DriversCars",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarRoutes_AspNetUsers_RenterId",
                table: "CarRoutes");

            migrationBuilder.DropForeignKey(
                name: "FK_DriversCars_AspNetUsers_UserId",
                table: "DriversCars");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800-d726-4fc8-83d9-d6b3ac1f582e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea1286-c198-4129-b3f3-b89d839581");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ApplicationUser",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6d5800-d726-4fc8-83d9-d6b3ac1f582e", 0, "d135d5d5-502a-4af9-9dad-cd8ab64a31c3", "guest@mail.com", false, null, true, null, false, null, "guest@mail.com", "guest@mail.com", "AQAAAAEAACcQAAAAEBdhmhkd2NctHOIf0FKPKGjmoerpZ3j0VZXfaSmCjRoyJB+zNFUthXGYnqCxL8g8gg==", null, false, null, false, "guest@mail.com" });

            migrationBuilder.InsertData(
                table: "ApplicationUser",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "dea1286-c198-4129-b3f3-b89d839581", 0, "07bac992-7c82-46fc-81e6-058e8d15f865", "agent@mail.com", false, null, true, null, false, null, "agent@mail.com", "agent@mail.com", "AQAAAAEAACcQAAAAEJ1fxz0POUhozYKBgzlG/qA/fmuSTErW0T0TLRvsbj2HUYSCCye0zhKUTCq2hQxjpg==", null, false, null, false, "agent@mail.com" });

            migrationBuilder.AddForeignKey(
                name: "FK_CarRoutes_ApplicationUser_RenterId",
                table: "CarRoutes",
                column: "RenterId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DriversCars_ApplicationUser_UserId",
                table: "DriversCars",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
