using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iPath.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class LastVisit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "node_lastvisits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NodeImports",
                table: "NodeImports");

            migrationBuilder.RenameTable(
                name: "NodeImports",
                newName: "node_data_import");

            migrationBuilder.AddPrimaryKey(
                name: "PK_node_data_import",
                table: "node_data_import",
                column: "NodeId");

            migrationBuilder.CreateTable(
                name: "servicerequest_lastvisits",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ServiceRequestId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DocumentNodeId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicerequest_lastvisits", x => new { x.UserId, x.ServiceRequestId });
                    table.ForeignKey(
                        name: "FK_servicerequest_lastvisits_documents_DocumentNodeId",
                        column: x => x.DocumentNodeId,
                        principalTable: "documents",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_servicerequest_lastvisits_servicerequests_ServiceRequestId",
                        column: x => x.ServiceRequestId,
                        principalTable: "servicerequests",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_servicerequest_lastvisits_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_servicerequest_lastvisits_Date",
                table: "servicerequest_lastvisits",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_servicerequest_lastvisits_DocumentNodeId",
                table: "servicerequest_lastvisits",
                column: "DocumentNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_servicerequest_lastvisits_ServiceRequestId",
                table: "servicerequest_lastvisits",
                column: "ServiceRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "servicerequest_lastvisits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_node_data_import",
                table: "node_data_import");

            migrationBuilder.RenameTable(
                name: "node_data_import",
                newName: "NodeImports");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NodeImports",
                table: "NodeImports",
                column: "NodeId");

            migrationBuilder.CreateTable(
                name: "node_lastvisits",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    NodeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_node_lastvisits", x => new { x.UserId, x.NodeId });
                    table.ForeignKey(
                        name: "FK_node_lastvisits_servicerequests_NodeId",
                        column: x => x.NodeId,
                        principalTable: "servicerequests",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_node_lastvisits_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_node_lastvisits_Date",
                table: "node_lastvisits",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_node_lastvisits_NodeId",
                table: "node_lastvisits",
                column: "NodeId");
        }
    }
}
