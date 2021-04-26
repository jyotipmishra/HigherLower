using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HigherOrLower.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    GameId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    RemainingCards = table.Column<int>(nullable: false),
                    FaceValue = table.Column<int>(nullable: false),
                    IsGameOver = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.GameId);
                });

            migrationBuilder.CreateTable(
                name: "Card",
                columns: table => new
                {
                    CardId = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    FaceValue = table.Column<int>(nullable: false),
                    NextValue = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.CardId);
                    table.ForeignKey(
                        name: "FK_Card_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Card_GameId",
                table: "Card",
                column: "GameId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Card");

            migrationBuilder.DropTable(
                name: "Game");
        }
    }
}
