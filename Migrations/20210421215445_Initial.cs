using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LeTwitchBot.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TwitchID = table.Column<int>(type: "INTEGER", nullable: false),
                    TwitchUsername = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Visits",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VisitDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VisitorID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Visits_Visitors_VisitorID",
                        column: x => x.VisitorID,
                        principalTable: "Visitors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Visits_VisitorID",
                table: "Visits",
                column: "VisitorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropTable(
                name: "Visitors");
        }
    }
}
