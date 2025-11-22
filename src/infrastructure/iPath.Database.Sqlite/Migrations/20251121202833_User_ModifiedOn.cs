using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iPath.EF.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class User_ModifiedOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_roles_users_user_id",
                table: "roles");

            migrationBuilder.DropIndex(
                name: "ix_roles_user_id",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "roles");

            migrationBuilder.AlterDatabase(
                collation: "NOCASE");

            migrationBuilder.AddColumn<DateTime>(
                name: "modified_on",
                table: "users",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "modified_on",
                table: "users");

            migrationBuilder.AlterDatabase(
                oldCollation: "NOCASE");

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "roles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_roles_user_id",
                table: "roles",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_roles_users_user_id",
                table: "roles",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
