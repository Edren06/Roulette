using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roulette.Migrations
{
    /// <inheritdoc />
    public partial class ModelCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NumberAttribute",
                columns: table => new
                {
                    NumberAttributeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    PayoutValue = table.Column<decimal>(type: "decimal(10, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberAttribute", x => x.NumberAttributeId);
                });

            migrationBuilder.CreateTable(
                name: "TableItem",
                columns: table => new
                {
                    TableItemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsLandable = table.Column<bool>(type: "bit", nullable: false),
                    NumberAttributeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableItem", x => x.TableItemId);
                    table.ForeignKey(
                        name: "FK_TableItem_NumberAttribute_NumberAttributeId",
                        column: x => x.NumberAttributeId,
                        principalTable: "NumberAttribute",
                        principalColumn: "NumberAttributeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Spin",
                columns: table => new
                {
                    SpinId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SpinDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TableItemId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spin", x => x.SpinId);
                    table.ForeignKey(
                        name: "FK_Spin_TableItem_TableItemId",
                        column: x => x.TableItemId,
                        principalTable: "TableItem",
                        principalColumn: "TableItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TableItemAttribute",
                columns: table => new
                {
                    TableItemAttributeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TableItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberAttributeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableItemAttribute", x => x.TableItemAttributeId);
                    table.ForeignKey(
                        name: "FK_TableItemAttribute_NumberAttribute_NumberAttributeId",
                        column: x => x.NumberAttributeId,
                        principalTable: "NumberAttribute",
                        principalColumn: "NumberAttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableItemAttribute_TableItem_TableItemId",
                        column: x => x.TableItemId,
                        principalTable: "TableItem",
                        principalColumn: "TableItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bet",
                columns: table => new
                {
                    BetId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BetDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    TableItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    SpinId = table.Column<int>(type: "INTEGER", nullable: true),
                    Payout = table.Column<decimal>(type: "decimal(10, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bet", x => x.BetId);
                    table.ForeignKey(
                        name: "FK_Bet_Spin_SpinId",
                        column: x => x.SpinId,
                        principalTable: "Spin",
                        principalColumn: "SpinId");
                    table.ForeignKey(
                        name: "FK_Bet_TableItem_TableItemId",
                        column: x => x.TableItemId,
                        principalTable: "TableItem",
                        principalColumn: "TableItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bet_SpinId",
                table: "Bet",
                column: "SpinId");

            migrationBuilder.CreateIndex(
                name: "IX_Bet_TableItemId",
                table: "Bet",
                column: "TableItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Spin_TableItemId",
                table: "Spin",
                column: "TableItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TableItem_NumberAttributeId",
                table: "TableItem",
                column: "NumberAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TableItemAttribute_NumberAttributeId",
                table: "TableItemAttribute",
                column: "NumberAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TableItemAttribute_TableItemId",
                table: "TableItemAttribute",
                column: "TableItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bet");

            migrationBuilder.DropTable(
                name: "TableItemAttribute");

            migrationBuilder.DropTable(
                name: "Spin");

            migrationBuilder.DropTable(
                name: "TableItem");

            migrationBuilder.DropTable(
                name: "NumberAttribute");
        }
    }
}
