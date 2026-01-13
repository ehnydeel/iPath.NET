using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iPath.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class UserFkUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_communities_users_OwnerId",
                table: "communities");

            migrationBuilder.DropForeignKey(
                name: "FK_community_group_members_users_user_id",
                table: "community_group_members");

            migrationBuilder.DropForeignKey(
                name: "FK_group_members_users_UserId",
                table: "group_members");

            migrationBuilder.DropForeignKey(
                name: "FK_node_lastvisits_nodes_NodeId",
                table: "node_lastvisits");

            migrationBuilder.DropForeignKey(
                name: "FK_node_lastvisits_users_UserId",
                table: "node_lastvisits");

            migrationBuilder.DropForeignKey(
                name: "FK_nodes_users_owner_id",
                table: "nodes");

            migrationBuilder.DropForeignKey(
                name: "FK_notifications_users_UserId",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_questionnaire_groups_questionnaires_QuestionnaireId",
                table: "questionnaire_groups");

            migrationBuilder.DropForeignKey(
                name: "FK_questionnaire_responses_users_OwnerId",
                table: "questionnaire_responses");

            migrationBuilder.AlterColumn<string>(
                name: "Resource",
                table: "questionnaire_responses",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb");

            migrationBuilder.AddForeignKey(
                name: "FK_communities_users_OwnerId",
                table: "communities",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_community_group_members_users_user_id",
                table: "community_group_members",
                column: "user_id",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_group_members_users_UserId",
                table: "group_members",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_node_lastvisits_nodes_NodeId",
                table: "node_lastvisits",
                column: "NodeId",
                principalTable: "nodes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_node_lastvisits_users_UserId",
                table: "node_lastvisits",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_nodes_users_owner_id",
                table: "nodes",
                column: "owner_id",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_notifications_users_UserId",
                table: "notifications",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_questionnaire_groups_questionnaires_QuestionnaireId",
                table: "questionnaire_groups",
                column: "QuestionnaireId",
                principalTable: "questionnaires",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_questionnaire_responses_users_OwnerId",
                table: "questionnaire_responses",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_communities_users_OwnerId",
                table: "communities");

            migrationBuilder.DropForeignKey(
                name: "FK_community_group_members_users_user_id",
                table: "community_group_members");

            migrationBuilder.DropForeignKey(
                name: "FK_group_members_users_UserId",
                table: "group_members");

            migrationBuilder.DropForeignKey(
                name: "FK_node_lastvisits_nodes_NodeId",
                table: "node_lastvisits");

            migrationBuilder.DropForeignKey(
                name: "FK_node_lastvisits_users_UserId",
                table: "node_lastvisits");

            migrationBuilder.DropForeignKey(
                name: "FK_nodes_users_owner_id",
                table: "nodes");

            migrationBuilder.DropForeignKey(
                name: "FK_notifications_users_UserId",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_questionnaire_groups_questionnaires_QuestionnaireId",
                table: "questionnaire_groups");

            migrationBuilder.DropForeignKey(
                name: "FK_questionnaire_responses_users_OwnerId",
                table: "questionnaire_responses");

            migrationBuilder.AlterColumn<string>(
                name: "Resource",
                table: "questionnaire_responses",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_communities_users_OwnerId",
                table: "communities",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_community_group_members_users_user_id",
                table: "community_group_members",
                column: "user_id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_group_members_users_UserId",
                table: "group_members",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_node_lastvisits_nodes_NodeId",
                table: "node_lastvisits",
                column: "NodeId",
                principalTable: "nodes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_node_lastvisits_users_UserId",
                table: "node_lastvisits",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_nodes_users_owner_id",
                table: "nodes",
                column: "owner_id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notifications_users_UserId",
                table: "notifications",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_questionnaire_groups_questionnaires_QuestionnaireId",
                table: "questionnaire_groups",
                column: "QuestionnaireId",
                principalTable: "questionnaires",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_questionnaire_responses_users_OwnerId",
                table: "questionnaire_responses",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
