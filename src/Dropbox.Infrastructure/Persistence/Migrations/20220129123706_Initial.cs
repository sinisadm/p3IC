using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dropbox.Infrastructure.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MacAddress = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    QuotaLimit = table.Column<long>(type: "bigint", nullable: false),
                    OuotaUsed = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                    SyncFolder = table.Column<string>(type: "text", nullable: false),
                    RootFolderId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersDevices_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersDevices_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserDeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    RootFolderId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsFolder = table.Column<bool>(type: "boolean", nullable: false),
                    ItemName = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Items_ParentItemId",
                        column: x => x.ParentItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_UsersDevices_RootFolderId",
                        column: x => x.RootFolderId,
                        principalTable: "UsersDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_UsersDevices_UserDeviceId",
                        column: x => x.UserDeviceId,
                        principalTable: "UsersDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "DeletedAt", "IsDeleted", "MacAddress", "Name" },
                values: new object[] { new Guid("7cc09ce3-9452-4653-be3f-e5d602feb187"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "00-14-22-01-23-45", "Kompjuter 1" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DeletedAt", "Email", "IsDeleted", "OuotaUsed", "QuotaLimit" },
                values: new object[] { new Guid("4caec599-49b1-4a09-96a7-5cf3556fa21e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sinisadm@gmail.com", false, 0L, 2147483648L });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ParentItemId",
                table: "Items",
                column: "ParentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_RootFolderId",
                table: "Items",
                column: "RootFolderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_UserDeviceId",
                table: "Items",
                column: "UserDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDevices_DeviceId",
                table: "UsersDevices",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersDevices_UserId",
                table: "UsersDevices",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "UsersDevices");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
