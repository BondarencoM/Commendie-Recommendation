using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RecommendationService.Migrations
{
    public partial class AddComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    RecommendationId = table.Column<long>(nullable: true),
                    InterestId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Interests_InterestId",
                        column: x => x.InterestId,
                        principalTable: "Interests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Recommendations_RecommendationId",
                        column: x => x.RecommendationId,
                        principalTable: "Recommendations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_InterestId",
                table: "Comments",
                column: "InterestId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RecommendationId",
                table: "Comments",
                column: "RecommendationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");
        }
    }
}
