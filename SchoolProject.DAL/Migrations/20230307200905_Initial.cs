using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    Class_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Class_name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Class_description = table.Column<string>(type: "nvarchar(120)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.Class_ID);
                });

            migrationBuilder.CreateTable(
                name: "School",
                columns: table => new
                {
                    School_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    School_name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.School_ID);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    User_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    First_name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Last_name = table.Column<string>(type: "nvarchar(80)", nullable: false),
                    User_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date_of_birth = table.Column<DateTime>(type: "date", nullable: false),
                    Year_group = table.Column<int>(type: "int", nullable: false),
                    School_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.User_ID);
                    table.ForeignKey(
                        name: "FK_Person_School_School_ID",
                        column: x => x.School_ID,
                        principalTable: "School",
                        principalColumn: "School_ID",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ClassConnection_PersonsUser_ID",
                table: "ClassConnection",
                column: "PersonsUser_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Person_School_ID",
                table: "Person",
                column: "School_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassConnection");

            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "School");
        }
    }
}
