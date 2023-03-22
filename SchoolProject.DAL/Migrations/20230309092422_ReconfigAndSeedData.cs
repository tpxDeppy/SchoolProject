using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReconfigAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Class",
                columns: new[] { "Class_ID", "Class_description", "Class_name" },
                values: new object[,]
                {
                    { new Guid("941b2d53-f728-4c50-87f8-a8bb2fb6d8c2"), "How to sing", "Singing" },
                    { new Guid("b7f068af-3856-4d1b-9023-91a3d01ac1e0"), "How to act", "Acting" },
                    { new Guid("ebb7a0d9-730f-40f1-9b9c-541f371074ba"), "How to write movie scripts", "Scripting" }
                });

            migrationBuilder.InsertData(
                table: "School",
                columns: new[] { "School_ID", "School_name" },
                values: new object[] { new Guid("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"), "Hollywood School" });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "User_ID", "Date_of_birth", "First_name", "Last_name", "School_ID", "User_type", "Year_group" },
                values: new object[,]
                {
                    { new Guid("77fff675-6f16-4871-8110-345b540f598d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Johnny", "Depp", new Guid("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"), "Teacher", 0 },
                    { new Guid("b63071a1-2890-47ac-9f74-10af84b64ec3"), new DateTime(2005, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Angelina", "Jolie", new Guid("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"), "Pupil", 13 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Class",
                keyColumn: "Class_ID",
                keyValue: new Guid("941b2d53-f728-4c50-87f8-a8bb2fb6d8c2"));

            migrationBuilder.DeleteData(
                table: "Class",
                keyColumn: "Class_ID",
                keyValue: new Guid("b7f068af-3856-4d1b-9023-91a3d01ac1e0"));

            migrationBuilder.DeleteData(
                table: "Class",
                keyColumn: "Class_ID",
                keyValue: new Guid("ebb7a0d9-730f-40f1-9b9c-541f371074ba"));

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "User_ID",
                keyValue: new Guid("77fff675-6f16-4871-8110-345b540f598d"));

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "User_ID",
                keyValue: new Guid("b63071a1-2890-47ac-9f74-10af84b64ec3"));

            migrationBuilder.DeleteData(
                table: "School",
                keyColumn: "School_ID",
                keyValue: new Guid("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"));
        }
    }
}
