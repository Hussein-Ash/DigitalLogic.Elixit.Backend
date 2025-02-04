using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elixir.Migrations
{
    /// <inheritdoc />
    public partial class test3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("eee1f743-b16d-43ca-adca-9726d5fa03bf"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Active", "CreationDate", "Deleted", "Email", "Followings", "FullName", "Imgs", "Password", "PhoneNumber", "Role", "SavedProducts", "UserName" },
                values: new object[] { new Guid("1ba8ef69-7e7b-4db3-ba4c-355db6bd9374"), false, new DateTime(2025, 2, 4, 18, 45, 32, 37, DateTimeKind.Utc).AddTicks(3194), false, null, 0, "Admin", null, "$2a$11$9Jwo.74siDJFF1gfLLshOeK3V0iXxPpEg4957zAF9jiYs14HVmJsG", new List<string> { "01234567891" }, 3, 0, "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("1ba8ef69-7e7b-4db3-ba4c-355db6bd9374"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Active", "CreationDate", "Deleted", "Email", "Followings", "FullName", "Imgs", "Password", "PhoneNumber", "Role", "SavedProducts", "UserName" },
                values: new object[] { new Guid("eee1f743-b16d-43ca-adca-9726d5fa03bf"), false, new DateTime(2025, 2, 4, 18, 42, 22, 763, DateTimeKind.Utc).AddTicks(8339), false, null, 0, "Admin", null, "12345678", new List<string> { "01234567891" }, 3, 0, "Admin" });
        }
    }
}
