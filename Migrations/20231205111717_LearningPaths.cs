using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace codelab_exam_server.Migrations
{
    /// <inheritdoc />
    public partial class LearningPaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LearningPathId",
                table: "Topics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LearningPaths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningPaths", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_LearningPathId",
                table: "Topics",
                column: "LearningPathId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_LearningPaths_LearningPathId",
                table: "Topics",
                column: "LearningPathId",
                principalTable: "LearningPaths",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_LearningPaths_LearningPathId",
                table: "Topics");

            migrationBuilder.DropTable(
                name: "LearningPaths");

            migrationBuilder.DropIndex(
                name: "IX_Topics_LearningPathId",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "LearningPathId",
                table: "Topics");
        }
    }
}
