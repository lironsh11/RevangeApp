using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RevangesApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Revenges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Target = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    DramaLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revenges", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Revenges",
                columns: new[] { "Id", "Category", "CompletedDate", "Date", "Description", "DramaLevel", "Notes", "Status", "Target", "Title" },
                values: new object[] { 1, "AnnoyingCustomer", null, new DateTime(2025, 6, 2, 19, 51, 32, 130, DateTimeKind.Local).AddTicks(3494), "הלקוחה המעצבנת שהשפילה אותי בפני כולם תקבל את מה שמגיע לה!", 4, "צריכה להיות דרמטית במיוחד!", "Planned", "הלקוחה המעצבנת מהדואר", "נקמת השפתון האדום" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Revenges");
        }
    }
}
