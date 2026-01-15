using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iPath.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class Group_Community : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CommunityId",
                table: "groups",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_groups_CommunityId",
                table: "groups",
                column: "CommunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_groups_communities_CommunityId",
                table: "groups",
                column: "CommunityId",
                principalTable: "communities",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_groups_communities_CommunityId",
                table: "groups");

            migrationBuilder.DropIndex(
                name: "IX_groups_CommunityId",
                table: "groups");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                table: "groups");
        }
    }
}
