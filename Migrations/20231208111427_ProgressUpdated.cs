using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace codelab_exam_server.Migrations
{
    /// <inheritdoc />
    public partial class ProgressUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExerciseProgresses_LearningPaths_LearningPathId",
                table: "UserExerciseProgresses");

            migrationBuilder.DropIndex(
                name: "IX_UserExerciseProgresses_LearningPathId",
                table: "UserExerciseProgresses");

            migrationBuilder.DropColumn(
                name: "LearningPathId",
                table: "UserExerciseProgresses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LearningPathId",
                table: "UserExerciseProgresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserExerciseProgresses_LearningPathId",
                table: "UserExerciseProgresses",
                column: "LearningPathId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExerciseProgresses_LearningPaths_LearningPathId",
                table: "UserExerciseProgresses",
                column: "LearningPathId",
                principalTable: "LearningPaths",
                principalColumn: "Id");
        }
    }
}
