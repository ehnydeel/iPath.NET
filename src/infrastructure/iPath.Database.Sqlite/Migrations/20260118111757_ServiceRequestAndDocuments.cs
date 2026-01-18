using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iPath.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class ServiceRequestAndDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_annotations_nodes_ChildNodeId",
                table: "annotations");

            migrationBuilder.DropForeignKey(
                name: "FK_annotations_nodes_NodeId",
                table: "annotations");

            migrationBuilder.DropForeignKey(
                name: "FK_annotations_users_OwnerId",
                table: "annotations");

            migrationBuilder.DropForeignKey(
                name: "FK_groups_users_OwnerId",
                table: "groups");

            migrationBuilder.DropForeignKey(
                name: "FK_node_lastvisits_nodes_NodeId",
                table: "node_lastvisits");

            migrationBuilder.DropForeignKey(
                name: "FK_questionnaire_responses_nodes_NodeId",
                table: "questionnaire_responses");

            migrationBuilder.DropTable(
                name: "nodes");

            migrationBuilder.RenameColumn(
                name: "NodeId",
                table: "questionnaire_responses",
                newName: "ServiceRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_questionnaire_responses_NodeId",
                table: "questionnaire_responses",
                newName: "IX_questionnaire_responses_ServiceRequestId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "groups",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_groups_OwnerId",
                table: "groups",
                newName: "IX_groups_owner_id");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "annotations",
                newName: "owner");

            migrationBuilder.RenameColumn(
                name: "NodeId",
                table: "annotations",
                newName: "servicerequest_id");

            migrationBuilder.RenameColumn(
                name: "ChildNodeId",
                table: "annotations",
                newName: "document_id");

            migrationBuilder.RenameIndex(
                name: "IX_annotations_OwnerId",
                table: "annotations",
                newName: "IX_annotations_owner");

            migrationBuilder.RenameIndex(
                name: "IX_annotations_NodeId",
                table: "annotations",
                newName: "IX_annotations_servicerequest_id");

            migrationBuilder.RenameIndex(
                name: "IX_annotations_ChildNodeId",
                table: "annotations",
                newName: "IX_annotations_document_id");

            migrationBuilder.CreateTable(
                name: "servicerequests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ipath2_id = table.Column<int>(type: "INTEGER", nullable: true),
                    StorageId = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    owner_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    group_id = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDraft = table.Column<bool>(type: "INTEGER", nullable: false),
                    NodeType = table.Column<string>(type: "TEXT", nullable: false),
                    Visibility = table.Column<int>(type: "INTEGER", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicerequests", x => x.id);
                    table.ForeignKey(
                        name: "FK_servicerequests_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "groups",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_servicerequests_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "documents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ipath2_id = table.Column<int>(type: "INTEGER", nullable: true),
                    StorageId = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    owner_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    servicerequest_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SortNr = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentNodeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DocumentType = table.Column<string>(type: "TEXT", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    file = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documents", x => x.id);
                    table.ForeignKey(
                        name: "FK_documents_documents_ParentNodeId",
                        column: x => x.ParentNodeId,
                        principalTable: "documents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_documents_servicerequests_servicerequest_id",
                        column: x => x.servicerequest_id,
                        principalTable: "servicerequests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_documents_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_documents_owner_id",
                table: "documents",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_documents_ParentNodeId",
                table: "documents",
                column: "ParentNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_documents_servicerequest_id",
                table: "documents",
                column: "servicerequest_id");

            migrationBuilder.CreateIndex(
                name: "IX_servicerequests_group_id",
                table: "servicerequests",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_servicerequests_owner_id",
                table: "servicerequests",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "FK_annotations_documents_document_id",
                table: "annotations",
                column: "document_id",
                principalTable: "documents",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_annotations_servicerequests_servicerequest_id",
                table: "annotations",
                column: "servicerequest_id",
                principalTable: "servicerequests",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_annotations_users_owner",
                table: "annotations",
                column: "owner",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_groups_users_owner_id",
                table: "groups",
                column: "owner_id",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_node_lastvisits_servicerequests_NodeId",
                table: "node_lastvisits",
                column: "NodeId",
                principalTable: "servicerequests",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_questionnaire_responses_servicerequests_ServiceRequestId",
                table: "questionnaire_responses",
                column: "ServiceRequestId",
                principalTable: "servicerequests",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_annotations_documents_document_id",
                table: "annotations");

            migrationBuilder.DropForeignKey(
                name: "FK_annotations_servicerequests_servicerequest_id",
                table: "annotations");

            migrationBuilder.DropForeignKey(
                name: "FK_annotations_users_owner",
                table: "annotations");

            migrationBuilder.DropForeignKey(
                name: "FK_groups_users_owner_id",
                table: "groups");

            migrationBuilder.DropForeignKey(
                name: "FK_node_lastvisits_servicerequests_NodeId",
                table: "node_lastvisits");

            migrationBuilder.DropForeignKey(
                name: "FK_questionnaire_responses_servicerequests_ServiceRequestId",
                table: "questionnaire_responses");

            migrationBuilder.DropTable(
                name: "documents");

            migrationBuilder.DropTable(
                name: "servicerequests");

            migrationBuilder.RenameColumn(
                name: "ServiceRequestId",
                table: "questionnaire_responses",
                newName: "NodeId");

            migrationBuilder.RenameIndex(
                name: "IX_questionnaire_responses_ServiceRequestId",
                table: "questionnaire_responses",
                newName: "IX_questionnaire_responses_NodeId");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "groups",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_groups_owner_id",
                table: "groups",
                newName: "IX_groups_OwnerId");

            migrationBuilder.RenameColumn(
                name: "owner",
                table: "annotations",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "servicerequest_id",
                table: "annotations",
                newName: "NodeId");

            migrationBuilder.RenameColumn(
                name: "document_id",
                table: "annotations",
                newName: "ChildNodeId");

            migrationBuilder.RenameIndex(
                name: "IX_annotations_servicerequest_id",
                table: "annotations",
                newName: "IX_annotations_NodeId");

            migrationBuilder.RenameIndex(
                name: "IX_annotations_owner",
                table: "annotations",
                newName: "IX_annotations_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_annotations_document_id",
                table: "annotations",
                newName: "IX_annotations_ChildNodeId");

            migrationBuilder.CreateTable(
                name: "nodes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    group_id = table.Column<Guid>(type: "TEXT", nullable: true),
                    owner_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RootNodeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDraft = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NodeType = table.Column<string>(type: "TEXT", nullable: false),
                    ParentNodeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    SortNr = table.Column<int>(type: "INTEGER", nullable: true),
                    StorageId = table.Column<string>(type: "TEXT", nullable: true),
                    Visibility = table.Column<int>(type: "INTEGER", nullable: false),
                    ipath2_id = table.Column<int>(type: "INTEGER", nullable: true),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    file = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nodes", x => x.id);
                    table.ForeignKey(
                        name: "FK_nodes_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "groups",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_nodes_nodes_RootNodeId",
                        column: x => x.RootNodeId,
                        principalTable: "nodes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_nodes_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_nodes_group_id",
                table: "nodes",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_nodes_owner_id",
                table: "nodes",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_nodes_RootNodeId",
                table: "nodes",
                column: "RootNodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_annotations_nodes_ChildNodeId",
                table: "annotations",
                column: "ChildNodeId",
                principalTable: "nodes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_annotations_nodes_NodeId",
                table: "annotations",
                column: "NodeId",
                principalTable: "nodes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_annotations_users_OwnerId",
                table: "annotations",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_groups_users_OwnerId",
                table: "groups",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_node_lastvisits_nodes_NodeId",
                table: "node_lastvisits",
                column: "NodeId",
                principalTable: "nodes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_questionnaire_responses_nodes_NodeId",
                table: "questionnaire_responses",
                column: "NodeId",
                principalTable: "nodes",
                principalColumn: "id");
        }
    }
}
