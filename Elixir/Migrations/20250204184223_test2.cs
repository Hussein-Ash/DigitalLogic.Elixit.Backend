using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elixir.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f3f0e891-3212-4491-ab42-5ce37b3c221d"));

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Active", "CreationDate", "Deleted", "Email", "Followings", "FullName", "Imgs", "Password", "PhoneNumber", "Role", "SavedProducts", "UserName" },
                values: new object[] { new Guid("eee1f743-b16d-43ca-adca-9726d5fa03bf"), false, new DateTime(2025, 2, 4, 18, 42, 22, 763, DateTimeKind.Utc).AddTicks(8339), false, null, 0, "Admin", null, "12345678", new List<string> { "01234567891" }, 3, 0, "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("eee1f743-b16d-43ca-adca-9726d5fa03bf"));

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Products");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Deleted", "Email", "Followings", "FullName", "Imgs", "Password", "PhoneNumber", "Role", "SavedProducts", "UserName" },
                values: new object[] { new Guid("f3f0e891-3212-4491-ab42-5ce37b3c221d"), new DateTime(2025, 2, 4, 18, 23, 9, 909, DateTimeKind.Utc).AddTicks(9408), false, null, 0, "Admin", null, "12345678", new List<string> { "01234567891" }, 3, 0, "Admin" });
        }
    }
}
