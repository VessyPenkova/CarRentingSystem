using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentingSystem.Infrastucture.Migrations
{
    public partial class InitialMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800-d726-4fc8-83d9-d6b3ac1f582e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "010609e3-a31c-481a-a92b-114edf52ca18", "AQAAAAEAACcQAAAAEI7MUMkKVr9O6xxmjUYqqmBaRTW2I6lClLNNahsXGaxmAncDI1PbJGysTCSwcDWzDw==", "376f5f21-e70b-4277-9bd1-2fa947568e78" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4f211c87-44d0-4ee1-a09e-5a9e9088b91a", "AQAAAAEAACcQAAAAECM+gzaZ3QUWZT/6PIcq+waXOuRI+DFePHg1uW8MRsOpyCcxVyLSxodW6o4JUyTuSQ==", "0df468ca-2ad7-42e7-8831-e7a016c2aa04" });

            migrationBuilder.UpdateData(
                table: "Shipments",
                keyColumn: "ShipmentId",
                keyValue: 4,
                column: "ImageUrlShipmentGoogleMaps",
                value: "https://www.luxuryaircraftsolutions.com/wp-content/uploads/2020/05/image-226.png");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800-d726-4fc8-83d9-d6b3ac1f582e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2bfc614f-eb36-4862-aa4b-cfc3da2520fe", "AQAAAAEAACcQAAAAEOfmAp+M6kfDBRv1UhHOxZTbEPvEjO87EJREIfzkqv5a2+ot3BLgADwPSIAA5iteRg==", "387851ed-777a-492b-92a9-47e7c044989e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f0ffa6e9-7704-4aca-9107-057415918d23", "AQAAAAEAACcQAAAAEOAnjVnYxUzHvNQaYYekh9uriLwKfgYM41ZH7dxMeMC9IiGdnEUQsSJirL4HSZvyLA==", "c6e42f7f-05d2-48cc-8a25-f7af14198ce2" });

            migrationBuilder.UpdateData(
                table: "Shipments",
                keyColumn: "ShipmentId",
                keyValue: 4,
                column: "ImageUrlShipmentGoogleMaps",
                value: "https://th.bing.com/th/id/R.4f634d4c26e3f1a1cda6459f649713d1?rik=GYIFZQe3lUWPJA&pid=ImgRaw&r=0");
        }
    }
}
