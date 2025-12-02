using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iPath.EF.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class Group_NotificationSettings2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "notifications",
                table: "group_members",
                newName: "notification_target");

            migrationBuilder.RenameColumn(
                name: "notification_targets",
                table: "group_members",
                newName: "notification_source");

            migrationBuilder.RenameColumn(
                name: "notification_filter",
                table: "group_members",
                newName: "notification_settings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "notification_target",
                table: "group_members",
                newName: "notifications");

            migrationBuilder.RenameColumn(
                name: "notification_source",
                table: "group_members",
                newName: "notification_targets");

            migrationBuilder.RenameColumn(
                name: "notification_settings",
                table: "group_members",
                newName: "notification_filter");
        }
    }
}
