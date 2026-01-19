using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iPath.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class ServiceRequest_Group_Required : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_servicerequests_groups_group_id",
                table: "servicerequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "group_id",
                table: "servicerequests",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_servicerequests_groups_group_id",
                table: "servicerequests",
                column: "group_id",
                principalTable: "groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_servicerequests_groups_group_id",
                table: "servicerequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "group_id",
                table: "servicerequests",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_servicerequests_groups_group_id",
                table: "servicerequests",
                column: "group_id",
                principalTable: "groups",
                principalColumn: "id");
        }
    }
}
