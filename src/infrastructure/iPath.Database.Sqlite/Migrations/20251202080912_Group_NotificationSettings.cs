using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iPath.EF.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class Group_NotificationSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "notification_filter",
                table: "group_members",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "notification_targets",
                table: "group_members",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "notification_filter",
                table: "group_members");

            migrationBuilder.DropColumn(
                name: "notification_targets",
                table: "group_members");
        }
    }
}
