using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iPath.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class Community_Questionnaire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionnaireForCommunity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuestionnaireId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CommunityId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Usage = table.Column<int>(type: "INTEGER", nullable: false),
                    ExplicitVersion = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionnaireForCommunity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionnaireForCommunity_communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "communities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionnaireForCommunity_questionnaires_QuestionnaireId",
                        column: x => x.QuestionnaireId,
                        principalTable: "questionnaires",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireForCommunity_CommunityId",
                table: "QuestionnaireForCommunity",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireForCommunity_QuestionnaireId",
                table: "QuestionnaireForCommunity",
                column: "QuestionnaireId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionnaireForCommunity");
        }
    }
}
