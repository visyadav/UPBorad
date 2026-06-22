using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initialmigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentResults",
                columns: table => new
                {
                    RollNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CentreNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchoolNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnrolmentNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExamType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchoolName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrandTotal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxTotal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrandTotalWords = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExtraRemarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassingYear = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentResults", x => x.RollNumber);
                });

            migrationBuilder.CreateTable(
                name: "SubjectMarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentResultRollNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxMarksTheory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxMarksInternal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxMarksTotal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinMarksTheory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinMarksInternal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObtainedTheory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObtainedInternal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalMarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectMarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectMarks_StudentResults_StudentResultRollNumber",
                        column: x => x.StudentResultRollNumber,
                        principalTable: "StudentResults",
                        principalColumn: "RollNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectMarks_StudentResultRollNumber",
                table: "SubjectMarks",
                column: "StudentResultRollNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectMarks");

            migrationBuilder.DropTable(
                name: "StudentResults");
        }
    }
}
