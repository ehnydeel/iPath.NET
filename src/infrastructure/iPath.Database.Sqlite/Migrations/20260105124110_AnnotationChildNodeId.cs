using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iPath.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class AnnotationChildNodeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChildNodeId",
                table: "annotations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_annotations_ChildNodeId",
                table: "annotations",
                column: "ChildNodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_annotations_nodes_ChildNodeId",
                table: "annotations",
                column: "ChildNodeId",
                principalTable: "nodes",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_annotations_nodes_ChildNodeId",
                table: "annotations");

            migrationBuilder.DropIndex(
                name: "IX_annotations_ChildNodeId",
                table: "annotations");

            migrationBuilder.DropColumn(
                name: "ChildNodeId",
                table: "annotations");
        }
    }
}
