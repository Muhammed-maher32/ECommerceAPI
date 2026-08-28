using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations.Application
{
    /// <inheritdoc />
    public partial class Orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BuyerId = table.Column<Guid>(type: "uuid", nullable: false),
                    BuyerEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    OrderDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ShipToFirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShipToLastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShipToPhoneNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ShipToCountry = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShipToCity = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShipToStreet = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ShipToPostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DeliveryMethodName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DeliveryPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    PaymentIntentId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PictureUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BuyerId_OrderDate",
                table: "Orders",
                columns: new[] { "BuyerId", "OrderDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentIntentId",
                table: "Orders",
                column: "PaymentIntentId",
                unique: true,
                filter: "\"PaymentIntentId\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
