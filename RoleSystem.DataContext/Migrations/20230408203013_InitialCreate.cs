using System;
using Microsoft.EntityFrameworkCore.Migrations;
using RoleSystem.DataContext.DataContexts;
using RoleSystem.DataContext.Entities;

#nullable disable

namespace RoleSystem.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentFunctionId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    LastEdit = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastEditUser = table.Column<string>(type: "nvarchar(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleFunctions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    FunctionId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleFunctions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    LastEdit = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastEditUser = table.Column<string>(type: "nvarchar(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUsers", x => x.Id);
                });

            InsertDefaultFunctions(migrationBuilder);
            InsertDefaultRoles(migrationBuilder);
        }

        /// <summary>
        /// Inserts default functions to database
        /// </summary>
        /// <param name="migrationBuilder"></param>
        private void InsertDefaultFunctions(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 1, null, "RoleSystem.Manage", "Give access to all Role System functions", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 2, 1, "RoleSystem.Functions.Manage", "Give access to all functions", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 3, 2, "RoleSystem.Functions.Get", "Give access to get a list of functions", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 4, 2, "RoleSystem.Functions.Create", "Give access to create functions", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 5, 2, "RoleSystem.Functions.Update", "Give access to update functions", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 6, 2, "RoleSystem.Functions.Delete", "Give access to delete functions", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 7, 1, "RoleSystem.Roles.Manage", "Give access to all roles functions", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 8, 7, "RoleSystem.Roles.Get", "Give access to get all functions", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 9, 7, "RoleSystem.Roles.Create", "Give access to create roles", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 10, 7, "RoleSystem.Roles.Update", "Give access to update roles", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 11, 7, "RoleSystem.Roles.Delete", "Give access to delete roles", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 12, 7, "RoleSystem.Roles.Functions", "Give access to add and remove functions from Role", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 13, 12, "RoleSystem.Roles.Functions.Add", "Give access to add functions to all roles", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 14, 12, "RoleSystem.Roles.Functions.Remove", "Give access to remove functions from all roles", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 15, 7, "RoleSystem.Roles.Users", "Give access to add and remove users from all roles", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 16, 15, "RoleSystem.Roles.Users.Add", "Give access to assign users to all roles", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Functions",
                    columns: new string[] { "Id", "ParentFunctionId", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 17, 15, "RoleSystem.Roles.Users.Remove", "Give access to remove users from all roles", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

        }

        /// <summary>
        /// Inserts default role with access to all functions
        /// </summary>
        /// <param name="migrationBuilder"></param>
        private void InsertDefaultRoles(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                    table: "Roles",
                    columns: new string[] { "Id", "Name", "Description", "Created", "CreatedUser", "LastEdit", "LastEditUser" },
                    values: new object[] { 1, "Default Role", "Have access to all functions", DateTime.Now, "EF Migrations", DateTime.Now, "EF Migrations" });

            migrationBuilder.InsertData(
                    table: "Roles",
                    columns: new string[] { "Id", "RoleId", "FunctionId", "Created", "CreatedUser" },
                    values: new object[] { 1, 1, 1, DateTime.Now, "EF Migrations" });


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropTable(
                name: "RoleFunctions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "RoleUsers");
        }
    }
}
