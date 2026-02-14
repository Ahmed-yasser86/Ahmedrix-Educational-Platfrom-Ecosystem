using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoursesPlatform.Migrations
{
    public partial class CreateInstructorsTableForReal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. إنشاء الجدول من الصفر لأنه مش موجود في الحقيقة
            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                });

            // 2. إضافة العمود في Categories لو مش موجود
            // ملحوظة: لو العمود موجود فعلاً الـ Update-Database هيفشل، وقتها هنمسح الجزء ده
            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "Categories",
                type: "int",
                nullable: true);

            // 3. إضافة البيانات (Seed Data)
            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "Id", "Description", "Email", "Name", "ProfileImagePath" },
                values: new object[,]
                {
                    { 1, "Expert in Software Engineering.", "ahmed@example.com", "Dr. Ahmed Ali", "" },
                    { 2, "Senior Web Developer.", "sarah@example.com", "Eng. Sarah Hassan", "" }
                });

            // 4. عمل العلاقات
            migrationBuilder.CreateIndex(
                name: "IX_Categories_InstructorId",
                table: "Categories",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Instructors_InstructorId",
                table: "Categories",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Instructors_InstructorId",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Categories");
        }
    }
}