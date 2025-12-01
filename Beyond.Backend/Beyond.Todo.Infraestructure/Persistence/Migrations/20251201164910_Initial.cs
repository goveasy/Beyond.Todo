using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Beyond.Todo.Infraestructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TodoItemCategories",
                columns: table => new
                {
                    Category = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItemCategories", x => x.Category);
                });

            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Progression",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Percent = table.Column<decimal>(type: "numeric", nullable: false),
                    TodoItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Progression", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Progression_TodoItems_TodoItemId",
                        column: x => x.TodoItemId,
                        principalTable: "TodoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TodoItemCategories",
                column: "Category",
                values: new object[]
                {
                    "Learning",
                    "Personal",
                    "Work"
                });

            migrationBuilder.InsertData(
                table: "TodoItems",
                columns: new[] { "Id", "Category", "Description", "Title" },
                values: new object[] { 1, "Work", "Progreso de la construcion de la aplicacion beyond todo.", "Construir el sistema Beyond Todo." });

            migrationBuilder.InsertData(
                table: "Progression",
                columns: new[] { "Id", "Date", "Percent", "TodoItemId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 25m, 1 },
                    { 2, new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 35m, 1 },
                    { 3, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 40m, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Progression_TodoItemId",
                table: "Progression",
                column: "TodoItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Progression");

            migrationBuilder.DropTable(
                name: "TodoItemCategories");

            migrationBuilder.DropTable(
                name: "TodoItems");
        }
    }
}
