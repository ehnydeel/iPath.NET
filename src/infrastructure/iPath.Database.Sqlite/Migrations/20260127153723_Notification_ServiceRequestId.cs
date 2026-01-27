using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iPath.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class Notification_ServiceRequestId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ServiceRequestId",
                table: "notifications",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_notifications_ServiceRequestId",
                table: "notifications",
                column: "ServiceRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_notifications_servicerequests_ServiceRequestId",
                table: "notifications",
                column: "ServiceRequestId",
                principalTable: "servicerequests",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notifications_servicerequests_ServiceRequestId",
                table: "notifications");

            migrationBuilder.DropIndex(
                name: "IX_notifications_ServiceRequestId",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "ServiceRequestId",
                table: "notifications");
        }
    }
}
