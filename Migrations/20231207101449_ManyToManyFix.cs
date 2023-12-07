using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace codelab_exam_server.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LearningPathTopics_LearningPathId",
                table: "LearningPathTopics",
                column: "LearningPathId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningPathTopics_TopicId",
                table: "LearningPathTopics",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningPathTopics_LearningPaths_LearningPathId",
                table: "LearningPathTopics",
                column: "LearningPathId",
                principalTable: "LearningPaths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LearningPathTopics_Topics_TopicId",
                table: "LearningPathTopics",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningPathTopics_LearningPaths_LearningPathId",
                table: "LearningPathTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_LearningPathTopics_Topics_TopicId",
                table: "LearningPathTopics");

            migrationBuilder.DropIndex(
                name: "IX_LearningPathTopics_LearningPathId",
                table: "LearningPathTopics");

            migrationBuilder.DropIndex(
                name: "IX_LearningPathTopics_TopicId",
                table: "LearningPathTopics");
        }
    }
}
