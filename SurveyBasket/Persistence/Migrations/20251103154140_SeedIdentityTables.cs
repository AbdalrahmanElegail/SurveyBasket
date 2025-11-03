using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "666fd99b-9bac-426e-8c5e-c222cd2d1c79", "4b9216e0-ee85-4a44-8199-699d4619f0a7", true, false, "Member", "MEMBER" },
                    { "99668d56-a0cc-4fc6-bc45-d157060a4cde", "d5b57fb2-125e-4290-9d1b-3b46a93768bb", false, false, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "fc39c487-198d-452a-8451-4d1cb3157ceb", 0, "c895a154-d110-4c32-8827-23450f490fd8", "admin@survey-basket.com", true, "Survey Basket", "Admin", false, null, "ADMIN@SURVEY-BASKET.COM", "ADMIN@SURVEY-BASKET.COM", "AQAAAAIAAYagAAAAEHO85fnBm+Kdv5VbZp95EuFZDc7wl1HucXCvKQW2IJmMtT/kfo9gcbpj6PCrKUJ8gw==", null, false, "E58965767DC045B19EB9DB54B7FF2F00", false, "admin@survey-basket.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "polls:read", "99668d56-a0cc-4fc6-bc45-d157060a4cde" },
                    { 2, "permissions", "polls:add", "99668d56-a0cc-4fc6-bc45-d157060a4cde" },
                    { 3, "permissions", "polls:update", "99668d56-a0cc-4fc6-bc45-d157060a4cde" },
                    { 4, "permissions", "polls:delete", "99668d56-a0cc-4fc6-bc45-d157060a4cde" },
                    { 5, "permissions", "questions:read", "99668d56-a0cc-4fc6-bc45-d157060a4cde" },
                    { 6, "permissions", "questions:add", "99668d56-a0cc-4fc6-bc45-d157060a4cde" },
                    { 7, "permissions", "questions:update", "99668d56-a0cc-4fc6-bc45-d157060a4cde" },
                    { 8, "permissions", "users:read", "99668d56-a0cc-4fc6-bc45-d157060a4cde" },
                    { 9, "permissions", "users:add", "99668d56-a0cc-4fc6-bc45-d157060a4cde" },
                    { 10, "permissions", "users:update", "99668d56-a0cc-4fc6-bc45-d157060a4cde" },
                    { 11, "permissions", "roles:read", "99668d56-a0cc-4fc6-bc45-d157060a4cde" },
                    { 12, "permissions", "roles:add", "99668d56-a0cc-4fc6-bc45-d157060a4cde" },
                    { 13, "permissions", "roles:update", "99668d56-a0cc-4fc6-bc45-d157060a4cde" },
                    { 14, "permissions", "results:read", "99668d56-a0cc-4fc6-bc45-d157060a4cde" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "99668d56-a0cc-4fc6-bc45-d157060a4cde", "fc39c487-198d-452a-8451-4d1cb3157ceb" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "666fd99b-9bac-426e-8c5e-c222cd2d1c79");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "99668d56-a0cc-4fc6-bc45-d157060a4cde", "fc39c487-198d-452a-8451-4d1cb3157ceb" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99668d56-a0cc-4fc6-bc45-d157060a4cde");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fc39c487-198d-452a-8451-4d1cb3157ceb");
        }
    }
}
