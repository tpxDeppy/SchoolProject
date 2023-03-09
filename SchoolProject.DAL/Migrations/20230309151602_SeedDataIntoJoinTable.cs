using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataIntoJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassConnection");

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "User_ID",
                keyValue: new Guid("77fff675-6f16-4871-8110-345b540f598d"));

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "User_ID",
                keyValue: new Guid("b63071a1-2890-47ac-9f74-10af84b64ec3"));

            migrationBuilder.AlterColumn<int>(
                name: "Year_group",
                table: "Person",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_of_birth",
                table: "Person",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.CreateTable(
                name: "PersonClass",
                columns: table => new
                {
                    Class_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    User_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonClass", x => new { x.Class_ID, x.User_ID });
                    table.ForeignKey(
                        name: "FK_PersonClass_Class_Class_ID",
                        column: x => x.Class_ID,
                        principalTable: "Class",
                        principalColumn: "Class_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonClass_Person_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Person",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Class",
                columns: new[] { "Class_ID", "Class_description", "Class_name" },
                values: new object[] { new Guid("9f082281-2925-4261-a339-be2f4db65271"), "How to dance", "Dancing" });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "User_ID", "Date_of_birth", "First_name", "Last_name", "School_ID", "User_type", "Year_group" },
                values: new object[,]
                {
                    { new Guid("4ca1789c-b20c-4320-8472-c52ebeac47e0"), new DateTime(2005, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Angelina", "Jolie", new Guid("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"), "Pupil", 13 },
                    { new Guid("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc"), null, "Johnny", "Depp", new Guid("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"), "Teacher", null }
                });

            migrationBuilder.InsertData(
                table: "PersonClass",
                columns: new[] { "Class_ID", "User_ID" },
                values: new object[,]
                {
                    { new Guid("941b2d53-f728-4c50-87f8-a8bb2fb6d8c2"), new Guid("4ca1789c-b20c-4320-8472-c52ebeac47e0") },
                    { new Guid("b7f068af-3856-4d1b-9023-91a3d01ac1e0"), new Guid("4ca1789c-b20c-4320-8472-c52ebeac47e0") },
                    { new Guid("b7f068af-3856-4d1b-9023-91a3d01ac1e0"), new Guid("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc") },
                    { new Guid("ebb7a0d9-730f-40f1-9b9c-541f371074ba"), new Guid("4ca1789c-b20c-4320-8472-c52ebeac47e0") },
                    { new Guid("ebb7a0d9-730f-40f1-9b9c-541f371074ba"), new Guid("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonClass_User_ID",
                table: "PersonClass",
                column: "User_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonClass");

            migrationBuilder.DeleteData(
                table: "Class",
                keyColumn: "Class_ID",
                keyValue: new Guid("9f082281-2925-4261-a339-be2f4db65271"));

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "User_ID",
                keyValue: new Guid("4ca1789c-b20c-4320-8472-c52ebeac47e0"));

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "User_ID",
                keyValue: new Guid("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc"));

            migrationBuilder.AlterColumn<int>(
                name: "Year_group",
                table: "Person",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_of_birth",
                table: "Person",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ClassConnection",
                columns: table => new
                {
                    ClassesClass_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonsUser_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassConnection", x => new { x.ClassesClass_ID, x.PersonsUser_ID });
                    table.ForeignKey(
                        name: "FK_ClassConnection_Class_ClassesClass_ID",
                        column: x => x.ClassesClass_ID,
                        principalTable: "Class",
                        principalColumn: "Class_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassConnection_Person_PersonsUser_ID",
                        column: x => x.PersonsUser_ID,
                        principalTable: "Person",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "User_ID", "Date_of_birth", "First_name", "Last_name", "School_ID", "User_type", "Year_group" },
                values: new object[,]
                {
                    { new Guid("77fff675-6f16-4871-8110-345b540f598d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Johnny", "Depp", new Guid("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"), "Teacher", 0 },
                    { new Guid("b63071a1-2890-47ac-9f74-10af84b64ec3"), new DateTime(2005, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Angelina", "Jolie", new Guid("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"), "Pupil", 13 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassConnection_PersonsUser_ID",
                table: "ClassConnection",
                column: "PersonsUser_ID");
        }
    }
}
