using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_School_School_ID",
                table: "Person");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonClass_Class_Class_ID",
                table: "PersonClass");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonClass_Person_User_ID",
                table: "PersonClass");

            migrationBuilder.RenameColumn(
                name: "School_name",
                table: "School",
                newName: "SchoolName");

            migrationBuilder.RenameColumn(
                name: "School_ID",
                table: "School",
                newName: "SchoolID");

            migrationBuilder.RenameColumn(
                name: "User_ID",
                table: "PersonClass",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "Class_ID",
                table: "PersonClass",
                newName: "ClassID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonClass_User_ID",
                table: "PersonClass",
                newName: "IX_PersonClass_UserID");

            migrationBuilder.RenameColumn(
                name: "Year_group",
                table: "Person",
                newName: "YearGroup");

            migrationBuilder.RenameColumn(
                name: "User_type",
                table: "Person",
                newName: "UserType");

            migrationBuilder.RenameColumn(
                name: "School_ID",
                table: "Person",
                newName: "SchoolID");

            migrationBuilder.RenameColumn(
                name: "Last_name",
                table: "Person",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "First_name",
                table: "Person",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Date_of_birth",
                table: "Person",
                newName: "DateOfBirth");

            migrationBuilder.RenameColumn(
                name: "User_ID",
                table: "Person",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Person_School_ID",
                table: "Person",
                newName: "IX_Person_SchoolID");

            migrationBuilder.RenameColumn(
                name: "Class_name",
                table: "Class",
                newName: "ClassName");

            migrationBuilder.RenameColumn(
                name: "Class_description",
                table: "Class",
                newName: "ClassDescription");

            migrationBuilder.RenameColumn(
                name: "Class_ID",
                table: "Class",
                newName: "ClassID");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_School_SchoolID",
                table: "Person",
                column: "SchoolID",
                principalTable: "School",
                principalColumn: "SchoolID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonClass_Class_ClassID",
                table: "PersonClass",
                column: "ClassID",
                principalTable: "Class",
                principalColumn: "ClassID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonClass_Person_UserID",
                table: "PersonClass",
                column: "UserID",
                principalTable: "Person",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_School_SchoolID",
                table: "Person");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonClass_Class_ClassID",
                table: "PersonClass");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonClass_Person_UserID",
                table: "PersonClass");

            migrationBuilder.RenameColumn(
                name: "SchoolName",
                table: "School",
                newName: "School_name");

            migrationBuilder.RenameColumn(
                name: "SchoolID",
                table: "School",
                newName: "School_ID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "PersonClass",
                newName: "User_ID");

            migrationBuilder.RenameColumn(
                name: "ClassID",
                table: "PersonClass",
                newName: "Class_ID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonClass_UserID",
                table: "PersonClass",
                newName: "IX_PersonClass_User_ID");

            migrationBuilder.RenameColumn(
                name: "YearGroup",
                table: "Person",
                newName: "Year_group");

            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "Person",
                newName: "User_type");

            migrationBuilder.RenameColumn(
                name: "SchoolID",
                table: "Person",
                newName: "School_ID");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Person",
                newName: "Last_name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Person",
                newName: "First_name");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Person",
                newName: "Date_of_birth");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Person",
                newName: "User_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Person_SchoolID",
                table: "Person",
                newName: "IX_Person_School_ID");

            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "Class",
                newName: "Class_name");

            migrationBuilder.RenameColumn(
                name: "ClassDescription",
                table: "Class",
                newName: "Class_description");

            migrationBuilder.RenameColumn(
                name: "ClassID",
                table: "Class",
                newName: "Class_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_School_School_ID",
                table: "Person",
                column: "School_ID",
                principalTable: "School",
                principalColumn: "School_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonClass_Class_Class_ID",
                table: "PersonClass",
                column: "Class_ID",
                principalTable: "Class",
                principalColumn: "Class_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonClass_Person_User_ID",
                table: "PersonClass",
                column: "User_ID",
                principalTable: "Person",
                principalColumn: "User_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
