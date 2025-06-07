using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eko.Database.Migrations
{
    /// <inheritdoc />
    public partial class initial_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: new Guid("f5773de3-eb44-4733-8f7f-c3cec0a91cbf"));

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Email", "FirstName", "IsAdmin", "LastName", "Password" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "admin@gmail.com", "Admin", true, "Admin", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Email", "FirstName", "IsAdmin", "LastName", "Password" },
                values: new object[] { new Guid("f5773de3-eb44-4733-8f7f-c3cec0a91cbf"), "admin@gmail.com", "Admin", true, "Admin", "admin" });
        }
    }
}
